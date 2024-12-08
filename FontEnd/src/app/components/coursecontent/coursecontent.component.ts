import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { SessionStorageService } from '../../sessionstorage.service';

interface CourseContent {
  type: string;
  url: string;
  name: string;
  fileType: string;
  thumbnail?: string;
}

@Component({
  selector: 'app-coursecontent',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule], 
templateUrl: './coursecontent.component.html',
  styleUrls: ['./coursecontent.component.css'],
})
export class CourseContentComponent implements OnInit {

  youtubeLink = '';
  selectedFile: File | null = null;
  storedContent: CourseContent[] = [];
  role:string | undefined;

  // New properties for image modal
  isImageModalOpen = false;
  selectedImage: string = ''; // Declare selectedImage to store the selected image URL

  constructor(private toastr: ToastrService, private http: HttpClient,private SessionStorageService:SessionStorageService) {}

  ngOnInit(): void {
    this.loadStoredContent();
    this.role = this.SessionStorageService.getItem('role');
  }

  // Load content from local storage
  loadStoredContent(): void {
    const content = localStorage.getItem('course_content');
    if (content) {
      this.storedContent = JSON.parse(content);
    }
  }

  // Handle file selection
  onFileSelect(event: any): void {
    this.selectedFile = event.target.files[0];
  }

  // Determine file type and icon
  getFileTypeDetails(file: File): { type: string; icon: string } {
    const fileName = file.name.toLowerCase();
    const fileExtension = fileName.split('.').pop() || '';

    // Image types
    const imageExtensions = ['jpg', 'jpeg', 'png', 'gif', 'bmp', 'webp', 'svg'];
    if (imageExtensions.includes(fileExtension)) {
      return { type: 'image', icon: '/assets/icons/image-icon.png' };
    }

    // Video types
    const videoExtensions = ['mp4', 'avi', 'mov', 'wmv', 'flv', 'webm'];
    if (videoExtensions.includes(fileExtension)) {
      return { type: 'video', icon: '/assets/icons/video-icon.png' };
    }

    // Audio types
    const audioExtensions = ['mp3', 'wav', 'ogg', 'flac', 'm4a', 'wma'];
    if (audioExtensions.includes(fileExtension)) {
      return { type: 'audio', icon: '/assets/icons/audio-icon.png' };
    }

    // Document types
    const documentExtensions = ['doc', 'docx', 'txt', 'rtf', 'odt'];
    if (documentExtensions.includes(fileExtension)) {
      return { type: 'document', icon: '/assets/icons/document-icon.png' };
    }

    // Spreadsheet types
    const spreadsheetExtensions = ['xls', 'xlsx', 'csv'];
    if (spreadsheetExtensions.includes(fileExtension)) {
      return {
        type: 'spreadsheet',
        icon: '/assets/icons/spreadsheet-icon.png',
      };
    }

    // Presentation types
    const presentationExtensions = ['ppt', 'pptx', 'pps'];
    if (presentationExtensions.includes(fileExtension)) {
      return {
        type: 'presentation',
        icon: '/assets/icons/presentation-icon.png',
      };
    }

    // PDF
    if (fileExtension === 'pdf') {
      return { type: 'pdf', icon: '/assets/icons/pdf-icon.png' };
    }

    // Compressed files
    const compressedExtensions = ['zip', 'rar', '7z', 'tar', 'gz'];
    if (compressedExtensions.includes(fileExtension)) {
      return { type: 'compressed', icon: '/assets/icons/compressed-icon.png' };
    }

    // Fallback for unknown file types
    return { type: 'unknown', icon: '/assets/icons/file-icon.png' };
  }

  // Add content (supports all file types and YouTube links)
  addContent(): void {
    let content: CourseContent | undefined;

    if (this.selectedFile) {
      const fileReader = new FileReader();
      fileReader.onload = (e) => {
        const fileUrl = e.target?.result as string;
        const fileTypeDetails = this.getFileTypeDetails(this.selectedFile!);

        content = {
          type: fileTypeDetails.type,
          url: fileUrl,
          name: this.selectedFile!.name,
          fileType: this.selectedFile!.type,
          thumbnail: fileTypeDetails.icon,
        };

        if (content) {
          this.storeContent(content);
          this.resetForm();
          this.toastr.success('Content added successfully!');
        }
      };
      fileReader.readAsDataURL(this.selectedFile);
    } else if (this.youtubeLink.trim()) {
      const videoId = this.getYouTubeVideoId(this.youtubeLink.trim());
      if (videoId) {
        content = {
          type: 'youtube',
          url: this.youtubeLink.trim(),
          name: `YouTube Video ${videoId}`,
          fileType: 'video/youtube',
          thumbnail: `https://img.youtube.com/vi/${videoId}/hqdefault.jpg`,
        };
        this.storeContent(content);
        this.resetForm();
        this.toastr.success('YouTube link added successfully!');
      } else {
        this.toastr.warning('Invalid YouTube link!');
      }
    } else {
      this.toastr.warning('Please select a file or enter a YouTube link!');
    }
  }

  // Helper function to extract YouTube video ID
  getYouTubeVideoId(url: string): string | null {
    const regex =
      /(?:https?:\/\/(?:www\.)?youtube\.com\/(?:[^\/\n\s]+\/\S+\/|(?:v|e(?:mbed)?)\/|.*[?&]v=|youtu\.be\/)([a-zA-Z0-9_-]{11}))/;
    const match = url.match(regex);
    return match ? match[1] : null;
  }

  // Store content in local storage
  storeContent(content: CourseContent): void {
    const existingContent = JSON.parse(
      localStorage.getItem('course_content') || '[]'
    );
    existingContent.push(content);
    localStorage.setItem('course_content', JSON.stringify(existingContent));

    this.loadStoredContent(); // Reload to update UI
  }

  // Reset form
  resetForm(): void {
    this.youtubeLink = '';
    this.selectedFile = null;
  }

  // Play YouTube video in a modal
  playYouTube(url: string): void {
    const videoId = this.getYouTubeVideoId(url);
    if (videoId) {
      const playerContainer = document.createElement('div');
      playerContainer.classList.add(
        'fixed',
        'top-0',
        'left-0',
        'w-full',
        'h-full',
        'flex',
        'items-center',
        'justify-center',
        'bg-black',
        'bg-opacity-75',
        'z-50'
      );

      const iframe = document.createElement('iframe');
      iframe.width = '640';
      iframe.height = '360';
      iframe.src = `https://www.youtube.com/embed/${videoId}?autoplay=1`;
      iframe.allow =
        'accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture';
      iframe.allowFullscreen = true;
      iframe.style.border = 'none';

      const closeButton = document.createElement('button');
      closeButton.textContent = '✖';
      closeButton.classList.add(
        'absolute',
        'top-4',
        'right-4',
        'text-white',
        'text-xl',
        'font-bold',
        'bg-black',
        'bg-opacity-50',
        'rounded-full',
        'p-2',
        'hover:bg-opacity-75'
      );
      closeButton.onclick = () => {
        playerContainer.remove();
      };

      playerContainer.appendChild(iframe);
      playerContainer.appendChild(closeButton);
      document.body.appendChild(playerContainer);
    } else {
      console.error('Invalid YouTube URL');
    }
  }

  // Delete content
  deleteContent(index: number): void {
    const content = JSON.parse(localStorage.getItem('course_content') || '[]');
    content.splice(index, 1);
    localStorage.setItem('course_content', JSON.stringify(content));
    this.loadStoredContent();
  }

  openImageModal(imageUrl: string): void {
    this.selectedImage = imageUrl;
    this.isImageModalOpen = true;
  }

  // Close the image modal
  closeImageModal(): void {
    this.isImageModalOpen = false;
    this.selectedImage = ''; // Reset selected image
  }
}

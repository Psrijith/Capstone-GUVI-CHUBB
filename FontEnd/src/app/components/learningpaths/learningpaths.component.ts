import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

@Component({
  selector: 'app-learningpaths',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './learningpaths.component.html',
  styleUrl: './learningpaths.component.css',
})
export class LearningPathsComponent {
  learningPaths = [
    {
      name: 'Web Development',
      icon: 'https://cdn-icons-png.flaticon.com/512/2210/2210153.png',
      subPaths: [
        {
          name: 'Frontend Development',
          description:
            'Learn HTML, CSS, and JavaScript for building user interfaces.',
          topics: ['HTML', 'CSS', 'JavaScript', 'React', 'Angular'],
          progress: 45,
        },
        {
          name: 'Backend Development',
          description: 'Master server-side technologies and databases.',
          topics: ['Node.js', 'Express', 'MongoDB', 'SQL', 'API Development'],
          progress: 30,
        },
      ],
    },
    {
      name: 'Data Science',
      icon: 'https://cdn-icons-png.flaticon.com/512/4240/4240994.png',
      subPaths: [
        {
          name: 'Data Analysis',
          description: 'Analyze data with tools like Python and R.',
          topics: ['Python', 'Pandas', 'NumPy', 'R', 'Data Visualization'],
          progress: 60,
        },
        {
          name: 'Machine Learning',
          description: 'Build ML models using Python libraries.',
          topics: ['Scikit-learn', 'TensorFlow', 'PyTorch', 'Deep Learning'],
          progress: 25,
        },
      ],
    },
    {
      name: 'Mobile Development',
      icon: 'https://cdn-icons-png.flaticon.com/512/5738/5738031.png',
      subPaths: [
        {
          name: 'iOS Development',
          description: 'Develop apps for iOS using Swift and Xcode.',
          topics: [
            'Swift',
            'Xcode',
            'UIKit',
            'Core Data',
            'App Store Deployment',
          ],
          progress: 40,
        },
        {
          name: 'Android Development',
          description:
            'Learn Android app development using Java/Kotlin and Android Studio.',
          topics: [
            'Java',
            'Kotlin',
            'Android Studio',
            'Firebase',
            'Google Play Store',
          ],
          progress: 55,
        },
      ],
    },
    {
      name: 'Cloud Computing',
      icon: 'https://uxwing.com/wp-content/themes/uxwing/download/brands-and-social-media/android-studio-icon.png',
      subPaths: [
        {
          name: 'AWS Essentials',
          description: 'Understand cloud computing fundamentals using AWS.',
          topics: ['EC2', 'S3', 'Lambda', 'CloudFormation', 'IAM'],
          progress: 35,
        },
        {
          name: 'Azure Fundamentals',
          description: 'Learn about Microsoft Azure and cloud infrastructure.',
          topics: [
            'Virtual Machines',
            'Azure Blob Storage',
            'Azure Functions',
            'Networking',
          ],
          progress: 50,
        },
      ],
    },
    {
      name: 'Cybersecurity',
      icon: 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ47Jm6sKPnwu-TSNkcj1RqKmSbaYRUXBYZug&s',
      subPaths: [
        {
          name: 'Network Security',
          description:
            'Learn how to secure computer networks and infrastructures.',
          topics: [
            'Firewalls',
            'VPNs',
            'IDS/IPS',
            'Encryption',
            'Network Protocols',
          ],
          progress: 60,
        },
        {
          name: 'Ethical Hacking',
          description:
            'Master ethical hacking and penetration testing techniques.',
          topics: [
            'Kali Linux',
            'Metasploit',
            'Wireshark',
            'Nmap',
            'SQL Injection',
          ],
          progress: 40,
        },
      ],
    },
  ];

  openedPaths: string[] = [];

  togglePath(pathName: string): void {
    if (this.openedPaths.includes(pathName)) {
      this.openedPaths = this.openedPaths.filter((path) => path !== pathName);
    } else {
      this.openedPaths.push(pathName);
    }
  }
}
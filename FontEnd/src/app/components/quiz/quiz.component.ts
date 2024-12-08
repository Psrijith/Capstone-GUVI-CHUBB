import { Component } from '@angular/core';
import { QuizService } from '../../services/quiz.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Chart } from 'chart.js'; // Import Chart.js

@Component({
  selector: 'app-quiz',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './quiz.component.html',
  styleUrls: ['./quiz.component.css'],
})
export class QuizComponent {
  userMessage: string = ''; // Topic for the quiz
  quizData: any[] = []; // Parsed quiz data from API
  userAnswers: string[] = []; // Store user's answers
  correctAnswers: string[] = []; // Store correct answers as an array
  explanations: string[] = []; // Store explanations for incorrect answers
  isLoading: boolean = false;
  score: number | null = null;
  showExplanations: boolean = false; // Toggle to show explanations

  chart: any = null; // Chart variable to hold the chart instance
  feedbackMessage: string = ''; // Feedback message to display based on score

  constructor(private apiService: QuizService) {}

  // Fetch quiz from API
  fetchQuiz(): void {
    if (this.userMessage.trim()) {
      this.isLoading = true;
      this.apiService.getChatCompletion(this.userMessage).subscribe(
        (response) => {
          try {
            const rawContent = response.choices[0].message.content;
            const sanitizedContent = rawContent
              .replace(/```json|```/g, '')
              .trim();

            // Parse the sanitized JSON
            const data = JSON.parse(sanitizedContent);
            console.log(data);
            if (data.quiz && Array.isArray(data.quiz.questions)) {
              this.quizData = data.quiz.questions;
              this.correctAnswers = data.quiz.questions.map(
                (question: any) => question.correctAnswer
              );
              this.explanations = this.quizData.map(
                (q) => q.explanation || 'No explanation provided.'
              );
              this.userAnswers = Array(this.quizData.length).fill('');
            } else {
              throw new Error('Unexpected data structure');
            }
          } catch (error) {
            console.log('Failed to parse API response:', error);
          } finally {
            this.isLoading = false;
          }
        },
        (error) => {
          console.error('Error:', error);
          this.isLoading = false;
        }
      );
    }
  }

  // Calculate score and show explanations
  calculateScore(): void {
    this.score = 0;
    this.showExplanations = true;

    // Ensure quizData is valid before calculating the score
    if (this.quizData && this.quizData.length > 0) {
      for (let i = 0; i < this.quizData.length; i++) {
        const userAnswer = this.userAnswers[i];
        const correctAnswer = this.correctAnswers[i];

        if (userAnswer === correctAnswer) {
          this.score++;
        }
      }

      this.generateFeedback();
      this.generateChart();
    }
  }

  // Generate feedback message based on score
  generateFeedback(): void {
    if (this.score === null) {
      return; // Exit early if score is null (i.e., quiz hasn't been taken yet)
    }
    if (this.score === this.quizData.length) {
      this.feedbackMessage = 'Outstanding! Excellent work!';
    } else if (this.score >= 8) {
      this.feedbackMessage = 'Great job! You are very close!';
    } else if (this.score >= 4) {
      this.feedbackMessage = 'Good effort! You can improve more.';
    } else {
      this.feedbackMessage = 'Need to try really hard next time.';
    }
  }

  // Generate chart based on the score
  generateChart(): void {
    const ctx = document.getElementById('scoreChart') as HTMLCanvasElement;
    if (this.chart) {
      this.chart.destroy(); // Destroy previous chart instance if it exists
    }

    if (this.score !== null && this.quizData && this.quizData.length > 0) {
      this.chart = new Chart(ctx, {
        type: 'pie',
        data: {
          labels: ['Correct', 'Incorrect'],
          datasets: [
            {
              label: 'Your Performance',
              data: [this.score, this.quizData.length - this.score],
              backgroundColor: ['#4CAF50', '#FF5733'],
              hoverOffset: 4,
            },
          ],
        },
        options: {
          responsive: true,
          plugins: {
            legend: {
              position: 'top',
            },
            tooltip: {
              callbacks: {
                label: function (tooltipItem) {
                  return tooltipItem.raw + ' answers';
                },
              },
            },
          },
        },
      });
    }
  }
  
}

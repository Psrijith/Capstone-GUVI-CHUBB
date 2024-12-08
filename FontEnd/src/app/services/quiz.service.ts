import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class QuizService {
  private apiKey =
    'xai-bSdUMfyvz1TOa5IG2kOIejxQySTmPH9DpUdR1XrsWaxTvofyYOHXBcbZ8kmrEevqhKqyIg9VidwDpRp1';
  private baseURL = 'https://api.x.ai/v1';

  constructor(private http: HttpClient) {}

  // Function to get a response from the Grok/OpenAI API
  getChatCompletion(topic: string): Observable<any> {
    const headers = new HttpHeaders({
      Authorization: `Bearer ${this.apiKey}`,
      'Content-Type': 'application/json',
    });

    const body = {
      model: 'grok-beta',
      messages: [
        {
          role: 'system',
          content:
            'You are Grok, an intelligent assistant. Generate a JSON response for a quiz. The response should include 10 questions based on the topic provided, each with options (A, B, C, D), only one correct answer , mention asnwer as correctAnswer and explanations for each question.',
        },
        { role: 'user', content: `Generate a quiz on: ${topic}` },
      ],
    };

    return this.http.post<any>(`${this.baseURL}/chat/completions`, body, {
      headers,
    });
  }
}

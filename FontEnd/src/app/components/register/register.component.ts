import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MaterialModule } from '../../material.modules';
import { RouterModule } from '@angular/router';
import { userregister } from '../../model/user.model';
import { CommonModule } from '@angular/common';
import { UserService } from '../../services/user.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, MaterialModule, RouterModule, CommonModule],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'], // Fixed the styleUrls property (plural)
})
export class RegisterComponent {
  constructor(
    private builder: FormBuilder,
    private service: UserService,
    private toastr: ToastrService
  ) {}

  response: any;

  regform = this.builder.group({
    username: this.builder.control(
      '',
      Validators.compose([Validators.required, Validators.minLength(5)])
    ),
    email: this.builder.control(
      '',
      Validators.compose([Validators.email, Validators.required])
    ),
    password: this.builder.control(
      '',
      Validators.compose([Validators.minLength(6), Validators.required])
    ),
    confirmPassword: this.builder.control(
      '',
      Validators.compose([Validators.required, Validators.minLength(6)])
    ),
    role: this.builder.control('', Validators.required),
  });

  onSubmit() {
    if (this.regform.invalid) {
      this.toastr.error('Please fill all required fields correctly.');
      return;
    }

    if (this.regform.value.password !== this.regform.value.confirmPassword) {
      this.toastr.error('Passwords do not match.');
      return;
    }

    let obj: userregister = {
      username: this.regform.value.username as string,
      email: this.regform.value.email as string,
      password: this.regform.value.password as string,
      role: this.regform.value.role as string,
    };

    this.service.Userregistration(obj).subscribe({
      next: (response) => {
        if (response === 'User registered successfully.') {
          this.toastr.success('Registration successful');
        } else {
          // Printing a detailed error message here
          const errorMessage = response.error
            ? response.error
            : 'Unexpected error occurred.';
          this.toastr.error('Error in registering: ' + errorMessage);
        }
      },
      error: (err) => {
        console.log('Error occurred during registration:', err.error); 
        this.toastr.error(err.error);
      },
    });
  }
}

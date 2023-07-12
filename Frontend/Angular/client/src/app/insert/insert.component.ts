import { Component, Inject, OnInit, inject } from '@angular/core';
import { HousingLocation } from '../housing-location';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { HousingService } from '../housing.service';
import { MatSnackBar, MatSnackBarConfig, MAT_SNACK_BAR_DEFAULT_OPTIONS } from '@angular/material/snack-bar';
import { catchError } from 'rxjs';

@Component({
  selector: 'app-insert',
  templateUrl: './insert.component.html',
  styleUrls: ['./insert.component.css']
})
export class InsertComponent implements OnInit {
  housingService: HousingService = inject(HousingService);
  housinglocaion: HousingLocation | undefined;
  addDataForm!: FormGroup;
  imageSrc: string | undefined;

  constructor(private formBuilder: FormBuilder, private snackBar: MatSnackBar, @Inject(MAT_SNACK_BAR_DEFAULT_OPTIONS) private defaultSnackBarConfig: MatSnackBarConfig){
  }

  ngOnInit(): void {
    this.addDataForm = this.formBuilder.group({
      name: ['', Validators.required],
      city: ['',Validators.required],
      state: ['', Validators.required],
      image: [''],
      availableUnits: [0, Validators.required],
      wifi: [false],
      laundry: [false]
    });
    this.imageSrc = "https://loremflickr.com/640/480/business";
  }

  onSubmit(){
    if (this.addDataForm.valid){
      const formData = new FormData();
      this.housinglocaion = this.addDataForm.value as HousingLocation;
      formData.append('Name',this.addDataForm.get('name')?.value);
      formData.append('City',this.addDataForm.get('city')?.value);
      formData.append('State',this.addDataForm.get('state')?.value);
      formData.append('AvailableUnits',this.addDataForm.get('availableUnits')?.value);
      formData.append('Wifi',this.addDataForm.get('wifi')?.value);
      formData.append('Image',this.addDataForm.get('image')?.value);
      formData.append('Laundry',this.addDataForm.get('laundry')?.value);
      console.log(formData.getAll);
      this.housingService.insertHousing(formData)
      .pipe(
        catchError((error) => {
          console.error('Lỗi:', error);
          this.snackBar.open('Đã xảy ra lỗi cem console để biết thông tin chi tiết', 'Đóng', {
            duration: 5000
          });
          throw error;
        })
      )
      .subscribe((id: string) => {
        const snackBarConfig: MatSnackBarConfig = {
          ...this.defaultSnackBarConfig,
          verticalPosition: 'bottom', // Vị trí dọc: 'top', 'bottom'
          horizontalPosition: 'right', // Vị trí ngang: 'start', 'center', 'end', 'left', 'right'
        };

        this.snackBar.open('Thêm thành công', "Đóng", snackBarConfig)
      });
    }
   

  }

  onFileChange(event: Event): void {
    const inputElement = event.target as HTMLInputElement;
    if (inputElement.files && inputElement.files[0]) {
      const file = inputElement.files[0];
      const reader = new FileReader();
      reader.onload = (e) => {
        this.imageSrc = e.target?.result as string;
        this.addDataForm.get('image')?.setValue(file);
      };
      reader.readAsDataURL(file);
    }
  }

}

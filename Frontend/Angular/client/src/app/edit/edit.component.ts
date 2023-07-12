import { Component, inject, OnInit } from '@angular/core';
import { HousingLocation } from '../housing-location';
import { ActivatedRoute } from '@angular/router';
import { HousingService } from '../housing.service';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.css']
})
export class EditComponent{
  fromData = new FormData();
  uploadForm!: FormGroup;
  housinglocaion: HousingLocation | undefined;
  route: ActivatedRoute = inject(ActivatedRoute);
  housingServie: HousingService = inject(HousingService);
  imgsrc: string | undefined;
  housinglocationid: string;

  constructor(private formBuilder: FormBuilder, private snackBar: MatSnackBar) {
    this.housinglocationid = this.route.snapshot.params['id'];
    this.housingServie.getHousingLocationById(this.housinglocationid).subscribe((result: HousingLocation | undefined) => (this.housinglocaion = result));
    this.uploadForm = this.formBuilder.group({
      images: ['']
    });
  }
  
  handleRemoveClick(id: string){
    this.housingServie.removeHousing(id).subscribe((result: any) => {
      this.snackBar.open(result.message, "Đóng");
      
    })
  }

  handleClick() {
    const formData = new FormData();
    const file = this.uploadForm.get('images')?.value;
    formData.append("Housing", JSON.stringify(this.housinglocaion));
    if (file){
      formData.append("Images", file, file.name);
    }
    this.housingServie.updateHousingById(this.housinglocaion?.id, formData).subscribe(((result: any)=> 
    {this.snackBar.open(result.message, "Đóng")}));
  }

  onFileChange(event: Event): void {
    const inputElement = event.target as HTMLInputElement;
    if (inputElement.files && inputElement.files[0]) {
      const file = inputElement.files[0];
      console.log(file);
      const reader = new FileReader();
      this.uploadForm.get('images')?.setValue(file);
      reader.onload = (e) => {
        if (this.housinglocaion) {
          this.imgsrc = e.target?.result as string;
        }
      };
      reader.readAsDataURL(file);
    }
  }
}

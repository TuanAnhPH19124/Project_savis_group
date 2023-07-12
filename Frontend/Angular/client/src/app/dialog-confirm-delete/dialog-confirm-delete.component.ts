import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { BookingService } from '../booking.service';
import { BookingDetail } from '../Models/bookingGetRequestDto';

@Component({
  selector: 'delete-dialog-confirm',
  templateUrl: './dialog-confirm-delete.component.html',
  styleUrls: ['./dialog-confirm-delete.component.css']
})
export class DialogConfirmDeleteComponent {
  constructor(public dialogRef: MatDialogRef<DialogConfirmDeleteComponent>,
    private bookingService: BookingService
  ){}

  // onNoClick(): void {
  //   this.dialogRef.close();
  // }

  onOkClick(): void{
    this.dialogRef.close(true);
  }
}

export interface DiaLogData{
  id: string
}

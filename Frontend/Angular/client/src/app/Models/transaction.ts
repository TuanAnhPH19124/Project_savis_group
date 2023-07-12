import { TransactionDetailDto } from "./transactionDetail"

export interface TransactionDto{
  CheckInDate:Date,
  Total:number,
  PayMethod: string,
  Status: string,
  CustomerId: string
  TransactionDetails: TransactionDetailDto[]
}
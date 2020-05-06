## VNPost_OCR_System

### Deploy
* Link Web: https://vnpost.tech/

### Drive tài liệu
* Link drive: https://drive.google.com/drive/u/2/folders/1sXn6cJLndSQFr7N2Dv-mib6ZgDIxUkj1

### Database
* Link to attach files: 
https://drive.google.com/drive/u/0/folders/1qfN8cthFpYZVG1Cpp7ust1L73PZVGqtB

* 2 file .mdf và .LDF sẽ bị cho vào gitignore

* Nếu có bất kì thay đổi ở file attach nào:
-> Replace file trong folder theo link trên

##### Enable store procedure to run assembly:
sp_configure 'clr enabled', 1;    
GO    
RECONFIGURE;    
GO    
ALTER DATABASE VNPOST_Appointment SET TRUSTWORTHY ON;
GO
USE VNPOST_Appointment
GO
EXEC sp_changedbowner 'sa'
GO
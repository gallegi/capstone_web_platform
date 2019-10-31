# Guide Test Web responsive 
  khi run code tại google chrome bấm [F12] chọn **Together device toolbar** [Ctrl+Shift+M] để mô phỏng màn hình của điện thoại. Từ đó test được xem code của mình đã được responsive không. 
  
  Lưu ý  : refresh trang [F5] sau khi chuyển qua view của device để chạy lại javascripts
##
[Check device](https://stackoverflow.com/questions/3514784/what-is-the-best-way-to-detect-a-mobile-device) xem web có phải điện thoại hay không. Nếu là điện thoại thì xử lý thêm = javascript.
##
Với các Template thì hầu hết các template đều full responsive. Khi implement thêm code phải test xem code có đủ responsive không.

# Check list : 

- [ ] fix khung ở trang [Nhập giấy hẹn](https://vnpost.azurewebsites.net/giay-hen/nhap-giay-hen)
    ````html 
    <div class="page_wrapper content_all"> 
    ````
    
    File css bóp trang vào giữa -> bị thu nhỏ nằm trong [MasterPage.css](vnpost_ocr_system/CustomCSS/MasterPage/MasterPage.css)
    ````css
    .page_wrapper {
    margin-left: 5%; 
    margin-right: 5%; 
    margin-top: 70px;
    padding-left: 10%; 
    padding-right: 10%;
      }
    ````

- [ ] Issues : Table display Table display quá lớn, không phù hợp với điện thoại
- [ ] [Tra cứu đơn hàng](https://vnpost.azurewebsites.net/giay-hen/trang-thai-giay-hen) thì cái 
    ````html
    <div class="card-content wizard-content">
    ````
    không hợp lý, bỏ đi khi xem = điện thoại
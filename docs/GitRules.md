# 1. Issue:
- Mỗi task được assign mọi người log 1 issue (hoặc break thành các issue nhỏ hơn) trên gitlab
- Issue phải có description rõ ràng, milestone, assignee, deadline
- Các tiến độ thường xuyên được update trên phần chat của issue

# 2. Branch:
- Branch master chỉ được update ở 1 số thời điểm khi milestone complete.
- Mỗi người khi code 1 task nào đó cần tạo 1 branch riêng cho task đó. Đặt tên theo format Tên assignee/Tên task. 
Ví dụ: NamNT/screen1. Code xong, merge nhánh develop vào branch của mình để resolve conflict, sau đó push branch của minh
lên gitlab, tạo 1 merge request với source là branch của mình, target là develop.

# 3. Merge requests:
- Mỗi người không code trực tiếp trên develop hay master mà code trên branch của mình, dùng merge request để yêu cầu merge vào develop.
- Chỉ có 1 số người có quyền approve merge request.
- Khi approve merge request, chọn option delete source branch dể xóa branch mà vừa được merge vào develop khỏi gitlab

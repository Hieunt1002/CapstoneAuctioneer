# CapstoneAuctioneer
# Hướng dẫn cập nhật cơ sở dữ liệu với `Update-Database`

## 1. Mở Visual Studio

- Khởi động Visual Studio và mở dự án mà bạn muốn cập nhật cơ sở dữ liệu.

## 2. Cập nhật chuỗi kết nối cơ sở dữ liệu

- Trước khi chạy lệnh `Update-Database`, bạn cần đảm bảo rằng chuỗi kết nối cơ sở dữ liệu trong file `appsettings.json` đã được cấu hình đúng.

### Bước 1: Mở file `appsettings.json`

- Tìm và mở file `appsettings.json` trong dự án của bạn.

### Bước 2: Thay đổi chuỗi kết nối

- Tìm phần cấu hình `"ConnectionStrings"` và cập nhật chuỗi kết nối cho cơ sở dữ liệu của bạn. Ví dụ:

  ```json
  {
    "ConnectionStrings": {
      "DefaultConnection": "Server=your_server_name;Database=your_database_name;User Id=your_username;Password=your_password;"
    }
  }

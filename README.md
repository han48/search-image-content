# Search image content

Tìm kiếm hình ảnh thông qua nội dung của ảnh.

[Download](https://github.com/han48/search-image-content/blob/main/Release.md)

## Hoạt động

Khi người dùng chọn ảnh để thêm vào ứng dụng, ứng dụng sẽ sử dụng AI để nhận diện nội dung có trong ảnh và mô tả dưới dạng văn bản.
Người dùng có thể thực hiện tìm kiếm ảnh bằng cách nhập nội dung cần tìm kiếm vào ô tìm kiếm.

## Demo
https://github.com/user-attachments/assets/74c0dbdc-790c-497c-aa41-9c8c37b13454

## Lưu ý

Do hạn chế của thiết bị di động, việc sử dụng AI để chuyển đổi ảnh thành văn bản sẽ được thực hiện thông qua server AI.

## Config

```json
{
  "API_URL": "Web API URL"
}
```

### API requirement:

POST file image.

Response:

```
{
  "content": "a city is shown in this aerial photo.",
  "message": "OK",
  "time": 26.765355348587036
}
```

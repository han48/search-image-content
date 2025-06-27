# Search image content

**Khám phá Ứng dụng Đột Phá – Biến Hình Ảnh Thành Nội Dung Dễ Tìm Kiếm Nhờ AI**

Bạn đang sở hữu một thư viện ảnh khổng lồ và gặp khó khăn khi cần tìm lại một khoảnh khắc cụ thể? Ứng dụng AI đa nền tảng của chúng tôi – tương thích với Windows, macOS, Android và iOS – chính là giải pháp bạn đang tìm kiếm.

Ứng dụng sử dụng trí tuệ nhân tạo tiên tiến để **phân tích và mô tả nội dung hình ảnh dưới dạng văn bản**, giúp người dùng nhanh chóng tìm kiếm lại ảnh thông qua từ khóa, chủ đề hoặc mô tả chi tiết. Không cần nhớ tên tệp hay thời gian chụp, bạn chỉ cần nhập vào điều bạn đang tìm – “một bức ảnh hoàng hôn trên bãi biển” hay “bữa tiệc sinh nhật với bánh kem” – và ứng dụng sẽ đưa bạn đến đúng nơi.

**Tính năng nổi bật:**
- Chuyển đổi hình ảnh thành mô tả tự động bằng AI
- Tìm kiếm ảnh theo nội dung, chủ đề, hành động hoặc đối tượng
- Hỗ trợ đa nền tảng: dễ dàng đồng bộ và truy cập mọi lúc, mọi nơi
- Giao diện thân thiện, tối ưu cho người dùng

Hãy để AI giúp bạn quản lý ký ức và kỷ niệm một cách thông minh hơn. Tải về ngay hôm nay để trải nghiệm công nghệ nhận dạng hình ảnh đột phá, biến thư viện ảnh của bạn thành kho dữ liệu sống động và dễ khai thác.

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
  "content": "a city is shown in this aerial photo."
}
```

```python
modelImg2Txt = "microsoft/git-base-coco"

processorImg2Txt = AutoProcessor.from_pretrained(
    modelImg2Txt, cache_dir="cache")
modelImg2Txt = AutoModelForCausalLM.from_pretrained(
    modelImg2Txt, cache_dir="cache")

pixel_values = processorImg2Txt(
    images=image, return_tensors="pt").pixel_values
generated_ids = modelImg2Txt.generate(
    pixel_values=pixel_values, max_length=50)
generated_caption = processorImg2Txt.batch_decode(
    generated_ids, skip_special_tokens=True)[0]
```

```shell
cd Python
pyinstaller --onefile app.py
```

# Run

Tuý thuộc vào môi trường cần test, hãy chạy lệnh tương ứng (android, iOS, macos và windows).

```shell
dotnet build -t:Run -f net9.0-windows10.0.19041.0
dotnet build -t:Run -f net9.0-android
dotnet build -t:Run -f net9.0-maccatalyst
dotnet build -t:Run -f net9.0-ios
```

# Publish

## Thiết lập môi trường cho iOS

Đối với iOS cần download simc.mobileprovision và cài đặt trước khi thực hiện publish.
- Truy cập trang: https://developer.apple.com/account/resources/profiles/list
- Tìm và download simc
- Double click để cài đặt bằng xCode hoặc copy file vừa download được vào thư mục "~/Library/MobileDevice/Provisioning Profiles/"

## Publish CLI

Tuỳ thuộc vào môi trường cần publish, hãy chạy lệnh tương ứng (android, iOS, macos và windows).

### Đối với android

Cần thực hiện ký cho ứng dụng để đưa lên store.

Tạo keystore

```shell
keytool -genkeypair -v -keystore simc.keystore -alias simc -keyalg RSA -keysize 2048 -validity 10000
```

```shell
sh distribute_android.sh
```

### Đối với iOS

Cần thực hiện thêm việc upload ipa lên Apple Developer

```shell
sh distribute_ios.sh
```

Mở project trên xCode

```shell
dotnet tool install --global dotnet-xcsync --prerelease
```

```shell
xcsync generate --project ./application.csproj --target-framework-moniker net9.0-ios -f


```

### Windows và MACOS

```shell
dotnet publish -c Release -f net9.0-windows10.0.19041.0 -o ./publish/windows
dotnet publish -c Release -f net9.0-maccatalyst -o ./publish/macos
```

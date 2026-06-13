# Sinema Bilet Rezervasyon Sistemi - Geliştirme Notları

Bu dosya, projede şimdiye kadar yapılan işleri adım adım takip etmek için tutuluyor. Yalnızca tamamlanan kısımlar yazılır ve yeni bir adım bitince burası güncellenir.

## 1. Proje ve Klasör Yapısı
- Solution dosyası oluşturuldu: `CinemaReservation.sln`
- Servisler ayrı kök klasörlere ayrıldı:
  - `src/Catalog`
  - `src/Reservation`
- Her servis için basit katman yapısı kuruldu:
  - `Api`
  - `Application`
  - `Domain`
  - `Infrastructure`
- Docker altyapısı hazırlandı:
  - Catalog PostgreSQL
  - Reservation PostgreSQL
  - RabbitMQ
- Kalıcı veri için repo içinde volume klasörleri açıldı:
  - `docker/volumes/catalog-db`
  - `docker/volumes/reservation-db`
  - `docker/volumes/rabbitmq`

## 2. Catalog Servisi
- `Movie`, `Showtime`, `Ticket` entity'leri oluşturuldu
- `MovieGenre`, `MovieStatus`, `TicketStatus` enum'ları eklendi
- EF Core `DbContext` yazıldı
- Fluent API konfigürasyonları eklendi:
  - `Movies`
  - `Showtimes`
  - `Tickets`
- CQRS yapısı sadece Catalog servisinde kuruldu
- `MediatR` ile `CreateTicketCommand` ve handler yazıldı
- Aynı seans ve koltuk için ikinci ticket satışını engelleyen kontrol eklendi
- Ticket oluşturma endpoint’i `POST /api/tickets` olarak bağlandı
- `MoviesController` ile ilk controller tabanlı CRUD yapısı kuruldu
- `Movies` için `create`, `update`, `delete`, `list`, `get by id` endpointleri eklendi
- `Movies` request DTO'ları controller altındaki `Dtos/Requests` klasörüne taşındı
- Body validation için `[ApiController]` kullanıldı
- `UpdateMovieCommand`, `DeleteMovieCommand` ve query modelleri için MediatR validation pipeline eklendi
- Validation hataları API seviyesinde 400 olarak döndürülüyor
- Validation response içinde alan adı ve hata kodu birlikte dönüyor
- Örnek hata kodları: `ERR_REQUIRED_TITLE`, `ERR_DURATION_RANGE`, `ERR_REQUIRED_ID`
- `Program.cs` controller tabanlı hale getirildi ve Swagger üzerinden test edilebilir yapı kuruldu
- `ShowtimesController` eklendi ve `Showtime` CRUD endpointleri tamamlandı
- `TicketsController` eklendi ve `create`, `list`, `get by id`, `cancel` endpointleri tamamlandı
- `Tickets` cancel akışı hard delete olarak çalışıyor
- `TicketCancelledEvent` ile ticket iptali sonrası Reservation tarafında koltuk serbest bırakma akışı eklendi

## 3. RabbitMQ Ortak Yapısı
- Ortak mesaj kontratları için `src/Common/CinemaReservation.Common.Messaging` projesi açıldı
- `TicketPurchasedEvent` mesajı tanımlandı
- RabbitMQ bağlantısı için ortak extension yazıldı
- Catalog servisi ticket oluşturduktan sonra `TicketPurchasedEvent` publish ediyor
- Reservation servisi bu mesajı tüketiyor

## 4. Reservation Servisi
- `Hall` ve `Seat` entity'leri oluşturuldu
- `ReservationDbContext` yazıldı
- Fluent API konfigürasyonları eklendi:
  - `Halls`
  - `Seats`
- Koltuk modeli seans bazlı kuruldu:
  - aynı koltuk numarası farklı `ShowtimeId` için ayrı kayıt taşıyor
- `TicketPurchasedConsumer` eklendi
- Event gelince ilgili koltuk `IsReserved = true` yapılıyor

## 5. Konfigürasyon
- `Catalog` ve `Reservation` API'lerine PostgreSQL connection string'leri eklendi
- RabbitMQ ayarları `appsettings.json` içine yazıldı
- `.env` dosyası eklendi
- `docker-compose.yml` bind mount kullanacak şekilde güncellendi

## 6. Migration Durumu
- Catalog için ilk migration oluşturuldu
- Database update çalıştırıldı
- Reservation tarafı henüz sonraki adımda tamamlanacak

## 7. Sonraki Adımlar
- Reservation servisini tamamlamak
- RabbitMQ akışını manuel test etmek
- README eklemek

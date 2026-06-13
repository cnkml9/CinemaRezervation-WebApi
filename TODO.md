# Sinema Bilet Rezervasyon Sistemi - TODO Listesi

## 1. Proje ve Klasör Yapısı
- [x] Solution dosyasını düzenle
- [x] `Catalog` ve `Reservation` klasörlerini ayrı kökler altında topla
- [x] Her servis için basit ve anlaşılır katman yapısını kur
- [x] Ortak paket ve isimlendirme standardını netleştir
- [x] `docker-compose.yml` ile PostgreSQL ve RabbitMQ altyapısını hazır tut

## 2. Catalog Servisi
- [x] `Movie`, `Showtime`, `Ticket` entity'lerini oluştur
- [x] `MovieStatus` ve `TicketStatus` enum'larını belirle
- [x] EF Core `DbContext` ve Fluent API ayarlarını yaz
- [x] CQRS yapısını sadece bu serviste kullan
- [x] `MediatR` ile ticket satın alma komutunu oluştur
- [x] Ticket kaydından önce koltuk boş mu kontrol akışını ekle
- [x] Uygun ise `TicketPurchasedEvent` mesajını RabbitMQ'ya gönder
- [x] `Controllers` klasörü altında `MoviesController` oluştur
- [x] `Movies` için `create`, `update`, `delete`, `list`, `get by id` endpointlerini kur
- [x] `Showtimes` için `create`, `update`, `delete`, `list`, `get by id` endpointlerini kur
- [x] `Tickets` için `create`, `cancel`, `list`, `get by id` endpointlerini kur
- [x] CQRS’e uygun request/response DTO’larını ayrı klasörlerde organize et
- [x] `MediatR` command/query handler yapısını controller endpointleri ile bağla
- [x] Swagger üzerinden kolay test için endpoint isimleri ve route yapısını netleştir
- [x] Validation kurallarını ekle
- [x] Hata mesajlarını okunabilir ve kısa tut
- [x] Gerekli durumlarda `create` ve `update` için farklı command modelleri kullan
- [x] `delete` işlemlerinde hard delete kullan

## 3. Reservation Servisi
- [x] `Hall` ve `Seat` entity'lerini oluştur
- [x] EF Core `DbContext` ve Fluent API ayarlarını yaz
- [x] Reservation servisinde CQRS kullanma, sade servis yapısını koru
- [x] Koltukları seans bazlı modelle, aynı koltuk numarası farklı seanslarda ayrı durum taşısın
- [x] RabbitMQ consumer ile gelen ticket satın alma bilgisini işle
- [x] İlgili koltuğu `IsReserved = true` olarak güncelle
- [x] Koltuk ve salon ilişkisini doğru kur
- [ ] `Controllers` klasörü altında `HallsController` ve `SeatsController` oluştur
- [ ] `Halls` için `create`, `update`, `delete`, `list`, `get by id` endpointlerini planla
- [ ] `Seats` için `create`, `update`, `delete`, `list`, `get by id` endpointlerini planla
- [ ] Seans bazlı koltuk durumunu gösterecek `list by showtime` endpointini planla
- [ ] Reservation tarafında Swagger’dan rahat test edilecek route yapısını netleştir
- [ ] Reservation tarafı için validation kurallarını ekle
- [ ] Ticket satışı sonrası koltuk durumunu güncelleyen akışı endpointlerden bağımsız çalışacak şekilde koru

## 4. Veritabanı ve Migration
- [ ] İki servis için ayrı PostgreSQL bağlantılarını yapılandır
- [x] İlk migration dosyalarını üret
- [x] Database update komutlarını hazırla
- [ ] Docker volume ile verilerin kalıcı olmasını doğrula
- [ ] Gerekliyse Catalog tarafında `MovieStatus`, `TicketStatus` için seed/veri kuralı belirle
- [ ] Reservation tarafında koltukların seans bazlı tutulması için ek alan gereksinimi var mı kontrol et

## 5. API ve Çalıştırma
- [ ] İki servisi de ayağa kaldır
- [ ] Ticket satın alma akışını uçtan uca manuel test et
- [ ] RabbitMQ consumer akışını doğrula
- [ ] README içine çalıştırma adımlarını ekle
- [ ] Ödev sunumu için kısa akış notları hazırla
- [ ] Swagger üzerinden tüm CRUD ve ticket akışlarını test et
- [ ] Endpoint cevaplarını sade ve anlaşılır hale getir
- [ ] Silme işlemlerinde hard delete kullanma kararını uygula

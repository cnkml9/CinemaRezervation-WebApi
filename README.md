# Cinema Reservation API - Kısa Dokümantasyon

Bu proje iki servisten oluşur:

- `Catalog Service`: Film, seans ve bilet işlemlerini yönetir.
- `Reservation Service`: Salonları ve seans bazlı koltuk durumlarını yönetir.

Servisler birbirini direkt database üzerinden kullanmaz. Aralarındaki temel senkronizasyon RabbitMQ eventleri ile yapılır.

## Temel Mantık

Salon ve koltuk tarafının sahibi `Reservation Service`tir.

Film, seans ve bilet tarafının sahibi `Catalog Service`tir.

Koltuklar salon oluştururken otomatik oluşmaz. Salon sadece şu bilgileri tutar:

- salon adı
- toplam koltuk sayısı

Koltuklar, Catalog servisinde bir showtime oluşturulunca otomatik oluşur.

Örnek:

1. Reservation servisinde `Salon 1` oluşturulur. `totalSeats = 30`
2. Catalog servisinde bir film oluşturulur.
3. Catalog servisinde bu film için showtime oluşturulur ve `hallId = 1` verilir.
4. Catalog, RabbitMQ'ya `ShowtimeCreatedEvent` gönderir.
5. Reservation bu eventi yakalar.
6. Reservation, `hallId = 1` olan salonu bulur.
7. Salonun `totalSeats` değerine göre o showtime için koltukları üretir.

Koltuk numaraları şu formatta oluşur:

```text
A1, A2, A3, ... A10, B1, B2 ...
```

## RabbitMQ Ne İçin Kullanılıyor?

RabbitMQ üç durumda kullanılıyor.

### 1. Showtime Oluşturuldu

Catalog showtime oluşturunca şu event yayınlanır:

```text
ShowtimeCreatedEvent
```

Reservation bu eventi yakalar ve ilgili showtime için koltukları oluşturur.

### 2. Ticket Satın Alındı

Catalog ticket oluşturunca şu event yayınlanır:

```text
TicketPurchasedEvent
```

Reservation bu eventi yakalar ve ilgili koltuğu dolu yapar:

```text
isReserved = true
```

### 3. Ticket İptal Edildi

Catalog ticket iptal edince şu event yayınlanır:

```text
TicketCancelledEvent
```

Reservation bu eventi yakalar ve ilgili koltuğu boş yapar:

```text
isReserved = false
```

## Catalog Service Endpointleri

Base URL:

```text
http://localhost:5000
```

### Movies

#### GET /api/movies

Tüm filmleri listeler.

#### GET /api/movies/{id}

Tek filmi getirir.

Film yoksa:

```text
404 Not Found
```

#### POST /api/movies

Film oluşturur.

Örnek body:

```json
{
  "title": "Inception",
  "duration": 148,
  "genre": 1,
  "status": 1
}
```

Eksik veya hatalı alan varsa:

```text
400 Bad Request
```

#### PUT /api/movies/{id}

Film günceller.

Film yoksa:

```text
404 Not Found
```

#### DELETE /api/movies/{id}

Film siler.

Film yoksa:

```text
404 Not Found
```

### Showtimes

#### GET /api/showtimes

Tüm seansları listeler.

Response içinde `hallId` de gelir. Bu değer Reservation servisindeki salon id değeridir.

#### GET /api/showtimes/{id}

Tek seansı getirir.

Seans yoksa:

```text
404 Not Found
```

#### POST /api/showtimes

Seans oluşturur.

Örnek body:

```json
{
  "movieId": 1,
  "hallId": 1,
  "time": "2026-06-15T20:00:00Z",
  "price": 250
}
```

Bu endpoint sadece Catalog database'e showtime eklemez. Aynı zamanda RabbitMQ üzerinden `ShowtimeCreatedEvent` yayınlar.

Reservation bu eventi yakalayınca ilgili showtime için koltukları oluşturur.

Dikkat:

- `movieId` Catalog servisindeki film id değeridir.
- `hallId` Reservation servisindeki salon id değeridir.
- Salon oluştururken koltuklar oluşmaz.
- Koltuklar showtime oluşturulunca oluşur.

Olası cevaplar:

```text
201 Created
400 Bad Request
404 Not Found
409 Conflict
```

`404`: Movie bulunamadı.

`409`: Aynı salon ve saat için başka showtime vardır veya unique kural çakışmıştır.

#### PUT /api/showtimes/{id}

Seans günceller.

Not:

- Bu işlem yeniden koltuk üretmez.
- Koltuk üretimi sadece showtime create akışında yapılır.

#### DELETE /api/showtimes/{id}

Seans siler.

### Tickets

#### GET /api/tickets

Tüm ticket kayıtlarını listeler.

#### GET /api/tickets/{id}

Tek ticket kaydını getirir.

Ticket yoksa:

```text
404 Not Found
```

#### POST /api/tickets

Ticket satın alır.

Örnek body:

```json
{
  "showtimeId": 1,
  "userId": 10,
  "seatNumber": "t8"
}
```

Bu endpoint artık Reservation servisine bakar:

- Bu showtime için koltuklar oluşturulmuş mu?
- Verilen koltuk gerçekten var mı?
- Koltuk dolu mu?

Koltuk numarası büyük/küçük harf duyarlı değildir:

```text
t8 = T8
```

Kayıt yapılırken koltuk numarası normalize edilir:

```text
t8 -> T8
```

Olası cevaplar:

```text
201 Created
400 Bad Request
404 Not Found
409 Conflict
503 Service Unavailable
```

`404 Showtime`: Catalog tarafında showtime yok.

`404 Showtime seats`: Reservation tarafında bu showtime için koltuklar oluşmamış.

`404 Seat`: Girilen koltuk numarası yok.

`409 Conflict`: Koltuk zaten dolu veya daha önce satın alınmış.

`503 Service Unavailable`: Reservation servisine ulaşılamıyor.

Ticket başarıyla oluşunca Catalog RabbitMQ'ya `TicketPurchasedEvent` yayınlar. Reservation bu eventi yakalar ve koltuğu dolu yapar.

#### POST /api/tickets/{id}/cancel

Ticket iptal eder.

Başarılı olursa:

```text
204 No Content
```

Ticket yoksa:

```text
404 Not Found
```

İptal sonrası Catalog RabbitMQ'ya `TicketCancelledEvent` yayınlar. Reservation bu eventi yakalar ve koltuğu tekrar boş yapar.

## Reservation Service Endpointleri

Base URL:

```text
http://localhost:5001
```

### Halls

#### GET /api/halls

Tüm salonları listeler.

#### GET /api/halls/{id}

Tek salonu getirir.

Salon yoksa:

```text
404 Not Found
```

#### POST /api/halls

Salon oluşturur.

Örnek body:

```json
{
  "name": "Salon 1",
  "totalSeats": 30
}
```

Bu endpoint koltuk oluşturmaz.

Sadece salon bilgisini kaydeder. Koltuklar, Catalog tarafında showtime oluşturulduktan sonra RabbitMQ eventi ile otomatik oluşur.

#### PUT /api/halls/{id}

Salon bilgisini günceller.

Not:

- Daha önce oluşmuş showtime koltukları geriye dönük değişmez.
- Yeni `totalSeats` değeri bundan sonra oluşturulacak showtime'ları etkiler.

#### DELETE /api/halls/{id}

Salon siler.

Salon yoksa:

```text
404 Not Found
```

### Seats

#### GET /api/seats/showtime/{showtimeId}

Bir showtime için koltukları listeler.

Örnek response:

```json
[
  {
    "id": 1,
    "hallId": 1,
    "showtimeId": 1,
    "seatNo": "A1",
    "isReserved": false
  },
  {
    "id": 2,
    "hallId": 1,
    "showtimeId": 1,
    "seatNo": "A2",
    "isReserved": true
  }
]
```

Bu endpoint sadece okuma içindir.

Seat create/update/delete endpointleri yoktur. Çünkü koltukların yaşam döngüsü RabbitMQ eventleriyle yönetilir.

Bu showtime için koltuk yoksa:

```text
404 Not Found
```

## Örnek İşlem Senaryosu

### 1. Salon oluştur

Reservation:

```http
POST http://localhost:5001/api/halls
```

```json
{
  "name": "Salon 1",
  "totalSeats": 30
}
```

Bu aşamada koltuk oluşmaz.

### 2. Film oluştur

Catalog:

```http
POST http://localhost:5000/api/movies
```

```json
{
  "title": "Inception",
  "duration": 148,
  "genre": 1,
  "status": 1
}
```

### 3. Showtime oluştur

Catalog:

```http
POST http://localhost:5000/api/showtimes
```

```json
{
  "movieId": 1,
  "hallId": 1,
  "time": "2026-06-15T20:00:00Z",
  "price": 250
}
```

Bu işlemden sonra RabbitMQ ile Reservation'a event gider ve koltuklar oluşur.

### 4. Koltukları kontrol et

Reservation:

```http
GET http://localhost:5001/api/seats/showtime/1
```

Burada `A1`, `A2`, `A3` gibi koltukları görürsün.

### 5. Ticket satın al

Catalog:

```http
POST http://localhost:5000/api/tickets
```

```json
{
  "showtimeId": 1,
  "userId": 10,
  "seatNumber": "a1"
}
```

`a1` otomatik `A1` olarak değerlendirilir.

Başarılı olursa Reservation tarafında `A1` dolu olur.

### 6. Ticket iptal et

Catalog:

```http
POST http://localhost:5000/api/tickets/1/cancel
```

Başarılı olursa Reservation tarafında `A1` tekrar boş olur.

## Catalog Servisinde CQRS Nasıl Kullanılıyor?

Catalog servisinde controller'lar işi direkt database ile yapmaz.

Controller sadece request alır ve MediatR ile command/query gönderir.

Örnek:

- Film oluşturma: `CreateMovieCommand`
- Film güncelleme: `UpdateMovieCommand`
- Film listeleme: `GetMoviesQuery`
- Showtime oluşturma: `CreateShowtimeCommand`
- Ticket oluşturma: `CreateTicketCommand`

Command tarafı yazma işlemlerini yapar:

- create
- update
- delete
- ticket satın alma
- ticket iptal

Query tarafı okuma işlemlerini yapar:

- listeleme
- id ile getirme

Bu sayede Catalog servisinde okuma ve yazma akışları ayrılmış olur.

Reservation servisinde CQRS kullanılmıyor. Orası daha basit tutuldu; controller doğrudan `ReservationDbContext` ile çalışıyor.

## Kısa Özet

- Salon oluşturmak koltuk oluşturmaz.
- Showtime oluşturmak koltukları oluşturur.
- Ticket almak koltuğu doldurur.
- Ticket iptal etmek koltuğu boşaltır.
- RabbitMQ servisler arası senkronizasyon için kullanılır.
- Catalog servisinde CQRS vardır.
- Reservation servisi daha basit CRUD + event consumer yapısındadır.

# Otel Rehberi Uygulaması
Bu README dokümanı, Otel Rehberi uygulamasının kurulumu, çalıştırılması ve geliştirilmesi hakkında temel bilgileri içermektedir.
## Proje Konusu
Bu proje, basit bir otel rehberi uygulamasının geliştirilmesini kapsamaktadır. Uygulama, minimum iki mikroservis arasında haberleşen bir yapıda tasarlanmıştır ve kullanıcıların otel oluşturma, kaldırma, iletişim bilgisi ekleme ve kaldırma gibi temel işlevleri gerçekleştirmelerini sağlar. Ayrıca, otellerin yetkililerinin listelenmesi, detaylı bilgilerin getirilmesi ve konuma göre istatistik raporlarının oluşturulması gibi ek işlevler de sunar.
## Kullanılan Teknolojiler
- **Backend Framework:** .NET Core <br/>
- **Veritabanı:** PostgreSQL <br/>
- **Mesaj Kuyruğu:** RabbitMQ <br/>
- **Unit Test Framework:** NUnit <br/>
- **API Protokolleri:** REST <br/>
- **Diğer:** Git, Swagger <br/>
## Kurulum
**1. PostgreSQL Veritabanı Kurulumu:** İlk adım olarak, PostgreSQL veritabanınızı kurmanız gerekmektedir. Veritabanı adı, kullanıcı adı ve şifre gibi bilgileri not almayı unutmayınız. <br/>
**2. RabbitMQ Kurulumu:** Mesaj kuyruğu işlevselliğini sağlamak için RabbitMQ'yu kurmanız gerekmektedir. RabbitMQ'nun adresini ve erişim bilgilerini not almayı unutmayınız. <br/>
**3. Git Repository Klonlama:** Proje dosyalarını bilgisayarınıza klonlayınız. <br/>
**4. Proje Bağımlılıklarının Yüklenmesi:** Proje bağımlılıklarını yüklemek için 'Manage NuGet Packages' aracını kullanabilirsiniz <br/>
**5. Veritabanı Migration'ı:** Entity Framework Core kullanarak veritabanını oluşturmak için migration'ları uygulayınız. <br/>

## API Dökümantasyonu
Uygulamanın sunduğu API'leri aşağıda bulabilirsiniz. API'leri Swagger veya Postman gibi araçlarla test edebilirsiniz.

## Yardımcı Araçlar ve Kütüphaneler
- **.NET Core:** Backend geliştirme için kullanılmıştır.
- **Entity Framework Core:** Veritabanı işlemleri için kullanılmıştır.
- **RabbitMQ:** Mesaj kuyruğu işlevselliğini sağlamak için kullanılmıştır.

 ## API Endpointleri

 | HTTP Metodu  | Yol | Açıklama | 
| ------------- | ------------- |------------- |
| POST  | /hotel/addhotel | Yeni bir otel oluşturur  |
| DELETE  | /hotel/{id}  | Belirli bir oteli kaldırır  |
| POST  | /hotel/addcontact  | Bir otele iletişim bilgisi ekler |
|DELETE  | /hotel/removecontact/{id}  | Bir otelin iletişim bilgisini kaldırır  |
| POST  | /hotel/addresponsibility  | Bir otele yetkili ekler |
| DELETE  | /hotel/removeresponsibility/{id}  | Bir oteldeki yetkili bilgisini kaldırır |
| GET  | /hotel/getresponsibilities  | Tüm yetkilileri listeler |
| GET  | /hotel/getreports  | Otelle ilgili rapor bilgilerini getirir |

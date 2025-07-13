---

# Booking.com API Entegrasyonu

---

## 🏨 Proje Tanımı

Bu proje, RapidAPI üzerinde bulunan **Booking.com API**'si kullanılarak geliştirilmiş bir otel arama ve listeleme uygulamasıdır. Kullanıcıların belirlediği kriterlere uygun otelleri API üzerinden çekip web sitesinde listeleme işlemi gerçekleştirilmiştir.

---

## 🔍 Anasayfa İşlevselliği

Anasayfa, kullanıcıların otel arama kriterlerini girmesi için tasarlanmıştır. Kullanıcılardan aşağıdaki bilgiler alınır:

* 🗺️ **Filtrelemek İstedikleri Şehir Bilgisi**
* 📅 **Giriş ve Çıkış Tarihleri**
* 👤 **Yetişkin Kişi Sayısı**
* 🧒 **Çocuk Sayısı**

---

## ✔️ Sonuç Sayfası & Detay Görüntüleme

Girilen kriterlere uyan oteller liste halinde gösterilir. Kullanıcılar, listelenen otellerin temel bilgilerini görebilir ve daha detaylı bilgi almak için otelin üzerine tıklayabilirler.

* 🏠 **Otel Adı**
* ⭐ **Otel Puanı**
* 👥 **Kaç Kişi Tarafından Değerlendirme Yapıldığı**
* 💰 **Fiyat Bilgisi**
* ℹ️ **Adres, Şehir, Ülke**
* 🖼️ **Otele Ait Fotoğraflar**
* 🛁 **Otel Olanakları (Amenities)**

---

## 🛠️ Kullanılan Teknolojiler

* **💻 ASP.NET Core (8.0):** Projenin temel geliştirme çatısıdır.
* **🏗️ Tek Katmanlı Mimari:** Uygulama, hızlı geliştirme ve basitlik adına tek katmanlı bir yapıda tasarlanmıştır.
* **🌐 Booking.com API (RapidAPI Üzerinden)::** Temel otel verilerini çekmek için kullanılan ana API kaynağıdır. Projede Booking.com API'nin üç farklı endpoint'i etkin bir şekilde kullanılmıştır:
    * **🔍 Auto Complete Endpoint:** Kullanıcının girdiği şehir bilgisine karşılık gelen `City ID` ve `Destination ID` gibi konum verileri elde edildi.
    * **🏨 Search Hotels Endpoint:** Elde edilen `Destination ID` ve diğer filtreleme kriterleri (giriş/çıkış tarihleri, yetişkin/çocuk sayısı) kullanılarak otellerin listesi çekildi.
    * **🖼️ Get Hotel Photos Endpoint:** Listelenen otellerin `Hotel ID`'leri kullanılarak ilgili otellere ait fotoğraflar elde edildi.
    * **ℹ️ Get Hotel Details Endpoint:** Seçilen bir otelin tüm detaylı bilgilerini (açıklama, fiyat dökümü, daha fazla fotoğraf, olanaklar, inceleme skorları vb.) çekmek için kullanıldı.
* **📦 ViewModel Yapıları:** API'den gelen karmaşık JSON verilerini uygulamanın ihtiyaçlarına göre karşılamak, yönetmek ve View katmanına düzenli bir şekilde aktarmak için özel ViewModel sınıfları oluşturulmuştur.
* **HttpClient:** API isteklerini güvenli ve etkin bir şekilde göndermek ve yanıtları almak için kullanılmıştır.
* **Newtonsoft.Json:** API'den gelen JSON formatındaki verilerin C# nesnelerine dönüştürülmesi (deserialization) ve C# nesnelerinin JSON'a dönüştürülmesi (serialization) işlemleri için kullanılmıştır.

---

## ⚙️ Proje Detayları ve İş Akışı

1.  **Şehir ID'si ve Destination ID'si Elde Etme:**
    * API entegrasyonunun ilk adımı olarak, kullanıcının girdiği şehir bilgisi `Auto Complete Endpoint`'ine parametre olarak gönderilir. Bu istekten dönen yanıttan, ilgili şehrin benzersiz `City ID`'si ve otel aramalarında kullanılacak `Destination ID`'si alınır.
2.  **Otel Listesi Çekme:**
    * Elde edilen `Destination ID` ve kullanıcının belirttiği giriş/çıkış tarihleri, yetişkin/çocuk sayıları gibi diğer filtreleme kriterleri, `Search Hotels Endpoint`'ine gönderilir. Bu sayede, kriterlere uygun otellerin bir listesi (otel ID'leri, isimleri, puanları vb. içeren) alınır.
3.  **Otel Detayları ve Fotoğrafları Çekme:**
    * Otel listesi alındıktan sonra, kullanıcı bir otele tıkladığında, o otelin `Hotel ID`'si kullanılarak hem `Get Hotel Details Endpoint`'ine hem de `Get Hotel Photos Endpoint`'ine istek gönderilir.
    * `Get Hotel Details Endpoint`'inden otelin açıklaması, oda detayları, detaylı fiyat bilgileri, tüm olanakları gibi kapsamlı bilgiler çekilir.
    * `Get Hotel Photos Endpoint`'inden ise otelin yüksek çözünürlüklü fotoğrafları alınır.
4.  **Veri Yönetimi:**
    * API'lerden gelen tüm JSON veri yapıları, önceden tanımlanmış özel **ViewModel** sınıfları aracılığıyla C# nesnelerine dönüştürülerek projenin farklı katmanlarında kolayca kullanılabilir hale getirilmiştir. Bu yaklaşım, veri bütünlüğünü sağlar ve kod okunabilirliğini artırır.
  
## 🖼️ Proje Görselleri
<img width="1911" height="953" alt="Home" src="https://github.com/user-attachments/assets/807260f5-e19c-459e-8661-69fb25717ad5" />
<img width="1914" height="956" alt="reservation" src="https://github.com/user-attachments/assets/424af90a-e2ba-4629-acf5-dcebed1383ff" />
<img width="1917" height="953" alt="HotelList" src="https://github.com/user-attachments/assets/0c98597e-0571-4317-aaab-8cbc97c1795e" />
<img width="1915" height="955" alt="HotelList2" src="https://github.com/user-attachments/assets/f5102586-205a-4b9f-bf69-98b3bd4676a6" />
<img width="1908" height="952" alt="HotelDetail" src="https://github.com/user-attachments/assets/466217dd-f7f9-4078-b3bd-44577534b897" />
<img width="1915" height="952" alt="HotelDetail2" src="https://github.com/user-attachments/assets/173a58c9-7d5a-4884-b8c9-0ed86a5ff69d" />

## Gerekli Araçlar

- **.NET SDK**: 8.0 
- **SQL Server**: 2019 
- **Visual Studio**: 2022 

## Kurulum Adımları

1. **Bağlantı Dizesini Güncelleyin**:
   - Projenin `appsettings.json` dosyasını açın ve `ConnectionStrings` bölümündeki `DefaultConnection` ayarını kendi SQL Server ayarlarınıza göre güncellemeniz gerekecek:
     
     "ConnectionStrings": {
       "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=MyAppDB;User Id=YOUR_USER_ID;Password=YOUR_PASSWORD;"
     }
     

2. **Migration'ı Uygulama**:
   - Visual Studio'da "Package Manager Console" penceresini açın:
     - `Tools` menüsünden `NuGet Package Manager` ve ardından `Package Manager Console` seçeneğine tıklayın.
   - Aşağıdaki komutu çalıştırarak mevcut migration'ı uygulayın:
             Update-Database
     
  3. **Projeyi Çalıştırın**:
     
## API Erişimi
- Projeyi başlattıktan sonra Swagger arayüzüne erişecek proje.  Buradan API endpointlerini test edebilirsiniz.
     
     

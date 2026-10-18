# OptiRisk AI — İklim/Yangın Risk Skorlama (PoC)

OptiRisk AI; bir koordinatın orman yangını riskini **coğrafi bilgi sistemi (GIS)**, **yerel makine öğrenmesi (ML.NET)** ve **yapay zeka ajanı (Semantic Kernel)** birlikte kullanarak değerlendiren uçtan uca bir kavram kanıtı (PoC) projesidir. Amaç; deterministik mekansal hesaplamayı, canlı meteoroloji verisini ve LLM'in doğal dil yeteneğini tek bir akışta, **düşük/sıfır dış maliyetle** (yerel LLM) birleştirmek.

<img width="1610" alt="dashboard" src="https://github.com/user-attachments/assets/f5f0040b-dd1c-46d7-b773-72f68a083799" />

## Neden Geliştirdim

OptiRisk'i Teknopark İstanbul'un HASAT programı için yaptım. Çıkış noktam şu: 2026'da zorunlu afet sigortası (DASK'ın yerine gelen ZAS) ilk kez orman yangınını ve köy/kırsal yerleşimi de kapsayacak. Ama Türkiye'de "şu evin yanma riski ne kadar" diye ev bazında hesaplayan bir sistem yok; mevcut haritalar ilçe bazlı ve statik.

Ben de hava durumu (sıcaklık, nem, rüzgar), arazi eğimi ve bitki örtüsü verisini kullanarak bir koordinatın yangın riskini skorlayan çalışan bir prototip yazdım. Fikri, Kaliforniya'daki **Kettle** adlı iklim-sigorta girişiminden esinlendim; onların uydu verisi + yapay zekayla yangın riskini fiyatlama yaklaşımından. 20 günde hem bu konuyu araştırdım hem de prototipi geliştirdim.

> Bu bir demo/prototip;  Amacım, fikrin işe yarayıp yaramadığını kendi başıma uçtan uca gösterebilmekti. (Teknik olarak yaptığım: ML.NET ile bir yangın risk skoru + harita üstünde bölge filtresi + sonucu açıklayan bir yapay zeka asistanı.)

## Akış Nasıl Çalışıyor?

1. **GIS filtresi (fail-fast):** Seçilen nokta, NetTopologySuite ile `GeoData` klasöründeki GeoJSON orman poligonlarıyla eşleştirilir. Orman dışıysa anında reddedilir; gereksiz API/ML çağrısı yapılmaz.
2. **Canlı meteoroloji:** Orman içi noktalar için **Open-Meteo** API'den anlık sıcaklık, nem, rüzgar, yağış çekilir (koordinatlarda `InvariantCulture` ile locale hatası önlenir; API düşerse fallback veri ile akış çökmez).
3. **ML.NET skoru:** Canlı iklim verisi, ML.NET Model Builder ile **Algerian Forest Fires** veri setinde eğitilmiş modele verilerek bir FWI (Fire Weather Index) risk skoru üretir.
4. **AI ajanı (zırhlı karar):** Kararı **LLM değil, C# eşikleri verir** (örn. skor > 80 → `BLOCK_POLICY`). Semantic Kernel üzerinden yerel LLM (Ollama) yalnızca ilgili departmanın okuyacağı **gerekçe cümlesini** yazar. Böylece iş kararı LLM halüsinasyonuna bırakılmaz.
5. **Arka plan simülasyonu:** Bir background worker periyodik olarak örnek grid noktalarını tarar; sonuçlar web arayüzünde canlı akışta gösterilir.

## Teknoloji Yığını

- **Altyapı:** .NET 10, ABP Framework (katmanlı DDD), SQLite + EF Core
- **GIS:** NetTopologySuite (GeoJSON point-in-polygon)
- **ML:** ML.NET (FWI regresyon modeli)
- **AI:** Microsoft Semantic Kernel + Ollama (yerel LLM)
- **Dış servis:** Open-Meteo
- **Frontend:** ASP.NET Core / Razor, Leaflet.js, asenkron polling

## Mimari Not

Domain katmanında gerçek DDD uygulandı: entity'ler aggregate root, private setter ve davranışsal metotlarla tasarlandı. Proje ABP starter şablonu üzerine kurulduğu için şablondan gelen örnek (Author/Book) parçaları ve bazı MVP kısayolları (ör. listelemede tüm kaydı belleğe çekme) hâlâ mevcut — aşağıdaki yol haritasında temizleniyor.



## Çalıştırma

1. `dotnet restore`
2. `OptiRiskModel.mlnet` dosyasında `Copy to Output Directory → Copy if newer` ayarla.
3. `dotnet ef database update`
4. Ollama'yı çalıştır.
5. `dotnet run --project src/OptiRiskAI.Web`

---
*Geliştirici: Ömer Faruk Yazıcı · Bir öğrenme/PoC projesidir.*



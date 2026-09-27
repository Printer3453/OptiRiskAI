# OptiRisk AI - MVP

OptiRisk AI, iklim ve orman yangını risklerini coğrafi bilgi sistemleri (GIS), yerel makine öğrenmesi (ML.NET) ve otonom yapay zeka (Agentic Workflow) kullanarak analiz eden uçtan uca bir B2B SaaS Kavram Kanıtı (PoC) projesidir.

Projenin temel mimari amacı; deterministik mekansal hesaplamaları, canlı meteoroloji telemetrisini ve Büyük Dil Modellerinin (LLM) karar alma süreçlerini **Domain-Driven Design (DDD)** ve **Clean Architecture** prensiplerine uygun olarak, sıfır dış bağımlılık (sıfır API maliyeti) ile izole bir şekilde çalıştırabilmektir.

##  Proje Ne Yapıyor?

* **GIS Tabanlı Filtreleme (Fail-Fast):** Kullanıcının veya otonom sistemin seçtiği nokta, NetTopologySuite ile `GeoData` klasöründeki statik GeoJSON orman poligonlarıyla eşleştirilir. Sistem kaynaklarını korumak için, orman dışı koordinatlar anında reddedilir ve gereksiz API çağrıları engellenir.
* **Canlı Veri Boru Hattı:** Orman içi onaylanan koordinatlar için **Open-Meteo** API üzerinden limitsiz ve ücretsiz anlık meteorolojik veriler (Sıcaklık, Nem, Rüzgar, Yağış) çekilir.
* **Yerel Makine Öğrenmesi (ML.NET):** Çekilen canlı iklim verileri, sisteme gömülü yerel ML.NET modeline (LbfgsPoissonRegression) beslenerek saniyeler içinde bilimsel bir Aktüeryal Risk Skoru (FWI) üretilir.
* **Otonom Aksiyon (LLM):** Çıkan net FWI skoru ve mekansal bağlam, Semantic Kernel üzerinden yerel LLM'e (Ollama) iletilir. Ajan, risk durumuna göre otonom olarak kurumsal iş emirleri (Örn: `BLOCK_POLICY`, `LOG_ONLY`, `ALERT_TEAM`) ve departman yönlendirmeleri üretir.
* **7/24 Komuta Kontrol Merkezi:** Arka plan işçisi (Background Worker) haritayı otonom olarak tarar. Web arayüzündeki Canlı Radar (Live Feed) sayesinde, tespit edilen riskler ve ajanın aldığı kararlar Matrix tarzı bir terminal ekranında anlık olarak görselleştirilir.
<img width="1610" height="782" alt="image" src="https://github.com/user-attachments/assets/f5f0040b-dd1c-46d7-b773-72f68a083799" />

##  Teknoloji Yığını

* **Altyapı:** .NET 10.0, C#, ABP Framework (Clean Architecture)
* **GIS Motoru:** NetTopologySuite, System.Text.Json (GeoJSON Parsing)
* **Makine Öğrenmesi:** ML.NET (Aktüeryal FWI Modeli)
* **AI Entegrasyonu:** Microsoft Semantic Kernel, Ollama (Local-First LLM)
* **Dış Servisler:** Open-Meteo (Gerçek Zamanlı Hava Durumu)
* **Frontend:** ASP.NET Core MVC / Razor Pages, Bootstrap 5, Leaflet.js, Asenkron Auto-Polling
* **Veritabanı:** SQLite, Entity Framework Core

##  Hızlı Başlangıç

1. Depoyu klonlayıp bağımlılıkları yükleyin: `dotnet restore`
2. `OptiRiskAI.Application` projesi içindeki `OptiRiskModel.mlnet` dosyasının özelliklerinden `Copy to Output Directory` değerini `Copy if newer` olarak ayarlayın.
3. Veritabanı migrasyonlarını uygulayın: `dotnet ef database update`
4. Yerel LLM motoru Ollama'yı arka planda çalıştırın.
5. Web projesini ayağa kaldırın: `dotnet run --project src/OptiRiskAI.Web`

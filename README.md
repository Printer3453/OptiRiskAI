# OptiRisk AI - MVP

OptiRisk AI, iklim ve orman yangını risklerini coğrafi bilgi sistemleri (GIS) ve otonom yapay zeka (Agentic Workflow) kullanarak analiz eden bir Kavram Kanıtı (PoC) projesidir.

Projenin temel mimari amacı; deterministik mekansal hesaplamalar ile Büyük Dil Modellerinin (LLM) karar alma süreçlerini **Domain-Driven Design (DDD)** prensiplerine uygun olarak, birbirine karıştırmadan (tam izole) çalıştırabilmektir.

## 📌 Proje Ne Yapıyor?

* **Mekansal Analiz:** Kullanıcının harita üzerinden (Leaflet.js) seçtiği nokta, NetTopologySuite ile `GeoData` klasöründeki statik GeoJSON poligon verileriyle (Point-in-Polygon) eşleştirilir.
* **Deterministik Skorlama:** Yapay zeka kullanılmadan, tamamen poligon kesişimine (Contains) dayalı olarak "Ateşleme Olasılığı" ve "Yayılım Riski" skoru üretilir.
* **Otonom Aksiyon (LLM):** Çıkan kesin skor, Semantic Kernel üzerinden yerel LLM'e (Ollama) iletilir. Ajan, sistemden gelen skora göre kurumsal bir iş emri ve aksiyon kararı (Örn: `BLOCK_POLICY`, `LOG_ONLY`) üretir.

 <img width="1576" height="889" alt="Ekran görüntüsü 2026-09-16 002932" src="https://github.com/user-attachments/assets/b45f5dac-99a3-4577-8ae1-431383371a32" /><img width="1585" height="868" alt="Ekran görüntüsü 2026-09-16 002917" src="https://github.com/user-attachments/assets/d488a2e2-e5d9-45d4-8f82-3deab8640651" />

## 🛠️ Teknoloji Yığını

* **Altyapı:** .NET 10.0, C#, ABP Framework (Clean Architecture)
* **GIS Motoru:** NetTopologySuite, System.Text.Json (GeoJSON Parsing)
* **AI Entegrasyonu:** Microsoft Semantic Kernel, Ollama (Local-First)
* **Frontend:** ASP.NET Core MVC / Razor Pages, Bootstrap 5, Leaflet.js
* **Veritabanı:** SQLite, Entity Framework Core

## 🚀 Hızlı Başlangıç

1. Depoyu klonlayıp bağımlılıkları yükleyin: `dotnet restore`
2. Veritabanı migrasyonlarını uygulayın: `dotnet ef database update`
3. Ollama'yı arka planda çalıştırın.
4. Web projesini ayağa kaldırın: `dotnet run --project src/OptiRiskAI.Web`








# AR Garuda

Aplikasi AR interaktif untuk edukasi tentang Garuda Indonesia menggunakan Unity dan ARCore dengan teknologi image tracking.

## 📁 Struktur Project

```
AR-Garuda/
├── Assets/
│   ├── Scripts/          # Semua kode C# untuk AR functionality
│   ├── Prefabs/          # Marker list dan AR objects
│   ├── Scenes/           # Unity scenes
│   ├── Imported/         # Image Reference untuk track marker
│   ├── XR/               # AR/XR configurations
│   └── ...
```

## 🔍 Cara Membaca Kode

1. **Scripts Utama** - Buka folder `Assets/Scripts/`:
   - **Image Tracking**: Script untuk deteksi marker
   - **UI Interface**: Script untuk tampilan interface
   - **Rotate Image**: Script untuk animasi rotasi model

2. **Marker Setup** - Cek folder `Assets/Prefabs/`:
   - Berisi konfigurasi marker 1, 2, 3, 4
   - Setup untuk image tracking database
   - Model Garuda hasil import dari Sketchfab
   - Tekstur dan material

## 🎯 Cara Kerja

1. Aplikasi menggunakan ARCore untuk image tracking
2. Scan kartu marker (1-4) dengan kamera
3. Model 3D Garuda akan muncul di atas marker
4. UI informatif muncul dengan edukasi singkat

## 📱 Platform

- Android (ARCore supported devices)
- Unity 2021.3+ 
- ARCore SDK


## Video Demonstrasi
https://www.youtube.com/watch?v=xAQp-3ftxq8

## Link Kartu Marker
https://www.canva.com/design/DAGoJ9slan0/wiYyYcf3ga0n8XZANCqLRA/view?utm_content=DAGoJ9slan0&utm_campaign=designshare&utm_medium=link2&utm_source=uniquelinks&utlId=h4fc50f0c2e 


## 🛠️ Setup

1. Clone repository
2. Buka dengan Unity
3. Import ARCore package jika belum ada
4. Build untuk Android platform

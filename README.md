OnimeBestofrieeeendo 🎌
AnimeWorld: Be the hero. Rule your world.

✨ Funktioner
Shop - Gennemse og køb anime-varer med forskellige sjældenhedsniveauer
User Profiles - Brugerkonti med avatarer, niveauer og saldo
Sorting - Sortér produkter efter pris, navn eller sjældenhed
Contact Form - Send beskeder til butikken

🛠 Teknologi
Backend: ASP.NET Core 8.0 med Blazor Server
Database: PostgreSQL
Styles: Custom CSS med animationer
Language: C#

📁 Project Structure
```OnimeBestofrieeeendo/
├── Components/ # Reusable UI components
│ ├── Pages/ # Web pages (Home, Shop, etc.)
│ ├── Services/ # Business logic
│ └── Layout/ # Page layouts
├── Models/ # Data models
└── wwwroot/ # Static files (CSS, images, JS)```

🚀 Sådan kører du det
Krav:
.NET 8.0
.NET SDK version:  9.0.101+
PostgreSQL-database

1. Klon projektet
2. Kopiér og indsæt følgende indstillinger i din appsettings.json:
   {
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Host=ep-royal-sky-a237idgr-pooler.eu-central-1.aws.neon.tech;Username=neondb_owner;Password=npg_0lCxSkAVEUe9;Database=neondb;sslmode=require;"
  },
  "AllowedHosts": "*"
}
3. Kør applikationen

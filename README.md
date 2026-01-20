## CurrencyApp

A small **.NET 8 (C#)** console application that converts between **EUR ↔ HUF** using live exchange rates from **currencyapi.net**.

### What it does
- Interactive console menu (via **ConsoleLauncher**)
- Fetches latest FX rates from the API with HttpClient
- Parses JSON with System.Text.Json
- Caches the latest response into currencyCache.json (default cache TTL: 1440 minutes)

### Tech stack
- **.NET 8 / C#**
- **ConsoleLauncher** *(CLI menu UI)*
- **HttpClient** *(HTTP requests)*
- **System.Text.Json** *(JSON serialization)*
- Simple file-based cache *(currencyCache.json)*

### Setup (required)
This app requires the environment variable **CURRENCY_API_KEY**.

You can obtain an API key from currencyapi.net (free or paid plans) and use it to access the rates endpoint.

#### macOS / Linux (zsh/bash)
- export CURRENCY_API_KEY="YOUR_KEY_HERE"
- dotnet run

#### Windows (PowerShell)hell
- $env:CURRENCY_API_KEY="YOUR_KEY_HERE"
- dotnet run

### Run
- dotnet restore  
- dotnet run

### Notes
- Base currency used for the API request is USD, then the app derives EUR↔HUF from the returned USD-based rates.
- Cache file: currencyCache.json (created in the working directory).

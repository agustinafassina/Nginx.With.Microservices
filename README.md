# Nginx and microservices

This repository runs, with Docker Compose, an **Nginx reverse proxy** in front of **four .NET REST APIs**. Each API wraps a public external API and returns its response.

![Architecture: Nginx routing requests to four .NET APIs](image.png)

## Prerequisites

- [Docker](https://docs.docker.com/get-docker/) with Docker Compose v2
- [.NET Core 3.1 SDK](https://dotnet.microsoft.com/download/dotnet/3.1) (only to run the APIs outside Docker)

## How it works

Nginx listens on port **6001** and works as the single entry point (gateway), routing by path prefix:

| Path prefix | Service      | Container | Direct port | External API                                   |
|-------------|--------------|-----------|-------------|------------------------------------------------|
| `/api/v1`   | ApiOne.Dc    | `one`     | 6001        | [Dog CEO](https://dog.ceo/dog-api/)            |
| `/api/v2`   | ApiTwo.Dc    | `two`     | 6002        | [REST Countries](https://restcountries.com/)   |
| `/api/v3`   | ApiThree.Dc  | `three`   | 6003        | [ipapi](https://ipapi.co/)                     |
| `/api/v4`   | ApiFour.Dc   | `four`    | 6004        | [Random Data API](https://random-data-api.com/) |

Each API is also reachable directly through its own Nginx port (6002–6004). If an external API fails, the service answers `502 Bad Gateway`.

## Docker Compose

Run every command from the folder that contains `docker-compose.yml` (`/NginxWeb`):

```bash
cd NginxWeb
```

| Action | Command |
|--------|---------|
| 🚀 Start (detached) | `docker compose up -d` |
| 🕒 Rebuild images and start | `docker compose up -d --build` |
| 🩺 Status | `docker compose ps` |
| 📜 Logs | `docker compose logs -f nginx` |
| 🛑 Stop and remove | `docker compose down` |

## Endpoints

All endpoints are `GET`, through the gateway at `http://localhost:6001`:

| Service     | Endpoint             | Returns                       |
|-------------|----------------------|-------------------------------|
| ApiOne.Dc   | `/api/v1/pets`       | 3 random dog images (JSON)    |
| ApiTwo.Dc   | `/api/v2/countries`  | Country names and flags (JSON)|
| ApiThree.Dc | `/api/v3/ipa`        | Geolocation of the caller IP (JSON) |
| ApiFour.Dc  | `/api/v4/random`     | 2 random users (XML)          |

Examples:

```bash
curl http://localhost:6001/api/v1/pets
curl http://localhost:6001/api/v2/countries
curl http://localhost:6001/api/v3/ipa
curl http://localhost:6001/api/v4/random
```

## Run a microservice without Docker

```bash
cd ApiOne.Dc
dotnet run
```

The API starts at `https://localhost:5001` and Swagger UI opens at `/swagger`.
All four APIs use the same default ports, so to run several at the same time give each one its own URL:

```bash
cd ApiTwo.Dc
dotnet run --urls "http://localhost:5102"
```

## Project structure

```
├── ApiOne.Dc/     # /api/v1/pets
├── ApiTwo.Dc/     # /api/v2/countries
├── ApiThree.Dc/   # /api/v3/ipa
├── ApiFour.Dc/    # /api/v4/random
└── NginxWeb/      # Nginx config, Dockerfile and docker-compose.yml
```

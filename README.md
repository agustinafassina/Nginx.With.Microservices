# Nginx and microservices
This repository has a docker compose that raises a docker for an nginx and four dockers with their rest apis.

![Alt text](image.png)

## Docker compose
### Up 🚀
Run command where is the docker-compose.yml file. In /NinxWeb

```
docker-compose up -d
```
### Build 🕒
```
docker-compose up --build
```

### Status view 🩺
```
docker container ps
```

## Microservices 
### Paths: http://localhost:6001
- ApiOne.Dc: /api/v1/pets - GET
- ApiTwo.Dc: api/v2/countries - GET
- ApiThree.Dc: api/v3/ipa - GET
- ApiFour.Dc: api/v4/random - GET

### Run the microservices separately by example:
#### Api One
```
cd /ApiOne.Dc
dotnet run
```

#### Api Two
```
cd /ApiTwo.Dc
dotnet run
```

#### Api Three
```
cd /ApiThree.Dc
dotnet run
```

#### Api Four
```
cd /ApiFour.Dc
dotnet run
```

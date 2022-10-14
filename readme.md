# House Dashboard Server

Server for taking various data (stored in DynamoDB currently) and making it available via an API.

Some example endpoints below - swagger is enabled for more up to date api documentation.

```
GET /Rainfall/{station-id}
GET /RiverLevel/{station-id}
GET /Summary
GET /Temperature/{station-id}/inside
GET /Temperature/{station-id}/outside
```

## Deployment

Currently only hosted on a local raspi docker instance. Running with the following command as part of a manual deploy
bash script.

Docker build should work now, updated raspi to buster so using the following base
image `mcr.microsoft.com/dotnet/sdk:5.0-buster-slim-arm32v7`:

```
docker build . -t hds --network=host
# or for specific dockerfile...
docker build -f Dockerfile.local . -t hds --network=host

docker run --rm -it --network=host --name house-dashboard-server -v $local-credential-folder:/root/.aws/credentials:ro hds 
```

## SEPA Station ID's

### Rainfall stations

| Station Name | Station Id |
|--------------|------------|
| Harperrig    | 15200      |
| Whitburn     | 14881      |
| Gogarbank    | 15196      |

### River stations

| Station Name | Id       |
|--------------|----------|
| Whitburn     | 14881 |
| Almondell    | 14869 |
| Cragiehall   | 14867 |


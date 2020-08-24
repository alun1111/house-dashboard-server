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

Currently only hosted on a local raspi docker instance. Running with the following command as part of a manual deploy bash script.

```bash
docker run --rm -d -p 5000:5000 \
--name house-dashboard-server \
-v /path/to/house-dashboard-server:/app/ \
-v /path/to/house-dashboard-server-creds:/root/.aws/credentials:ro \
-w /app/ \
-e ASPNETCORE_URLS=http://+:5000 \
-e ASPNETCORE_ENVIRONMENT=Development \
mcr.microsoft.com/dotnet/core/sdk:3.0 \
dotnet run --urls http://0.0.0.0:5000
```

## Future

- Deploy to AWS, need to add some authentication and "prodify" a little before this.
- Add some optimisation (caching) around DynamoDB calls.
- Add more data sources (one I plan to do is whats-my-next-bin-collection).


FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# Copy everything
COPY samples/ samples/
COPY src/ src/

WORKDIR samples/BlazorServer
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish --no-restore -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/sdk:7.0
WORKDIR /app
RUN dotnet dev-certs https
COPY --from=build-env /app/samples/BlazorServer/out .
ENTRYPOINT ["dotnet", "BlazorServer.dll"]

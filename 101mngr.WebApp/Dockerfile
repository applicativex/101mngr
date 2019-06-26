# Build stage
FROM microsoft/dotnet:2.2-sdk AS build-env
WORKDIR /build

# copy and build everything else
COPY . .

RUN ls /build
RUN dotnet restore

# Dotnet Build and publish 
RUN dotnet publish 101mngr.WebApp/101mngr.WebApp.csproj -c Release -o /app/out

# Clean up to free up spaces
WORKDIR /app/out
RUN ls /app/out

## Build runtime image
FROM microsoft/dotnet:2.2-aspnetcore-runtime

WORKDIR /app
COPY --from=build-env /app/out .

## Env variable

CMD ["dotnet", "101mngr.WebApp.dll"]

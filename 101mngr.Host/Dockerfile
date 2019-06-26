# Build stage
FROM microsoft/dotnet:2.2-sdk AS build-env
WORKDIR /build

# copy and build everything else
COPY . .

RUN ls /build
RUN dotnet restore

# Dotnet Build and publish 
RUN dotnet publish 101mngr.Host/101mngr.Host.csproj -c Release -o /app/out

# Clean up to free up spaces

WORKDIR /app/out
RUN ls /app/out
RUN rm -rf /build

# Runtime stage
FROM microsoft/dotnet:2.2-aspnetcore-runtime

WORKDIR /app
COPY --from=build-env /app/out .
RUN ls /app

CMD ["dotnet", "101mngr.Host.dll"]

# docker build -f .\101mngr.Host\Dockerfile -t 101mngr/host .
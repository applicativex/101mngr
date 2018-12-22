FROM microsoft/dotnet:2.1-sdk AS build-env
WORKDIR /app

# Vars required for the build
ARG VERSION_SUFFIX_OPT
ARG PUSH=false

# For Debug
RUN echo "VERSION_SUFFIX_OPT=$VERSION_SUFFIX_OPT, PUSH=$PUSH"
# copy and build everything else
COPY . ./

# do restore
RUN dotnet restore --ignore-failed-sources
RUN dotnet publish 101mngr.AuthorizationServer.csproj -c Release -o /app/out

# Clean up to free up spaces
WORKDIR /app/out
RUN ls /app/out

## Build runtime image
FROM microsoft/dotnet:2.1-aspnetcore-runtime

WORKDIR /app
COPY --from=build-env /app/out .

## Env variable

CMD ["dotnet", "101mngr.AuthorizationServer.dll"]

#FROM microsoft/dotnet:2.2-aspnetcore-runtime

FROM node:latest

ENV ADB_IP="192.168.1.1"
ENV REACT_NATIVE_PACKAGER_HOSTNAME="192.255.255.255"
ENV EXPO_DEBUG=false

RUN mkdir -p /app
WORKDIR /app

COPY ./101mngr.Mobile/package.json  ./

RUN npm set progress=false && \
    npm install && npm install -g expo-cli

COPY ./101mngr.Mobile/ .
RUN ls

CMD [ "expo", "start" ]

EXPOSE 19000
EXPOSE 19001
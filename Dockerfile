
#FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build-env

FROM ubuntu:20.04
WORKDIR /app

COPY . ./
#COPY ./webroot ./webroot

# Restore as distinct layers
#RUN dotnet restore

RUN apt-get update
RUN apt-get install -y curl
#RUN apt-get install -y gnupg2
RUN apt-get update
#RUN curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add -
#RUN apt-add-repository https://packages.microsoft.com/ubuntu/20.04/prod
#RUN apt-get update
#RUN apt-get install -y dotnet-sdk-3.1

RUN ./dotnet-install.sh -c 3.1

# Build and publish a release
#RUN dotnet publish -c Release -o out


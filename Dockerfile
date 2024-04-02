FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

COPY ./PGHttpRequests.sln ./
COPY ./RequestsApi/RequestsApi.csproj ./RequestsApi/RequestsApi.csproj
RUN dotnet restore

COPY ./RequestsApi ./RequestsApi/
RUN dotnet publish -c release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "RequestsApi.dll"]
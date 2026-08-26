FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src
COPY ["PlaySync.API/PlaySync.API.csproj", "PlaySync.API/"]
RUN dotnet restore "PlaySync.API/PlaySync.API.csproj"

COPY . .
WORKDIR "/src/PlaySync.API"
RUN dotnet publish "PlaySync.API.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

RUN apt-get update \
    && apt-get install -y --no-install-recommends libgssapi-krb5-2 \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
USER $APP_UID
ENTRYPOINT ["dotnet", "PlaySync.API.dll"]

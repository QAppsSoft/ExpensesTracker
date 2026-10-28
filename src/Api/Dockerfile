FROM mcr.microsoft.com/dotnet/aspnet:9.0@sha256:8bbd3f6fcc157792d96f0932107d9d435df814a2c058ce2891a267fe341dea1a AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0@sha256:7e3a4bd66701a451823db2028154ae16ff191bc452596ba618ef4d0c58f9cd01 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Api/Api.csproj", "Api/"]
RUN dotnet restore "Api/Api.csproj"
COPY . .
WORKDIR "/src/Api"
RUN dotnet build "./Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]

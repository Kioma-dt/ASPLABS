FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Web_453503_Avramenko.API/Web_453503_Avramenko.API.csproj", "Web_453503_Avramenko.API/"]
COPY ["Web_453503_Avramenko.Domain/Web_453503_Avramenko.Domain.csproj", "Web_453503_Avramenko.Domain/"]
RUN dotnet restore "Web_453503_Avramenko.API/Web_453503_Avramenko.API.csproj"
COPY . .
WORKDIR "/src/Web_453503_Avramenko.API"
RUN dotnet build "./Web_453503_Avramenko.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Web_453503_Avramenko.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Web_453503_Avramenko.API.dll"]

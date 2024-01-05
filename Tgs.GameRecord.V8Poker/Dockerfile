#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["Tgs.GameRecord.V8Poker/Tgs.GameRecord.V8Poker.csproj", "Tgs.GameRecord.V8Poker/"]
COPY ["Tgs.GameRecord.V8Poker.Common/Tgs.GameRecord.V8Poker.Common.csproj", "Tgs.GameRecord.V8Poker.Common/"]
COPY ["Tgs.GameRecord.V8Poker.Core/Tgs.GameRecord.V8Poker.Core.csproj", "Tgs.GameRecord.V8Poker.Core/"]
RUN dotnet restore "./Tgs.GameRecord.V8Poker/./Tgs.GameRecord.V8Poker.csproj"
COPY . .
WORKDIR "/src/Tgs.GameRecord.V8Poker"
RUN dotnet build "./Tgs.GameRecord.V8Poker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Tgs.GameRecord.V8Poker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.GameRecord.V8Poker.dll"]
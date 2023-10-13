#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BuildConfiguration=Release
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["Tgs.GameRecord.V8Poker/Tgs.GameRecord.V8Poker.csproj", "Tgs.GameRecord.V8Poker/"]
COPY ["Tgs.GameRecord.V8Poker.Common/Tgs.GameRecord.V8Poker.Common.csproj", "Tgs.GameRecord.V8Poker.Common/"]
COPY ["Tgs.GameRecord.V8Poker.Core/Tgs.GameRecord.V8Poker.Core.csproj", "Tgs.GameRecord.V8Poker.Core/"]
RUN dotnet restore "Tgs.GameRecord.V8Poker/Tgs.GameRecord.V8Poker.csproj"
COPY . .
WORKDIR "/src/Tgs.GameRecord.V8Poker"
RUN dotnet build "Tgs.GameRecord.V8Poker.csproj" -c $BuildConfiguration -o /app/build

FROM build AS publish
ARG BuildConfiguration
RUN dotnet publish "Tgs.GameRecord.V8Poker.csproj" -c $BuildConfiguration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.GameRecord.V8Poker.dll"]
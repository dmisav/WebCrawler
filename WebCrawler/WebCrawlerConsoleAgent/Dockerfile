FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["WebCrawlerConsoleAgent/WebCrawlerConsoleAgent.csproj", "WebCrawlerConsoleAgent/"]
RUN dotnet restore "WebCrawlerConsoleAgent/WebCrawlerConsoleAgent.csproj"
COPY . .
WORKDIR "/src/WebCrawlerConsoleAgent"
RUN dotnet build "WebCrawlerConsoleAgent.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebCrawlerConsoleAgent.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebCrawlerConsoleAgent.dll"]

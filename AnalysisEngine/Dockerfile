FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["AnalysisEngine/AnalysisEngine.csproj", "AnalysisEngine/"]
COPY ["Contract/Contract.csproj", "Contract/"]

RUN dotnet restore "AnalysisEngine/AnalysisEngine.csproj"
COPY . .
WORKDIR /src/AnalysisEngine
RUN dotnet build "AnalysisEngine.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AnalysisEngine.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "AnalysisEngine.dll"]

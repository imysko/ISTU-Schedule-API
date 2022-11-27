FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["getting-service/getting-service.csproj", "getting-service/"]
RUN dotnet restore "getting-service/getting-service.csproj"
COPY . .
WORKDIR "/src/getting-service"
RUN dotnet build "getting-service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "getting-service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "getting-service.dll"]

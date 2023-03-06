FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /app

COPY *.csproj ./
RUN dotnet restore --use-current-runtime

COPY . .
RUN dotnet build -c Debug -o build --use-current-runtime --self-contained false --no-restore

FROM mcr.microsoft.com/dotnet/runtime:6.0
WORKDIR /app
COPY --from=build /app/build .
ENTRYPOINT ["dotnet", "getting-service.dll"]
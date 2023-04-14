FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /app

COPY *.csproj ./
RUN dotnet restore --use-current-runtime

COPY . .
RUN dotnet build -c Debug -o build --use-current-runtime --self-contained false --no-restore

FROM mcr.microsoft.com/dotnet/runtime:7.0
WORKDIR /app
COPY --from=build /app/build .
COPY updatedates.json .
ENTRYPOINT ["dotnet", "getting-service.dll"]
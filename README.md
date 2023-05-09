# ISTU-Schedule

## Contains a service
* getting-service - for receiving data from a Smart-schedule-IRNITU [telegram bot](https://t.me/Smart_schedule_IRNITU_bot)
* API - includes GET requests to get the schedule

### Before launch
- you need to create in [directory](config) common.env [template](config/common.env.example) and add database name, username, user password and copy connection string for database
```make
POSTGRES_DB=
POSTGRES_USER=
POSTGRES_PASSWORD=
ConnectionStrings__ScheduleDB="Host=schedule-db;Port=5432;Database=${POSTGRES_DB};Username=${POSTGRES_USER};Password=${POSTGRES_PASSWORD};IncludeErrorDetail=true"
```
- you need to create in [directory](config) telegram.env [template](config/telegram.env.example) and add ApiId, ApiHash, PhoneNumber and ChatBotId
```make
TelegramConfig__ApiId=
TelegramConfig__ApiHash=
TelegramConfig__PhoneNumber=
TelegramConfig__ChatBotId=
```

### Before launch
1. install docker
2. install socat

### For launch
1. make docker images
```shell
sudo docker compose build
```
2. run docker containers in the background
```shell
sudo docker compose up -d
```

### After first launch
1. connect to the getting-service terminal
```shell
socat TCP:localhost:5723 -
```
2. await telegram login code and write this code in the terminal (also write a password if you need)
3. write command __*all*__ in the terminal for get all data 
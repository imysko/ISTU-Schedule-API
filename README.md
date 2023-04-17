# ISTU-Schedule

## Contains a service
* getting-service - for receiving data from a Smart-schedule-IRNITU [telegram bot](https://t.me/Smart_schedule_IRNITU_bot)
* API - includes GET requests to get the schedule

### Before launch
- you need to create in [directory](config) common.env [template](config/common.env.example) and add connection string for database
```make
ConnectionStrings__ScheduleDB=Host=<host>;Port=<port>;Database=<database>;Username=<username>;Password=<password>;IncludeErrorDetail=<true or false>
```
- you need to create in [directory](config) telegram.env [template](config/telegram.env.example) and add ApiId, ApiHash, PhoneNumber and ChatBotId
```make
TelegramConfig__ApiId=
TelegramConfig__ApiHash=
TelegramConfig__PhoneNumber=
TelegramConfig__ChatBotId=
```

### For launch
1. make docker images
```shell
sudo docker-compose build
```
2. run docker containers in the background
```shell
sudo docker-compose up -d
```

### After launch
await telegram login code and add code in [telegram.env](config/telegram.env), then save the file
```make
TelegramConfig__LoginCode=
```
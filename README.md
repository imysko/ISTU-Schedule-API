# ISTU-Schedule

## Contains a service
* getting-service - for receiving data from a Smart-schedule-IRNITU (telegram bot)
* API - includes GET requests to get the schedule

### Before launch
- you need to create appsettings.json and add a connections settings to postgresql and telegram api in getting-service: [example app settings](getting-service/appsettings.json.example)
```json
"ConnectionStrings": {
  "ScheduleDB": "Host=<host>;Port=<port>;Database=<database>;Username=<user>;Password=<password>",
  "TelegramApiId": "<id>",
  "TelegramApiHash": "<hash>",
  "TelegramPhoneNumber": "<phone_number>"
  "ChatBotId": "<chat_id>"
}
```
- you need to to create appsettings.json add a connections settings to postgresql in API: [example app settings](API/appsettings.json.example)
```json
"ConnectionStrings": {
  "ScheduleDB": "Host=<host>;Port=<port>;Database=<database>;Username=<user>;Password=<password>",
}
```
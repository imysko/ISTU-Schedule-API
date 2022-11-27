# ISTU-Schedule

## Contains a service
* getting-service - for receiving data from a Smart-schedule-IRNITU (telegram bot)

### Before launch
- you need to create a launch settings getting-service: [example](getting-service/Properties/launchSettings.json.example)
- add envs:<br/>
```
  "environmentVariables": {
    "ApiId": "Your Telegram API id",
    "ApiHash": "Your Telegram API hash",
    "PhoneNumber": "Your phone number"
  }
```
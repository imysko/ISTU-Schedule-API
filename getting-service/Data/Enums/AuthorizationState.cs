namespace getting_service.Data.Enums;

public enum AuthorizationState
{
    Unauthorized = 0,
    WaitPhone = 1,
    WaitCode = 2,
    WaitPassword = 3,
    Authorized = 4
}
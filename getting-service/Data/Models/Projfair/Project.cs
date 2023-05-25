namespace getting_service.Data.Models.Projfair;

public record Project(
    int Id,
    string Title,
    ProjectState State);
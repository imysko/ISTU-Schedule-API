namespace getting_service.Data.Models.Projfair;

public record ProjectsResponse(
    List<Project>? Data,
    int? ProjectCount);
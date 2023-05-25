using System.Net.Http.Json;
using getting_service.Data.Enums;
using getting_service.Data.Models.Projfair;

namespace getting_service.Clients;

public static class ProjfairClient
{
    private static readonly HttpClient Client = new()
    {
        BaseAddress = new Uri("https://projfair.istu.edu/"),
    };
    
    public static async Task<Project?> FindProject(string? title)
    {
        var response = await Client.GetFromJsonAsync<ProjectsResponse>($"api/projects/filter?title={title}");
        return response!.Data!.FirstOrDefault(p => p.State.Id == ProjectStates.Active);
    }
}
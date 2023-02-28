using getting_service.DataBase.Context;
using getting_service.DataBase.Models;
using Newtonsoft.Json.Linq;

namespace getting_service.DataBase.Controllers;

public class ScheduleDbController
{
    private readonly ScheduleDbContext _context;
    
    public ScheduleDbController(ScheduleDbContext context)
    {
        _context = context;
    }
    
    public async Task LoadJson(string path)
    {
        var response = JObject.Parse(await File.ReadAllTextAsync(path));
        var key = response.First!.Path;
        
        switch (key)
        {
            case "institutes":
            {
                await PutInstitutes(response[key]!.ToObject<List<Institute>>()!);
                return;
            }
            case "groups":
            {
                await PutGroups(response[key]!.ToObject<List<Group>>()!);
                return;
            }
            case "teachers":
            {
                await PutTeachers(response[key]!.ToObject<List<Teacher>>()!);
                return;
            }
            case "classrooms":
            {
                await PutClassrooms(response[key]!.ToObject<List<Classroom>>()!);
                return;
            }
            case "lessons":
            {
                await PutLessonsTime(response[key]!.ToObject<List<LessonsTime>>()!);
                return;
            }
            case "disciplines":
            {
                await PutDisciplines(response[key]!.ToObject<List<Discipline>>()!);
                return;
            }
            case "schedule":
            {
                await PutSchedule(response[key]!.ToObject<List<Schedule>>()!);
                return;
            }
            default: return;
        };
    }

    private async Task PutInstitutes(List<Institute> list)
    {
        list.ForEach(el => _context.Institutes.Add(el));

        await _context.SaveChangesAsync();
    }
    
    private async Task PutGroups(List<Group> list)
    {
        list.ForEach(el => _context.Groups.Add(el));

        await _context.SaveChangesAsync();
    }
    
    private async Task PutTeachers(List<Teacher> list)
    {
        list.ForEach(el => _context.Teachers.Add(el));

        await _context.SaveChangesAsync();
    }
    
    private async Task PutClassrooms(List<Classroom> list)
    {
        list.ForEach(el => _context.Classrooms.Add(el));

        await _context.SaveChangesAsync();
    }
    
    private async Task PutLessonsTime(List<LessonsTime> list)
    {
        list.ForEach(el => _context.LessonsTimes.Add(el));

        await _context.SaveChangesAsync();
    }
    
    private async Task PutDisciplines(List<Discipline> list)
    {
        list.ForEach(el => _context.Disciplines.Add(el));

        await _context.SaveChangesAsync();
    }
    
    private async Task PutSchedule(List<Schedule> list)
    {
        list.ForEach(el => _context.Schedules.Add(el));

        await _context.SaveChangesAsync();
    }
}
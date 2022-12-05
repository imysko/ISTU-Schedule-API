using getting_service.Models;
using Newtonsoft.Json.Linq;

namespace getting_service;

public static class ScheduleController
{
    public static void LoadJson(string path)
    {
        JObject response = JObject.Parse(File.ReadAllText(path));
        var key = response.First!.Path;
        
        switch (key)
        {
            case "institutes":
            {
                PutInstitutes(response[key]!.ToObject<List<Institute>>()!);
                return;
            }
            case "groups":
            {
                PutGroups(response[key]!.ToObject<List<Group>>()!);
                return;
            }
            case "teachers":
            {
                PutTeachers(response[key]!.ToObject<List<Teacher>>()!);
                return;
            }
            case "classrooms":
            {
                PutClassrooms(response[key]!.ToObject<List<Classroom>>()!);
                return;
            }
            case "lessons":
            {
                PutLessonsTime(response[key]!.ToObject<List<LessonsTime>>()!);
                return;
            }
            case "lessons_names":
            {
                PutDisciplines(response[key]!.ToObject<List<Discipline>>()!);
                return;
            }
            case "schedule":
            {
                PutSchedule(response[key]!.ToObject<List<ScheduleRecord>>()!);
                return;
            }
            default: return;
        };
    }

    private static void PutInstitutes(List<Institute> list)
    {
    }
    
    private static void PutGroups(List<Group> list)
    {
    }
    
    private static void PutTeachers(List<Teacher> list)
    {
    }
    
    private static void PutClassrooms(List<Classroom> list)
    {
    }
    
    private static void PutLessonsTime(List<LessonsTime> list)
    {
    }
    
    private static void PutDisciplines(List<Discipline> list)
    {
    }
    
    private static void PutSchedule(List<ScheduleRecord> list)
    {
    }
}
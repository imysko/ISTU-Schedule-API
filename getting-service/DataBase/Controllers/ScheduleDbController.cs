using System.Globalization;
using getting_service.DataBase.Context;
using getting_service.DataBase.Models;
using Microsoft.EntityFrameworkCore;
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
                await PutSchedule(response[key]!.ToObject<List<ScheduleViewModel>>()!);
                return;
            }
            default:
            {
                return;
            }
        }
    }

    private async Task PutInstitutes(List<Institute> list)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(DateTime.Now);
        Console.ResetColor();
        
        list.ForEach(el =>
        {
            if (_context.Institutes.Any(c => c.InstituteId == el.InstituteId))
            {
                _context.Entry(el).State = EntityState.Modified;
            }
            else
            {
                _context.Institutes.Add(el);
                _context.Entry(el).State = EntityState.Added;
            }
        });

        await _context.SaveChangesAsync();
        
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Institutes was received");
        Console.ResetColor();
        
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(DateTime.Now);
        Console.ResetColor();
    }
    
    private async Task PutGroups(List<Group> list)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(DateTime.Now);
        Console.ResetColor();
        
        list.ForEach(el =>
        {
            if (_context.Groups.Any(c => c.GroupId == el.GroupId))
            {
                _context.Entry(el).State = EntityState.Modified;
            }
            else
            {
                _context.Groups.Add(el);
                _context.Entry(el).State = EntityState.Added;
            }
        });

        await _context.SaveChangesAsync();
        
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Groups was received");
        Console.ResetColor();
        
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(DateTime.Now);
        Console.ResetColor();
    }
    
    private async Task PutTeachers(List<Teacher> list)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(DateTime.Now);
        Console.ResetColor();
        
        list.ForEach(el =>
        {
            if (_context.Teachers.Any(c => c.TeacherId == el.TeacherId))
            {
                _context.Entry(el).State = EntityState.Modified;
            }
            else
            {
                _context.Teachers.Add(el);
                _context.Entry(el).State = EntityState.Added;
            }
        });

        await _context.SaveChangesAsync();
        
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Teachers was received");
        Console.ResetColor();
        
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(DateTime.Now);
        Console.ResetColor();
    }
    
    private async Task PutClassrooms(List<Classroom> list)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(DateTime.Now);
        Console.ResetColor();
        
        list.ForEach(el =>
        {
            if (_context.Classrooms.Any(c => c.ClassroomId == el.ClassroomId))
            {
                _context.Entry(el).State = EntityState.Modified;
            }
            else
            {
                _context.Classrooms.Add(el);
                _context.Entry(el).State = EntityState.Added;
            }
        });

        await _context.SaveChangesAsync();
        
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Classrooms was received");
        Console.ResetColor();
        
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(DateTime.Now);
        Console.ResetColor();
    }
    
    private async Task PutLessonsTime(List<LessonsTime> list)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(DateTime.Now);
        Console.ResetColor();
        
        list.ForEach(el =>
        {
            if (_context.LessonsTimes.Any(c => c.LessonId == el.LessonId))
            {
                _context.Entry(el).State = EntityState.Modified;
            }
            else
            {
                _context.LessonsTimes.Add(el);
                _context.Entry(el).State = EntityState.Added;
            }
        });

        await _context.SaveChangesAsync();
        
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("LessonsTime was received");
        Console.ResetColor();
        
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(DateTime.Now);
        Console.ResetColor();
    }
    
    private async Task PutDisciplines(List<Discipline> list)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(DateTime.Now);
        Console.ResetColor();
        
        list.ForEach(el =>
        {
            if (_context.Disciplines.Any(c => c.DisciplineId == el.DisciplineId))
            {
                _context.Entry(el).State = EntityState.Modified;
            }
            else
            {
                _context.Disciplines.Add(el);
                _context.Entry(el).State = EntityState.Added;
            }
        });

        await _context.SaveChangesAsync();
        
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Disciplines was received");
        Console.ResetColor();
        
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(DateTime.Now);
        Console.ResetColor();
    }
    
    private async Task PutSchedule(List<ScheduleViewModel> list)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(DateTime.Now);
        Console.ResetColor();
        
        foreach (var el in list)
        {
            var scheduleGroups = el.GroupsIds?
                .Where(id => id != null && _context.Groups.Any(g => g.GroupId == id))
                .Distinct()
                .Select(id => new ScheduleGroup
                {
                    ScheduleId = el.ScheduleId,
                    GroupId = (int)id
                }) ?? new List<ScheduleGroup>();
            
            var scheduleTeachers = el.TeachersIds?
                .Where(id => id != null && _context.Teachers.Any(t => t.TeacherId == id))
                .Distinct()
                .Select(id => new ScheduleTeacher
                {
                    ScheduleId = el.ScheduleId,
                    TeacherId = (int)id
                }) ?? new List<ScheduleTeacher>();
            
            var newSchedule = new Schedule
            {
                ScheduleId = el.ScheduleId,
                GroupsVerbose = el.GroupsVerbose,
                TeachersVerbose = el.TeachersVerbose,
                ClassroomId = el.ClassroomsIds?
                    .FirstOrDefault(id => _context.Classrooms.Any(c => c.ClassroomId == id)) ?? null,
                ClassroomVerbose = el.ClassroomsVerbose,
                DisciplineId = el.DisciplineId,
                DisciplineVerbose = el.DisciplineVerbose,
                LessonId = el.LessonId,
                Subgroup = el.Subgroup,
                LessonType = el.LessonType,
                Date = DateOnly.ParseExact(el.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                ScheduleGroups = scheduleGroups.ToList(),
                ScheduleTeachers = scheduleTeachers.ToList()
            };
            
            if ((_context.Schedules?.Any(s => s.ScheduleId == newSchedule.ScheduleId)).GetValueOrDefault())
            {
                _context.Entry(newSchedule).State = EntityState.Modified;
                
                newSchedule.ScheduleGroups
                    .ToList()
                    .ForEach(relations =>
                    {
                        if (_context.ScheduleGroups.Any(sg =>
                                sg.ScheduleId == relations.ScheduleId && sg.GroupId == relations.GroupId))
                        {
                            _context.Entry(relations).State = EntityState.Modified;   
                        }
                        else
                        {
                            _context.ScheduleGroups.Add(relations);
                        }
                    });
                
                newSchedule.ScheduleTeachers
                    .ToList()
                    .ForEach(relations =>
                    {
                        if (_context.ScheduleTeachers.Any(st =>
                                st.ScheduleId == relations.ScheduleId && st.TeacherId == relations.TeacherId))
                        {
                            _context.Entry(relations).State = EntityState.Modified;   
                        }
                        else
                        {
                            _context.ScheduleTeachers.Add(relations);
                        }
                    });
            }
            else
            {
                _context.Schedules?.Add(newSchedule);
                _context.Entry(newSchedule).State = EntityState.Added;
            }
        }
        
        await _context.SaveChangesAsync();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Schedule was received");
        Console.ResetColor();
        
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(DateTime.Now);
        Console.ResetColor();
    }
}
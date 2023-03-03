using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using getting_service.DataBase.Context;
using getting_service.DataBase.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace getting_service.DataBase.Controllers;

[SuppressMessage("ReSharper", "InconsistentlySynchronizedField")]
public class ScheduleDbController
{
    private readonly object _lock = new();
    private readonly ScheduleDbContext _context;

    public ScheduleDbController(ScheduleDbContext context)
    {
        _context = context;
    }

    public async Task LoadJson(string path)
    {
        var response = JObject.Parse(await File.ReadAllTextAsync(path));
        var key = response.First!.Path;

        File.Delete(path);

        switch (key)
        {
            case "institutes":
            {
                await CallTaskWithLogging(
                    async () => await PutInstitutes(response[key]!.ToObject<List<Institute>>()!),
                    " Institutes was received "
                );
                break;
            }
            case "groups":
            {
                await CallTaskWithLogging(
                    async () => await PutGroups(response[key]!.ToObject<List<Group>>()!),
                    " Groups was received "
                );
                break;
            }
            case "teachers":
            {
                await CallTaskWithLogging(
                    async () => await PutTeachers(response[key]!.ToObject<List<Teacher>>()!),
                    " Teachers was received "
                );
                break;
            }
            case "classrooms":
            {
                await CallTaskWithLogging(
                    async () => await PutClassrooms(response[key]!.ToObject<List<Classroom>>()!),
                    " Classrooms was received "
                );
                break;
            }
            case "lessons":
            {
                await CallTaskWithLogging(
                    async () => await PutLessonsTime(response[key]!.ToObject<List<LessonsTime>>()!),
                    " LessonsTime was received "
                );
                break;
            }
            case "disciplines":
            {
                await CallTaskWithLogging(
                    async () => await PutDisciplines(response[key]!.ToObject<List<Discipline>>()!),
                    " Disciplines was received "
                );
                break;
            }
            case "schedule":
            {
                await CallTaskWithLogging(
                    async () => await PutSchedule(response[key]!.ToObject<List<ScheduleViewModel>>()!),
                    " Schedule was received "
                );
                break;
            }
        }
    }

    private async Task PutInstitutes(List<Institute> list)
    {
        var existingInstitutesById = await _context.Institutes.ToDictionaryAsync(i => i.InstituteId);

        var newInstitutes = new List<Institute>();
        foreach (var institute in list)
        {
            if (existingInstitutesById.TryGetValue(institute.InstituteId, out var existingInstitute))
            {
                _context.Entry(existingInstitute).CurrentValues.SetValues(institute);
            }
            else
            {
                newInstitutes.Add(institute);
            }
        }

        _context.Institutes.AddRange(newInstitutes);
        _context.Institutes.UpdateRange(existingInstitutesById.Values.Except(newInstitutes));

        await _context.SaveChangesAsync();
    }

    private async Task PutGroups(List<Group> list)
    {
        var existingGroupsById = await _context.Groups.ToDictionaryAsync(g => g.GroupId);

        var newGroups = new List<Group>();
        foreach (var group in list)
        {
            if (existingGroupsById.TryGetValue(group.GroupId, out var existingGroup))
            {
                _context.Entry(existingGroup).CurrentValues.SetValues(group);
            }
            else
            {
                newGroups.Add(group);
            }
        }

        _context.Groups.AddRange(newGroups);
        _context.Groups.UpdateRange(existingGroupsById.Values.Except(newGroups));

        await _context.SaveChangesAsync();
    }

    private async Task PutTeachers(List<Teacher> list)
    {
        var existingTeachersById = await _context.Teachers.ToDictionaryAsync(t => t.TeacherId);

        var newTeachers = new List<Teacher>();
        foreach (var teacher in list)
        {
            if (existingTeachersById.TryGetValue(teacher.TeacherId, out var existingTeacher))
            {
                _context.Entry(existingTeacher).CurrentValues.SetValues(teacher);
            }
            else
            {
                newTeachers.Add(teacher);
            }
        }

        _context.Teachers.AddRange(newTeachers);
        _context.Teachers.UpdateRange(existingTeachersById.Values.Except(newTeachers));

        await _context.SaveChangesAsync();
    }

    private async Task PutClassrooms(List<Classroom> list)
    {
        var existingClassroomsById = await _context.Classrooms.ToDictionaryAsync(c => c.ClassroomId);

        var newClassrooms = new List<Classroom>();
        foreach (var classroom in list)
        {
            if (existingClassroomsById.TryGetValue(classroom.ClassroomId, out var existingClassroom))
            {
                _context.Entry(existingClassroom).CurrentValues.SetValues(classroom);
            }
            else
            {
                newClassrooms.Add(classroom);
            }
        }

        _context.Classrooms.AddRange(newClassrooms);
        _context.Classrooms.UpdateRange(existingClassroomsById.Values.Except(newClassrooms));

        await _context.SaveChangesAsync();
    }

    private async Task PutLessonsTime(List<LessonsTime> list)
    {
        var existingLessonsTimesById = await _context.LessonsTimes.ToDictionaryAsync(lt => lt.LessonId);

        var newLessonsTimes = new List<LessonsTime>();
        foreach (var lessonsTime in list)
        {
            if (existingLessonsTimesById.TryGetValue(lessonsTime.LessonId, out var existingLessonsTime))
            {
                _context.Entry(existingLessonsTime).CurrentValues.SetValues(lessonsTime);
            }
            else
            {
                newLessonsTimes.Add(lessonsTime);
            }
        }

        _context.LessonsTimes.AddRange(newLessonsTimes);
        _context.LessonsTimes.UpdateRange(existingLessonsTimesById.Values.Except(newLessonsTimes));

        await _context.SaveChangesAsync();
    }

    private async Task PutDisciplines(List<Discipline> list)
    {
        var existingDisciplinesById = await _context.Disciplines.ToDictionaryAsync(d => d.DisciplineId);

        var newDisciplines = new List<Discipline>();
        foreach (var discipline in list)
        {
            if (existingDisciplinesById.TryGetValue(discipline.DisciplineId, out var existingDiscipline))
            {
                _context.Entry(existingDiscipline).CurrentValues.SetValues(discipline);
            }
            else
            {
                newDisciplines.Add(discipline);
            }
        }

        _context.Disciplines.AddRange(newDisciplines);
        _context.Disciplines.UpdateRange(existingDisciplinesById.Values.Except(newDisciplines));

        await _context.SaveChangesAsync();
    }

    private async Task PutSchedule(List<ScheduleViewModel> list)
    {
        var existingSchedules = await _context.Schedules.ToListAsync();
        var existingClassrooms = await _context.Classrooms.ToListAsync();
        var existingDisciplines = await _context.Disciplines.ToListAsync();
        var existingGroups = await _context.Groups.ToListAsync();
        var existingTeachers = await _context.Teachers.ToListAsync();
        var existingLessonsTimes = await _context.LessonsTimes.ToListAsync();
        var existingSchedulesGroups = await _context.ScheduleGroups.ToListAsync();
        var existingSchedulesTeachers = await _context.ScheduleTeachers.ToListAsync();

        var newSchedules = new List<Schedule>();
        var newScheduleGroupsList = new List<ScheduleGroup>();
        var newSchedulesTeachersList = new List<ScheduleTeacher>();

        foreach (var el in list)
        {
            if (el.GroupsIds != null)
            {
                foreach (var id in el.GroupsIds)
                {
                    if (id == null ||
                        existingSchedulesGroups.Any(s => s.ScheduleId == el.ScheduleId && s.GroupId == id.Value) ||
                        newScheduleGroupsList.Any(s => s.ScheduleId == el.ScheduleId && s.GroupId == id.Value))
                        continue;
                    
                    if (existingGroups.All(g => g.GroupId != id)) continue;
                        
                    newScheduleGroupsList.Add(new ScheduleGroup
                    {
                        ScheduleId = el.ScheduleId,
                        GroupId = id.Value
                    });
                }
            }

            if (el.TeachersIds != null)
            {
                foreach (var id in el.TeachersIds)
                {
                    if (id == null ||
                        existingSchedulesTeachers.Any(s => s.ScheduleId == el.ScheduleId && s.TeacherId == id.Value) ||
                        newSchedulesTeachersList.Any(s => s.ScheduleId == el.ScheduleId && s.TeacherId == id.Value))
                        continue;
                    
                    if (existingTeachers.All(t => t.TeacherId != id)) continue;
                    
                    newSchedulesTeachersList.Add(new ScheduleTeacher
                    {
                        ScheduleId = el.ScheduleId,
                        TeacherId = id.Value
                    });
                }
            }

            var newSchedule = new Schedule
            {
                ScheduleId = el.ScheduleId,
                GroupsVerbose = el.GroupsVerbose,
                TeachersVerbose = el.TeachersVerbose,
                ClassroomId = existingClassrooms.FirstOrDefault(c =>
                    el.ClassroomsIds != null && el.ClassroomsIds.Any(it => it == c.ClassroomId))?.ClassroomId ?? null,
                ClassroomVerbose = el.ClassroomsVerbose,
                DisciplineId =
                    existingDisciplines.FirstOrDefault(d => d.DisciplineId == el.DisciplineId)?.DisciplineId ?? null,
                DisciplineVerbose = el.DisciplineVerbose,
                LessonId = existingLessonsTimes.FirstOrDefault(l => l.LessonId == el.LessonId)?.LessonId ?? null,
                Subgroup = el.Subgroup,
                LessonType = el.LessonType,
                Date = DateOnly.ParseExact(el.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture)
            };

            var existingSchedule = existingSchedules.FirstOrDefault(s => s.ScheduleId == newSchedule.ScheduleId);
            if (existingSchedule != null)
            {
                _context.Entry(existingSchedule).CurrentValues.SetValues(newSchedule);
            }
            else
            {
                newSchedules.Add(newSchedule);
            }
        }

        _context.Schedules.AddRange(newSchedules);
        _context.Schedules.UpdateRange(existingSchedules.Except(newSchedules));

        _context.ScheduleGroups.AddRange(newScheduleGroupsList);
        _context.ScheduleTeachers.AddRange(newSchedulesTeachersList);

        await _context.SaveChangesAsync();
    }

    private static async Task CallTaskWithLogging(Func<Task> asyncFunction, string message)
    {
        await Task.Run(async () =>
        {
            var startDateTime = DateTime.Now;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"{startDateTime} -");
            Console.ResetColor();

            await asyncFunction();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{message}");
            Console.ResetColor();

            var endDateTime = DateTime.Now;
            var diffInSeconds = (endDateTime - startDateTime).TotalSeconds;

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"- finished in {(int)diffInSeconds} seconds");
            Console.ResetColor();
        });
    }
}
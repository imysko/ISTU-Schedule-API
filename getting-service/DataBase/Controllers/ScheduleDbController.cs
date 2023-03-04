using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using getting_service.DataBase.Context;
using getting_service.DataBase.Models;
using Newtonsoft.Json.Linq;

namespace getting_service.DataBase.Controllers;

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
                    "Institutes"
                );
                break;
            }
            case "groups":
            {
                await CallTaskWithLogging(
                    async () => await PutGroups(response[key]!.ToObject<List<Group>>()!),
                    "Groups"
                );
                break;
            }
            case "teachers":
            {
                await CallTaskWithLogging(
                    async () => await PutTeachers(response[key]!.ToObject<List<Teacher>>()!),
                    "Teachers"
                );
                break;
            }
            case "classrooms":
            {
                await CallTaskWithLogging(
                    async () => await PutClassrooms(response[key]!.ToObject<List<Classroom>>()!),
                    "Classrooms"
                );
                break;
            }
            case "lessons":
            {
                await CallTaskWithLogging(
                    async () => await PutLessonsTime(response[key]!.ToObject<List<LessonsTime>>()!),
                    "Lessons Time"
                );
                break;
            }
            case "disciplines":
            {
                await CallTaskWithLogging(
                    async () => await PutDisciplines(response[key]!.ToObject<List<Discipline>>()!),
                    "Disciplines"
                );
                break;
            }
            case "schedule":
            {
                await CallTaskWithLogging(
                    async () => await PutSchedule(response[key]!.ToObject<List<ScheduleViewModel>>()!),
                    "Schedule"
                );
                break;
            }
        }
    }

    private Task PutInstitutes(List<Institute> list)
    {
        lock (_lock)
        {
            var existingInstitutesById = _context.Institutes.ToDictionary(i => i.InstituteId);

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

            _context.SaveChanges();
        }

        return Task.CompletedTask;
    }

    private Task PutGroups(List<Group> list)
    {
        lock (_lock)
        {
            var existingGroupsById = _context.Groups.ToDictionary(g => g.GroupId);

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

            _context.SaveChanges();
        }

        return Task.CompletedTask;
    }

    private Task PutTeachers(List<Teacher> list)
    {
        lock (_lock)
        {
            var existingTeachersById = _context.Teachers.ToDictionary(t => t.TeacherId);

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

            _context.SaveChanges();
        }

        return Task.CompletedTask;
    }

    private Task PutClassrooms(List<Classroom> list)
    {
        lock (_lock)
        {
            var existingClassroomsById = _context.Classrooms.ToDictionary(c => c.ClassroomId);

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

            _context.SaveChanges();
        }

        return Task.CompletedTask;
    }

    private Task PutLessonsTime(List<LessonsTime> list)
    {
        lock (_lock)
        {
            var existingLessonsTimesById = _context.LessonsTimes.ToDictionary(lt => lt.LessonId);

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

            _context.SaveChanges();
        }

        return Task.CompletedTask;
    }

    private Task PutDisciplines(List<Discipline> list)
    {
        lock (_lock)
        {
            var existingDisciplinesById = _context.Disciplines.ToDictionary(d => d.DisciplineId);

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

            _context.SaveChanges();
        }

        return Task.CompletedTask;
    }

    private Task PutSchedule(List<ScheduleViewModel> list)
    {
        lock (_lock)
        {
            var existingSchedules = _context.Schedules.ToList();
            var existingClassrooms = _context.Classrooms.ToList();
            var existingDisciplines = _context.Disciplines.ToList();
            var existingGroups = _context.Groups.ToList();
            var existingTeachers = _context.Teachers.ToList();
            var existingLessonsTimes = _context.LessonsTimes.ToList();
            var existingSchedulesGroups = _context.ScheduleGroups.ToList();
            var existingSchedulesTeachers = _context.ScheduleTeachers.ToList();

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
                            existingSchedulesTeachers.Any(s =>
                                s.ScheduleId == el.ScheduleId && s.TeacherId == id.Value) ||
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
                                      el.ClassroomsIds != null && el.ClassroomsIds.Any(it => it == c.ClassroomId))
                                  ?.ClassroomId ??
                                  null,
                    ClassroomVerbose = el.ClassroomsVerbose,
                    DisciplineId =
                        existingDisciplines.FirstOrDefault(d => d.DisciplineId == el.DisciplineId)?.DisciplineId ??
                        null,
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

            _context.SaveChanges();
        }

        return Task.CompletedTask;
    }

    private static async Task CallTaskWithLogging(Func<Task> asyncFunction, string message)
    {
        await Task.Run(async () =>
        {
            var startDateTime = DateTime.Now;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"{startDateTime} - ");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{message} was received");
            Console.ResetColor();
            
            await asyncFunction();

            var endDateTime = DateTime.Now;
            var diffInSeconds = (endDateTime - startDateTime).TotalSeconds;
            
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"{endDateTime} - ");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{message} was processed");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($" - finished in {(int)diffInSeconds} seconds");
            Console.ResetColor();
        });
    }
}
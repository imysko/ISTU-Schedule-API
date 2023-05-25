using System.Globalization;
using getting_service.Clients;
using getting_service.Data.Enums;
using getting_service.Data.Models.Schedule;
using getting_service.DataBase.Context;
using getting_service.DataBase.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace getting_service.Controllers;

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
            case "other_disciplines":
            {
                await CallTaskWithLogging(
                    async () => await PutOtherDisciplines(response[key]!.ToObject<List<OtherDisciplineResponse>>()!),
                    "Other Disciplines"
                );
                break;
            }
            case "queries":
            {
                await CallTaskWithLogging(
                    async () => await PutQueries(response[key]!.ToObject<List<Query>>()!),
                    "Queries"
                );
                break;
            }
            case "schedule":
            {
                await CallTaskWithLogging(
                    async () => await PutSchedules(response[key]!.ToObject<List<ScheduleResponse>>()!),
                    "Schedule"
                );
                break;
            }
        }
    }

    private async Task PutInstitutes(List<Institute> institutes)
    {
        foreach (var institute in institutes)
        {
            var existingInstitute = await _context.Institutes.FindAsync(institute.InstituteId);

            if (existingInstitute != null)
            {
                _context.Entry(existingInstitute).CurrentValues.SetValues(institute);
            }
            else
            {
                await _context.Institutes.AddAsync(institute);
            }
        }

        await _context.SaveChangesAsync();
    }

    private async Task PutGroups(List<Group> groups)
    {
        foreach (var group in groups)
        {
            var existingGroup = await _context.Groups.FindAsync(group.GroupId);

            if (existingGroup != null)
            {
                _context.Entry(existingGroup).CurrentValues.SetValues(group);
            }
            else
            {
                await _context.Groups.AddAsync(group);
            }
        }

        await _context.SaveChangesAsync();
    }

    private async Task PutTeachers(List<Teacher> teachers)
    {
        foreach (var teacher in teachers)
        {
            var existingTeacher = await _context.Teachers.FindAsync(teacher.TeacherId);

            if (existingTeacher != null)
            {
                _context.Entry(existingTeacher).CurrentValues.SetValues(teacher);
            }
            else
            {
                await _context.Teachers.AddAsync(teacher);
            }
        }

        await _context.SaveChangesAsync();
    }

    private async Task PutClassrooms(List<Classroom> classrooms)
    {
        foreach (var classroom in classrooms)
        {
            var existingClassroom = await _context.Classrooms.FindAsync(classroom.ClassroomId);

            if (existingClassroom != null)
            {
                _context.Entry(existingClassroom).CurrentValues.SetValues(classroom);
            }
            else
            {
                await _context.Classrooms.AddAsync(classroom);
            }
        }

        await _context.SaveChangesAsync();
    }

    private async Task PutLessonsTime(List<LessonsTime> lessonsTimes)
    {
        foreach (var lessonsTime in lessonsTimes)
        {
            var existingLessonsTime = await _context.LessonsTimes.FindAsync(lessonsTime.LessonId);

            if (existingLessonsTime != null)
            {
                _context.Entry(existingLessonsTime).CurrentValues.SetValues(lessonsTime);
            }
            else
            {
                await _context.LessonsTimes.AddAsync(lessonsTime);
            }
        }

        await _context.SaveChangesAsync();
    }

    private async Task PutDisciplines(List<Discipline> disciplines)
    {
        foreach (var discipline in disciplines)
        {
            var existingDiscipline = await _context.Disciplines.FindAsync(discipline.DisciplineId);

            if (existingDiscipline != null)
            {
                _context.Entry(existingDiscipline).CurrentValues.SetValues(discipline);
            }
            else
            {
                await _context.Disciplines.AddAsync(discipline);
            }
        }

        await _context.SaveChangesAsync();
    }

    private async Task PutOtherDisciplines(List<OtherDisciplineResponse> otherDisciplines)
    {
        var newOtherDisciplines = new List<OtherDiscipline>();
        foreach (var el in otherDisciplines)
        {
            var newOtherDiscipline = await ConvertOtherDisciplineResponseToOtherDiscipline(el);
            newOtherDisciplines.Add(newOtherDiscipline);
        }
        
        foreach (var otherDiscipline in newOtherDisciplines)
        {
            var existingDiscipline = await _context.OtherDisciplines.FindAsync(otherDiscipline.OtherDisciplineId);

            if (existingDiscipline != null)
            {
                _context.Entry(existingDiscipline).CurrentValues.SetValues(otherDiscipline);
            }
            else
            {
                await _context.OtherDisciplines.AddAsync(otherDiscipline);
            }
        }

        await _context.SaveChangesAsync();
    }
    
    private async Task PutQueries(List<Query> queries)
    {
        foreach (var query in queries)
        {
            var existingQuery = await _context.Queries.FindAsync(query.QueryId);

            if (existingQuery != null)
            {
                _context.Entry(existingQuery).CurrentValues.SetValues(query);
            }
            else
            {
                await _context.Queries.AddAsync(query);
            }
        }

        await _context.SaveChangesAsync();
    }

    private async Task PutSchedules(IReadOnlyCollection<ScheduleResponse> schedules)
    {
        const int batchSize = 10;

        for (var i = 0; i < schedules.Count; i += batchSize)
        {
            var batch = schedules.Skip(i).Take(batchSize).ToList();

            var newSchedules = new List<Schedule>();
            foreach (var el in batch)
            {
                var newSchedule = await ConvertScheduleResponseToSchedule(el);
                newSchedules.Add(newSchedule);
            }

            foreach (var schedule in newSchedules)
            {
                await PutSchedule(schedule);
                await PutScheduleGroups(schedule);
                await PutScheduleTeachers(schedule);
            }

            await _context.SaveChangesAsync();
        }
    }

    private async Task PutSchedule(Schedule schedule)
    {
        var existingSchedule = await _context.Schedules.FirstOrDefaultAsync(s => s.ScheduleId == schedule.ScheduleId);

        if (existingSchedule != null)
        {
            _context.Entry(existingSchedule).CurrentValues.SetValues(schedule);
        }
        else
        {
            await _context.Schedules.AddAsync(schedule);
        }
    }

    private async Task PutScheduleGroups(Schedule schedule)
    {
        var newScheduleGroups = new List<ScheduleGroup>();
        foreach (var scheduleGroup in schedule.ScheduleGroups)
        {
            var existingScheduleGroup =
                await _context.ScheduleGroups.FirstOrDefaultAsync(sg =>
                    sg.ScheduleId == scheduleGroup.ScheduleId && sg.GroupId == scheduleGroup.GroupId);

            if (existingScheduleGroup != null)
            {
                _context.Entry(existingScheduleGroup).CurrentValues.SetValues(scheduleGroup);
            }
            else
            {
                newScheduleGroups.Add(scheduleGroup);
            }
        }

        await _context.ScheduleGroups.AddRangeAsync(newScheduleGroups);
    }

    private async Task PutScheduleTeachers(Schedule schedule)
    {
        var newScheduleTeachers = new List<ScheduleTeacher>();
        foreach (var scheduleTeacher in schedule.ScheduleTeachers)
        {
            var existingScheduleTeacher =
                await _context.ScheduleTeachers.FirstOrDefaultAsync(st =>
                    st.ScheduleId == scheduleTeacher.ScheduleId && st.TeacherId == scheduleTeacher.TeacherId);

            if (existingScheduleTeacher != null)
            {
                _context.Entry(existingScheduleTeacher).CurrentValues.SetValues(scheduleTeacher);
            }
            else
            {
                newScheduleTeachers.Add(scheduleTeacher);
            }
        }

        await _context.ScheduleTeachers.AddRangeAsync(newScheduleTeachers);
    }

    private async Task<List<ScheduleTeacher>> FindScheduleTeachers(ScheduleResponse schedule)
    {
        if (schedule.TeachersIds == null) return new List<ScheduleTeacher>();

        var scheduleTeachers = new List<ScheduleTeacher>();
        foreach (var teacherId in schedule.TeachersIds)
        {
            if (teacherId != null && await _context.Teachers.AnyAsync(t => t.TeacherId == teacherId))
            {
                scheduleTeachers.Add(new ScheduleTeacher
                {
                    ScheduleId = schedule.ScheduleId,
                    TeacherId = teacherId.Value
                });
            }
        }

        return scheduleTeachers.DistinctBy(st => new { st.ScheduleId, st.TeacherId }).ToList();
    }

    private async Task<List<ScheduleGroup>> FindScheduleGroups(ScheduleResponse schedule)
    {
        if (schedule.GroupsIds == null) return new List<ScheduleGroup>();

        var scheduleGroups = new List<ScheduleGroup>();
        foreach (var groupId in schedule.GroupsIds)
        {
            if (groupId != null && await _context.Groups.AnyAsync(g => g.GroupId == groupId))
            {
                scheduleGroups.Add(new ScheduleGroup
                {
                    ScheduleId = schedule.ScheduleId,
                    GroupId = groupId.Value
                });
            }
        }

        return scheduleGroups.DistinctBy(sg => new { sg.ScheduleId, sg.GroupId }).ToList();
    }

    private async Task<int?> FindClassroomId(ScheduleResponse schedule)
    {
        if (schedule.ClassroomsIds == null) return null;

        var classroom =
            await _context.Classrooms.FirstOrDefaultAsync(c => schedule.ClassroomsIds.Contains(c.ClassroomId));

        return classroom?.ClassroomId;
    }

    private async Task<OtherDiscipline> ConvertOtherDisciplineResponseToOtherDiscipline(OtherDisciplineResponse otherDisciplineResponse)
    {
        var projfairProject = otherDisciplineResponse is { Type: OtherDisciplineType.Project, IsActive: true }
            ? await ProjfairClient.FindProject(otherDisciplineResponse.DisciplineTitle)
            : null;

        return new OtherDiscipline()
        {
            OtherDisciplineId = otherDisciplineResponse.OtherDisciplineId,
            DisciplineTitle = otherDisciplineResponse.DisciplineTitle,
            IsOnline = otherDisciplineResponse.IsOnline,
            Type = otherDisciplineResponse.Type,
            IsActive = otherDisciplineResponse.IsActive,
            ProjectActive = otherDisciplineResponse.ProjectActive,
            ProjfairProjectId = projfairProject?.Id
        };
    }

    private async Task<Schedule> ConvertScheduleResponseToSchedule(ScheduleResponse scheduleResponse)
    {
        var classroomId = await FindClassroomId(scheduleResponse);
        var scheduleGroups = await FindScheduleGroups(scheduleResponse);
        var scheduleTeachers = await FindScheduleTeachers(scheduleResponse);
        var otherDiscipline = await _context.OtherDisciplines.FindAsync(scheduleResponse.OtherDisciplineId);

        return new Schedule
        {
            ScheduleId = scheduleResponse.ScheduleId,
            GroupsVerbose = scheduleResponse.GroupsVerbose,
            TeachersVerbose = scheduleResponse.TeachersVerbose,
            ClassroomId = classroomId,
            ClassroomVerbose = scheduleResponse.ClassroomsVerbose,
            DisciplineId = scheduleResponse.DisciplineId,
            DisciplineVerbose = scheduleResponse.DisciplineVerbose,
            OtherDisciplineId = scheduleResponse.OtherDisciplineId,
            QueryId = scheduleResponse.QueryId,
            LessonId = scheduleResponse.LessonId,
            Subgroup = scheduleResponse.Subgroup,
            LessonType = scheduleResponse.LessonType != LessonType.Unknown ? scheduleResponse.LessonType :
                otherDiscipline is { Type: OtherDisciplineType.Project, IsActive: true } ? LessonType.Project :
                LessonType.Unknown,
            Date = DateOnly.ParseExact(scheduleResponse.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture),
            ScheduleGroups = scheduleGroups,
            ScheduleTeachers = scheduleTeachers
        };
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
using API.DataBase.Context;
using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// builder.Services.AddApiVersioning(options =>
// {
//     options.DefaultApiVersion = new ApiVersion(1);
//     options.ReportApiVersions = true;
//     options.AssumeDefaultVersionWhenUnspecified = true;
//     options.ApiVersionReader = ApiVersionReader.Combine(
//         new UrlSegmentApiVersionReader(),
//         new HeaderApiVersionReader("api-version"));
// }).AddApiExplorer(options =>
// {
//     options.GroupNameFormat = "'v'V";
//     options.SubstituteApiVersionInUrl = true;
// });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "ISTU Schedule API",
            Description = "This API let getting ISTU schedule",
            Version = "1.0"
        });
        // options.SwaggerDoc("v2", new OpenApiInfo
        // {
        //     Title = "ISTU Schedule API",
        //     Description = "This API let getting ISTU schedule",
        //     Version = "2.0"
        // });
        options.EnableAnnotations();
    });

builder.Services.AddDbContext<ScheduleContext>(
    options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("ScheduleDB"));
    });

builder.Services.AddControllers().AddNewtonsoftJson(
    options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate;
    });

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    // options.SwaggerEndpoint("/swagger/v2/swagger.json", "v2");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
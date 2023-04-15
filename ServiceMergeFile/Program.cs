using Microsoft.OpenApi.Models;
using System.Reflection;
using ServiceMergeFile;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Объединение файлов",
        Version = "1.0.0.0",
        Description = ""
    });
    var basePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
    var xmlPath = Path.Combine(basePath, "ServiceMergeFile.xml");
    options.IncludeXmlComments(xmlPath);
});

builder.Services.AddService();//регистрации сервисов проекта

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.MapControllers();

app.Run();

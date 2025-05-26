using PRN232_SchoolMedicalAPI;
using SchoolMedical_DataAccess.DTOModels;
using PRN232_SchoolMedicalAPI.Helpers;
using SchoolMedical_BusinessLogic;
using SchoolMedical_DataAccess.Entities;
using SchoolMedical_DataAccess.Data;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddApplication(builder.Configuration);

builder.Services.AddControllers(op =>
{
	op.Filters.Add(new ResultManipulator());
});


// Set the minimum level to Debug
builder.Logging.SetMinimumLevel(LogLevel.Debug);

var app = builder.Build();

app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseMiddleware<ResponseHandlerMiddleware>();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<SchoolhealthdbContext>();

    await context.Database.EnsureCreatedAsync();
    await SeedData.SeedAsync(context);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

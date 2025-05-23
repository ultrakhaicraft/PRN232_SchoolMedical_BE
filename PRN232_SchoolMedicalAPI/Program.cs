using PRN232_SchoolMedicalAPI;
using SchoolMedical_DataAccess.DTOModels;
using PRN232_SchoolMedicalAPI.Helpers;
using SchoolMedical_BusinessLogic;

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

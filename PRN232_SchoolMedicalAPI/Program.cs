using PRN232_SchoolMedicalAPI;
using SchoolMedical_DataAccess.DTOModels;
using PRN232_SchoolMedicalAPI.Helpers;
using SchoolMedical_BusinessLogic;
using SchoolMedical_DataAccess.Entities;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddApplication(builder.Configuration);

builder.Services.AddControllers(op =>
{
	op.Filters.Add(new ResultManipulator());
})
	.AddJsonOptions(opt =>
	{
		opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
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

}
app.UseCors("AllowAllOrigins");

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

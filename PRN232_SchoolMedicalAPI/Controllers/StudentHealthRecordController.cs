using Microsoft.AspNetCore.Mvc;
using PRN232_SchoolMedicalAPI.Helpers;
using SchoolMedical_BusinessLogic.Core;
using SchoolMedical_BusinessLogic.Interface;
using SchoolMedical_DataAccess.DTOModels;

namespace PRN232_SchoolMedicalAPI.Controllers;

[ApiController]
[Route("api/student-health-record")]
//[Authorize]
public class StudentHealthRecordController : Controller
{

	private readonly IStudentHealthRecordService _studentHealthRecordService;

	public StudentHealthRecordController(IStudentHealthRecordService studentHealthRecordService)
	{
		_studentHealthRecordService = studentHealthRecordService;
	}

	[HttpGet("get-all")]
	public async Task<IActionResult> GetAllRecord([FromQuery] StudentHealthRecordQuery request)
	{

		var records = await _studentHealthRecordService.GetAllRecords(request);
		HttpContext.Items["CustomMessage"] = "Get all records successfully";
		return Ok(records);

	}
	[HttpGet("get-detail")]
	public async Task<IActionResult> GetRecordDetailByID([FromQuery] string recordId)
	{
		var record = await _studentHealthRecordService.GetRecordByIdAsync(recordId);
		HttpContext.Items["CustomMessage"] = "Displaying the record details";
		return Ok(record);
	}

	[HttpPost("create-record")]
	public async Task<IActionResult> CreateRecord([FromBody] StudentHealthRecordCreateModel record)
	{
		var createdBy = User.Claims.GetUserIdFromJwtToken() ?? "Unknown";
		var recordId = await _studentHealthRecordService.CreateRecordAsync(record, createdBy);
		HttpContext.Items["CustomMessage"] = "Record created successfully";
		return CreatedAtAction(nameof(GetRecordDetailByID), new { recordId }, recordId);
	}

	[HttpPut("update-record")]
	public async Task<IActionResult> UpdateRecord([FromBody] StudentHealthRecordUpdateModel record, [FromQuery] string recordId)
	{
		var createdBy = User.Claims.GetUserIdFromJwtToken() ?? "Unknown";
		await _studentHealthRecordService.UpdateRecordAsync(record, recordId, createdBy);
		HttpContext.Items["CustomMessage"] = "Record updated successfully";
		return Ok();
	}

	[HttpDelete("delete-record")]
	public async Task<IActionResult> DeleteRecord([FromQuery] string recordId)
	{
		await _studentHealthRecordService.DeleteRecordAsync(recordId);
		HttpContext.Items["CustomMessage"] = "Record deleted successfully";
		return Ok();
	}
}

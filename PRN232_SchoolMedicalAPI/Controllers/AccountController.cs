using Microsoft.AspNetCore.Mvc;
using SchoolMedical_BusinessLogic.Interface;
using SchoolMedical_DataAccess.DTOModels;
using SchoolMedical_DataAccess.Enums;

namespace PRN232_SchoolMedicalAPI.Controllers;

[ApiController]
[Route("api/account")]
//[Authorize]
public class AccountController : ControllerBase
{
	private readonly IAccountService _accountService;
	public AccountController(IAccountService accountService)
	{
		_accountService = accountService;
	}

	[HttpGet("get-all")]
	public async Task<IActionResult> GetAllAccount([FromQuery] AccountQuery request)
	{

		var accounts = await _accountService.GetAllAccount(request);
		HttpContext.Items["CustomMessage"] = "Login successfully, granting token";
		return Ok(accounts);

	}
	[HttpGet("get-detail")]
	public async Task<IActionResult> GetAccountDetailByID([FromQuery] string userId)
	{
		var accounts = await _accountService.GetAccountDetailById(userId);
		HttpContext.Items["CustomMessage"] = "Login successfully, granting token";
		return Ok(accounts);
	}

	[HttpPost("create-account")]
	public async Task<IActionResult> CreateNewAccount([FromBody] AccountCreateRequest request )
	{
		string id = await _accountService.CreateNewAccount(request);
		HttpContext.Items["CustomMessage"] = "Account created successfully!";
		return Ok(id);
	}

	[HttpPut("update-account")]
	public async Task<IActionResult> UpdateAccount([FromBody] AccountUpdateRequest request, string userId)
	{
		await _accountService.UpdateAccount(userId, request);
		HttpContext.Items["CustomMessage"] = "Account updated successfully!";
		return Ok(userId);
	}

	[HttpDelete("delete-account")]
	public async Task<IActionResult> SoftDeleteAccount([FromQuery] string userId)
	{
		await _accountService.SoftDeleteAccount(userId);
		HttpContext.Items["CustomMessage"] = "Account deleted successfully!";
		return Ok(userId);
	}

	[HttpPatch("change-status")]
	public async Task<IActionResult> ChangeAccountStatus([FromQuery] string userId, [FromQuery] AccountStatus status )
	{
		await _accountService.ChangeAccountStatus(userId, status);
		HttpContext.Items["CustomMessage"] = "Account status changed successfully!";
		return Ok(userId);
	}

	
}

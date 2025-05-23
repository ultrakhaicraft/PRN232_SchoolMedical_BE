using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedical_BusinessLogic.Interface;
using SchoolMedical_DataAccess.DTOModels;


namespace PRN232_SchoolMedicalAPI.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
	private readonly IAuthService _authService;

	public AuthController(IAuthService authService)
	{
		_authService = authService;
	}

	[AllowAnonymous]
	[HttpPost("login")]
	public async Task<IActionResult> Login([FromBody] LoginRequest request)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest();
		}
		var data = await _authService.Login(request);
		return Ok(data);
	}

	

	[AllowAnonymous]
	[HttpPost("register")]
	public async Task<IActionResult> RegisterAsParent([FromBody] RegisterRequest request, bool IsParent)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest();
		}
		var result = await _authService.RegisteAsync(request, IsParent);
		return Ok(result);

	}
}

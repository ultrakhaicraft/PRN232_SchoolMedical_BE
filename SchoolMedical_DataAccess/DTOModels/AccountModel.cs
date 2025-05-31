using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolMedical_DataAccess.DTOModels;

public class AccountDetailModel
{
	public string Id { get; set; } = null!;
	public string? ParentId { get; set; }
	public string? ParentName { get; set; }
	public string FullName { get; set; } = null!;
	public string Email { get; set; } = null!;
	public string? PhoneNumber { get; set; }
	public string Role { get; set; } = null!;
	public string? Address { get; set; }
	public string? Status { get; set; }
}

public class AccountViewModel
{
	public string Id { get; set; } = null!;	
	public string FullName { get; set; } = null!;
	public string Email { get; set; } = null!;
	public string Role { get; set; } = null!;
	public string? Status { get; set; }
}

public class AccountCreateRequest
{
	[Required]
	public string FullName { get; set; } = null!;
	[Required]
	public string Email { get; set; } = null!;
	[Required]
	public string Password { get; set; } = null!;
	[Required]
	public string? PhoneNumber { get; set; } = null!;
	[Required]
	public string Role { get; set; } = null!;
	[Required]
	public string? Address { get; set; } = null!;

	public string? ParentId { get; set; }
}

public class AccountUpdateRequest
{
	[Required]
	public string FullName { get; set; } = null!;
	[Required]
	public string Email { get; set; } = null!;
	[Required]
	public string Password { get; set; } = null!;
	[Required]
	public string? PhoneNumber { get; set; } = null!;
	[Required]
	public string Role { get; set; } = null!;
	[Required]
	public string? Address { get; set; } = null!;

	public string? ParentId { get; set; }
}


using SchoolMedical_BusinessLogic.Interface;
using SchoolMedical_BusinessLogic.Utility;
using SchoolMedical_DataAccess.DTOModels;
using SchoolMedical_DataAccess.Entities;
using SchoolMedical_DataAccess.Enums;
using SchoolMedical_DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SchoolMedical_BusinessLogic.Core;

public class AccountService : IAccountService
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IGenericRepository<Account> _accountRepository;
	

	public AccountService(IUnitOfWork unitOfWork, IGenericRepository<Account> accountRepository)
	{
		_unitOfWork = unitOfWork;
		_accountRepository = accountRepository;
	}

	public Task ChangeAccountStatus(string userId, AccountStatus status)
	{
		try
		{
			var account = _unitOfWork.GetRepository<Account>().Find(user => user.Id == userId && user.Status != AccountStatus.Inactive.ToString());
			if (account == null)
			{
				throw new AppException("Account not found or already inactive.");
			}
			account.Status = status.ToString();
			_unitOfWork.GetRepository<Account>().Update(account);
			_unitOfWork.Save();
			return Task.CompletedTask;
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw new Exception(e.Message);
		}
	}

	public async Task<string> CreateNewAccount(AccountCreateRequest request)
	{
		try
		{
			if (!IsValid(request.Password))
			{
				throw new AppException("Password does not meet the required criteria.");
			}

			var account = new Account
			{
				Id = Guid.NewGuid().ToString(),
				FullName = request.FullName,
				Email = request.Email,
				PhoneNumber = request.PhoneNumber,
				Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
				Address = request.Address,
				Role = request.Role,
				Status = AccountStatus.Active.ToString(),
				ParentId = request.ParentId
			};
			
			await _unitOfWork.GetRepository<Account>().InsertAsync(account);
			await _unitOfWork.SaveAsync();
			return account.Id;
		}
		catch (Exception e)
		{

			Console.WriteLine(e);
			throw new Exception(e.Message);
		}
	}

	public async Task<AccountDetailModel> GetAccountDetailById(string userId)
	{
		try
		{
			var accounts= await  _unitOfWork.GetRepository<Account>().FindAsync(user => user.Id == userId);
			if (accounts == null)
			{
				throw new AppException("Account not found.");
			}
			if (accounts.Status == AccountStatus.Inactive.ToString())
			{
				throw new AppException("Account is inactive.");
			}

			return new AccountDetailModel
			{
				Id = accounts.Id,
				FullName = accounts.FullName,
				Email = accounts.Email,
				PhoneNumber = accounts.PhoneNumber,
				Address = accounts.Address,
				Role = accounts.Role,
				Status = accounts.Status,
				ParentId = accounts.ParentId,
				ParentName = accounts.Parent != null ? accounts.Parent.FullName : null
			};

		}
		catch (Exception e)
		{

			Console.WriteLine(e);
			throw new Exception(e.Message);
		}
	}

	public async Task<PagingModel<AccountViewModel>> GetAllAccount(AccountQuery request)
	{
		try
		{
			var query = await _unitOfWork.GetRepository<Account>().GetAllAsync();
			
			// Filter Based on Status (using string-to-enum conversion)
			if (!string.IsNullOrEmpty(request.Status.ToString()))
			{
				if (Enum.TryParse<AccountStatus>(request.Status.ToString(), true, out var parsedStatus))
				{
					query = query.Where(account => account.Status == parsedStatus.ToString());
				}
			}

			// Filter Based on Role (using string-to-enum conversion)
			if (!string.IsNullOrEmpty(request.Role.ToString()))
			{
				if (Enum.TryParse<AccountRole>(request.Role.ToString(), true, out var parsedRole))
				{
					query = query.Where(account => account.Role == parsedRole.ToString());
				}
			}

			// Search Based on FullName (case-insensitive search using ToLower)
			if (!string.IsNullOrEmpty(request.FullName))
			{
				string nameFilter = request.FullName.ToLower();
				query = query.Where(account => account.FullName != null && account.FullName.ToLower().Contains(nameFilter));
			}

			if (query == null || !query.Any())
			{
				throw new AppException("No accounts found.");
			}

			var accountViews = query.Select(account => new AccountViewModel
			{
				Id = account.Id,
				FullName = account.FullName,
				Email = account.Email,
				Role = account.Role,
				Status = account.Status,
			});

			var pagingModel = await PagingExtension.ToPagingModel<AccountViewModel>(accountViews, request.PageNumber, request.PageSize); // Default page index and size

			return new PagingModel<AccountViewModel>
			{
				PageIndex = pagingModel.PageIndex,
				PageSize = pagingModel.PageSize,
				TotalCount = pagingModel.TotalCount,
				TotalPages = pagingModel.TotalPages,
				Data = pagingModel.Data
			};
		}
		catch (Exception e)
		{

			Console.WriteLine(e);
			throw new Exception(e.Message);
		}
	}

	public Task SoftDeleteAccount(string userId)
	{
		try
		{

			var account = _unitOfWork.GetRepository<Account>().Find(user => user.Id == userId && user.Status != AccountStatus.Inactive.ToString());
			if (account == null)
			{
				throw new Exception("Account not found or already inactive.");
			}
			account.Status = AccountStatus.Inactive.ToString();
			_unitOfWork.GetRepository<Account>().Update(account);
			_unitOfWork.Save();
			return Task.CompletedTask; 
		}
		catch (Exception e)
		{

			Console.WriteLine(e);
			throw new Exception(e.Message);
		}
	}

	public Task UpdateAccount(string userId, AccountUpdateRequest request)
	{
		try
		{
			var account = _unitOfWork.GetRepository<Account>().Find(user => user.Id == userId && user.Status != AccountStatus.Inactive.ToString());
			if (account == null)
			{
				throw new AppException("Account not found or already inactive.");
			}
			account.FullName = request.FullName;
			account.Email = request.Email;
			account.PhoneNumber = request.PhoneNumber;
			if(!IsValid(request.Password))
			{
				throw new AppException("Password does not meet the required criteria.");
			}
			account.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
			account.Address = request.Address;
			account.Role = request.Role;
			account.ParentId = request.ParentId;
			_unitOfWork.GetRepository<Account>().Update(account);
			_unitOfWork.Save();
			return Task.CompletedTask;
		}
		catch (Exception e)
		{

			Console.WriteLine(e);
			throw new Exception(e.Message);
		}
	}

	public async Task<AccountDetailModel> getStudentDetail(string parentId)
	{
		try
		{
			//Get student id through parent id
			var accounts = await _unitOfWork.GetRepository<Account>().FindAsync(user => user.ParentId == parentId);
			if (accounts == null)
			{
				throw new AppException("Account not found.");
			}
			if(accounts.Role != AccountRole.Student.ToString())
			{
				throw new AppException("Account is not a student.");
			}
			if (accounts.Status == AccountStatus.Inactive.ToString())
			{
				throw new AppException("Account is inactive.");
			}

			return new AccountDetailModel
			{
				Id = accounts.Id,
				FullName = accounts.FullName,
				Email = accounts.Email,
				PhoneNumber = accounts.PhoneNumber,
				Address = accounts.Address,
				Role = accounts.Role,
				Status = accounts.Status,
				ParentId = accounts.ParentId,
				ParentName = accounts.Parent != null ? accounts.Parent.FullName : null
			};

		}
		catch (Exception e)
		{

			Console.WriteLine(e);
			throw new Exception(e.Message);
		}
	}

	//private methods
	public bool IsValid(string password)
	{
		if (password.Length < 8) return false;

		if (!Regex.IsMatch(password, @"[a-zA-Z]")) return false;

		if (!Regex.IsMatch(password, @"[0-9]")) return false;

		if (!Regex.IsMatch(password, @"[@#$%^&*!_]")) return false;

		return true;
	}
}

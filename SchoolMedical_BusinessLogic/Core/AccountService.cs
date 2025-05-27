using SchoolMedical_BusinessLogic.Interface;
using SchoolMedical_DataAccess.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolMedical_BusinessLogic.Core;

public class AccountService : IAccountService
{
	public Task<UserDetailModel> GetUserDetailByIdAsync(string userId)
	{
		throw new NotImplementedException();
	}
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SchoolMedical_BusinessLogic.Interface;

public interface IJwtUtils
{
	public JWTToken GenerateToken(IEnumerable<Claim> claims, JwtModel? jwtModel, User user);
}

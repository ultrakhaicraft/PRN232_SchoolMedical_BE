using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolMedical_DataAccess.Enums;

public enum AccountRole
{
	Student, 
	Parent, 
	SchoolNurse, 
	Manager, 
	Admin
}

public enum AccountStatus
{
	Active,
	Inactive, //Soft Delete
}

public enum IsAvailable
{
	Yes=1,
	No=0
}

public enum RecordStatus
{
	Active,
	Inactive, //Soft Delete
}

public enum RequestStatus
{
	Pending,
	Approved,
	Rejected,
	Deleted, //Soft Delete
}

//Become deleted through Soft Delete API
public enum EventStatus
{
	Upcoming,
	Ongoing,
	Completed,
	Cancelled,
	
}



﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolMedical_DataAccess.Interfaces;

public interface IUnitOfWork : IDisposable
{
	IGenericRepository<T> GetRepository<T>() where T : class;
	void Save();
	Task SaveAsync();
	void BeginTransaction();
	void CommitTransaction();
	void RollBack();

}

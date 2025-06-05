using Org.BouncyCastle.Asn1.Ocsp;
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
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SchoolMedical_BusinessLogic.Core
{
	public class StudentHealthRecordService : IStudentHealthRecordService
	{
		private readonly IUnitOfWork _unitOfWork;

		public StudentHealthRecordService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<string> CreateRecordAsync(StudentHealthRecordCreateModel record, string createdBy)
		{
			try
			{
				await Task.Delay(100);
				var studentHealthRecord = new Studenthealthrecord
				{
					Id = Guid.NewGuid().ToString(),
					StudentId = record.StudentId,
					Height = record.Height,
					Allergies = record.Allergies,
					ChronicDiseases = record.ChronicDiseases,
					Vision = record.Vision,
					Hearing = record.Hearing,
					Status = record.Status,
					CreatedBy = createdBy, // Assuming CreatedBy is a property in the create model
					
				};

				await _unitOfWork.GetRepository<Studenthealthrecord>().InsertAsync(studentHealthRecord);
				await _unitOfWork.SaveAsync(); // Ensure the save operation completes
				return studentHealthRecord.Id;
			}
			catch (Exception e)
			{
				Console.WriteLine("Error creating student health record: " + e.Message);
				throw new Exception("An error occurred while creating the student health record.", e);
			}
		}

		public async Task DeleteRecordAsync(string recordId)
		{
			try
			{
				await Task.Delay(100);
				var record = _unitOfWork.GetRepository<Studenthealthrecord>().GetById(recordId);
				if (record == null)
				{
					throw new Exception("Student health record not found.");
				}
				_unitOfWork.GetRepository<Studenthealthrecord>().Delete(record);
				await _unitOfWork.SaveAsync(); // Ensure the save operation completes
				return;

			}
			catch (Exception e)
			{
				Console.WriteLine("Error deleting student health record: " + e.Message);
				throw new Exception("An error occurred while deleting the student health record.", e);
			}
		}

		public async Task<PagingModel<StudentHealthRecordViewModel>> GetAllRecords(StudentHealthRecordQuery recordQuery)
		{
			try
			{
				await Task.Delay(100);
				var records = _unitOfWork.GetRepository<Studenthealthrecord>().GetAll();
				

				//Search by student name by accessing Student Object
				if (!String.IsNullOrEmpty(recordQuery.StudentName))
				{
					records = records.Where(r => r.Student.FullName.Contains(recordQuery.StudentName, StringComparison.OrdinalIgnoreCase));
				}

				//Filter by status
				// Filter Based on Status (using string-to-enum conversion)
				if (!string.IsNullOrEmpty(recordQuery.Status.ToString()))
				{
					if (Enum.TryParse<AccountStatus>(recordQuery.Status.ToString(), true, out var parsedStatus))
					{
						records = records.Where(account => account.Status == parsedStatus.ToString());
					}
				}

				if (records == null || !records.Any())
				{
					throw new Exception("No student health records found.");
				}

				var recordResponse = records.Select(record => new StudentHealthRecordViewModel
				{
					Id = record.Id,
					StudentId = record.StudentId,
					StudentName = record.Student.FullName, // Assuming Student has a FullName property
					CreatedBy = record.CreatedByNavigation.FullName, // Assuming CreatedByNavigation has a FullName property
					Status = record.Status
				});

				var pagingModel = await PagingExtension.ToPagingModel<StudentHealthRecordViewModel>(recordResponse, recordQuery.PageNumber, recordQuery.PageNumber);

				return new PagingModel<StudentHealthRecordViewModel>
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
				Console.WriteLine("Error getting all student health record: " + e.Message);
				throw new Exception("An error occurred while getting all the student health record.", e);
			}
		}

		public async Task<StudentHealthRecordDetailModel> GetRecordByIdAsync(string recordId)
		{
			try
			{
				await Task.Delay(100); 
				var record = _unitOfWork.GetRepository<Studenthealthrecord>().GetById(recordId);

				if (record == null)
				{
					throw new Exception("Student health record not found.");
				}

				var recordResponse = new StudentHealthRecordDetailModel
				{
					Id = record.Id,
					StudentId = record.StudentId,
					StudentName = record.Student.FullName, // Assuming Student has a Name property
					CreatedBy = record.CreatedByNavigation.FullName, // Assuming CreatedByNavigation has a Name property
					Height = record.Height,
					Allergies = record.Allergies,
					ChronicDiseases = record.ChronicDiseases,
					Vision = record.Vision,
					Hearing = record.Hearing,
					Status = record.Status,
					vaccineRecordViewModels = record.Vaccinerecords.Select(v => new VaccineRecordViewModel
					{
						Id = v.Id,
						StudentId = v.StudentId,
						StudentName = v.Student.FullName, // Assuming Student has a FullName property
						RecordDate = v.RecordDate,
						Vaccine = v.Vaccine,
						Status = v.Status
					}).ToList(),
					treatmentRecordViewModels = record.Treatmentrecords.Select(t => new TreatmentRecordViewModel
					{
						Id = t.Id,
						StudentId = t.StudentId,
						StudentName = t.Student.FullName, // Assuming Student has a FullName property
						RecordDate = t.RecordDate,
						Treatment = t.Treatment,
						Status = t.Status
					}).ToList()

				};

				return recordResponse;


			}
			catch (Exception e)
			{
				Console.WriteLine("Error creating student health record: " + e.Message);
				throw new Exception("An error occurred while creating the student health record.", e);
			}
		}

		public async Task UpdateRecordAsync(StudentHealthRecordUpdateModel record, string recordId,string createdBy)
		{
			try
			{
				var existingRecord =  await _unitOfWork.GetRepository<Studenthealthrecord>().GetByIdAsync(recordId);

				existingRecord!.StudentId = record.StudentId?? "";
				existingRecord.Height = record.Height;
				existingRecord.Allergies = record.Allergies;
				existingRecord.ChronicDiseases = record.ChronicDiseases;
				existingRecord.Vision = record.Vision;
				existingRecord.Hearing = record.Hearing;
				existingRecord.Status = record.Status;
				existingRecord.CreatedBy = createdBy; // Assuming CreatedBy is a property in the update model


				await _unitOfWork.GetRepository<Studenthealthrecord>().UpdateAsync(existingRecord);
				await _unitOfWork.SaveAsync(); // Ensure the save operation completes
				return;
			}
			catch (Exception e)
			{
				Console.WriteLine("Error creating student health record: " + e.Message);
				throw new Exception("An error occurred while creating the student health record.", e);
			}
		}
	}
}

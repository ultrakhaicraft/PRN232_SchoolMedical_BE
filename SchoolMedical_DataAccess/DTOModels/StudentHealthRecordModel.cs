using SchoolMedical_DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolMedical_DataAccess.DTOModels;

public class StudentHealthRecordDetailModel
{
	public string Id { get; set; } = null!;

	public string StudentId { get; set; } = null!;

	public string StudentName { get; set; } = null!;

	public string CreatedBy { get; set; } = null!;

	public int? Height { get; set; }

	public string? Allergies { get; set; }

	public string? ChronicDiseases { get; set; }

	public string? Vision { get; set; }

	public string? Hearing { get; set; }

	public string? Status { get; set; }

	public List<VaccineRecordViewModel> vaccineRecordViewModels { get; set; } = new List<VaccineRecordViewModel>();
	public List<TreatmentRecordViewModel> treatmentRecordViewModels { get; set; } = new List<TreatmentRecordViewModel>();

}

public class StudentHealthRecordViewModel
{
	public string Id { get; set; } = null!;

	public string StudentId { get; set; } = null!;

	public string StudentName { get; set; } = null!;

	public string CreatedBy { get; set; } = null!;
	public string? Status { get; set; }
}

public class StudentHealthRecordCreateModel
{
	public string Id { get; set; } = null!;

	public string StudentId { get; set; } = null!;

	public string StudentName { get; set; } = null!;

	public string CreatedBy { get; set; } = null!;

	public int? Height { get; set; }

	public string? Allergies { get; set; }

	public string? ChronicDiseases { get; set; }

	public string? Vision { get; set; }

	public string? Hearing { get; set; }

	public string? Status { get; set; }
}

public class StudentHealthRecordUpdateModel
{
	public string Id { get; set; } = null!;

	public string StudentId { get; set; } = null!;

	public string StudentName { get; set; } = null!;

	public string CreatedBy { get; set; } = null!;

	public int? Height { get; set; }

	public string? Allergies { get; set; }

	public string? ChronicDiseases { get; set; }

	public string? Vision { get; set; }

	public string? Hearing { get; set; }

	public string? Status { get; set; }
}

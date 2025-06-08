using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolMedical_DataAccess.DTOModels
{
    public class IncidentRecordDetailModel
    {
        public string Id { get; set; } = null!;

        public string StudentId { get; set; } = null!;

        public string HandleBy { get; set; } = null!;

        public string? IncidentType { get; set; }

        public string? Description { get; set; }

        public DateTime DateOccurred { get; set; }

        public string? Status { get; set; }
    }

    public class IncidentRecordCreateRequest
    {
        public string StudentId { get; set; } = null!;

        public string HandleBy { get; set; } = null!;

        public string? IncidentType { get; set; }

        public string? Description { get; set; }

        public DateTime DateOccurred { get; set; }

        public string? Status { get; set; }
    }

    public class IncidentRecordUpdateRequest
    {
        public string StudentId { get; set; } = null!;

        public string HandleBy { get; set; } = null!;

        public string? IncidentType { get; set; }

        public string? Description { get; set; }

        public DateTime DateOccurred { get; set; }

        public string? Status { get; set; }
    }
    
    public class IncidentRecordViewModel
    {
        public string Id { get; set; } = null!;

        public string StudentId { get; set; } = null!;

        public string StudentName { get; set; } = null!;

        public string? IncidentType { get; set; }

        public DateTime DateOccurred { get; set; }

        public string? Status { get; set; }
    }
}

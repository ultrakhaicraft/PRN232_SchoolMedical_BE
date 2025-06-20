﻿using SchoolMedical_DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolMedical_DataAccess.DTOModels
{
    public class MedicalSupplyDTO
    {
        public string? Id { get; set; }

        public string? CreatedBy { get; set; }

        public string? Name { get; set; } 

        public string? Description { get; set; }

        public int Amount { get; set; }

        public bool? IsAvailable { get; set; }

        public bool IsDeleted { get; set; }

        public String? CreatedByNavigationId { get; set; } 
    }
}

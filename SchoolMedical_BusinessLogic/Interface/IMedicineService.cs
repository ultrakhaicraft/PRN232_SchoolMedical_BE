using SchoolMedical_DataAccess.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolMedical_BusinessLogic.Interface
{
    public interface IMedicineService
    {
        Task<PagingModel<MedicineResponseDto>> GetMedicinesAsync(MedicineFilterRequestDto request);
        Task<MedicineResponseDto?> GetMedicineByIdAsync(string id);
        Task<MedicineResponseDto> CreateMedicineAsync(CreateMedicineRequestDto request, string createdBy);
        Task<MedicineResponseDto> UpdateMedicineAsync(UpdateMedicineRequestDto request, string updatedBy);
        Task<bool> DeleteMedicineAsync(string id);
    }
}

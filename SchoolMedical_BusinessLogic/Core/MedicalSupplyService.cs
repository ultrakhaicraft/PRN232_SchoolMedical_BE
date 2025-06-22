using SchoolMedical_BusinessLogic.Interface;
using SchoolMedical_DataAccess.DTOModels;
using SchoolMedical_DataAccess.Entities;
using SchoolMedical_DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolMedical_BusinessLogic.Core
{
    public class MedicalSupplyService : IMedicalSupplyService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IGenericRepository<Medicalsupply> medicalSupply;

        public async Task<MedicalSupplyDTO> CreateMedicalSupplyAsync(MedicalSupplyDTO request, string createdBy)
        {
            var newSupply = new Medicalsupply
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name ?? "",
                Description = request.Description,
                Amount = request.Amount,
                IsAvailable = request.IsAvailable ?? true,
                IsDeleted = false,
                CreatedBy = createdBy
            };

            await medicalSupply.InsertAsync(newSupply);
            await unitOfWork.SaveAsync();

            return new MedicalSupplyDTO
            {
                Id = newSupply.Id,
                Name = newSupply.Name,
                Description = newSupply.Description,
                Amount = newSupply.Amount,
                IsAvailable = newSupply.IsAvailable,
                IsDeleted = newSupply.IsDeleted,
                CreatedBy = newSupply.CreatedBy
            };
        }

        public async Task<PagingModel<MedicalSupplyDTO>> GetAllMedicalSupplyAsync(MedicalSupplyDTO request)
        {
            int pageIndex = 1; 
            int pageSize = 10; 

            var allData = await medicalSupply.GetAllAsync();

            int totalCount = allData.Count();
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var pagedData = allData
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new MedicalSupplyDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Amount = x.Amount,
                    IsAvailable = x.IsAvailable,
                    IsDeleted = x.IsDeleted,
                    CreatedBy = x.CreatedBy,
                    CreatedByNavigationId = x.CreatedByNavigation.Id
                })
                .ToList();

            return new PagingModel<MedicalSupplyDTO>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                Data = pagedData
            };
        }


        public async Task<MedicalSupplyDTO?> GetMedicalSupplyByIdAsync(string id)
        {
            var entity = await medicalSupply.GetByIdAsync(id);
            if (entity == null || entity.IsDeleted)
                return null;

            return new MedicalSupplyDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Amount = entity.Amount,
                IsAvailable = entity.IsAvailable,
                IsDeleted = entity.IsDeleted,
                CreatedBy = entity.CreatedBy,
                CreatedByNavigationId = entity.CreatedByNavigation?.Id
            };
        }

        public async Task<bool> SoftDeleteMedicalSupplyAsync(string id)
        {
            var entity = await medicalSupply.GetByIdAsync(id);
            if (entity == null || entity.IsDeleted)
                return false;

            entity.IsDeleted = true;
            medicalSupply.Update(entity);
            await unitOfWork.SaveAsync();

            return true;
        }

        public async Task<MedicalSupplyDTO> UpdateMedicalSupplyAsync(MedicalSupplyDTO request, string medicineId)
        {
            var entity = await medicalSupply.GetByIdAsync(medicineId);
            if (entity == null || entity.IsDeleted)
                throw new Exception("Medical supply not found");

            entity.Name = request.Name ?? entity.Name;
            entity.Description = request.Description;
            entity.Amount = request.Amount;
            entity.IsAvailable = request.IsAvailable ?? entity.IsAvailable;

            medicalSupply.Update(entity);
            await unitOfWork.SaveAsync();

            return new MedicalSupplyDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Amount = entity.Amount,
                IsAvailable = entity.IsAvailable,
                IsDeleted = entity.IsDeleted,
                CreatedBy = entity.CreatedBy,
                CreatedByNavigationId = entity.CreatedByNavigation?.Id
            };
        }
    }
}

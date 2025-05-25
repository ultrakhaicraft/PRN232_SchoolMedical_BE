using Microsoft.EntityFrameworkCore;
using SchoolMedical_BusinessLogic.Interface;
using SchoolMedical_DataAccess.DTOModels;
using SchoolMedical_DataAccess.Entities;
using SchoolMedical_DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SchoolMedical_BusinessLogic.Core
{
    public class MedicineService : IMedicineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Medicine> _medicineRepository;

        public MedicineService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _medicineRepository = _unitOfWork.GetRepository<Medicine>();
        }

        public async Task<PagingModel<MedicineResponseDto>> GetMedicinesAsync(MedicineFilterRequestDto request)
        {
            var query = _medicineRepository.Include(m => m.CreatedByNavigation)
                .Where(m => !m.IsDeleted);

            // Apply filters
            if (!string.IsNullOrEmpty(request.Name))
            {
                query = query.Where(m => m.Name.ToLower().Contains(request.Name.ToLower()));
            }

            if (request.IsAvailable.HasValue)
            {
                query = query.Where(m => m.IsAvailable == request.IsAvailable.Value);
            }

            // Apply sorting
            query = ApplySorting(query, request.SortBy, request.IsDescending);

            // Get total count
            var totalCount = await query.CountAsync();

            // Apply paging
            var medicines = await query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(m => new MedicineResponseDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    Amount = m.Amount,
                    IsAvailable = m.IsAvailable,
                    CreatedBy = m.CreatedBy,
                    CreatedByName = m.CreatedByNavigation.FullName ?? "Unknown"
                })
                .ToListAsync();

            return new PagingModel<MedicineResponseDto>
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / request.PageSize),
                Data = medicines
            };
        }

        public async Task<MedicineResponseDto?> GetMedicineByIdAsync(string id)
        {
            var medicine = await _medicineRepository
                .Include(m => m.CreatedByNavigation)
                .Where(m => m.Id == id && !m.IsDeleted)
                .Select(m => new MedicineResponseDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    Amount = m.Amount,
                    IsAvailable = m.IsAvailable,
                    CreatedBy = m.CreatedBy,
                    CreatedByName = m.CreatedByNavigation.FullName ?? "Unknown",
                })
                .FirstOrDefaultAsync();

            return medicine;
        }

        public async Task<MedicineResponseDto> CreateMedicineAsync(CreateMedicineRequestDto request, string createdBy)
        {
            var medicine = new Medicine
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name,
                Description = request.Description,
                Amount = request.Amount,
                IsAvailable = request.IsAvailable ?? true,
                CreatedBy = createdBy,
                IsDeleted = false
            };

            await _medicineRepository.InsertAsync(medicine);
            await _unitOfWork.SaveAsync();

            // Reload with navigation properties
            var createdMedicine = await _medicineRepository
                .Include(m => m.CreatedByNavigation)
                .FirstOrDefaultAsync(m => m.Id == medicine.Id);

            return new MedicineResponseDto
            {
                Id = createdMedicine.Id,
                Name = createdMedicine.Name,
                Description = createdMedicine.Description,
                Amount = createdMedicine.Amount,
                IsAvailable = createdMedicine.IsAvailable,
                CreatedBy = createdMedicine.CreatedBy,
                CreatedByName = createdMedicine.CreatedByNavigation?.FullName ?? "Unknown"
            };
        }

        public async Task<MedicineResponseDto> UpdateMedicineAsync(UpdateMedicineRequestDto request, string updatedBy)
        {
            var medicine = await _medicineRepository.GetByIdAsync(request.Id);
            if (medicine == null || medicine.IsDeleted)
            {
                throw new ArgumentException("Medicine not found");
            }

            medicine.Name = request.Name;
            medicine.Description = request.Description;
            medicine.Amount = request.Amount;
            medicine.IsAvailable = request.IsAvailable;

            await _medicineRepository.UpdateAsync(medicine);
            await _unitOfWork.SaveAsync();

            // Reload with navigation properties
            var updatedMedicine = await _medicineRepository
                .Include(m => m.CreatedByNavigation)
                .FirstOrDefaultAsync(m => m.Id == medicine.Id);

            return new MedicineResponseDto
            {
                Id = updatedMedicine.Id,
                Name = updatedMedicine.Name,
                Description = updatedMedicine.Description,
                Amount = updatedMedicine.Amount,
                IsAvailable = updatedMedicine.IsAvailable,
                CreatedBy = updatedMedicine.CreatedBy,
                CreatedByName = updatedMedicine.CreatedByNavigation?.FullName ?? "Unknown"
            };
        }

        public async Task<bool> DeleteMedicineAsync(string id)
        {
            var medicine = await _medicineRepository.GetByIdAsync(id);
            if (medicine == null || medicine.IsDeleted)
            {
                return false;
            }

            medicine.IsDeleted = true;
            await _medicineRepository.UpdateAsync(medicine);
            await _unitOfWork.SaveAsync();
            return true;
        }

        private IQueryable<Medicine> ApplySorting(IQueryable<Medicine> query, string? sortBy, bool isDescending)
        {
            if (string.IsNullOrEmpty(sortBy))
                sortBy = "Name";

            Expression<Func<Medicine, object>> keySelector = sortBy.ToLower() switch
            {
                "name" => m => m.Name,
                "amount" => m => m.Amount,
                "isavailable" => m => m.IsAvailable ?? false,
                "createdby" => m => m.CreatedBy,
                _ => m => m.Name
            };

            return isDescending
                ? query.OrderByDescending(keySelector)
                : query.OrderBy(keySelector);
        }
    }
}

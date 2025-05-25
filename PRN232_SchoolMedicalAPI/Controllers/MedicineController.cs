using Microsoft.AspNetCore.Mvc;
using SchoolMedical_BusinessLogic.Interface;
using SchoolMedical_BusinessLogic.Utility;
using SchoolMedical_DataAccess.DTOModels;
using System.Security.Claims;

namespace PRN232_SchoolMedicalAPI.Controllers
{
    [ApiController]
    [Route("api/medicine")]
    public class MedicineController : ControllerBase
    {
        private readonly IMedicineService _medicineService;

        public MedicineController(IMedicineService medicineService)
        {
            _medicineService = medicineService;
        }

        /// <summary>
        /// Get paginated list of medicines with filtering and sorting
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetMedicines([FromQuery] MedicineFilterRequestDto request)
        {
            var result = await _medicineService.GetMedicinesAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// Get medicine details by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMedicineById(string id)
        {
            var medicine = await _medicineService.GetMedicineByIdAsync(id);
            if (medicine == null)
            {
                throw new KeyNotFoundException("Medicine not found");
            }

            return Ok(medicine);
        }

        /// <summary>
        /// Create new medicine
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateMedicine([FromBody] CreateMedicineRequestDto request)
        {
            // Validation is handled by ResultManipulator middleware
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentUserId = GetCurrentUserId();
            var medicine = await _medicineService.CreateMedicineAsync(request, currentUserId);

            return CreatedAtAction(nameof(GetMedicineById), new { id = medicine.Id }, medicine);
        }

        /// <summary>
        /// Update existing medicine
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMedicine(string id, [FromBody] UpdateMedicineRequestDto request)
        {
            if (id != request.Id)
            {
                throw new AppException("ID mismatch between route and request body");
            }

            // Validation is handled by ResultManipulator middleware
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentUserId = GetCurrentUserId();
            var medicine = await _medicineService.UpdateMedicineAsync(request, currentUserId);

            return Ok(medicine);
        }

        /// <summary>
        /// Soft delete medicine (permanent deletion)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicine(string id)
        {
            var result = await _medicineService.DeleteMedicineAsync(id);
            if (!result)
            {
                throw new KeyNotFoundException("Medicine not found");
            }

            return Ok(new { message = "Medicine deleted" });
        }

        private string GetCurrentUserId()
        {
            // Extract user ID from JWT token or current context
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "system";
        }
    }
}

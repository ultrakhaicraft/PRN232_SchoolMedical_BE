using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232_SchoolMedicalAPI.Helpers;
using SchoolMedical_BusinessLogic.Interface;
using SchoolMedical_BusinessLogic.Utility;
using SchoolMedical_DataAccess.DTOModels;
using System.Security.Claims;

namespace PRN232_SchoolMedicalAPI.Controllers;

[ApiController]
[Route("api/medicine")]
//[Authorize]
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
        [HttpGet("get-all")]
        //[Authorize(Roles ="SchoolNurse, Admin, Manager")]
        public async Task<IActionResult> GetMedicines([FromQuery] MedicineFilterRequestDto request)
        {
            var result = await _medicineService.GetAllMedicinesAsync(request);
			    HttpContext.Items["CustomMessage"] = "Retrieving All MedicinesSuccessful";
			    return Ok(result);
        }

        /// <summary>
        /// Get medicine details by ID
        /// </summary>
        [HttpGet("get-detail")]
		//[Authorize(Roles = "SchoolNurse, Admin, Manager")]
		public async Task<IActionResult> GetMedicineById(string medicineId)
        {
        var medicine = await _medicineService.GetMedicineDetailByIdAsync(medicineId);
        if (medicine == null)
        {
            throw new KeyNotFoundException("Medicine not found");
        }
			HttpContext.Items["CustomMessage"] = "Medicine Found";
			return Ok(medicine);
        }

        /// <summary>
        /// Create new medicine
        /// </summary>
        [HttpPost("create-medicine")]
		//[Authorize(Roles ="SchoolNurse, Admin, Manager")]
		public async Task<IActionResult> CreateMedicine([FromBody] CreateMedicineRequestDto request)
        {
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var currentUserId = User.Claims.GetUserIdFromJwtToken();
        var medicine = await _medicineService.CreateMedicineAsync(request, currentUserId);

			HttpContext.Items["CustomMessage"] = "Medicine created successfully";
			return CreatedAtAction(nameof(GetMedicineById), new { id = medicine.Id }, medicine);
        }

        /// <summary>
        /// Update existing medicine
        /// </summary>
        [HttpPut("update-medicine")]
		//[Authorize(Roles ="SchoolNurse, Admin, Manager")]
		public async Task<IActionResult> UpdateMedicine(string medicineId, [FromBody] UpdateMedicineRequestDto request)
        {
       
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var currentUserId = User.Claims.GetUserIdFromJwtToken();
		var medicine = await _medicineService.UpdateMedicineAsync(request, medicineId);
		HttpContext.Items["CustomMessage"] = "Medicine updated successfully";
		return Ok(medicine);
        }

        /// <summary>
        /// Soft delete medicine (permanent deletion)
        /// </summary>
        [HttpDelete("delete-medicine")]
		//[Authorize(Roles ="SchoolNurse, Admin, Manager")]
		public async Task<IActionResult> DeleteMedicine(string medicineId)
        {
        var result = await _medicineService.SoftDeleteMedicineAsync(medicineId);   
			HttpContext.Items["CustomMessage"] = "Medicine deleted successfully";
			return Ok(result);
        }

   
}

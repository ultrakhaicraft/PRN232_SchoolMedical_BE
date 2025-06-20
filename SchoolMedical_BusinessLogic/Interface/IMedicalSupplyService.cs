using SchoolMedical_DataAccess.DTOModels;
namespace SchoolMedical_BusinessLogic.Interface
{
    public interface IMedicalSupplyService
    {
        Task<PagingModel<MedicalSupplyDTO>> GetAllMedicalSupplyAsync(MedicalSupplyDTO request);
        Task<MedicalSupplyDTO?> GetMedicalSupplyByIdAsync(string id);
        Task<MedicalSupplyDTO> CreateMedicalSupplyAsync(MedicalSupplyDTO request, string createdBy);
        Task<MedicalSupplyDTO> UpdateMedicalSupplyAsync(MedicalSupplyDTO request, string medicineId);
        Task<bool> SoftDeleteMedicalSupplyAsync(string id);
    }
}

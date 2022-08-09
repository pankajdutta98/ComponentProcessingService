using ComponentProcessingService.Models.DTOs;
using System.Threading.Tasks;

namespace ComponentProcessingService.Repository.IRepository
{
    public interface IOrderProcessingRepo
    {        
        Task<ProcessResponse> getProcessResponse(char ComponentType, float? Qty);  
        
        int CompleteProcessing(OrderDataDto orderData);
    }
}

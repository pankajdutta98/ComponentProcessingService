using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ComponentProcessingService.Models.DTOs;
using ComponentProcessingService.Repository.IRepository;
using System;
using System.Threading.Tasks;

namespace ComponentProcessingService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommponentProcessingController : ControllerBase
    {
        private readonly IOrderProcessingRepo _IRepo;
        public CommponentProcessingController(IOrderProcessingRepo IRepo)
        {
            _IRepo = IRepo;
        }

        [HttpGet("ProcessDetail")]
        public async Task<IActionResult> processDetail([FromQuery] ProcessRequest request)
        {
            try
            {
                if (request.Qty <= 0)
                {
                    return BadRequest(new { message = "Please enter a valid quantity" });
                }

                if (request.ComponentType == 'I' || request.ComponentType == 'A')
                {
                    ProcessResponse response = new ProcessResponse();
                    response = await _IRepo.getProcessResponse(request.ComponentType ?? ' ', request.Qty);
                    if (response == null || string.IsNullOrEmpty(response.RequestId))
                    {
                        return BadRequest(new { message = "An Error Occured" });
                    }
                    return Ok(response);
                }
                else
                {
                    return BadRequest(new { message = "Invalid Request" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("CompleteProcessing")]
        public IActionResult completeProcessing([FromBody] OrderDataDto data)
        {

            try
            {
                if (data == null)
                {
                    return BadRequest(new { message = "Invalid Request" });
                }

                int status = _IRepo.CompleteProcessing(data);
                if (status < 0)
                {
                    return BadRequest(new { message = "Invalid Request" });
                }
                else if (status == 0)
                {
                    return BadRequest(new { message = "Some error occured while saving data" });
                }
                else
                {
                    return Ok(new { data = data, message = "Return Order Placed Successfully" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

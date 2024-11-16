using Application.Features.Properties.Commands;
using Application.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {
        private readonly ISender _mediatrSender;

        public PropertiesController(ISender mediatrSender)
        {
            _mediatrSender = mediatrSender;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddNewProperty([FromBody] NewProperty newPropertyRequest)
        {
            bool isSuccessful = await _mediatrSender.Send(new CreatePropertyRequest(newPropertyRequest));
            if (isSuccessful)
            {
                return Ok("Property Created Successfully");
            }
            return BadRequest("Failed to create property");
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateProperty([FromBody] UpdateProperty updateProperty)
        {
            bool isSuccesful = await _mediatrSender.Send(new UpdatePropertyRequest(updateProperty));
            if (isSuccesful)
            {
                return Ok("Property Updated");
            }
            return BadRequest("Property not found");
        }
    }
}

using Application.Features.Properties.Commands;
using Application.Features.Properties.Queries;
using Application.Models;
using Domain;
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

        [HttpGet("getById")]
        public async Task<IActionResult> GetPropertyById(int id)
        {
            var response = await _mediatrSender.Send(new GetPropertyById(id));
            if (response != null)
            {
                return Ok(response);
            }
            return NotFound("Property Could Not be Found!");
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetProperties()
        {
            List<PropertyDto> propertyDtos = await _mediatrSender.Send(new GetPropertiesRequest());
            if (propertyDtos != null)
            {
                return Ok(propertyDtos);
            }
            return NotFound("No Properties was found");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProperty(int id)
        {
            bool isSuccessful = await _mediatrSender.Send(new DeletePropertyRequest(id));
            if (isSuccessful)
            {
                return Ok("Property Deleted");
            }
            return NotFound("Property Not Found");
        }
    }
}

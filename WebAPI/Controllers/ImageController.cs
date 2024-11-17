using Application.Features.Image.Commands;
using Application.Features.Image.Queries;
using Application.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly ISender _mediatrSender;

        public ImageController(ISender mediatrSender)
        {
            _mediatrSender = mediatrSender;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddNewImage([FromBody] NewImage newImage)
        {
            bool isSuccessful = await _mediatrSender.Send(new CreateImageRequest(newImage));
            if (isSuccessful)
            {
                return Ok("Image Created");
            }
            return BadRequest("Failed to create Image");
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateImage([FromBody] UpdateImage updateImage)
        {
            bool isSuccessful = await _mediatrSender.Send(new UpdateImageRequest(updateImage));
            if (isSuccessful)
            {
                return Ok("Image Updated Succesfully");
            }
            return NotFound("Image Not Found");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            bool isSuccesful = await _mediatrSender.Send(new DeleteImageRequest(id));
            if (isSuccesful)
            {
                return Ok("Image Deleted");
            }
            return NotFound("Image Not Found");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetImageById(int id)
        {
            ImageDto image = await _mediatrSender.Send(new GetImageByIdRequest(id));
            if (image != null)
            {
                return Ok(image);
            }
            return NotFound("Image Not Found");
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllImagesAsync()
        {
            List<ImageDto> imageDtos = await _mediatrSender.Send(new GetImagesRequest());
            if (imageDtos != null)
            {
                return Ok(imageDtos);
            }
            return NotFound("No Image Record Found");
        }
    }
}
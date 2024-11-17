using Application.Models;
using Application.Repositories;
using AutoMapper;
using Domain;
using MediatR;

namespace Application.Features.Image.Commands
{
    public class CreateImageRequest : IRequest<bool>
    {
        public NewImage NewImage { get; set; }

        public CreateImageRequest(NewImage newImage)
        {
            NewImage = newImage;
        }
    }

    public class CreateImageRequestHandler : IRequestHandler<CreateImageRequest, bool>
    {
        private readonly IImageRepo _repo;
        private readonly IMapper _mapper;

        public CreateImageRequestHandler(IImageRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<bool> Handle(CreateImageRequest request, CancellationToken cancellationToken)
        {
            Images image = _mapper.Map<Images>(request.NewImage);
            await _repo.AddNewAsync(image);
            return true;
        }
    }
}

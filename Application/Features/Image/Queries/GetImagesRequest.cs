using Application.Models;
using Application.PipelineBehavior.Contracts;
using Application.Repositories;
using AutoMapper;
using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Image.Queries
{
    public class GetImagesRequest : IRequest<List<ImageDto>>, ICacheable
    {
        public string CacheKey { get; set; }
        public bool BypassCache { get; set; }
        public TimeSpan? SlidingExpiration { get; set; }

        public GetImagesRequest()
        {
            CacheKey = "GetImageRequests";
        }
    }

    public class GetImagesRequestHandler : IRequestHandler<GetImagesRequest, List<ImageDto>>
    {
        private readonly IImageRepo _repo;
        private readonly IMapper _mapper;

        public GetImagesRequestHandler(IImageRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<ImageDto>> Handle(GetImagesRequest request, CancellationToken cancellationToken)
        {
            List<Images> images = await _repo.GetAllASync();
            if (images != null)
            {
                List<ImageDto> imageDtos = [];
                foreach (var item in images)
                {
                    ImageDto tempVariable = _mapper.Map<ImageDto>(item);
                    imageDtos.Add(tempVariable);
                }
                return imageDtos;
            }
            return null;
        }
    }
}

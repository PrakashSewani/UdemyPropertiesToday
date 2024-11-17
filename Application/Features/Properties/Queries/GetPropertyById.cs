using Application.Models;
using Application.PipelineBehavior.Contracts;
using Application.Repositories;
using AutoMapper;
using Domain;
using MediatR;

namespace Application.Features.Properties.Queries
{
    public class GetPropertyById : IRequest<PropertyDto>, ICacheable
    {
        public int PropertyId { get; set; }
        public string CacheKey { get; set; }
        public bool BypassCache { get; set; }
        public TimeSpan? SlidingExpiration { get; set; }
        public bool ValueModified { get; set; }

        public GetPropertyById(int propertyId)
        {
            PropertyId = propertyId;
            CacheKey = $"GetPropertyById:{propertyId}";
            ValueModified = false;
        }
    }

    public class GetPropertyByIdHandler : IRequestHandler<GetPropertyById, PropertyDto>
    {
        private readonly IPropertyRepo _propertyRepo;
        private readonly IMapper _mapper;

        public GetPropertyByIdHandler(IPropertyRepo propertyRepo, IMapper mapper)
        {
            _propertyRepo = propertyRepo;
            _mapper = mapper;
        }

        public async Task<PropertyDto> Handle(GetPropertyById request, CancellationToken cancellationToken)
        {
            Property propertyInDb = await _propertyRepo.GetByIdAsync(request.PropertyId);
            if (propertyInDb != null)
            {
                PropertyDto propertyDto = _mapper.Map<PropertyDto>(propertyInDb);
                return propertyDto;
            }
            return null;
        }
    }
}

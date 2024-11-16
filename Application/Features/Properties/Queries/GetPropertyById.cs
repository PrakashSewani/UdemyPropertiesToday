using Application.Models;
using Application.Repositories;
using AutoMapper;
using Domain;
using MediatR;

namespace Application.Features.Properties.Queries
{
    public class GetPropertyById : IRequest<PropertyDto>
    {
        public int PropertyId { get; set; }

        public GetPropertyById(int propertyId)
        {
            PropertyId = propertyId;
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

using Application.Models;
using Application.Repositories;
using AutoMapper;
using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Properties.Queries
{
    public class GetPropertiesRequest : IRequest<List<PropertyDto>>
    {
    }

    public class GetPropertiesRequestHandler : IRequestHandler<GetPropertiesRequest, List<PropertyDto>>
    {
        private readonly IPropertyRepo _propertyRepo;
        private readonly IMapper _mapper;

        public GetPropertiesRequestHandler(IPropertyRepo propertyRepo, IMapper mapper)
        {
            _propertyRepo = propertyRepo;
            _mapper = mapper;
        }

        public async Task<List<PropertyDto>> Handle(GetPropertiesRequest request, CancellationToken cancellationToken)
        {
            List<Property> properties = await _propertyRepo.GetAllAsync();
            if (properties != null)
            {
                List<PropertyDto> propertyDtos = [];
                foreach (var item in properties)
                {
                    PropertyDto tempVariable = _mapper.Map<PropertyDto>(item);
                    propertyDtos.Add(tempVariable);
                }
                return propertyDtos;
            }
            return null;
        }
    }
}

using Application.Models;
using Application.PipelineBehavior.Contracts;
using Application.Repositories;
using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Properties.Commands
{
    public class UpdatePropertyRequest : IRequest<bool>, ICacheable
    {
        public UpdateProperty UpdateProperty { get; set; }
        public string CacheKey { get; set; }
        public bool BypassCache { get; set; }
        public TimeSpan? SlidingExpiration { get; set; }
        public bool ValueModified { get; set; }

        public UpdatePropertyRequest(UpdateProperty updateProperty)
        {
            UpdateProperty = updateProperty;
            CacheKey = $"GetPropertyById:{UpdateProperty.Id}";
            ValueModified = true;
        }
    }

    public class UpdatePropertyRequestHandler : IRequestHandler<UpdatePropertyRequest, bool>
    {
        private readonly IPropertyRepo _propertyRepo;

        public UpdatePropertyRequestHandler(IPropertyRepo propertyRepo)
        {
            _propertyRepo = propertyRepo;
        }

        public async Task<bool> Handle(UpdatePropertyRequest request, CancellationToken cancellationToken)
        {
            Property propertyinDb = await _propertyRepo.GetByIdAsync(request.UpdateProperty.Id);
            if (propertyinDb != null)
            {
                propertyinDb.Name = request.UpdateProperty.Name;
                propertyinDb.Lounges = request.UpdateProperty.Lounges;
                propertyinDb.Dining = request.UpdateProperty.Dining;
                propertyinDb.Rates = request.UpdateProperty.Rates;
                propertyinDb.Bathrooms = request.UpdateProperty.Bathrooms;
                propertyinDb.Bedrooms = request.UpdateProperty.Bedrooms;
                propertyinDb.Address = request.UpdateProperty.Address;
                propertyinDb.Description = request.UpdateProperty.Description;
                propertyinDb.ErfSize = request.UpdateProperty.ErfSize;
                propertyinDb.FloorSize = request.UpdateProperty.FloorSize;
                propertyinDb.Kitechens = request.UpdateProperty.Kitechens;
                propertyinDb.Levies = request.UpdateProperty.Levies;
                propertyinDb.PetsAllowed = request.UpdateProperty.PetsAllowed;
                propertyinDb.Price = request.UpdateProperty.Price;
                propertyinDb.Type = request.UpdateProperty.Type;

                await _propertyRepo.UpdateAsync(propertyinDb);
                return true;
            }
            return false;
        }
    }
}

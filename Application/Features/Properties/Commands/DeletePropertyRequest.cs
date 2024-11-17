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
    public class DeletePropertyRequest : IRequest<bool>, ICacheable
    {
        public int PropertyId { get; set; }
        public string CacheKey { get; set; }
        public bool BypassCache { get; set; }
        public TimeSpan? SlidingExpiration { get; set; }
        public bool ValueModified { get; set; }

        public DeletePropertyRequest(int propertyId)
        {
            PropertyId = propertyId;
            CacheKey = $"GetPropertyById:{propertyId}";
            ValueModified = true;
        }
    }

    public class DeletePropertyRequestHandler : IRequestHandler<DeletePropertyRequest, bool>
    {
        private readonly IPropertyRepo _propertyRepo;

        public DeletePropertyRequestHandler(IPropertyRepo propertyRepo)
        {
            _propertyRepo = propertyRepo;
        }

        public async Task<bool> Handle(DeletePropertyRequest request, CancellationToken cancellationToken)
        {
            Property propertyInDb = await _propertyRepo.GetByIdAsync(request.PropertyId);
            if (propertyInDb != null)
            {
                await _propertyRepo.DeleteAsync(propertyInDb);
                return true;
            }
            return false;
        }
    }
}

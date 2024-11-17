using Application.PipelineBehavior.Contracts;
using Application.Repositories;
using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Application.Features.Image.Commands
{
    public class DeleteImageRequest : IRequest<bool>, ICacheable
    {
        public int Id { get; set; }
        public string CacheKey { get; set; }
        public bool BypassCache { get; set; }
        public TimeSpan? SlidingExpiration { get; set; }
        public bool ValueModified { get; set; }

        public DeleteImageRequest(int id)
        {
            Id = id;
            CacheKey = $"GetImageById:{id}";
            ValueModified = true;
        }
    }

    public class DeleteImageRequestHandler : IRequestHandler<DeleteImageRequest, bool>
    {
        private readonly IImageRepo _repo;

        public DeleteImageRequestHandler(IImageRepo repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(DeleteImageRequest request, CancellationToken cancellationToken)
        {
            Images imageInDb = await _repo.GetByIdAsync(request.Id);
            if (imageInDb != null)
            {
                await _repo.DeleteAsync(imageInDb);
                return true;
            }
            return false;
        }
    }
}

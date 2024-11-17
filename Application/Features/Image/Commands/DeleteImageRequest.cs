using Application.Repositories;
using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Image.Commands
{
    public class DeleteImageRequest : IRequest<bool>
    {
        public int Id { get; set; }

        public DeleteImageRequest(int id)
        {
            Id = id;
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

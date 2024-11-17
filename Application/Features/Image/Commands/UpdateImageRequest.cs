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

namespace Application.Features.Image.Commands
{
    public class UpdateImageRequest : IRequest<bool>
    {
        public UpdateImage UpdateImage { get; set; }

        public UpdateImageRequest(UpdateImage updateImage)
        {
            UpdateImage = updateImage;
        }
    }

    public class UpdateImageRequestHandler : IRequestHandler<UpdateImageRequest, bool>
    {
        private readonly IImageRepo _imageRepo;

        public UpdateImageRequestHandler(IImageRepo imageRepo)
        {
            _imageRepo = imageRepo;
        }

        public async Task<bool> Handle(UpdateImageRequest request, CancellationToken cancellationToken)
        {
            Images imageInDb = await _imageRepo.GetByIdAsync(request.UpdateImage.Id);
            if (imageInDb != null)
            {
                imageInDb.Name = request.UpdateImage.Name;
                imageInDb.Path = request.UpdateImage.Path;
                await _imageRepo.UpdateAsync(imageInDb);
                return true;
            }
            return false;
        }
    }
}

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
using static System.Net.Mime.MediaTypeNames;

namespace Application.Features.Image.Commands
{
    public class UpdateImageRequest : IRequest<bool>, ICacheable
    {
        public UpdateImage UpdateImage { get; set; }
        public string CacheKey { get; set; }
        public bool BypassCache { get; set; }
        public TimeSpan? SlidingExpiration { get; set; }
        public bool ValueModified { get; set; }

        public UpdateImageRequest(UpdateImage updateImage)
        {
            UpdateImage = updateImage;
            CacheKey = $"GetImageById:{updateImage.Id}";
            ValueModified = true;
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

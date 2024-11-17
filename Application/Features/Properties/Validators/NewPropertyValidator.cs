using Application.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Properties.Validators
{
    public class NewPropertyValidator:AbstractValidator<NewProperty>
    {
        public NewPropertyValidator()
        {
            RuleFor(np => np.Name)
                .NotEmpty().WithMessage("Property Name is required")
                .MaximumLength(14).WithMessage("Property Name should not exceed 14 characters");
        }
    }
}

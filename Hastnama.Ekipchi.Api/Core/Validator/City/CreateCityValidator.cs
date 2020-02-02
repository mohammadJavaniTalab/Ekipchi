﻿using FluentValidation;
using Hastnama.Ekipchi.Common.Message;
using Hastnama.Ekipchi.Data.City;

namespace Hastnama.Ekipchi.Api.Core.Validator.City
{
    public class CreateCityValidator : AbstractValidator<CreateCityDto>
    {
        public CreateCityValidator()
        {
            RuleFor(dto => dto.Name)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage(PersianErrorMessage.CityNameIsInvalid);

            RuleFor(dto => dto.CountyId)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .GreaterThanOrEqualTo(1).WithMessage(PersianErrorMessage.InvalidCountyId);
        }
    }
}
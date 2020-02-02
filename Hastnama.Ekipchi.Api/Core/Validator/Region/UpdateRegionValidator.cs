﻿using FluentValidation;
using Hastnama.Ekipchi.Common.Message;
using Hastnama.Ekipchi.Data.City;
using Hastnama.Ekipchi.Data.Country;
using Hastnama.Ekipchi.Data.Province;
using Hastnama.Ekipchi.Data.Region;

namespace Hastnama.Ekipchi.Api.Core.Validator.City
{
    public class UpdateRegionValidator : AbstractValidator<UpdateRegionDto>
    {
        public UpdateRegionValidator()
        {
            RuleFor(dto => dto.Name)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage(PersianErrorMessage.RegionNameIsInvalid);

            RuleFor(dto => dto.Id)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .GreaterThanOrEqualTo(1).WithMessage(PersianErrorMessage.InvalidRegionId);
            
            RuleFor(dto => dto.CityId)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .GreaterThanOrEqualTo(0).WithMessage(PersianErrorMessage.InvalidCityId);


        }
    }
}
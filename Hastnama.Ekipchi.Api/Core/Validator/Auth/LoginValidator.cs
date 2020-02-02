﻿using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using FluentValidation;
using Hastnama.Ekipchi.Common.Message;
using Hastnama.Ekipchi.Data.Auth;

namespace Hastnama.Ekipchi.Api.Core.Validator.Auth
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(dto => dto)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Must(dto => (!string.IsNullOrEmpty(dto.Username) && dto.Username.Length < 16)
                             || (!string.IsNullOrEmpty(dto.Email) && new EmailAddressAttribute().IsValid(dto.Email))
                             || (!string.IsNullOrEmpty(dto.Mobile) && dto.Mobile.Length <= 11 &&
                                 Regex.IsMatch(dto.Mobile, "^[0-9 ]+$")))
                .WithMessage(PersianErrorMessage.InvalidUserCredential);

            RuleFor(dto => dto.Password)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage(PersianErrorMessage.InvalidUserCredential)
                .MaximumLength(16).WithMessage(PersianErrorMessage.InvalidUserCredential);
        }
    }
}
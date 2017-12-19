using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using MovieAPI.IBLL;
using MovieAPI.Model;

namespace MovieAPI.Validators
{
    public class usersInsertValidator : AbstractValidator<RegisterUserRequestJSON>
    {
        
        public usersInsertValidator()
        {

            RuleFor(x => x.email).NotEmpty().WithMessage("Email Address cannot be blank")
            .EmailAddress().WithMessage("Please enter a valid email address");


            RuleFor(x => x.firstName).NotEmpty().WithMessage("The First Name cannot be blank.")
                                        .Length(2, 50).WithMessage("The First Name should be between 2 and 50 characters.");

            RuleFor(x => x.network).NotEmpty().WithMessage("network cannot be blank.");

        }

    }
}
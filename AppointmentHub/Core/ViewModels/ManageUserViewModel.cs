using AppointmentHub.Controllers;
using AppointmentHub.Core.Validators;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace AppointmentHub.Core.ViewModels
{
    [Validator(typeof(ManageUserFormValidator))]
    public class ManageUserViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public IEnumerable<ApplicationRoleViewModel> Roles { get; set; }

        public IEnumerable<string> Roles1 { get; set; }

        public string Heading
        {
            get
            {
                return string.IsNullOrEmpty(Id) ? "Create" : "Edit";
            }
        }
        public string Action
        {
            get
            {
                Expression<Func<AdminController, ActionResult>> update =
                    (c => c.UpdateUser(this));

                Expression<Func<AdminController, ActionResult>> create =
                    (c => c.CreateUser(this));

                var action = (!string.IsNullOrEmpty(Id)) ? update : create;
                return (action.Body as MethodCallExpression).Method.Name;
            }
        }

        public ManageUserViewModel()
        {
            Roles = new Collection<ApplicationRoleViewModel>();
            Roles1 = new Collection<string>();
        }
    }
}
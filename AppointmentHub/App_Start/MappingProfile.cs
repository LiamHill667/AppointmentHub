
using AppointmentHub.Core.Models;
using AppointmentHub.Core.ViewModels;
using AutoMapper;
using System;

namespace AppointmentHub.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, ApplicationUserViewModel>().ReverseMap();
            CreateMap<UserAvailability, UserAvailabilityViewModel>().ReverseMap();
            CreateMap<AppointmentType, AppointmentTypeViewModel>().ReverseMap();
            CreateMap<Notification, NotificationViewModel>().ReverseMap();
            CreateMap<ApplicationRole, ApplicationRoleViewModel>().ReverseMap();
            CreateMap<ApplicationUserRole, ApplicationUserRoleViewModel>().ReverseMap();

            CreateMap<UserAvailability, AvailabilityFormViewModel>()
                .ForMember(dest => dest.Date, opts => opts.MapFrom(src => src.DateTime.ToString("dd/MM/yyyy")))
                .ForMember(dest => dest.StartTime, opts => opts.MapFrom(src => src.DateTime.TimeOfDay));

            CreateMap<AvailabilityFormViewModel, UserAvailability>()
               .ForMember(dest => dest.DateTime, opts => opts.MapFrom(src => DateTime.Parse($"{src.Date} {src.StartTime}")));




        }
    }
}
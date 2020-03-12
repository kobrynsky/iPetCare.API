using System;
using System.Collections.Generic;
using System.Text;
using Application.Dtos.Users;
using AutoMapper;
using Domain.Models;

namespace Application.Infrastructure
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            MapsForUser();
        }

        private void MapsForUser()
        {
            CreateMap<ApplicationUser, UserGetAllDtoResponse>();
        }
    }
}

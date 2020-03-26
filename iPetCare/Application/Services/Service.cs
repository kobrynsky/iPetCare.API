using System;
using System.Threading.Tasks;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace Application.Services
{
    public class Service
    {
        protected IMapper Mapper { get; }
        protected DataContext Context { get; }
        protected UserManager<ApplicationUser> UserManager { get; }
        protected string CurrentlyLoggedUserName { get; }
        protected ApplicationUser CurrentlyLoggedUser { get; set; }

        public Service(IServiceProvider serviceProvider)
        {
            var userAccessor = serviceProvider.GetService<IUserAccessor>();
            CurrentlyLoggedUserName = userAccessor.GetCurrentUsername();

            Context = serviceProvider.GetService<DataContext>();
            Mapper = serviceProvider.GetService<IMapper>();
            UserManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
            AssignCurrentlyLoggedUser();
        }

        protected void AssignCurrentlyLoggedUser()
        {
            if (CurrentlyLoggedUserName == null)
            {
                CurrentlyLoggedUser = null;
                return;
            }
            CurrentlyLoggedUser = UserManager.FindByNameAsync(CurrentlyLoggedUserName).Result;
        }

    }
}

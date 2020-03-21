using System;
using Application.Interfaces;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace Application.Services
{
    public class Service
    {
        protected IMapper Mapper { get; }
        protected DataContext Context { get; }
        protected IUserAccessor UserAccessor { get; }

        public Service(IServiceProvider serviceProvider)
        {
            Context = serviceProvider.GetService<DataContext>();
            UserAccessor = serviceProvider.GetService<IUserAccessor>();
            Mapper = serviceProvider.GetService<IMapper>();
        }

    }
}

using Microsoft.Extensions.DependencyInjection;
using System;
using XProject.Contract.Repository.Infrastructure;
using XProject.Contract.Repository.Models;

namespace XProject.Service.Base
{
    public abstract class Service
    {
        protected readonly IUnitOfWork UnitOfWork;

        protected Service(IServiceProvider serviceProvider)
        {
            UnitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
        }
    }
    public class Service<T> : Service where T : Entity, new() 
    {
        public new readonly IUnitOfWork UnitOfWork;
        public readonly IRepository<T> Repository;

        public Service(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            UnitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
            Repository = serviceProvider.GetRequiredService<IRepository<T>>();
        }
    }
}
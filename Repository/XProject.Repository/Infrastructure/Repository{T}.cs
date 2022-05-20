using Invedia.Data.EF.Interfaces.DbContext;
using Invedia.Data.EF.Services.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using XProject.Contract.Repository.Infrastructure;
using XProject.Contract.Repository.Models;
using XProject.Core.Utils;

namespace XProject.Repository.Infrastructure
{
    public class Repository<T> : EntityStringRepository<T>, IRepository<T> where T : Entity, new()
    {
        public Repository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
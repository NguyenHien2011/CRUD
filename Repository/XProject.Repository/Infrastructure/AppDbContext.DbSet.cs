using Microsoft.EntityFrameworkCore;
using XProject.Contract.Repository.Models;

namespace XProject.Repository.Infrastructure
{
    public sealed partial class AppDbContext
    {
       
        public DbSet<Log> logs { get; set; }
    }
}
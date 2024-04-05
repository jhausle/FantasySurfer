using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using JokesWebApp.Models;

namespace JokesWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<JokesWebApp.Models.Joke> Joke { get; set; }
        public DbSet<JokesWebApp.Models.MemberInfo> MemberInfo { get; set; }
        public DbSet<JokesWebApp.Models.Contest> Contest { get; set; }
        public DbSet<JokesWebApp.Models.Points> Points { get; set; }
        public DbSet<JokesWebApp.Models.SurferPoints> SurferPoints { get; set; }
        public DbSet<JokesWebApp.Models.Surfers> Surfers { get; set; }
        public DbSet<JokesWebApp.Models.FsRosterEntity> FsRosters { get; set; }
        public DbSet<JokesWebApp.Models.WslRosterEntity> WslRosters { get; set; }
    }
}

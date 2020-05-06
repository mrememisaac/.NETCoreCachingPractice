using Microsoft.EntityFrameworkCore;
using sm_coding_challenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sm_coding_challenge.Persistence
{
    public class SMContext : DbContext
    {
        public SMContext()
        {
        }

        public SMContext(DbContextOptions<SMContext> options)
            : base(options)
        {
        }

        public virtual DbSet<PlayerModel> Players { get; set; }
        public virtual DbSet<Kick> Kicks{ get; set; }
        public virtual DbSet<Rush> Rushes{ get; set; }
        public virtual DbSet<Pass> Passes{ get; set; }
        public virtual DbSet<Receive> Receives { get; set; }

    }
}

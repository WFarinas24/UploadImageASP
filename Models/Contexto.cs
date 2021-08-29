using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema_Gatos.Models
{
    public class Contexto : DbContext
    {
        public Contexto( DbContextOptions<Contexto> options) : base(options)
        {

        }
        public DbSet<tbGato> tbGato { get; set; }
    }
}

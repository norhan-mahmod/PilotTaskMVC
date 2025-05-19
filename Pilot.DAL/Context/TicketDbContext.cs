using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pilot.DAL.Entities;

namespace Pilot.DAL.Context
{
    public class TicketDbContext : DbContext
    {
        public TicketDbContext(DbContextOptions<TicketDbContext> options) : base(options) { }

        public DbSet<IssueType> IssueTypes { get; set; }
        public DbSet<CustomerTicket> CustomerTickets { get; set; }

    }
}

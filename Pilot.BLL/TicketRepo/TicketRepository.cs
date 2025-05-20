using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pilot.DAL.Context;
using Pilot.DAL.Entities;

namespace Pilot.BLL.TicketRepo
{
    public class TicketRepository : ITicketRepository
    {
        private readonly ILogger<TicketRepository> logger;
        private readonly TicketDbContext context;

        public TicketRepository(ILogger<TicketRepository> logger , TicketDbContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public async Task AddTicket(CustomerTicket ticket)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@FullName",ticket.FullName),
                    new SqlParameter("@MobileNumber",ticket.MobileNumber),
                    new SqlParameter("@Email",ticket.Email),
                    new SqlParameter("@IsuueTypeId",ticket.IssueTypeId),
                    new SqlParameter("@Description",ticket.Description),
                    new SqlParameter("@Priority",(int)ticket.Priority)
                };
                await context.Database.ExecuteSqlRawAsync("exec SP_AddTicket @FullName, @MobileNumber , @Email, @IsuueTypeId , @Description , @Priority", parameters);
            }
            catch(Exception ex)
            {
                logger.LogError(ex.InnerException?.Message);
            }
        }

        public async Task<List<CustomerTicket>> GetAllTickets(int? IssueTypeId, int? Priority)
        {
            try
            {
                var IssueIdParam = new SqlParameter("@IssueTypeId", IssueTypeId ?? (object)DBNull.Value);
                var PriorityParam = new SqlParameter("@Priority", Priority ?? (object)DBNull.Value);

                var tickets = await context.CustomerTickets
                    .FromSqlRaw("exec SP_GetTickets @IssueTypeId , @Priority " ,
                    IssueIdParam, PriorityParam).ToListAsync();
                return tickets;
                
            }
            catch(Exception ex)
            {
                logger.LogError(ex.InnerException?.Message);
                return new List<CustomerTicket>();
            }
        }

        public async Task<CustomerTicket> GetTicketById(int Id)
        {
            try
            {
                var param = new SqlParameter("@Id", Id);
                var ticket = await context.CustomerTickets.FromSqlRaw("exec SP_GetTicketById @Id", param)
                    .ToListAsync();
                return ticket.FirstOrDefault();
            }
            catch(Exception ex)
            {
                logger.LogError(ex.InnerException?.Message);
                return new CustomerTicket();
            }
        }

        public async Task UpdateTicket(CustomerTicket ticket)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@Id",ticket.Id),
                    new SqlParameter("@FullName",ticket.FullName),
                    new SqlParameter("@MobileNumber",ticket.MobileNumber),
                    new SqlParameter("@Email",ticket.Email),
                    new SqlParameter("@IsuueTypeId",ticket.IssueTypeId),
                    new SqlParameter("@Description",ticket.Description),
                    new SqlParameter("@Priority",(int)ticket.Priority)
                };
                await context.Database.ExecuteSqlRawAsync("exec SP_UpdateTicket @Id, @FullName , @MobileNumber , @Email , @IsuueTypeId , @Description , @Priority", parameters);
            }
            catch(Exception ex)
            {
                logger.LogError(ex.InnerException?.Message);
            }
        }
    }
}

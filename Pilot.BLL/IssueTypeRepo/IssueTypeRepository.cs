using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pilot.DAL.Context;
using Pilot.DAL.Entities;

namespace Pilot.BLL.IssueTypeRepo
{
    public class IssueTypeRepository : IIssueTypeRepository
    {
        private readonly ILogger<IssueTypeRepository> logger;
        private readonly TicketDbContext context;

        public IssueTypeRepository(ILogger<IssueTypeRepository> logger , TicketDbContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public async Task<List<IssueType>> GetAllIssue()
        {
            try
            {
                var issueTypes = await context.IssueTypes.FromSqlRaw("exec SP_GetIssueTypes").ToListAsync();
                return issueTypes;
            }
            catch(Exception ex)
            {
                logger.LogError(ex.InnerException?.Message);
                return new List<IssueType>();
            }
        }
    }
}

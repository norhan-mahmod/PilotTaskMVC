using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pilot.DAL.Entities;

namespace Pilot.BLL.TicketRepo
{
    public interface ITicketRepository
    {
        Task<List<CustomerTicket>> GetAllTickets(int? IssueTypeId , int? Priority);
        Task<CustomerTicket> GetTicketById(int Id);
        Task AddTicket(CustomerTicket ticket);
        Task UpdateTicket(CustomerTicket ticket);
    }
}

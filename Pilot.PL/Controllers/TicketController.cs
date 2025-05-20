using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pilot.BLL.IssueTypeRepo;
using Pilot.BLL.TicketRepo;
using Pilot.DAL.Entities;

namespace Pilot.PL.Controllers
{
    public class TicketController : Controller
    {
        private readonly IIssueTypeRepository issueTypeRepository;
        private readonly ITicketRepository ticketRepository;

        public TicketController(IIssueTypeRepository issueTypeRepository , ITicketRepository ticketRepository)
        {
            this.issueTypeRepository = issueTypeRepository;
            this.ticketRepository = ticketRepository;
        }

        public async Task<IActionResult> Index(int? IssueTypeId, int? Priority)
        {
            var issueTypes = await issueTypeRepository.GetAllIssue();
            ViewBag.IssueTypes = issueTypes;
            ViewBag.Priorities = Enum.GetValues(typeof(TicketPriority))
                                .Cast<TicketPriority>()
                                .Select(p => new SelectListItem()
                                { Text = p.ToString(), Value = ((int)p).ToString() })
                                .ToList();
            var tickets = await ticketRepository.GetAllTickets(IssueTypeId, Priority);
            return View(tickets);
        }

        public async Task<IActionResult> GetTicketDetails(int Id)
        {
            try
            {
                var ticket = await ticketRepository.GetTicketById(Id);
                if (ticket is null)
                    return NotFound();

                var issueTypes = await issueTypeRepository.GetAllIssue();
                ViewBag.IssueTypes = issueTypes;
                return View(ticket);
            }
            catch(Exception ex)
            {
                return new BadRequestObjectResult(ex.InnerException?.Message);
            }
        }

        public async Task<IActionResult> Create()
        {
            var issueTypes = await issueTypeRepository.GetAllIssue();
            TempData["IssueTypes"] = issueTypes;
            TempData["Priorities"]  = Enum.GetValues(typeof(TicketPriority))
                                .Cast<TicketPriority>()
                                .Select(p => new SelectListItem()
                                { Text = p.ToString(), Value = ((int)p).ToString() })
                                .ToList();
            return View(new CustomerTicket());
        }
        [HttpPost]
        public async Task<IActionResult> Create(CustomerTicket customerTicket)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    var issueTypes = await issueTypeRepository.GetAllIssue();
                    TempData["IssueTypes"] = issueTypes;
                    TempData["Priorities"] = Enum.GetValues(typeof(TicketPriority))
                                        .Cast<TicketPriority>()
                                        .Select(p => new SelectListItem()
                                        { Text = p.ToString(), Value = ((int)p).ToString() })
                                        .ToList();
                    return View(customerTicket);
                }
                
                await ticketRepository.AddTicket(customerTicket);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return new BadRequestObjectResult(ex.InnerException.Message);
            }
        }

        public async Task<IActionResult> Update(int Id)
        {
            try
            {
                var ticket = await ticketRepository.GetTicketById(Id);
                if (ticket is null)
                    return NotFound();
                var issueTypes = await issueTypeRepository.GetAllIssue();
                TempData["IssueTypes"] = issueTypes;
                TempData["Priorities"] = Enum.GetValues(typeof(TicketPriority))
                                    .Cast<TicketPriority>()
                                    .Select(p => new SelectListItem()
                                    { Text = p.ToString(), Value = ((int)p).ToString() })
                                    .ToList();
                return View(ticket);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.InnerException.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Update(CustomerTicket customerTicket)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var issueTypes = await issueTypeRepository.GetAllIssue();
                    TempData["IssueTypes"] = issueTypes;
                    TempData["Priorities"] = Enum.GetValues(typeof(TicketPriority))
                                        .Cast<TicketPriority>()
                                        .Select(p => new SelectListItem()
                                        { Text = p.ToString(), Value = ((int)p).ToString() })
                                        .ToList();
                    return View(customerTicket);
                }

                await ticketRepository.UpdateTicket(customerTicket);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.InnerException.Message);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pilot.DAL.Entities;

namespace Pilot.BLL.IssueTypeRepo
{
    public interface IIssueTypeRepository
    {
        Task<List<IssueType>> GetAllIssue();
    }
}

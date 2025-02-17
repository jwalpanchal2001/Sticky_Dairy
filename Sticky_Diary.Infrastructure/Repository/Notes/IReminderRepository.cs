using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sticky_Dairy.Domain.Models.Entities;

namespace Sticky_Dairy.Infrastructure.Repository.Notes
{
    public interface IReminderRepository
    {
        Task AddOrUpdateReminderAsync(Reminder reminder);
    }
}

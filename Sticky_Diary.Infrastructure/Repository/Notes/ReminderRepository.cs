using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sticky_Dairy.Domain.Models.Entities;
using Sticky_Dairy.Infrastructure.Data;

namespace Sticky_Dairy.Infrastructure.Repository.Notes
{
    public class ReminderRepository : IReminderRepository
    {
        private readonly Sticky_Dairy_dbContext _context;

        public ReminderRepository(Sticky_Dairy_dbContext context)
        {
            _context = context;
        }

        public async Task AddOrUpdateReminderAsync(Reminder reminder)
        {
            var existingReminder = await _context.Reminders.FirstOrDefaultAsync(r => r.NoteID == reminder.NoteID);
            if (existingReminder != null)
            {
                existingReminder.ReminderAt = reminder.ReminderAt;
                _context.Reminders.Update(existingReminder);
            }
            else
            {
                _context.Reminders.Add(reminder);
            }
            await _context.SaveChangesAsync();
        }
    }
}

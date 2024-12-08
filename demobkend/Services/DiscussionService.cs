using demobkend.Data;
using demobkend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace demobkend.Services
{
    public class DiscussionService
    {
        private readonly AppDbContext _context;

        public DiscussionService(AppDbContext context)
        {
            _context = context;
        }

        // Create a new discussion
        public async Task<Discussion> CreateDiscussionAsync(Discussion discussion)
        {
            if (discussion == null)
                throw new ArgumentNullException(nameof(discussion));

            // Ensure that the discussion status is set to Open by default
            discussion.Status = "Open";
            discussion.PostDate = DateTime.UtcNow;

            _context.Discussions.Add(discussion);
            await _context.SaveChangesAsync();

            return discussion;
        }

        // Get a discussion by ID
        public async Task<Discussion> GetDiscussionByIdAsync(int id)
        {
            return await _context.Discussions
                .FirstOrDefaultAsync(d => d.DiscussionId == id);
        }

        // Get all discussions
        public async Task<IEnumerable<Discussion>> GetAllDiscussionsAsync()
        {
            return await _context.Discussions.ToListAsync();
        }

        // Close a discussion (mark as solved)
        public async Task<bool> CloseDiscussionAsync(int id)
        {
            var discussion = await _context.Discussions.FindAsync(id);
            if (discussion == null)
                return false;

            discussion.Status = "Closed";
            _context.Discussions.Update(discussion);
            await _context.SaveChangesAsync();

            return true;
        }

        // Open a discussion (mark as unsolved)
        public async Task<bool> OpenDiscussionAsync(int id)
        {
            var discussion = await _context.Discussions.FindAsync(id);
            if (discussion == null)
                return false;

            discussion.Status = "Open";
            _context.Discussions.Update(discussion);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}

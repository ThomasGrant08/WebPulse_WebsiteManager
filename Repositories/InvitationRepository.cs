using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPulse_WebManager.Data;
using WebPulse_WebManager.Models;

namespace WebPulse_WebManager.Repositories
{
    public class InvitationRepository : Repository<Invitation>
    {
        public InvitationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override IEnumerable<Invitation> FindAll(int page = 0, int max = int.MaxValue, Func<Invitation, bool>? filter = null, Func<Invitation, dynamic>? order = null, bool orderAscending = true)
        {
            IEnumerable<Invitation> query = _context.Invitation.Include(invitation => invitation.Group).Include(invitation => invitation.InvitedUser);

            if(filter != null) query = query.Where(filter);

            if(order != null)
            {
                if(orderAscending) query = query.OrderBy(order);
                else query = query.OrderByDescending(order);
            }

            return query.Skip(page * max).Take(max);
        }

        public override async Task<Invitation?> FindById(int id)
        {
            return await _context.Invitation.Include(invitation => invitation.Group).Include(invitation => invitation.InvitedUser).Where(invitation => invitation.Id == id).FirstAsync();
        }

        public override async Task<Invitation> Insert(Invitation entity)
        {
            return await base.Insert(entity);
        }

        public override async Task<bool> Update(Invitation oldEntity, Invitation newEntity)
        {
            oldEntity.Group = newEntity.Group;
            oldEntity.InvitedUser = newEntity.InvitedUser;
            oldEntity.InvitedBy = newEntity.InvitedBy;
            oldEntity.Accepted = newEntity.Accepted;
            oldEntity.LastUpdatedAt = newEntity.LastUpdatedAt;

            return await base.Update(oldEntity, newEntity);
        }

        public async Task<bool> Update(Invitation newEntity)
        {
            Invitation? oldEntity = await FindById(newEntity.Id);

            if(oldEntity == null) return false;
            return await Update(oldEntity, newEntity);
        }

    }
}

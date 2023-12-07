using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPulse_WebManager.Data;
using WebPulse_WebManager.Models;

namespace WebPulse_WebManager.Repositories
{
    public class CredentialRepository : Repository<Credential>
    {
        public CredentialRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override IEnumerable<Credential> FindAll(int page = 0, int max = int.MaxValue, Func<Credential, bool>? filter = null, Func<Credential, dynamic>? order = null, bool orderAscending = true)
        {
            IEnumerable<Credential> query = _context.Credential.Include(credential => credential.AssignedUsers).Include(credential => credential.Website);

            if(filter != null) query = query.Where(filter);

            if(order != null)
            {
                if(orderAscending) query = query.OrderBy(order);
                else query = query.OrderByDescending(order);
            }

            return query.Skip(page * max).Take(max);
        }

        public override async Task<Credential?> FindById(int id)
        {
            return await _context.Credential.Include(credential => credential.AssignedUsers).Include(credential => credential.Website).Where(credential => credential.Id == id).FirstAsync();
        }

        public override async Task<Credential> Insert(Credential entity)
        {
            return await base.Insert(entity);
        }

        public override async Task<bool> Update(Credential oldEntity, Credential newEntity)
        {
            oldEntity.Username = newEntity.Username;
            oldEntity.Password = newEntity.Password;
            oldEntity.AssignedUsers = newEntity.AssignedUsers;
            oldEntity.Website = newEntity.Website;

            return await base.Update(oldEntity, newEntity);
        }
    }
}

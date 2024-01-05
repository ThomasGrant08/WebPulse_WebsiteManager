using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPulse_WebManager.Data;
using WebPulse_WebManager.Models;
using WebPulse_WebManager.Utility;

namespace WebPulse_WebManager.Repositories
{
    public class CredentialRepository : Repository<Credential>
    {
        public CredentialRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override IEnumerable<Credential> FindAll(int page = 0, int max = int.MaxValue, Func<Credential, bool>? filter = null, Func<Credential, dynamic>? order = null, bool orderAscending = true)
        {
            IEnumerable<Credential> query = _context.Credential.Where(credential => credential.DeletedAt == null)
                                                                    .Include(credential => credential.AssignedUsers)
                                                                    .Include(credential => credential.Website)
                                                                        .ThenInclude(credential => credential.Group);
            if (filter != null) query = query.Where(filter);

            if (order != null)
            {
                if (orderAscending) query = query.OrderBy(order);
                else query = query.OrderByDescending(order);
            }

            // Modify the Password field using FromBase64()
            return query.Skip(page * max).Take(max).Select(credential =>
            {
                try { credential.Password = credential.Password.FromBase64(); }
                catch { }
                return credential;
            });
        }


        public override async Task<Credential?> FindById(int id)
        {
            Credential credential = await _context.Credential
                .Include(c => c.AssignedUsers)
                .Include(c => c.Website)
                    .ThenInclude(c => c.Group)
                .Where(c => c.Id == id)
                .FirstAsync();

            // Modify the Password field using FromBase64()
            try { credential.Password = credential.Password.FromBase64(); }
            catch { }

            return credential;
        }

        public override async Task<Credential> Insert(Credential entity)
        {
            entity.Password = entity.Password.ToBase64();
            return await base.Insert(entity);
        }

        public override async Task<bool> Update(Credential oldEntity, Credential newEntity)
        {
            oldEntity.Username = newEntity.Username;
            oldEntity.Password = newEntity.Password.ToBase64();
            oldEntity.AssignedUsers = newEntity.AssignedUsers;
            oldEntity.Website = newEntity.Website;
            oldEntity.LastUpdatedAt = newEntity.LastUpdatedAt;
            return await base.Update(oldEntity, newEntity);
        }

        public async Task<bool> Update(Credential newEntity)
        {
            Credential? oldEntity = await FindById(newEntity.Id);

            if (oldEntity == null) return false;
            return await Update(oldEntity, newEntity);
        }
    }
}

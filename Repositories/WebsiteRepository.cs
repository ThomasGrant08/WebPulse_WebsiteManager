using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPulse_WebManager.Data;
using WebPulse_WebManager.Models;

namespace WebPulse_WebManager.Repositories
{
    public class WebsiteRepository : Repository<Website>
    {
        public WebsiteRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override IEnumerable<Website> FindAll(int page = 0, int max = int.MaxValue, Func<Website, bool>? filter = null, Func<Website, dynamic>? order = null, bool orderAscending = true)
        {
            IEnumerable<Website> query = _context.Website.Include(website => website.Users).Include(website => website.Group).Where(website => website.DeletedAt == null);

            if(filter != null) query = query.Where(filter);

            if(order != null)
            {
                if(orderAscending) query = query.OrderBy(order);
                else query = query.OrderByDescending(order);
            }

            return query.Skip(page * max).Take(max);
        }

        public override async Task<Website?> FindById(int id)
        {
            return await _context.Website.Include(website => website.Users).Include(credential => credential.Credentials).Where(website => website.Id == id && website.DeletedAt == null).FirstAsync();
        }

        public override async Task<Website> Insert (Website entity)
        {
            return await base.Insert(entity);
        }

        public override async Task<bool> Update(Website oldEntity, Website newEntity)
        {
            oldEntity.Name = newEntity.Name;
            oldEntity.Url = newEntity.Url;
            oldEntity.Users = newEntity.Users;
            oldEntity.Group = newEntity.Group;
            oldEntity.Credentials = newEntity.Credentials;
            oldEntity.LastUpdatedAt = newEntity.LastUpdatedAt;

            return await base.Update(oldEntity, newEntity);
        }

        public async Task<bool> Update(Website newEntity)
        {
            Website? oldEntity = await FindById(newEntity.Id);

            if(oldEntity == null) return false;
            return await Update(oldEntity, newEntity);
        }


    }
}

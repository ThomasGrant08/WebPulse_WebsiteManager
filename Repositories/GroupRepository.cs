using WebPulse_WebManager.Models;
using WebPulse_WebManager.Data;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using System.Collections;
using System.Linq;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using System.Security.Claims;
using WebPulse_WebManager.Utility;

namespace WebPulse_WebManager.Repositories
{
    public class GroupRepository: Repository<Group>
    {
        public GroupRepository(ApplicationDbContext context) : base(context)
        {
        }
        public override IEnumerable<Group> FindAll(int page = 0, int max = int.MaxValue, Func<Group, bool>? filter = null, Func<Group, dynamic>? order = null, bool orderAscending = true)
        {
            IEnumerable<Group> query = _context.Group.Where(group => group.DeletedAt == null).Include(group => group.AssignedUsers).Include(group => group.Websites);

            if (filter != null) query = query.Where(filter);

            if(order != null)
            {
                if(orderAscending) query = query.OrderBy(order);
                else query = query.OrderByDescending(order);
            }

            return query.Skip(page * max).Take(max);
        }

        public override async Task<Group?> FindById(int id)
        {
            return await _context.Group.Include(group => group.AssignedUsers).Include(group => group.Websites).Where(group => group.Id == id).FirstAsync();
        }

        public override async Task<Group> Insert(Group entity)
        {
            return await base.Insert(entity);
        }

        public override async Task<bool> Update(Group oldEntity, Group newEntity)
        {
            oldEntity.Name = newEntity.Name;
            oldEntity.Description = newEntity.Description;
            oldEntity.AssignedUsers = newEntity.AssignedUsers;
            oldEntity.Websites = newEntity.Websites;

            return await base.Update(oldEntity, newEntity);
        }

    }
}

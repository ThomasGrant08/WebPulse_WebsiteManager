using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WebPulse_WebManager.Data;
using WebPulse_WebManager.Models;
using WebPulse_WebManager.Utility;

namespace WebPulse_WebManager.Services
{
    public class CleanupService
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger<CleanupService> _logger;

        public CleanupService(ApplicationDbContext context, ILogger<CleanupService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void CleanupOldRecords()
        {

            try
            {
                _logger.LogInformation("Cleanup started at: {time}", DateTimeOffset.Now);
                var dbSets = _context.GetType().GetProperties()
                .Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                .ToList();

                foreach (var dbSetProperty in dbSets)
                {
                    _logger.LogInformation("Checking {TableName} for records to delete.", dbSetProperty.Name);
                    var entityType = dbSetProperty.PropertyType.GetGenericArguments()[0];

                    var deletedAtProperty = entityType.GetProperty("DeletedAt");

                    if (deletedAtProperty != null)
                    {
                        var recordsToDelete = ((IQueryable<object>)dbSetProperty.GetValue(_context))
                            .AsEnumerable() // Switch to client evaluation
                            .Where(e =>
                                (DateTime?)deletedAtProperty.GetValue(e) != null &&
                                (DateTime?)deletedAtProperty.GetValue(e) < DateTime.UtcNow.AddWeeks(-4)
                            )
                            .ToList();

                        _logger.LogInformation("Found {RecordCount} records in {TableName} to delete.", recordsToDelete.Count, entityType.Name);
                        // Delete the records
                        _context.RemoveRange(recordsToDelete.Cast<object>());
                        _logger.LogInformation("{RecordCount} records deleted from {TableName}.", recordsToDelete.Count, entityType.Name);

                    }


                }

                _context.SaveChanges();

                _logger.LogInformation("Cleanup finished at: {time}", DateTimeOffset.Now);

            } catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred executing cleanup");
            }
            
        }

    }
}

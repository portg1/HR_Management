using HR_Management.Application.Contracts.Persistense;
using HR_Management.Application.DTOs.LeaveAllocation;
using HR_Management.Domain;
using Microsoft.EntityFrameworkCore;

namespace HR_Management.Persistence.Repositories;

public class LeaveAllocationRepository : GenericRepository<LeaveAllocation>,ILeaveAllocationRepository
{
    private readonly LeaveManagementDbContext _dbContext;

    public LeaveAllocationRepository(LeaveManagementDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<LeaveAllocation>> GetleaveAllocationWithDetails()
    {
        return _dbContext.LeaveAllocations
            .Include(la => la.LeaveType)
            .ToListAsync();
    }

    public Task<LeaveAllocation?> GetleaveAllocationWithDetails(int id)
    {
        return _dbContext.LeaveAllocations
            .Include(la => la.LeaveType)
            .FirstOrDefaultAsync(la => la.Id == id);
    }
}
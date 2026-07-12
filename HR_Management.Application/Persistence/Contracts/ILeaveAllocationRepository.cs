using HR_Management.Domain;

namespace HR_Management.Application.Persistence.Contracts;

public interface ILeaveAllocationRepository:IGenericRepository<LeaveAllocation>
{
    Task<List<LeaveAllocation>> GetleaveAllocationWithDetails();
    Task<LeaveAllocation> GetleaveAllocationWithDetails(int id);
}
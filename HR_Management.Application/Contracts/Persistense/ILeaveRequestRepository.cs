using HR_Management.Domain;

namespace HR_Management.Application.Contracts.Persistense;

public interface ILeaveRequestRepository:IGenericRepository<LeaveRequest>
{
    Task<LeaveRequest?> GetLeaveRequestWithDetails(int id);
    Task<List<LeaveRequest>>  GetLeaveRequestsWithDetails();
    Task ChangeApprovalStatus(LeaveRequest leaveRequest,bool? approvalStatus);
    
}
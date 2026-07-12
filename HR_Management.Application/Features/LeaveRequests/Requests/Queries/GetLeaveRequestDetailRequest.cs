using HR_Management.Application.DTOs.LeaveRequest;
using MediatR;

namespace HR_Management.Application.Features.LeaveRequests.Requests.Queries;

public class GetLeaveRequestDetailRequest: IRequest<LeaveRequestDto>, IRequest
{
    public int Id { get; set; }
}
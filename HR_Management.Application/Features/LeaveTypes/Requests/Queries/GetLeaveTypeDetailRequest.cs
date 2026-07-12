using HR_Management.Application.DTOs;
using HR_Management.Application.DTOs.LeaveType;
using MediatR;

namespace HR_Management.Application.Features.LeaveTypes.Requests.Queries;

public class GetLeaveTypeDetailRequest: IRequest<LeaveTypeDto>, IRequest
{
    public int Id { get; set; }
}
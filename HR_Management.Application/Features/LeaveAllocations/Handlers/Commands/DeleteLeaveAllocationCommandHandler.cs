using AutoMapper;
using HR_Management.Application.Features.LeaveAllocations.Requests.Commands;
using HR_Management.Application.Persistence.Contracts;
using MediatR;

namespace HR_Management.Application.Features.LeaveAllocations.Handlers.Commands;

public class DeleteLeaveAllocationCommandHandler : IRequestHandler<DeleteLeaveAllocationCommand, Unit>
{
    private readonly ILeaveAllocationRepository _leaveAllocationRepository;
    private readonly IMapper _mapper;

    public DeleteLeaveAllocationCommandHandler(
        ILeaveAllocationRepository leaveAllocationRepository,
        IMapper mapper)
    {
        _leaveAllocationRepository = leaveAllocationRepository;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(
        DeleteLeaveAllocationCommand request,
        CancellationToken cancellationToken)
    {
        var leaveAllocation = await _leaveAllocationRepository.Get(request.Id);
        await _leaveAllocationRepository.Delete(leaveAllocation);
        return Unit.Value;
    }
}

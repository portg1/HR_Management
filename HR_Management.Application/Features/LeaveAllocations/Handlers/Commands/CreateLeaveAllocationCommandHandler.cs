using AutoMapper;
using HR_Management.Application.Features.LeaveAllocations.Requests.Commands;
using HR_Management.Application.Persistence.Contracts;
using HR_Management.Domain;
using MediatR;

namespace HR_Management.Application.Features.LeaveAllocations.Handlers.Commands;

public class CreateLeaveAllocationCommandHandler : IRequestHandler<CreateLeaveAllocationCommand, int>
{
    private readonly ILeaveAllocationRepository _leaveAllocationRepository;
    private readonly IMapper _mapper;

    public CreateLeaveAllocationCommandHandler(ILeaveAllocationRepository leaveAllocationRepository, IMapper mapper)
    {
        _leaveAllocationRepository = leaveAllocationRepository;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateLeaveAllocationCommand command, CancellationToken cancellationToken)
    {
        var leaveAllocation = _mapper.Map<LeaveAllocation>(command.LeaveAllocationDto);
        leaveAllocation = await _leaveAllocationRepository.Add(leaveAllocation);
        return leaveAllocation.Id;
    }
}

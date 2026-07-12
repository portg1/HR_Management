using AutoMapper;
using HR_Management.Application.DTOs.LeaveAllocation.Validators;
using HR_Management.Application.Exceptions;
using HR_Management.Application.Features.LeaveAllocations.Requests.Commands;
using HR_Management.Application.Persistence.Contracts;
using MediatR;

namespace HR_Management.Application.Features.LeaveAllocations.Handlers.Commands;

public class UpdateLeaveAllocationCommandHandler : IRequestHandler<UpdateLeaveAllocationCommand,Unit>

{
    private readonly ILeaveAllocationRepository _leaveAllocationRepository;
    private readonly IMapper _mapper;
    private readonly ILeaveTypeRepository _leaveTypeRepository;

    public UpdateLeaveAllocationCommandHandler(ILeaveAllocationRepository leaveAllocationRepository, IMapper mapper,
        ILeaveTypeRepository leaveTypeRepository)
    {
        _leaveAllocationRepository = leaveAllocationRepository;
        _mapper = mapper;
        _leaveTypeRepository = leaveTypeRepository;
    }
    public async Task<Unit> Handle(UpdateLeaveAllocationCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateLeaveAllocationDtoValidator(_leaveTypeRepository);
        var validationResult = await validator.ValidateAsync(request.LeaveAllocationDto);
        if (validationResult.IsValid == false)
        {
            throw new ValidationException(validationResult);
        }

        var leaveAllocation = await _leaveAllocationRepository.Get(request.LeaveAllocationDto.Id);
        _mapper.Map(request.LeaveAllocationDto, leaveAllocation);
        await _leaveAllocationRepository.Update(leaveAllocation);
        return Unit.Value;
    }
}

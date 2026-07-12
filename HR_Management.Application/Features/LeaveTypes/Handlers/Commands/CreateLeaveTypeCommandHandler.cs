using AutoMapper;
using HR_Management.Application.DTOs.LeaveType.Validators;
using HR_Management.Application.Features.LeaveTypes.Requests.Commands;
using HR_Management.Application.Persistence.Contracts;
using HR_Management.Domain;
using MediatR;

namespace HR_Management.Application.Features.LeaveTypes.Handlers.Commands;

public class CreateLeaveTypeCommandHandler:IRequestHandler<CreateLeaveTypeCommand,int>
{
    private readonly ILeaveTypeRepository _leaveTypeRepository;
    private readonly IMapper _mapper;

    public CreateLeaveTypeCommandHandler(ILeaveTypeRepository leaveTypeRepository, IMapper mapper )
    {
        _leaveTypeRepository = leaveTypeRepository;
        _mapper = mapper;
    }
    public async Task<int> Handle(CreateLeaveTypeCommand command, CancellationToken cancellationToken)
    {
        var validator = new ILeaveTypeDtoValidator();
        var validationResult = await validator.ValidateAsync(command.LeaveTypeDto);
        if (validationResult.IsValid == false)
        {
            throw new Exception();

        }
        var leaveType = _mapper.Map<LeaveType>(command.LeaveTypeDto);
        leaveType = await _leaveTypeRepository.Add(leaveType);
        return leaveType.Id;
    }
}
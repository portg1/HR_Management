using AutoMapper;
using HR_Management.Application.Exceptions;
using HR_Management.Application.Features.LeaveTypes.Requests.Commands;
using HR_Management.Application.Persistence.Contracts;
using MediatR;

namespace HR_Management.Application.Features.LeaveTypes.Handlers.Commands;

public class DeleteLeaveTypeCommandHandler:IRequestHandler<DeleteLeaveTypeCommand,Unit>
{
    private readonly ILeaveTypeRepository _leaveTypeRepository;
    private readonly IMapper _mapper;

    public DeleteLeaveTypeCommandHandler(ILeaveTypeRepository leaveTypeRepository, IMapper mapper)
    {
        _leaveTypeRepository = leaveTypeRepository;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(DeleteLeaveTypeCommand request, CancellationToken cancellationToken)
    {
        var leaveType = await _leaveTypeRepository.Get(request.Id);
        if (leaveType == null)
        {
            throw new NotFoundException(nameof(leaveType), request.Id);
        }
        await _leaveTypeRepository.Delete(leaveType);
        return Unit.Value;
    }
    
}

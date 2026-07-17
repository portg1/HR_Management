using AutoMapper;
using HR_Management.Application.Contracts.Infrastructure;
using HR_Management.Application.Contracts.Persistense;
using HR_Management.Application.Exceptions;
using HR_Management.Application.DTOs.LeaveType.Validators;
using HR_Management.Application.Features.LeaveTypes.Requests.Commands;
using HR_Management.Domain;
using MediatR;

namespace HR_Management.Application.Features.LeaveTypes.Handlers.Commands;

public class CreateLeaveTypeCommandHandler:IRequestHandler<CreateLeaveTypeCommand,int>
{
    // Cache key that matches GetLeaveTypeListRequestHandler — invalidate on write
    private const string CacheKey = "leave_types_list";

    private readonly ILeaveTypeRepository _leaveTypeRepository;
    private readonly IMapper _mapper;
    private readonly ICacheService _cacheService;

    public CreateLeaveTypeCommandHandler(
        ILeaveTypeRepository leaveTypeRepository,
        IMapper mapper,
        ICacheService cacheService)
    {
        _leaveTypeRepository = leaveTypeRepository;
        _mapper = mapper;
        _cacheService = cacheService;
    }
    public async Task<int> Handle(CreateLeaveTypeCommand command, CancellationToken cancellationToken)
    {
        var validator = new ILeaveTypeDtoValidator();
        var validationResult = await validator.ValidateAsync(command.LeaveTypeDto);
        if (validationResult.IsValid == false)
        {
            throw new ValidationException(validationResult);

        }
        var leaveType = _mapper.Map<LeaveType>(command.LeaveTypeDto);
        leaveType = await _leaveTypeRepository.Add(leaveType);

        // Invalidate cache so the list query fetches fresh data next time
        await _cacheService.RemoveAsync(CacheKey, cancellationToken);

        return leaveType.Id;
    }
}
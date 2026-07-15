using FluentValidation;
using HR_Management.Application.Contracts.Persistense;

namespace HR_Management.Application.DTOs.LeaveRequest.Validators;

public class CreateLeaveRequestDtoValidator:AbstractValidator<CreateLeaveRequestDto>
{
    public CreateLeaveRequestDtoValidator(ILeaveTypeRepository leaveTypeRepository)
    {
        RuleFor(p => p.StartDate)
            .LessThan(p => p.EndDate)
            .WithMessage("{propertyName} must be before {comparisonValue}.}");
        RuleFor(p => p.EndDate)
            .GreaterThan(p => p.StartDate)
            .WithMessage("{propertyName} must be after {comparisonValue}.");
        RuleFor(q => q.LeaveTypeId)
            .GreaterThan(0)
            .MustAsync(async (id, token) =>
            {
                var leaveTypeExist = await leaveTypeRepository.Exist(id);
                return leaveTypeExist;
            })
            .WithMessage("{propertyName} does not exist.");
    }
    
}

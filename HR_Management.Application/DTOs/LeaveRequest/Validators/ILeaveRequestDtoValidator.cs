using FluentValidation;
using HR_Management.Application.Persistence.Contracts;

namespace HR_Management.Application.DTOs.LeaveRequest.Validators;

public class ILeaveRequestDtoValidator : AbstractValidator<UpdateLeaveRequestDto>
{
    public ILeaveRequestDtoValidator(ILeaveTypeRepository leaveTypeRepository)
    {
        RuleFor(p => p.StartDate)
            .LessThan(p => p.EndDate)
            .WithMessage("{PropertyName} must be before {comparisonValue}.");
        RuleFor(p => p.EndDate)
            .GreaterThan(p => p.StartDate)
            .WithMessage("{PropertyName} must be after {comparisonValue}.");
        RuleFor(q => q.LeaveTypeId)
            .GreaterThan(0)
            .MustAsync(async (id, token) =>
            {
                var leaveTypeExist = await leaveTypeRepository.Exist(id);
                return !leaveTypeExist;
            })
            .WithMessage("{PropertyName} does not exist.");
    }
}
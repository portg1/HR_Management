using FluentValidation;

namespace HR_Management.Application.DTOs.LeaveType.Validators;

public class ILeaveTypeDtoValidator : AbstractValidator<ILeaveTypeDto>
{
    public ILeaveTypeDtoValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("{propertyName} is required")
            .NotNull()
            .MaximumLength(50).WithMessage("{propertyName} must not exceed 50 characters");
        RuleFor(p => p.DefaultDay)
            .NotEmpty().WithMessage("{propertyName} is required")
            .GreaterThan(0).WithMessage("{propertyName} must be greater than 0")
            .LessThan(100).WithMessage("{propertyName} must be less than 100");
    }
    
}
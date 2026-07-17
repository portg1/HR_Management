    using AutoMapper;
using HR_Management.Application.Contracts.Persistense;
using HR_Management.Application.DTOs.LeaveAllocation;
using HR_Management.Application.Exceptions;
using HR_Management.Application.Features.LeaveAllocations.Handlers.Commands;
using HR_Management.Application.Features.LeaveAllocations.Requests.Commands;
using HR_Management.Application.Profiles;
using HR_Management.Application.UnitTests.Mocks;
using Moq;
using Xunit;

namespace HR_Management.Application.UnitTests.LeaveAllocations.Command
{
    public class CreateLeaveAllocationCommandHandlerTests
    {
        private readonly Mock<ILeaveAllocationRepository> _mockRepository;
        private readonly Mock<ILeaveTypeRepository> _mockLeaveTypeRepository;
        private readonly IMapper _mapper;

        public CreateLeaveAllocationCommandHandlerTests()
        {
            _mockRepository = MockLeaveAllocationRepository.GetLeaveAllocationRepository();
            _mockLeaveTypeRepository = MockLeaveRepository.GetLeaveTypeRepository();

            var mapperConfig = new MapperConfiguration(m =>
            {
                m.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task ValidCommand_AddsAndReturnsId()
        {
            var command = new CreateLeaveAllocationCommand
            {
                LeaveAllocationDto = new CreateLeaveAllocationDto
                {
                    NumberOfDays = 12,
                    LeaveTypeId = 1,
                    Priod = 2026
                }
            };

            var handler = new CreateLeaveAllocationCommandHandler(
                _mockRepository.Object, _mapper, _mockLeaveTypeRepository.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.Equal(3, result);

            _mockRepository.Verify(r => r.Add(It.Is<HR_Management.Domain.LeaveAllocation>(la =>
                la.NumberOfDays == 12 &&
                la.LeaveTypeId == 1 &&
                la.Priod == 2026)), Times.Once);
        }

        [Fact]
        public async Task InvalidCommand_ThrowsValidation()
        {
            var command = new CreateLeaveAllocationCommand
            {
                LeaveAllocationDto = new CreateLeaveAllocationDto
                {
                    NumberOfDays = 0,
                    LeaveTypeId = 0,
                    Priod = 2020
                }
            };

            var handler = new CreateLeaveAllocationCommandHandler(
                _mockRepository.Object, _mapper, _mockLeaveTypeRepository.Object);

            await Assert.ThrowsAsync<ValidationException>(() =>
                handler.Handle(command, CancellationToken.None));

            _mockRepository.Verify(r => r.Add(It.IsAny<HR_Management.Domain.LeaveAllocation>()), Times.Never);
        }
    }
}

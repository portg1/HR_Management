using AutoMapper;
using HR_Management.Application.Contracts.Infrastructure;
using HR_Management.Application.Contracts.Persistense;
using HR_Management.Application.DTOs.LeaveRequest;
using HR_Management.Application.Features.LeaveRequests.Handlers.Commands;
using HR_Management.Application.Features.LeaveRequests.Requests.Commands;
using HR_Management.Application.Profiles;
using HR_Management.Application.UnitTests.Mocks;
using Moq;
using Xunit;

namespace HR_Management.Application.UnitTests.LeaveRequests.Command
{
    public class CreateLeaveRequestCommandHandlerTests
    {
        private readonly Mock<ILeaveRequestRepository> _mockRepository;
        private readonly Mock<ILeaveTypeRepository> _mockLeaveTypeRepository;
        private readonly Mock<IEmailSender> _mockEmailSender;
        private readonly IMapper _mapper;

        public CreateLeaveRequestCommandHandlerTests()
        {
            _mockRepository = MockLeaveRequestRepository.GetLeaveRequestRepository();
            _mockLeaveTypeRepository = MockLeaveRepository.GetLeaveTypeRepository();
            _mockEmailSender = new Mock<IEmailSender>();
            _mockEmailSender.Setup(e => e.SendEmail(It.IsAny<HR_Management.Application.Models.Email>()))
                .ReturnsAsync(true);

            var mapperConfig = new MapperConfiguration(m =>
            {
                m.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task ValidCommand_AddsAndReturnsId()
        {
            var command = new CreateLeaveRequestCommand
            {
                LeaveRequestDto = new CreateLeaveRequestDto
                {
                    StartDate = new DateTime(2026, 3, 1),
                    EndDate = new DateTime(2026, 3, 5),
                    LeaveTypeId = 1,
                    DateRequested = new DateTime(2026, 3, 1),
                    RequestComments = "Vacation request"
                }
            };

            var handler = new CreateLeaveRequestCommandHandler(
                _mockRepository.Object, _mapper, _mockLeaveTypeRepository.Object, _mockEmailSender.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal(3, result.Id);
            Assert.Equal("Leave Request Created Successfully", result.Message);

            _mockRepository.Verify(r => r.Add(It.Is<HR_Management.Domain.LeaveRequest>(lr =>
                lr.LeaveTypeId == 1 &&
                lr.StartDate == new DateTime(2026, 3, 1) &&
                lr.EndDate == new DateTime(2026, 3, 5))), Times.Once);

            _mockEmailSender.Verify(e => e.SendEmail(It.IsAny<HR_Management.Application.Models.Email>()), Times.Once);
        }

        [Fact]
        public async Task InvalidCommand_ReturnsFailure()
        {
            var command = new CreateLeaveRequestCommand
            {
                LeaveRequestDto = new CreateLeaveRequestDto
                {
                    StartDate = new DateTime(2026, 3, 5),
                    EndDate = new DateTime(2026, 3, 1),
                    LeaveTypeId = 0,
                    DateRequested = new DateTime(2026, 3, 1),
                    RequestComments = "Invalid request"
                }
            };

            var handler = new CreateLeaveRequestCommandHandler(
                _mockRepository.Object, _mapper, _mockLeaveTypeRepository.Object, _mockEmailSender.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("Creation Failed", result.Message);
            Assert.NotNull(result.Errors);
            Assert.NotEmpty(result.Errors);

            _mockRepository.Verify(r => r.Add(It.IsAny<HR_Management.Domain.LeaveRequest>()), Times.Never);
            _mockEmailSender.Verify(e => e.SendEmail(It.IsAny<HR_Management.Application.Models.Email>()), Times.Never);
        }
    }
}

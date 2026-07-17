using AutoMapper;
using HR_Management.Application.Contracts.Infrastructure;
using HR_Management.Application.Contracts.Persistense;
using HR_Management.Application.DTOs.LeaveType;
using HR_Management.Application.Profiles;
using HR_Management.Application.UnitTests.Mocks;
using Moq;
using HR_Management.Application.Features.LeaveTypes.Handlers.Commands;
using HR_Management.Application.Features.LeaveTypes.Requests.Commands;
using HR_Management.Domain;

namespace HR_Management.Application.UnitTests.LeaveTypes.Command
{
    public class CreateLeaveTypeCommandHandlerTests
    {
        private readonly Mock<ILeaveTypeRepository> _mockRepository;
        private readonly Mock<ICacheService> _mockCacheService;
        private readonly IMapper _mapper;

        public CreateLeaveTypeCommandHandlerTests()
        {
            _mockRepository = MockLeaveRepository.GetLeaveTypeRepository();
            _mockCacheService = new Mock<ICacheService>();
            var mapperConfig = new MapperConfiguration(m =>
            {
                m.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task ValidCommand_AddsAndReturnsId()
        {
            var command = new CreateLeaveTypeCommand
            {
                LeaveTypeDto = new CreateLeaveTypeDto
                {
                    Name = "Test Vacation",
                    DefaultDay = 10
                }
            };

            _mockRepository
                .Setup(repository => repository.Add(It.IsAny<LeaveType>()))
                .ReturnsAsync((LeaveType leaveType) =>
                {
                    leaveType.Id = 3;
                    return leaveType;
                });

            var handler = new CreateLeaveTypeCommandHandler(_mockRepository.Object, _mapper, _mockCacheService.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.Equal(3, result);

            _mockRepository.Verify(repository => repository.Add(
                It.Is<LeaveType>(leaveType =>
                    leaveType.Name == "Test Vacation" &&
                    leaveType.DefaultDay == 10)),
                Times.Once);

            // Verify cache was invalidated after create
            _mockCacheService.Verify(cs => cs.RemoveAsync("leave_types_list", CancellationToken.None), Times.Once);
        }

    }
}

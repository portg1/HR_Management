using AutoMapper;
using HR_Management.Application.Contracts.Persistense;
using HR_Management.Application.DTOs.LeaveAllocation;
using HR_Management.Application.Features.LeaveAllocations.Handlers.Queries;
using HR_Management.Application.Features.LeaveAllocations.Requests.Queries;
using HR_Management.Application.Profiles;
using HR_Management.Application.UnitTests.Mocks;
using Moq;
using Xunit;

namespace HR_Management.Application.UnitTests.LeaveAllocations.Queries
{
    public class GetLeaveAllocationListRequestHandlerTests
    {
        private readonly Mock<ILeaveAllocationRepository> _mockRepository;
        private readonly IMapper _mapper;

        public GetLeaveAllocationListRequestHandlerTests()
        {
            _mockRepository = MockLeaveAllocationRepository.GetLeaveAllocationRepository();

            var mapperConfig = new MapperConfiguration(m =>
            {
                m.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task Handle_ReturnsLeaveAllocationList()
        {
            var handler = new GetLeaveAllocationListRequestHandler(_mockRepository.Object, _mapper);

            var result = await handler.Handle(new GetLeaveAllocationListRequest(), CancellationToken.None);

            Assert.NotNull(result);
            Assert.IsType<List<LeaveAllocationDto>>(result);
            Assert.True(result.Count > 0);
            Assert.NotNull(result[0].LeaveType);
        }
    }
}

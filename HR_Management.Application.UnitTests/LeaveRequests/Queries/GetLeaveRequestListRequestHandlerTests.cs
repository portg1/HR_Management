using AutoMapper;
using HR_Management.Application.Contracts.Persistense;
using HR_Management.Application.DTOs.LeaveRequest;
using HR_Management.Application.Features.LeaveRequests.Handlers.Queries;
using HR_Management.Application.Features.LeaveRequests.Requests.Queries;
using HR_Management.Application.Profiles;
using HR_Management.Application.UnitTests.Mocks;
using Moq;
using Xunit;

namespace HR_Management.Application.UnitTests.LeaveRequests.Queries
{
    public class GetLeaveRequestListRequestHandlerTests
    {
        private readonly Mock<ILeaveRequestRepository> _mockRepository;
        private readonly IMapper _mapper;

        public GetLeaveRequestListRequestHandlerTests()
        {
            _mockRepository = MockLeaveRequestRepository.GetLeaveRequestRepository();

            var mapperConfig = new MapperConfiguration(m =>
            {
                m.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task Handle_ReturnsLeaveRequestList()
        {
            var handler = new GetLeaveRequestListRequestHandler(_mockRepository.Object, _mapper);

            var result = await handler.Handle(new GetLeaveRequestsListRequest(), CancellationToken.None);

            Assert.NotNull(result);
            Assert.IsType<List<LeaveRequestListDto>>(result);
            Assert.True(result.Count > 0);
            Assert.NotNull(result[0].LeaveType);
        }
    }
}

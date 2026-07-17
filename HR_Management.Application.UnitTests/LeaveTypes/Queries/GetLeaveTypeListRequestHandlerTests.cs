using AutoMapper;
using HR_Management.Application.Contracts.Infrastructure;
using HR_Management.Application.Contracts.Persistense;
using HR_Management.Application.DTOs.LeaveType;
using HR_Management.Application.Features.LeaveTypes.Requests.Queries;
using HR_Management.Application.Profiles;
using HR_Management.Application.UnitTests.Mocks;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HR_Management.Application.Features.LeaveTypes.Handlers.Queries;
using Xunit;

namespace HR_Management.Application.UnitTests.LeaveTypes.Queries;

public class GetLeaveTypeListRequestHandlerTests
{
    private readonly Mock<ILeaveTypeRepository> _mockRepository;
    private readonly Mock<ICacheService> _mockCacheService;
    private readonly IMapper _mapper;

    public GetLeaveTypeListRequestHandlerTests()
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
    public async Task GetLeaveTypeListTest()
    {
        // Arrange
        var handler = new GetLeaveTypeListRequestHandler(_mockRepository.Object, _mapper, _mockCacheService.Object);
        
        // Act
        var result = await handler.Handle(new GetLeaveTypeListRequest(), CancellationToken.None);
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<LeaveTypeDto>>(result);
        Assert.True(result.Count > 0);
    }
}
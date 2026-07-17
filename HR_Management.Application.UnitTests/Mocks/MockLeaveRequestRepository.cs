using HR_Management.Application.Contracts.Persistense;
using HR_Management.Domain;
using Moq;

namespace HR_Management.Application.UnitTests.Mocks;

public static class MockLeaveRequestRepository
{
    public static Mock<ILeaveRequestRepository> GetLeaveRequestRepository()
    {
        var leaveRequests = new List<LeaveRequest>()
        {
            new LeaveRequest
            {
                Id = 1,
                StartDate = new DateTime(2026, 1, 1),
                EndDate = new DateTime(2026, 1, 5),
                LeaveTypeId = 1,
                LeaveType = new LeaveType { Id = 1, Name = "Test Vaction", DefaultDay = 10 },
                DateRequested = new DateTime(2026, 1, 1),
                RequestComments = "Test comment 1",
                Approved = true
            },
            new LeaveRequest
            {
                Id = 2,
                StartDate = new DateTime(2026, 2, 1),
                EndDate = new DateTime(2026, 2, 3),
                LeaveTypeId = 2,
                LeaveType = new LeaveType { Id = 2, Name = "Test Sick", DefaultDay = 15 },
                DateRequested = new DateTime(2026, 2, 1),
                RequestComments = "Test comment 2",
                Approved = false
            }
        };

        var mockRepo = new Mock<ILeaveRequestRepository>();
        mockRepo.Setup(r => r.GetLeaveRequestsWithDetails()).ReturnsAsync(leaveRequests);
        mockRepo.Setup(r => r.Add(It.IsAny<LeaveRequest>()))
            .ReturnsAsync((LeaveRequest leaveRequest) =>
            {
                leaveRequest.Id = 3;
                return leaveRequest;
            });
        return mockRepo;
    }
}

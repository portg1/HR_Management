using HR_Management.Application.Contracts.Persistense;
using HR_Management.Domain;
using Moq;

namespace HR_Management.Application.UnitTests.Mocks;

public static class MockLeaveAllocationRepository
{
    public static Mock<ILeaveAllocationRepository> GetLeaveAllocationRepository()
    {
        var leaveAllocations = new List<LeaveAllocation>()
        {
            new LeaveAllocation
            {
                Id = 1,
                NumberOfDays = 10,
                LeaveTypeId = 1,
                LeaveType = new LeaveType { Id = 1, Name = "Test Vaction", DefaultDay = 10 },
                Priod = 2026,
                DateCreated = new DateTime(2026, 1, 1)
            },
            new LeaveAllocation
            {
                Id = 2,
                NumberOfDays = 15,
                LeaveTypeId = 2,
                LeaveType = new LeaveType { Id = 2, Name = "Test Sick", DefaultDay = 15 },
                Priod = 2026,
                DateCreated = new DateTime(2026, 1, 1)
            }
        };

        var mockRepo = new Mock<ILeaveAllocationRepository>();
        mockRepo.Setup(r => r.GetleaveAllocationWithDetails()).ReturnsAsync(leaveAllocations);
        mockRepo.Setup(r => r.Add(It.IsAny<LeaveAllocation>()))
            .ReturnsAsync((LeaveAllocation leaveAllocation) =>
            {
                leaveAllocation.Id = 3;
                return leaveAllocation;
            });
        return mockRepo;
    }
}

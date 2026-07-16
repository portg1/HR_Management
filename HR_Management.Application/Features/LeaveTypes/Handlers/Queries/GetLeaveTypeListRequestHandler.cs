using AutoMapper;
using HR_Management.Application.Contracts.Persistense;
using HR_Management.Application.DTOs.LeaveType;
using HR_Management.Application.Features.LeaveTypes.Requests.Queries;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HR_Management.Application.Features.LeaveTypes.Handlers.Queries
{
    public class GetLeaveTypeListRequestHandler : IRequestHandler<GetLeaveTypeListRequest, List<LeaveTypeDto>>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IMapper _mapper;

        public GetLeaveTypeListRequestHandler(ILeaveTypeRepository leaveTypeRepository, IMapper mapper)
        {
            _leaveTypeRepository = leaveTypeRepository;
            _mapper = mapper;
        }

        public async Task<List<LeaveTypeDto>> Handle(GetLeaveTypeListRequest request, CancellationToken cancellationToken)
        {
            var leaveTypes = await _leaveTypeRepository.GetAll();
            
            if (leaveTypes == null)
                return new List<LeaveTypeDto>();
                
            return _mapper.Map<List<LeaveTypeDto>>(leaveTypes);
        }
    }
}
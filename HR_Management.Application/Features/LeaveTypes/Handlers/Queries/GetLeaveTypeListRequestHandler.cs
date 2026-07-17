using AutoMapper;
using HR_Management.Application.Contracts.Infrastructure;
using HR_Management.Application.Contracts.Persistense;
using HR_Management.Application.DTOs.LeaveType;
using HR_Management.Application.Features.LeaveTypes.Requests.Queries;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HR_Management.Application.Features.LeaveTypes.Handlers.Queries
{
    // Handler for GetLeaveTypeListRequest — uses cache-aside pattern
    public class GetLeaveTypeListRequestHandler : IRequestHandler<GetLeaveTypeListRequest, List<LeaveTypeDto>>
    {
        // Cache key for leave type list
        private const string CacheKey = "leave_types_list";

        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public GetLeaveTypeListRequestHandler(
            ILeaveTypeRepository leaveTypeRepository,
            IMapper mapper,
            ICacheService cacheService)
        {
            _leaveTypeRepository = leaveTypeRepository;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<List<LeaveTypeDto>> Handle(GetLeaveTypeListRequest request, CancellationToken cancellationToken)
        {
            // 1 : Try cache first — return if found
            var cached = await _cacheService.GetAsync<List<LeaveTypeDto>>(CacheKey, cancellationToken);
            if (cached is not null)
                return cached;

            // 2 : Cache miss — fetch from database
            var leaveTypes = await _leaveTypeRepository.GetAll();

            // 3 : Return empty list if DB has no data
            if (leaveTypes is null)
                return new List<LeaveTypeDto>();

            // 4 : Map entities to DTOs and store in cache for next time
            var dtos = _mapper.Map<List<LeaveTypeDto>>(leaveTypes);
            await _cacheService.SetAsync(CacheKey, dtos, cancellationToken: cancellationToken);

            return dtos;
        }
    }
}
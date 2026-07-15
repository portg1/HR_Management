using HR_Management.Application.DTOs.LeaveRequest;
using HR_Management.Application.Features.LeaveRequests.Requests.Commands;
using HR_Management.Application.Features.LeaveRequests.Requests.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HR_Managemen.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveRequestController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LeaveRequestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/<LeaveRequestController>
        [HttpGet]
        public async Task<ActionResult<List<LeaveRequestListDto>>> Get()
        {
            var leaveRequests = await _mediator.Send(new GetLeaveRequestsListRequest());
            return Ok(leaveRequests);
        }

        // GET api/<LeaveRequestController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LeaveRequestDto>> Get(int id)
        {
            var leaveRequest = await _mediator.Send(
                new GetLeaveRequestDetailRequest { Id = id });

            return Ok(leaveRequest);
        }

        // POST api/<LeaveRequestController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateLeaveRequestDto leaveRequest)
        {
            var command = new CreateLeaveRequestCommand
            {
                LeaveRequestDto = leaveRequest
            };

            var response = await _mediator.Send(command);
            return Ok(response);
        }

        // PUT api/<LeaveRequestController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(
            int id,
            [FromBody] UpdateLeaveRequestDto leaveRequest)
        {
            leaveRequest.Id = id;

            var command = new UpdateLeaveRequestCommand
            {
                Id = id,
                LeaveRequestDto = leaveRequest
            };

            await _mediator.Send(command);
            return NoContent();
        }

        // PUT api/<LeaveRequestController>/ChangeApproval/5
        [HttpPut("changeapproval/{id}")]
        public async Task<ActionResult> ChangeApproval(
            int id,
            [FromBody] ChangeLeaveRequestApprovalDto changeLeaveRequestApproval)
        {
            //changeLeaveRequestApproval.Id = id;

            var command = new UpdateLeaveRequestCommand
            {
                Id = id,
                ChangeLeaveRequestApprovalDto = changeLeaveRequestApproval
            };

            await _mediator.Send(command);
            return NoContent();
        }

        // DELETE api/<LeaveRequestController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteLeaveRequestCommand { Id = id });
            return NoContent();
        }

    }
}

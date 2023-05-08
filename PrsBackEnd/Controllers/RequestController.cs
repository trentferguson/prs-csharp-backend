using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrsBackEnd.Models;

namespace PrsBackEnd.Controllers
{
    [Route("requests")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly PrsDbContext _context;

        public RequestController(PrsDbContext context)
        {
            _context = context;
        }

        // Get all requests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequests()
        {
            return await _context.Requests.Include(r => r.User).ToListAsync();
        }

        // get request by Id
        [HttpGet("{id}")]
        public async Task<ActionResult<Request>> GetRequest(int id)
        {
            //var request = await _context.Requests.FindAsync(id);

            var request = await _context.Requests.Where(r => r.Id == id)
                                                  .Include(r => r.User)
                                                  .FirstOrDefaultAsync();

            if (request == null)
            {
                return NotFound();
            }

            return request;
        }

        // update request
        [HttpPut]
        public async Task<IActionResult> PutRequest([FromBody] Request UpdatedRequest)
        {
            var request = new Request();


            _context.Entry(UpdatedRequest).State = EntityState.Modified;

            try
            {
                request = UpdatedRequest;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(UpdatedRequest.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(request);
        }

        // POST: /Request
        [HttpPost]
        public async Task<ActionResult<Request>> PostRequest([FromBody] Request request)
        {
            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequest", new { id = request.Id }, request);
        }

        // DELETE: /Request/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);

            if (request == null)
            {
                return NotFound();
            }

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RequestExists(int id)
        {
            return _context.Requests.Any(e => e.Id == id);
        }

        [HttpPut("/approve")]
        public async Task<IActionResult> ApproveRequest([FromBody] Request request)
        {
            var req = await _context.Requests.FindAsync(request.Id);

            if (req == null)
            {
                return NotFound();
            }

            req.Status = "APPROVED";

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("/reject")]
        public async Task<IActionResult> RejectRequest([FromBody] Request request)
        {
            var req = await _context.Requests.FindAsync(request.Id);

            if (req == null)
            {
                return NotFound();
            }

            req.Status = "REJECTED";

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("/review")]
        public async Task<IActionResult> ReviewRequest([FromBody] Request request)
        {
            var req = await _context.Requests.FindAsync(request.Id);

            if (req == null) { return NotFound(); }

            if (req.Total == 0) { throw new Exception("Request not valid"); }

            req.Status = (req.Total <= 50 && req.Total > 0) ? "APPROVED" : "REVIEW";

            return Ok();
        }

        // REOPEN: /reopen
        [HttpPut]
        [Route("/reopen")]
        public async Task<ActionResult<Request>> Reopen([FromBody] Request UpdatedRequest)
        {
            var request = await _context.Requests.FindAsync(UpdatedRequest.Id);

            if (request == null)
            {
                return NotFound();
            }
            else
            {
                request.Status = "REOPENED";
            }
            await _context.SaveChangesAsync();

            return request;
        }

        [HttpGet("/requests/list-review/{id}")]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequestsForReview(int id)
        {
            var req = await _context.Requests.Where(r => r.Status == "REVIEW" && r.UserId != id && (r.User.IsAdmin && r.User.IsReviewer)).ToListAsync();

            return req;
        }
    }
}

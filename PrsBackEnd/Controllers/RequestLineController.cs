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
    [Route("request-lines")]
    [ApiController]
    public class RequestLineController : ControllerBase
    {
        private readonly PrsDbContext _context;

        public RequestLineController(PrsDbContext context)
        {
            _context = context;
        }

        // get all (/request-lines)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestLine>>> GetRequestLines()
        {
            return await _context.RequestLines
                                             .Include(r => r.Request).ThenInclude(request => request.User)
                                             .Include(r => r.Product).ThenInclude(product => product.Vendor)
                                             .ToListAsync();
        }

        // get by ID /request-line/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RequestLine>> GetRequestLine(int id)
        {
            var requestLine = await _context.RequestLines.FindAsync(id);


            if (requestLine == null)
            {
                return NotFound();
            }

            return requestLine;
        }

        // PUT: api/RequestLine/5
        [HttpPut]
        public async Task<IActionResult> PutRequestLine([FromBody] RequestLine requestLine)
        {

            _context.Entry(requestLine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestLineExists(requestLine.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: /request-line
        [HttpPost]
        public async Task<ActionResult<RequestLine>> PostRequestLine([FromBody] RequestLine requestLine)
        {
            _context.RequestLines.Add(requestLine);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequestLine", new { id = requestLine.Id }, requestLine);
        }

        // DELETE: /request-line/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequestLine(int id)
        {
            var requestLine = await _context.RequestLines.FindAsync(id);
            if (requestLine == null)
            {
                return NotFound();
            }

            _context.RequestLines.Remove(requestLine);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RequestLineExists(int id)
        {
            return _context.RequestLines.Any(e => e.Id == id);
        }

        void RecalcRequestTotal(int requestId)
        {
            //get the total
            //find the request
            //update the request
            //SaveChanges();
        }
    }
}

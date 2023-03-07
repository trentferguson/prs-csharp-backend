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

        // get all requestlines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestLine>>> GetRequestLines()
        {
            return await _context.RequestLines
                                             .Include(r => r.Request).ThenInclude(request => request.User)
                                             .Include(r => r.Product).ThenInclude(product => product.Vendor)
                                             .ToListAsync();
        }

        // get requestline by Id
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

        // update requestline
        [HttpPut]
        public async Task<IActionResult> PutRequestLine([FromBody] RequestLine requestLine)
        {

            _context.Entry(requestLine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                await Recalculate(requestLine.RequestId);
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

        // create new requestline
        [HttpPost]
        public async Task<ActionResult<RequestLine>> Create([FromBody] RequestLine requestLine)
        {
            _context.RequestLines.Add(requestLine);
            await _context.SaveChangesAsync();
            await Recalculate(requestLine.RequestId);
            
            return CreatedAtAction("GetRequestLineById", new { id = requestLine.Id }, requestLine);
        }

        // delete requestline by Id
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

            await Recalculate(requestLine.RequestId);

            return NoContent();
        }

        private bool RequestLineExists(int id)
        {
            return _context.RequestLines.Any(e => e.Id == id);
        }

        // recalculate the requestline total
        private async Task Recalculate(int requestId)
        {

            var total = await _context.RequestLines
                                        .Where(req => req.RequestId == requestId)
                                        .Include(req => req.Product)
                                        .Select(req => new { linetotal = req.Quantity * req.Product.Price })
                                        .SumAsync(t => t.linetotal);

            var request = await _context.Requests.FindAsync(requestId);

            request.Total = total;

            await _context.SaveChangesAsync();

            throw new NotImplementedException();

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BiometricsIntegrationWebAPI;
using BiometricsIntegrationWebAPI.Models;

namespace BiometricsIntegrationWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RawDatasController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public RawDatasController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/RawDatas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RawData>>> GetRawData()
        {
            return await _context.RawData.ToListAsync();
        }

        // GET: api/RawDatas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RawData>> GetRawData(Guid id)
        {
            var rawData = await _context.RawData.FindAsync(id);

            if (rawData == null)
            {
                return NotFound();
            }

            return rawData;
        }

        // PUT: api/RawDatas/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRawData(Guid id, RawData rawData)
        {
            if (id != rawData.Id)
            {
                return BadRequest();
            }

            _context.Entry(rawData).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RawDataExists(id))
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

        // POST: api/RawDatas
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<RawData>> PostRawData(RawData rawData)
        {
            _context.RawData.Add(rawData);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRawData", new { id = rawData.Id }, rawData);
        }

        // DELETE: api/RawDatas/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<RawData>> DeleteRawData(Guid id)
        {
            var rawData = await _context.RawData.FindAsync(id);
            if (rawData == null)
            {
                return NotFound();
            }

            _context.RawData.Remove(rawData);
            await _context.SaveChangesAsync();

            return rawData;
        }

        private bool RawDataExists(Guid id)
        {
            return _context.RawData.Any(e => e.Id == id);
        }
    }
}

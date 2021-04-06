using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentInstructionWithDB.Models;

namespace PaymentInstructionWithDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentInstructionsController : ControllerBase
    {
        private readonly PaymentInstructionContext _context;

        public PaymentInstructionsController(PaymentInstructionContext context)
        {
            _context = context;
        }

        // GET: api/PaymentInstructions
        [HttpGet]
        //public async Task<IActionResult> GetPaymentInstructions([FromQuery] PageFilter filter)
        public async Task<ActionResult<IEnumerable<PaymentInstruction>>> GetPaymentInstructions([FromQuery] PageFilter filter)
        {
            //var result = await _context.PaymentInstructions
            //.Skip((filter.page - 1) * filter.pageSize)
            //.Take(filter.pageSize)
            //.OrderBy(pi => pi.BeneficiaryName)
            //.Select(pi => new PaymentInstructionOutput
            //{
            //    Id = pi.Id,
            //    BeneficiaryName = pi.BeneficiaryName,
            //    BeneficiaryBiccode = pi.BeneficiaryBiccode
            //})
            //.ToListAsync();

            //return Ok(result);
            return await _context.PaymentInstructions
                .Skip((filter.page - 1) * filter.pageSize)
                .Take((filter.pageSize == 0) ? 10 : filter.pageSize).ToListAsync();
        }

        class PaymentInstructionOutput
        {
            public int Id { get; set; }
            public string BeneficiaryName { get; set; }
            public string BeneficiaryBiccode { get; set; }
        }

        // GET: api/PaymentInstructions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentInstruction>> GetPaymentInstruction(int id)
        {
            var paymentInstruction = await _context.PaymentInstructions.FindAsync(id);

            if (paymentInstruction == null)
            {
                return NotFound();
            }

            return paymentInstruction;
        }
        // GET: api/PaymentInstructions/5
        [HttpGet("{id}/History")]
        public async Task<IEnumerable<PaymentInstructionHistory>> GetPaymentInstructionHistory(int id)
        {
            var result = (from p in _context.PaymentInstructionHistories where  p.PaymentInstructionId == id select p);
            return await result.ToListAsync();
         }


        // PUT: api/PaymentInstructions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaymentInstruction(int id, PaymentInstruction paymentInstruction)
        {
            if (id != paymentInstruction.Id)
            {
                return BadRequest();
            }
            
            _context.Entry(paymentInstruction).State = EntityState.Modified;

            try
            {
                using (var db = new PaymentInstructionContext())
                {
                    var paymentInstructionOld = (from p in db.PaymentInstructions where p.Id == id select p).FirstOrDefault();
                    PaymentInstructionHistory paymentInstructionHistory = new PaymentInstructionHistory();
                    paymentInstructionHistory.PaymentInstructionId = id;
                    paymentInstructionHistory.BillTypeId = paymentInstructionOld.BillTypeId;
                    paymentInstructionHistory.CurrencyId = paymentInstructionOld.CurrencyId;
                    paymentInstructionHistory.BeneficiaryName = paymentInstructionOld.BeneficiaryName;
                    paymentInstructionHistory.BeneficiaryBiccode = paymentInstructionOld.BeneficiaryBiccode;
                    this._context.Add(paymentInstructionHistory);
                }

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentInstructionExists(id))
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

        // POST: api/PaymentInstructions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PaymentInstruction>> PostPaymentInstruction(PaymentInstruction paymentInstruction)
        {
            _context.PaymentInstructions.Add(paymentInstruction);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPaymentInstruction", new { id = paymentInstruction.Id }, paymentInstruction);
        }

        // DELETE: api/PaymentInstructions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaymentInstruction(int id)
        {
            var paymentInstruction = await _context.PaymentInstructions.FindAsync(id);
            if (paymentInstruction == null)
            {
                return NotFound();
            }

            _context.PaymentInstructions.Remove(paymentInstruction);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PaymentInstructionExists(int id)
        {
            return _context.PaymentInstructions.Any(e => e.Id == id);
        }
    }
}
public class PageFilter
{
    public int page { get; set; }
    public int pageSize { get; set; }
}

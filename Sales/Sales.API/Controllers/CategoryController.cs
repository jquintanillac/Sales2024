using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sales.API.Data;
using Sales.Shared.Entity;

namespace Sales.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly DataContext _context;

        public CategoryController(DataContext context)
        {
            _context = context;
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetAsyncId(int id)
        {
            var categoria = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (categoria is null)
            {
                return NotFound();
            }

            return Ok(categoria);

        }

        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            return Ok(await _context.Categories.ToListAsync());
        }


        [HttpPut]
        public async Task<ActionResult> Put(Category categoria)
        {
            _context.Update(categoria);
            try
            {
                await _context.SaveChangesAsync();
                return Ok(categoria);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe un registro con el mismo nombre.");
                }
                else
                {
                    return BadRequest(dbUpdateException.InnerException.Message);
                }
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }

        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var afectedRows = await _context.Categories
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();

            if (afectedRows == 0)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(Category categoria)
        {
            _context.Add(categoria);
            try
            {
                await _context.SaveChangesAsync();
                return Ok(categoria);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe una categoria con el mismo nombre.");
                }
                else
                {
                    return BadRequest(dbUpdateException.InnerException.Message);
                }
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }

        }


    }
}

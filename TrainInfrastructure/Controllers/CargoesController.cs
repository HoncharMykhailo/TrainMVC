using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrainDomain.Model;
using TrainInfrastructure;
using static System.Collections.Specialized.BitVector32;

namespace TrainInfrastructure.Controllers
{
    public class CargoesController : Controller
    {
        private readonly DbtrainContext _context;

        public CargoesController(DbtrainContext context)
        {
            _context = context;
        }

        // GET: Cargoes
        public async Task<IActionResult> Index(int? id, string? name)
        {
            //var dbtrainContext = _context.Cargos.Include(c => c.Client).Include(c => c.Station);
            // return View(await dbtrainContext.ToListAsync());

            if (id == null) return RedirectToAction("Clients", "Index");
            ViewBag.ClientId = id;
            ViewBag.ClientName = name;
            var cargoByClient = _context.Cargos.Where(c=>c.ClientId == id).Include(c=>c.Client);
            return View (await cargoByClient.ToListAsync());
        }

        // GET: Cargoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cargo = await _context.Cargos
                .Include(c => c.Client)
                .Include(c => c.Station)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cargo == null)
            {
                return NotFound();
            }

            return View(cargo);
        }

        // GET: Cargoes/Create
        public IActionResult Create(int clientId)
        {
           // ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Email");
           // ViewData["StationId"] = new SelectList(_context.Stations, "Id", "Name");

            ViewBag.ClientId=clientId;
            ViewBag.ClientName = _context.Clients.Where(c => c.Id == clientId).FirstOrDefault().Name;

            return View();
        }

        // POST: Cargoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int clientId,[Bind("Id,ClientId,Weight,Volume,Contain,StationId")] Cargo cargo)
        {
            cargo.ClientId = clientId;

            Client client = _context.Clients.FirstOrDefault(c => c.Id == clientId);
            cargo.Client = client;
            ModelState.Clear();
            TryValidateModel(cargo);

            if (ModelState.IsValid)
            {
                _context.Add(cargo);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Cargoes", new { id = clientId, name = _context.Clients.Where(c => c.Id == clientId).FirstOrDefault().Name });
            }
           return RedirectToAction("Index", "Cargoes", new { id = clientId, name= _context.Clients.Where(c => c.Id == clientId).FirstOrDefault().Name });
        }

        // GET: Cargoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cargo = await _context.Cargos.FindAsync(id);
            if (cargo == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Email", cargo.ClientId);
            ViewData["StationId"] = new SelectList(_context.Stations, "Id", "Name", cargo.StationId);
            return View(cargo);
        }

        // POST: Cargoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClientId,Weight,Volume,Contain,StationId")] Cargo cargo)
        {
            if (id != cargo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cargo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CargoExists(cargo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Email", cargo.ClientId);
            ViewData["StationId"] = new SelectList(_context.Stations, "Id", "Name", cargo.StationId);
            return View(cargo);
        }

        // GET: Cargoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cargo = await _context.Cargos
                .Include(c => c.Client)
                .Include(c => c.Station)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cargo == null)
            {
                return NotFound();
            }

            return View(cargo);
        }

        // POST: Cargoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cargo = await _context.Cargos.FindAsync(id);
            if (cargo != null)
            {
                _context.Cargos.Remove(cargo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CargoExists(int id)
        {
            return _context.Cargos.Any(e => e.Id == id);
        }
    }
}

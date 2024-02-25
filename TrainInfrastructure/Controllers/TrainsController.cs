using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrainDomain.Model;
using TrainInfrastructure;

namespace TrainInfrastructure.Controllers
{
    public class TrainsController : Controller
    {
        private readonly DbtrainContext _context;

        public TrainsController(DbtrainContext context)
        {
            _context = context;
        }

        // GET: Trains
        public async Task<IActionResult> Index(int? id, string? name)
        {
            //var dbtrainContext = _context.Trains.Include(t => t.Station);
            //return View(await dbtrainContext.ToListAsync());

            if (id == null) return RedirectToAction("Stations", "Index");
            ViewBag.StationId = id;
            ViewBag.Name = name; 
            var trainByStation=_context.Trains.Where(t=>t.StationId== id).Include(t=>t.Station);
            return View(await trainByStation.ToListAsync());

        }

        // GET: Trains/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var train = await _context.Trains
                .Include(t => t.Station)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (train == null)
            {
                return NotFound();
            }

            return View(train);
        }

        // GET: Trains/Create
        public IActionResult Create(int stationId)
        {
            // ViewData["StationId"] = new SelectList(_context.Stations, "Id", "Name");
            ViewBag.StationId = stationId;
            ViewBag.StationName = _context.Stations.Where(s => s.Id == stationId).FirstOrDefault().Name;
            return View();
        }

        // POST: Trains/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int stationId, [Bind("Id,Model,Power,StationId")] Train train)
        {
            train.StationId = stationId;
            //Station station = _context.Stations.FirstOrDefault(s=>s.Id==stationId);
            Station station = _context.Stations.Include(s=>s.City).FirstOrDefault(s => s.Id == stationId);
            train.Station = station;
            ModelState.Clear();
            TryValidateModel(train);
            if (ModelState.IsValid)
            {
                _context.Add(train);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "Trains", new { id = stationId, name = _context.Stations.Where(s => s.Id == stationId).FirstOrDefault().Name });
            }
            //ViewData["StationId"] = new SelectList(_context.Stations, "Id", "Name", train.StationId);
            //return View(train);
            return RedirectToAction("Index", "Trains", new { id = stationId, name = _context.Stations.Where(s => s.Id == stationId).FirstOrDefault().Name });
        }

        // GET: Trains/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var train = await _context.Trains.FindAsync(id);
            if (train == null)
            {
                return NotFound();
            }
            ViewData["StationId"] = new SelectList(_context.Stations, "Id", "Name", train.StationId);
            return View(train);
        }

        // POST: Trains/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Model,Power,StationId")] Train train)
        {
            if (id != train.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(train);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainExists(train.Id))
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
            ViewData["StationId"] = new SelectList(_context.Stations, "Id", "Name", train.StationId);
            return View(train);
        }

        // GET: Trains/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var train = await _context.Trains
                .Include(t => t.Station)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (train == null)
            {
                return NotFound();
            }

            return View(train);
        }

        // POST: Trains/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var train = await _context.Trains.FindAsync(id);
            if (train != null)
            {
                _context.Trains.Remove(train);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainExists(int id)
        {
            return _context.Trains.Any(e => e.Id == id);
        }
    }
}

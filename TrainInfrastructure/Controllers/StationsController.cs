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
    public class StationsController : Controller
    {
        private readonly DbtrainContext _context;

        public StationsController(DbtrainContext context)
        {
            _context = context;
        }

        // GET: Stations
        public async Task<IActionResult> Index(int? id, string? name)
        {
            //var dbtrainContext = _context.Stations.Include(s => s.City);
            //return View(await dbtrainContext.ToListAsync());

            if (id == null) return RedirectToAction("Cities", "Index");
            ViewBag.CityId = id;
            ViewBag.CityName = name;
            var stationByCity = _context.Stations.Where(s => s.CityId == id).Include(s => s.City);
            return View(await stationByCity.ToListAsync());

        }

        // GET: Stations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var station = await _context.Stations
                .Include(s => s.City)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (station == null)
            {
                return NotFound();
            }

            //return View(station);
            return RedirectToAction("Index","Trains",new {id=station.Id,name=station.Name});
        }

        // GET: Stations/Create
        public IActionResult Create(int cityId)
        {
            //ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name");

            ViewBag.CityId = cityId;
            ViewBag.CityName = _context.Cities.Where(c => c.Id == cityId).FirstOrDefault().Name;

            return View();
        }

        // POST: Stations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int cityId, [Bind("Id,CityId,Name,Phone")] Station station)
        {
            station.CityId = cityId;

            City city=_context.Cities.FirstOrDefault(c=> c.Id == cityId);
            station.City = city;
            ModelState.Clear();
            TryValidateModel(station);


            if (ModelState.IsValid)
            {
                _context.Add(station);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index","Stations",new {id=cityId, name= _context.Cities.Where(c => c.Id == cityId).FirstOrDefault().Name });
            }
            //ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name", station.CityId);
            return RedirectToAction("Index", "Stations", new { id = cityId, name = _context.Cities.Where(c => c.Id == cityId).FirstOrDefault().Name });
        }

        // GET: Stations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var station = await _context.Stations.FindAsync(id);
            if (station == null)
            {
                return NotFound();
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name", station.CityId);
            return View(station);
        }

        // POST: Stations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CityId,Name,Phone")] Station station)
        {
            if (id != station.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(station);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StationExists(station.Id))
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
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name", station.CityId);
            return View(station);
        }

        // GET: Stations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var station = await _context.Stations
                .Include(s => s.City)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (station == null)
            {
                return NotFound();
            }

            return View(station);
        }

        // POST: Stations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var station = await _context.Stations.FindAsync(id);
            if (station != null)
            {
                _context.Stations.Remove(station);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StationExists(int id)
        {
            return _context.Stations.Any(e => e.Id == id);
        }
    }
}

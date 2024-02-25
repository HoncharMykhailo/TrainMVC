using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class DriversController : Controller
    {
        private readonly DbtrainContext _context;

        public DriversController(DbtrainContext context)
        {
            _context = context;
        }

        // GET: Drivers
        public async Task<IActionResult> Index(int? id,string? model)
        {
            // var dbtrainContext = _context.Drivers.Include(d => d.Train);
            // return View(await dbtrainContext.ToListAsync());

            if (id == 0) return RedirectToAction("Trains", "Index");
            ViewBag.TrainId = id;
            ViewBag.Model = model;
            var driverByTrain = _context.Drivers.Where(d => d.TrainId == id).Include(d => d.Train);
            return View(await driverByTrain.ToListAsync());

        }

        // GET: Drivers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driver = await _context.Drivers
                .Include(d => d.Train)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (driver == null)
            {
                return NotFound();
            }

            return View(driver);
        }

        // GET: Drivers/Create
        public IActionResult Create(int trainId)
        {
            //ViewData["TrainId"] = new SelectList(_context.Trains, "Id", "Model");

            ViewBag.TrainId = trainId;
            ViewBag.TrainModel = _context.Trains.Where(t=>t.Id== trainId).FirstOrDefault().Model;

            return View();
        }

        // POST: Drivers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int trainId, int stationId, [Bind("Id,Name,Phone,Email,TrainId")] Driver driver)
        {

            driver.TrainId = trainId;
            Station station = _context.Stations.Include(s => s.City).FirstOrDefault(s => s.Id == stationId);
            Train train = _context.Trains.Include(t => t.Station).FirstOrDefault(t => t.Id == trainId);
            train.Station = station;
            driver.Train = train;
            ModelState.Clear();
            TryValidateModel(driver);
            if (ModelState.IsValid)
            {
                _context.Add(driver);
                await _context.SaveChangesAsync();
              //  return RedirectToAction(nameof(Index));
              return RedirectToAction("Index", "Drivers", new {id=trainId, model= _context.Trains.Where(t => t.Id == trainId).FirstOrDefault().Model });
            }
            // ViewData["TrainId"] = new SelectList(_context.Trains, "Id", "Model", driver.TrainId);
            // return View(driver);
            return RedirectToAction("Index", "Drivers", new { id = trainId, model = _context.Trains.Where(t => t.Id == trainId).FirstOrDefault().Model });
        }

        // GET: Drivers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driver = await _context.Drivers.FindAsync(id);
            if (driver == null)
            {
                return NotFound();
            }
            ViewData["TrainId"] = new SelectList(_context.Trains, "Id", "Model", driver.TrainId);
            return View(driver);
        }

        // POST: Drivers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Phone,Email,TrainId")] Driver driver)
        {
            if (id != driver.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(driver);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DriverExists(driver.Id))
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
            ViewData["TrainId"] = new SelectList(_context.Trains, "Id", "Model", driver.TrainId);
            return View(driver);
        }

        // GET: Drivers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driver = await _context.Drivers
                .Include(d => d.Train)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (driver == null)
            {
                return NotFound();
            }

            return View(driver);
        }

        // POST: Drivers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var driver = await _context.Drivers.FindAsync(id);
            if (driver != null)
            {
                _context.Drivers.Remove(driver);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DriverExists(int id)
        {
            return _context.Drivers.Any(e => e.Id == id);
        }
    }
}

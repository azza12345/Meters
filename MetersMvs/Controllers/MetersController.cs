using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Entities; // Make sure to import the correct namespace

using MetersMVC.ViewModels;
using Infrastructure.Data;

public class MetersController : Controller
{
    private readonly IMapper _mapper;
    private readonly MeterDbContext _context;

    public MetersController(IMapper mapper, MeterDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    // GET: Meters
    public async Task<IActionResult> Index()
    {
        var meters = await _context.Meters.ToListAsync();
        var meterViewModels = _mapper.Map<List<MeterViewModel>>(meters);
        return View(meterViewModels);
    }

    // GET: Meters/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var meter = await _context.Meters.FindAsync(id);
        if (meter == null)
        {
            return NotFound();
        }

        var meterViewModel = _mapper.Map<MeterViewModel>(meter);
        return View(meterViewModel);
    }

    // GET: Meters/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Meters/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MeterViewModel meterViewModel)
    {
        if (ModelState.IsValid)
        {
            var meter = _mapper.Map<MeterEntity>(meterViewModel); // Use MeterEntity here
            _context.Meters.Add(meter);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(meterViewModel);
    }

    // GET: Meters/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var meter = await _context.Meters.FindAsync(id);
        if (meter == null)
        {
            return NotFound();
        }

        var meterViewModel = _mapper.Map<MeterViewModel>(meter);
        return View(meterViewModel);
    }

    // POST: Meters/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, MeterViewModel meterViewModel)
    {
        if (id != meterViewModel.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var meter = _mapper.Map<MeterEntity>(meterViewModel); // Use MeterEntity here
                _context.Update(meter);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MeterExists(meterViewModel.Id))
                {
                    return NotFound();
                }
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(meterViewModel);
    }

    // GET: Meters/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var meter = await _context.Meters.FindAsync(id);
        if (meter == null)
        {
            return NotFound();
        }

        var meterViewModel = _mapper.Map<MeterViewModel>(meter);
        return View(meterViewModel);
    }

    // POST: Meters/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var meter = await _context.Meters.FindAsync(id);
        if (meter != null)
        {
            _context.Meters.Remove(meter);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private bool MeterExists(int id)
    {
        return _context.Meters.Any(e => e.Id == id);
    }
}

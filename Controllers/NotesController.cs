using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo.Data;
using Todo.Models;

namespace Todo.Controllers
{
   [Authorize]
    public class NotesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IConfiguration configuration;
        public NotesController(ApplicationDbContext context,IConfiguration Config)
        {
            configuration = Config;
            _context = context;
        }

        
        // GET: Notes
        public async Task<IActionResult> Index(string searchString,string tagString)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (_context.Notes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Todos'  is null.");
            }

            var notes = from n in _context.Notes select n;
            if (!String.IsNullOrEmpty((searchString)))
            {
                notes = notes.Where(n => n.Contents!.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(tagString))
            {
                notes = notes.Where(n => n.Tags!.Contains(tagString));
            }

            notes = notes.Where(n => n.Email == email);
            notes = notes.OrderBy(n => n.EndDate);
            Console.WriteLine("User id is: "+userId);
            Console.WriteLine("User email is "+email);
            return View(await notes.ToListAsync());
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("/Notes/Share/{uuid}/")]
        public async Task<IActionResult> Share(string? uuid)
        {
            
            if (_context.Notes != null && uuid!=null )
            {
                Console.WriteLine("UUID is: "+uuid);
                Console.WriteLine("We expect it to be "+"051530ff-f9ad-42cd-9c27-735e48cd2229");
                List<Note> notes = await _context.Notes.ToListAsync();
                var note = notes.Where(model => model.Uuid == uuid);
                Console.WriteLine("Length is: "+note.ToArray().Length);
                if (note.ToArray().Length >= 1)
                {
                    Console.WriteLine("Is this running?");
                    Note myNote = note.ToArray()[0];

                    if (myNote.Share)
                    {
                        return View(myNote);
                    }


                }
            }
            
            return View("NotFound");
        }

     
       

        // GET: Notes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (id == null || _context.Notes == null)
            {
                return NotFound();
            }

            var note = await _context.Notes .Where(n=>n.Email == email)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (note == null)
            {
                return NotFound();
            }

            ViewBag.url = configuration.GetValue<String>("myUrl");
            ViewBag.share = note.Share;
            ViewBag.uuid = note.Uuid;
            return View(note);
        }

        // GET: Notes/Create
        public IActionResult Create()
        {
            ViewBag.email = User.FindFirstValue(ClaimTypes.Email);
            ViewBag.uuid = Guid.NewGuid().ToString();
            return View();
        }

        // POST: Notes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Contents,StartDate,EndDate,Priority,Tags,Share,Uuid,Email")] Note note)
        {
            if (ModelState.IsValid)
            {
                _context.Add(note);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(note);
        }

        // GET: Notes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            
            if (id == null || _context.Notes == null)
            {
                return NotFound();
            }
            
            var note = await _context.Notes.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }
            ViewBag.email = User.FindFirstValue(ClaimTypes.Email);
            ViewBag.uuid = note.Uuid;
            
            
            ViewBag.startDate = note.StartDate.ToString().Substring(0,10);
            ViewBag.endDate = note.EndDate.ToString().Substring(0,10);
            ViewBag.url = configuration.GetValue<String>("myUrl");
            Console.WriteLine(note.StartDate);
            return View(note);
        }

        // POST: Notes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Contents,StartDate,EndDate,Priority,Tags,Share,Uuid,Email")] Note note)
        {
            if (id != note.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(note);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoteExists(note.Id))
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
            return View(note);
        }

        // GET: Notes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Notes == null)
            {
                return NotFound();
            }

            var note = await _context.Notes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        // POST: Notes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Notes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Todos'  is null.");
            }
            var note = await _context.Notes.FindAsync(id);
            if (note != null)
            {
                _context.Notes.Remove(note);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NoteExists(int id)
        {
          return (_context.Notes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

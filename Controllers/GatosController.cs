using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistema_Gatos.Models;
using System.Web;

namespace Sistema_Gatos.Controllers
{
    public class GatosController : Controller
    {
        private readonly Contexto _context;


        public GatosController(Contexto context)
        {
            _context = context;
        }

        [NonAction]
        private string ViewImage(byte[] arrayImage)

        {
            if (arrayImage == null)
            {
                return "#";
            }
            string base64String = Convert.ToBase64String(arrayImage, 0, arrayImage.Length);
            return "data:image/png;base64," + base64String;
        }


        // GET: Gatos
        public async Task<IActionResult> Index()
        {

            var lista = await _context.tbGato.ToArrayAsync();
            List<string> listaimagenes = new List<string>(); 

            foreach (var item in lista )
            {
                listaimagenes.Add( ViewImage(item.imagen)); 
            }

            ViewBag.listaImagenes = listaimagenes; 

            return View(lista);

        }

        // GET: Gatos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbGato = await _context.tbGato
                .FirstOrDefaultAsync(m => m.id == id);
            if (tbGato == null)
            {
                return NotFound();
            }

            return View(tbGato);
        }

        // GET: Gatos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Gatos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,nombre,imagen")] tbGato tbGato, IFormFile files)
        {
            if (ModelState.IsValid)
            {

                if (files != null)
                {
                    if (files.Length > 0)
                    {
                        //nombre del archivo
                        var fileName = Path.GetFileName(files.FileName);
                        //extension del archivo
                        var fileExtension = Path.GetExtension(fileName);
                        // se suman el nombre y la extension  FileName + FileExtension
                        var newFileName = String.Concat(Convert.ToString(Guid.NewGuid()), fileExtension);
                        using (var target = new MemoryStream())
                        {
                            files.CopyTo(target);
                            tbGato.imagen = target.ToArray();
                        }
                    }
                }
                _context.Add(tbGato);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tbGato);
        }

        // GET: Gatos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbGato = await _context.tbGato.FindAsync(id);
            if (tbGato == null)
            {
                return NotFound();
            }
            return View(tbGato);
        }

        // POST: Gatos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,nombre,imagen")] tbGato tbGato)
        {
            if (id != tbGato.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tbGato);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!tbGatoExists(tbGato.id))
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
            return View(tbGato);
        }

        // GET: Gatos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbGato = await _context.tbGato
                .FirstOrDefaultAsync(m => m.id == id);
            if (tbGato == null)
            {
                return NotFound();
            }

            return View(tbGato);
        }

        // POST: Gatos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tbGato = await _context.tbGato.FindAsync(id);
            _context.tbGato.Remove(tbGato);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool tbGatoExists(int id)
        {
            return _context.tbGato.Any(e => e.id == id);
        }
    }
}

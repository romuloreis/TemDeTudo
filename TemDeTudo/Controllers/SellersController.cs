using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.EntityFrameworkCore;
using TemDeTudo.Data;
using TemDeTudo.Models;

namespace TemDeTudo.Controllers
{
    public class SellersController : Controller
    {
        private readonly TemDeTudoContext _context;

        public SellersController(TemDeTudoContext context) {
            _context = context;
        }

        public IActionResult Index() {
            //List<Seller> sellers = _context.Seller.ToList();
            var sellers = _context.Seller.Include("Department").ToList();
            return View(sellers);
        }

        public IActionResult Create() { 
            return View();
        }

        [HttpPost]
        public IActionResult Create(Seller seller) {

            //Testa se foi passado um vendedor
            if (seller == null) { 
                //Retorna página não encontrada
                return NotFound();
            }

            seller.Department = _context.Department.FirstOrDefault();
            seller.DepartmentId = seller.Department.Id;

            //Adicionar o objeto vendedor ao banco
            _context.Add(seller);
            //Confirma/Persiste as alterações na base de dados
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.EntityFrameworkCore;
using TemDeTudo.Data;
using TemDeTudo.Models;
using TemDeTudo.Models.ViewModels;

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

            //Filtra os vendedores que ganham menos de 10k
            var trainees = sellers.Where(s => s.Salary <= 10000);

            //Filtra a lista e ordena em ordem CRESCENTE
            //por nome e depois por salario
            var SellersAscNameSalary = 
                sellers.OrderBy(s => s.Name)
                .ThenBy(s => s.Salary);

            return View(SellersAscNameSalary);
        }

        public IActionResult Create() {
            //Instanciar um SellerFormViewModel
            //Essa instância vai ter 2 propriedades
            //um vendedor e uma lista de departmamentos
            var viewModel = new SellerFormViewModel();
            //carregando os departamentos do banco
            viewModel.Departments = _context.Department.ToList();
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(Seller seller) {

            //Testa se foi passado um vendedor
            if (seller == null) {
                //Retorna página não encontrada
                return NotFound();
            }
            //seller.Department = _context.Department.FirstOrDefault();
            //seller.DepartmentId = seller.Department.Id;

            //Adicionar o objeto vendedor ao banco
            _context.Add(seller);
            //Confirma/Persiste as alterações na base de dados
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Details(int? id)
        {
            //Veifica se foi passado um id como parâmetro
            if (id == null)
            {
                return NotFound();
            }

            Seller seller = _context.Seller.Include("Department").FirstOrDefault(x => x.Id == id);

            //Se não localizou um vendedor com esse ID, vai para página de erro
            if (seller == null)
            {
                return NotFound();
            }

            return View(seller);
        }

        public IActionResult Delete(int? id) {
            if (id == null)
            {
                return NotFound();
            }
            Seller seller = _context.Seller
                .Include("Department")
                .FirstOrDefault(s => s.Id == id);

            if (seller == null) {
                return NotFound();
            }
            return View(seller);
        }

        [HttpPost]
        public IActionResult Delete(int id) {
            Seller seller = _context.Seller
                .FirstOrDefault(s => s.Id == id);

            if (seller == null)
            {
                return NotFound();
            }

            _context.Remove(seller);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            //Verificar se existe um vendedor com o id passado por parâmetro
            var seller = _context.Seller.First(s => s.Id == id);

            if (seller == null)
            {
                return NotFound();
            }

            //Cria uma lista de departamentos
            List<Department> departments = _context.Department.ToList();

            //Cria uma instância do viewmodel
            SellerFormViewModel viewModel = new SellerFormViewModel();
            viewModel.Seller = seller;
            viewModel.Departments = departments;

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(Seller seller) {
            //_context.Seller.Update(seller);
            _context.Update(seller);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Report() { 
            //Popular a lista de objetos vendedores,
            //trazendo as informações
            //do departamento de cada vendedor
            //List<Seller> sellers
            var sellers = _context.Seller.Include("Department").ToList();

            ViewData["TotalFolhaPagamento"] = sellers.Sum(s => s.Salary);

            ViewData["Maior"] = sellers.Max(s => s.Salary);

            ViewData["Menor"] = sellers.Min(s => s.Salary);

            ViewData["Media"] = sellers.Average(s => s.Salary);

            ViewData["Ricos"] = sellers.Count(s => s.Salary >= 30000);

            return View();
        }

    }
}

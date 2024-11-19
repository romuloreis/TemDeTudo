using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace TemDeTudo.Models
{
    public class Seller
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Nome do Vendedor")]
        [StringLength(30, ErrorMessage = "O nome deve ter no máximo 30 characteres")]
        public string Name { get; set; }
        [EmailAddress(ErrorMessage = "E-mail Inválido")]
        public string Email { get; set; }
        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime BirthDate { get; set; }
        [Range(1400, 50000, ErrorMessage = "Fora do valor limite")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public double Salary { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public List<SalesRecord> Sales { get; set; }
            = new List<SalesRecord>();

        public double TotalSales(
            DateTime initial, DateTime final) {
            return Sales.Where(
                sr => sr.Date >= initial &&
                sr.Date <= final).Sum(sr => sr.Price);
        }
    }
}

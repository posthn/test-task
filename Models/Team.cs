using System.ComponentModel.DataAnnotations;

namespace Project01.Models
{
    public class Team
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Пожалуйста введите наименование команды")]
        public string Name { get; set; } = "no name";
    }
}
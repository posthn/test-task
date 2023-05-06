using System.ComponentModel.DataAnnotations;

namespace Project01.Models
{
    public class Person
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Пожалуйста введите имя")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Пожалуйста введите фамилию")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Пожалуйста укажите пол")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Пожалуйста введите дату рождения")]
        [Range(typeof(string), "1950-01-01", "2100-01-01", ErrorMessage = "Пожалуйста введите корректную дату")]
        public string BirthDate { get; set; }

        [Required]
        public Team Team { get; set; }

        [Required(ErrorMessage = "Пожалуйста выберите страну")]
        public string Country { get; set; }
    }
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HospitalDataBase.Models
{
    public class Doctors
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter First Name")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter Last Name")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter the Position")]
        public string Position { get; set; }

        [Display(Name = "About Information")]
        public string AboutInfo { get; set; }

        public string? ImagePath { get; set; }

        public List<Patients> Patients { get; set; } = new List<Patients>();

        public Doctors()
        {
        }
    }
}

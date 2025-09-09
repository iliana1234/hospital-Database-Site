using System.ComponentModel.DataAnnotations;

namespace HospitalDataBase.Models
{
    public class Patients
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter SSN number")]
        [Display(Name = "SSN Number")]
        public int SSN { get; set; }

        [Required(ErrorMessage = "Please enter First Name")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter Last Name")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Prescribed Medicine")]
        public Medicine Meds { get; set; }

        public string Diagnosis { get; set; }

        public enum Medicine
        {
            Paracetamol,
            Ibuprofen,
            Antibiotics,
            Aspirin,
            Penicillin,
            Insulin,
            Metformin,
            None
        }

        // Add the Foreign key and the relation with the Doctors table
        [Display(Name = "Treating Doctor")]
        public int DoctorsId { get; set; }
        [Display(Name = "Treating Doctor")]
        public Doctors Doctors { get; set; }

        public Patients()
        {

        }
    }
}


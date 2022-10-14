namespace devdeer.EfCorePoolingSample.Ui.ConsoleApp.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class Person
    {
        #region properties

        [Key]
        public long Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Number { get; set; } = null!;

        [StringLength(100)]
        public string? Firstname { get; set; }

        [StringLength(100)]
        public string? Lastname { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        #endregion
    }
}
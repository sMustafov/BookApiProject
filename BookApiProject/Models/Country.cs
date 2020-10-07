namespace BookApiProject.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class Country
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage = "Country cannot be more than 50 characters")]
        public string Name { get; set; }
        public virtual ICollection<Author> Authors { get; set; }
    }
}

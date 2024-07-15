using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MohiuddinCoreMasterDetailCrud.Models
{

    public class Module
    {
        public int ModuleId { get; set; }

        [Required]
        [StringLength(100)]
        public string ModuleName { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
    }


}
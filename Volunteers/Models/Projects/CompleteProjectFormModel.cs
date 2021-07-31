using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static Volunteers.Data.DataConstants;

namespace Volunteers.Models.Projects
{
    public class CompleteProjectFormModel
    {
        public string  Id { get; set; }

        [Display(Name = "Upload a photo of the finished project")]
        public string CompletedImage { get; set; }
    }
}

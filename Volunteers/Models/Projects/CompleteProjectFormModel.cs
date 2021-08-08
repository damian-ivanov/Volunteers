using System.ComponentModel.DataAnnotations;

namespace Volunteers.Models.Projects
{
    public class CompleteProjectFormModel
    {
        public string  Id { get; set; }

        [Display(Name = "Upload a photo of the finished project")]
        public string CompletedImage { get; set; }
    }
}

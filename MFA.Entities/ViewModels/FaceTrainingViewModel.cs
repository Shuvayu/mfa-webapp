using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MFA.Entities.ViewModels
{
    public class FaceTrainingViewModel
    {
        [Required]
        public List<IFormFile> Files { get; set; }
        [Required]
        public string FaceName { get; set; }
    }
}

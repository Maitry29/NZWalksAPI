using System.ComponentModel.DataAnnotations;

namespace NZWalks.UI.Models
{
    public class AddRegionViewModel
    {
        [Required(ErrorMessage = "Code is required")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Image URL is required")]
        [Url]
        public string RegionImageUrl { get; set; }
    }
}

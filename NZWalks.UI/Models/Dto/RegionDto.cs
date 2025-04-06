using System.ComponentModel.DataAnnotations;

namespace NZWalks.UI.Models.DTO
{
    public class RegionDto
    {
        public Guid Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        public string RegionImageUrl { get; set; }
    }

}

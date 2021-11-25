using System.ComponentModel.DataAnnotations;

namespace ProITM.Shared
{
    public class Group
    {
        [Required]
        public string Name { get; set; }

        public string Id { get; set; }
    }
}

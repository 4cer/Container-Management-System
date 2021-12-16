using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProITM.Shared
{
    // Receptacle model for Docker REST Api
    public class ImageModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        // Maps to REPOSITORY in output of "docker images"
        [Required(ErrorMessage ="To pole jest wymagane")]
        public string DockerImageName { get; set; }

        // Generated server-side, on creation
        public DateTime Created { get; set; }

        // User-friendly name
        [Required(ErrorMessage = "To pole jest wymagane")]
        [StringLength(50, ErrorMessage = "Nazwa może mieć maksymalnie 50 znaków")]
        public string DisplayName { get; set; }

        [StringLength(1000, ErrorMessage = "Opis może mieć maksymalnie 1000 znaków")]
        public string Description { get; set; }

        // Maps to TAG in output of "docker images"
        public string Version { get; set; }

        public ImageModel()
        {
            this.Created = DateTime.UtcNow;
        }

        // size doesn't matter - Ghandi, 2021
        // amount of nukes matter - Also Ghandi

        // TODO 150 describe Image datum
    }
}

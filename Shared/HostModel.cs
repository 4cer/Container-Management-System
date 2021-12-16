using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProITM.Shared
{
    public class HostModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Required(ErrorMessage ="To pole jest wymagane")]
        [StringLength(50, ErrorMessage = "Nazwa może mieć od 1 do 50 znaków", MinimumLength = 1)]
        public string DisplayName { get; set; }

        public bool IsWindows { get; set; }

        #region marked for delete/unlink from DB
        // Unused, as expected
        [Obsolete("URI is the only thing needed")]
        public string IP { get; set; }

        // Unused, as expected
        [Obsolete("URI is the only thing needed")]
        public int Port { get; set; }
        #endregion

        [Required(ErrorMessage = "To pole jest wymagane")]
        [Url(ErrorMessage = "Niepoprawny Url")]
        // Only addressing field in actual use
        public string URI { get; set; }

        // TODO 149 Describe Host datum
    }
}

using System.ComponentModel.DataAnnotations;

namespace L01_2021_601.Models
{
    public class Publicaciones
    {
        [Key]
        public int publicacionId { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public int usuarioId { get; set; }
    }
}

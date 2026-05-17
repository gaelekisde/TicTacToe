using System;
using System.Collections.Generic;
using System.Text;

namespace JueguitoLenguajes.Models
{
    public class Jugador
    {
        public string Nombre {  get; set; }
        public string UrlImagen { get; set; }
        public int Victorias { get; set; }
        public string Contrasenia { get; set; }
        public int NivelBot { get; set; } = 1;
    }
}

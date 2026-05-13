using JueguitoLenguajes.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Windows;

namespace JueguitoLenguajes.Services
{
    public class SesionServices
    {
        // Patrones regex para validaciones
        private static readonly Regex NombreUsuarioRegex = new(@"^[a-zA-Z0-9_]{3,20}$", RegexOptions.Compiled);
        private static readonly Regex ContraseniaRegex = new(@"^(?=.*[a-zA-Z])(?=.*\d).{6,}$", RegexOptions.Compiled);
        private static readonly Regex MovimientoValidoRegex = new(@"^[0-2],[0-2]$", RegexOptions.Compiled);
        private static readonly Regex JsonValidoRegex = new(@"^\s*[\[\{].*[\]\}]\s*$", RegexOptions.Compiled | RegexOptions.Singleline);

        public List<Jugador> Cuentas;
        public Jugador IniciarSesion(string nombre, string contrasenia)
        {
            // Validar formato de nombre de usuario usando regex
            if (!ValidarNombreUsuario(nombre))
            {
                MessageBox.Show("Nombre de usuario inválido. Debe contener entre 3-20 caracteres alfanuméricos.");
                return null;
            }

            var temp = Cuentas.FirstOrDefault(c => c.Nombre == nombre && c.Contrasenia == contrasenia);
            if (temp == null)
            {
                MessageBox.Show("Nombre o contraseña incorrecta");
                return null;
            }
            else
            {
                return temp;
            }
        }

        public bool RegistrarCuenta(string nombre, string contraseña)
        {
            // Validar formato de nombre de usuario usando regex
            if (!ValidarNombreUsuario(nombre))
            {
                MessageBox.Show("Nombre de usuario inválido. Debe contener entre 3-20 caracteres alfanuméricos o guión bajo.");
                return false;
            }

            // Validar formato de contraseña usando regex
            if (!ValidarContrasenia(contraseña))
            {
                MessageBox.Show("Contraseña inválida. Debe tener al menos 6 caracteres, incluyendo letras y números.");
                return false;
            }

            bool existe = Cuentas.Any(c => c.Nombre == nombre);
            if (existe)
                return false;

            Jugador nuevo = new Jugador()
            {
                Nombre = nombre,
                Contrasenia = contraseña,
            };

            Cuentas.Add(nuevo);
            return true;
        }

        public void SerializarCuentas()
        {
            var cuentas = JsonSerializer.Serialize(Cuentas);
            File.WriteAllText("cuentas.txt", cuentas);
        }

        public void DeserializarCuentas()
        {
            if (File.Exists("cuentas.txt"))
            {
                string json = File.ReadAllText("cuentas.txt");

                // Validar que el contenido tenga formato JSON válido usando regex
                if (!string.IsNullOrWhiteSpace(json) && ValidarJsonFormato(json))
                {
                    try
                    {
                        Cuentas = JsonSerializer.Deserialize<List<Jugador>>(json) ?? new List<Jugador>();
                    }
                    catch
                    {
                        Cuentas = new List<Jugador>();
                    }
                }
                else
                {
                    Cuentas = new List<Jugador>();
                }
            }
            else
            {
                Cuentas = new List<Jugador>();
            }
        }

        ~SesionServices()
        {
            SerializarCuentas();
        }

        // Métodos de validación usando regex

        /// <summary>
        /// Valida que el nombre de usuario contenga solo caracteres alfanuméricos y guiones bajos (3-20 caracteres)
        /// </summary>
        public bool ValidarNombreUsuario(string nombre)
        {
            return !string.IsNullOrWhiteSpace(nombre) && NombreUsuarioRegex.IsMatch(nombre);
        }

        /// <summary>
        /// Valida que la contraseña tenga al menos 6 caracteres con letras y números
        /// </summary>
        public bool ValidarContrasenia(string contrasenia)
        {
            return !string.IsNullOrWhiteSpace(contrasenia) && ContraseniaRegex.IsMatch(contrasenia);
        }

        /// <summary>
        /// Valida que un movimiento tenga el formato correcto (ej: "0,0" hasta "2,2")
        /// </summary>
        public bool ValidarMovimiento(string movimiento)
        {
            return !string.IsNullOrWhiteSpace(movimiento) && MovimientoValidoRegex.IsMatch(movimiento);
        }

        /// <summary>
        /// Valida que un string tenga formato JSON básico
        /// </summary>
        private bool ValidarJsonFormato(string json)
        {
            return !string.IsNullOrWhiteSpace(json) && JsonValidoRegex.IsMatch(json);
        }
    }
}

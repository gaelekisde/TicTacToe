using CommunityToolkit.Mvvm.Input;
using JueguitoLenguajes.Models;
using JueguitoLenguajes.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace JueguitoLenguajes.ViewModel
{
    public enum Turno
    {
        X,
        O
    }

    public class TicTacToe : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private void Notificar(string nombre)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
        }

        public TicTacToe()
        {
            Sesion = new SesionServices();
            Sesion.DeserializarCuentas();

            InicializarTablero();

            MarcarCasillaCommand = new RelayCommand<object>(MarcarCasilla);
            ReiniciarJuegoCommand = new RelayCommand(ReiniciarJuego);
            IniciarSesionJ1Command = new RelayCommand(IniciarSesionJ1);
            IniciarSesionJ2Command = new RelayCommand(IniciarSesionJ2);
            IniciarSesionBotCommand = new RelayCommand(IniciarConBot);
            LanzarMonedaCommand = new RelayCommand(LanzarMoneda);
            MostrarRegistroCommand = new RelayCommand(irARegistro);
            RegistrarCommand = new RelayCommand(Registrar);
            VolverLoginCommand = new RelayCommand(VolverLogin);
            VolverALoginJ1Command = new RelayCommand(VolverALoginJ1);
        }

        public ICommand MarcarCasillaCommand { get; set; }
        public ICommand ReiniciarJuegoCommand { get; set; }
        public ICommand IniciarSesionJ1Command { get; set; }
        public ICommand IniciarSesionJ2Command { get; set; }
        public ICommand IniciarSesionBotCommand { get; set; }
        public ICommand LanzarMonedaCommand { get; set; }
        public ICommand MostrarRegistroCommand { get; set; }
        public ICommand RegistrarCommand { get; set; }
        public ICommand VolverLoginCommand { get; set; }
        public ICommand VolverALoginJ1Command { get; set; }

        // Control de vistas
        private bool mostrarLoginJ1 = true;
        public bool MostrarLoginJ1
        {
            get => mostrarLoginJ1;
            set
            {
                mostrarLoginJ1 = value;
                Notificar(nameof(MostrarLoginJ1));
            }
        }

        private bool mostrarLoginJ2 = false;
        public bool MostrarLoginJ2
        {
            get => mostrarLoginJ2;
            set
            {
                mostrarLoginJ2 = value;
                Notificar(nameof(MostrarLoginJ2));
            }
        }

        private bool mostrarJuego = false;
        public bool MostrarJuego
        {
            get => mostrarJuego;
            set
            {
                mostrarJuego = value;
                Notificar(nameof(MostrarJuego));
            }
        }

        private bool mostrarRegistro = false;
        public bool MostrarRegistro
        {
            get => mostrarRegistro;
            set
            {
                mostrarRegistro = value;
                Notificar(nameof(MostrarRegistro));
            }
        }

        private bool esMultijugador = false;
        public bool EsMultijugador
        {
            get => esMultijugador;
            set
            {
                esMultijugador = value;
                Notificar(nameof(EsMultijugador));
            }
        }

        private Turno turnoActual = Turno.X;
        public Turno TurnoActual
        {
            get => turnoActual;
            set
            {
                turnoActual = value;
                Notificar(nameof(TurnoActual));
                Notificar(nameof(MensajeTurno));
                Notificar(nameof(ColorTurno));
            }
        }

        private SesionServices Sesion { get; set; }

        private int victoriasJ1 = 0;
        public int VictoriasJ1
        {
            get => victoriasJ1;
            set
            {
                victoriasJ1 = value;
                Notificar(nameof(VictoriasJ1));
            }
        }

        private int victoriasJ2 = 0;
        public int VictoriasJ2
        {
            get => victoriasJ2;
            set
            {
                victoriasJ2 = value;
                Notificar(nameof(VictoriasJ2));
            }
        }

        private int empates = 0;
        public int Empates
        {
            get => empates;
            set
            {
                empates = value;
                Notificar(nameof(Empates));
            }
        }

        private string[,] tablero = new string[3, 3];
        private string ganador = "";
        public string Ganador
        {
            get => ganador;
            set
            {
                ganador = value;
                Notificar(nameof(Ganador));
            }
        }

        private bool juegoActivo = true;
        public bool JuegoActivo
        {
            get => juegoActivo;
            set
            {
                juegoActivo = value;
                Notificar(nameof(JuegoActivo));
            }
        }

        public Jugador J1 { get; set; }
        public Jugador J2 { get; set; }

        public List<Jugador> Records { get; set; }
        public Random r = new();

        private string nombrej1 = "";
        public string NombreJ1
        {
            get => nombrej1;
            set
            {
                nombrej1 = value;
                Notificar(nameof(NombreJ1));
            }
        }

        private string pwdj1 = "";
        public string PwdJ1
        {
            get => pwdj1;
            set
            {
                pwdj1 = value;
                Notificar(nameof(PwdJ1));
            }
        }

        private string nombrej2 = "";
        public string NombreJ2
        {
            get => nombrej2;
            set
            {
                nombrej2 = value;
                Notificar(nameof(NombreJ2));
            }
        }

        private string pwdj2 = "";
        public string PwdJ2
        {
            get => pwdj2;
            set
            {
                pwdj2 = value;
                Notificar(nameof(PwdJ2));
            }
        }

        private string nombreRegistro = "";
        public string NombreRegistro
        {
            get => nombreRegistro;
            set
            {
                nombreRegistro = value;
                Notificar(nameof(NombreRegistro));
            }
        }

        private string pwdRegistro = "";
        public string PwdRegistro
        {
            get => pwdRegistro;
            set
            {
                pwdRegistro = value;
                Notificar(nameof(PwdRegistro));
            }
        }

        public string MensajeTurno => $"Turno de {(TurnoActual == Turno.X ? "X" : "O")}";

        public string ColorTurno => TurnoActual == Turno.X ? "#FF8C00" : "#4ADE80";

        public void IniciarSesionJ1()
        {
            if (string.IsNullOrWhiteSpace(nombrej1) || string.IsNullOrWhiteSpace(pwdj1))
            {
                MessageBox.Show("Por favor ingresa nombre y contraseña");
                return;
            }

            J1 = Sesion.IniciarSesion(nombrej1, pwdj1);
            if (J1 != null)
            {
                VictoriasJ1 = J1.Victorias;
                MostrarLoginJ1 = false;
                MostrarLoginJ2 = true;
            }
        }

        public void irARegistro()
        {
            MostrarLoginJ1 = false;
            MostrarRegistro = true;
            NombreRegistro = "";
            PwdRegistro = "";
        }

        public void Registrar()
        {
            if (string.IsNullOrWhiteSpace(nombreRegistro) || string.IsNullOrWhiteSpace(pwdRegistro))
            {
                MessageBox.Show("Por favor ingresa nombre y contraseña");
                return;
            }

            bool registrado = Sesion.RegistrarCuenta(nombreRegistro, pwdRegistro);
            if (registrado)
            {
                MessageBox.Show("¡Cuenta creada exitosamente! Ahora puedes iniciar sesión.");
                Sesion.SerializarCuentas();
                VolverLogin();
            }
            else
            {
                MessageBox.Show("El usuario ya existe. Elige otro nombre.");
            }
        }

        public void VolverLogin()
        {
            MostrarRegistro = false;
            MostrarLoginJ1 = true;
            NombreRegistro = "";
            PwdRegistro = "";
        }

        public void VolverALoginJ1()
        {
            MostrarLoginJ2 = false;
            MostrarLoginJ1 = true;
            NombreJ2 = "";
            PwdJ2 = "";
        }

        public void IniciarSesionJ2()
        {
            if (string.IsNullOrWhiteSpace(nombrej2) || string.IsNullOrWhiteSpace(pwdj2))
            {
                MessageBox.Show("Por favor ingresa nombre y contraseña");
                return;
            }

            J2 = Sesion.IniciarSesion(nombrej2, pwdj2);
            if (J2 != null)
            {
                VictoriasJ2 = J2.Victorias;
                EsMultijugador = true;
                MostrarLoginJ2 = false;
                MostrarJuego = true;
                LanzarMoneda();
            }
        }

        public void IniciarConBot()
        {
            J2 = new Jugador() { Nombre = "Gala", Victorias = 666 };
            VictoriasJ2 = 666;
            EsMultijugador = false;
            MostrarLoginJ2 = false;
            MostrarJuego = true;
            LanzarMoneda();
        }

        public void LanzarMoneda()
        {
            int lanzamiento = r.Next(0, 2);
            TurnoActual = lanzamiento == 0 ? Turno.X : Turno.O;
        }
        private bool VerificarVictoria()
        {
            string marcaActual = TurnoActual == Turno.X ? "X" : "O";

            // Verificar filas
            for (int i = 0; i < 3; i++)
            {
                if (!string.IsNullOrEmpty(tablero[i, 0]) &&
                    tablero[i, 0] == marcaActual &&
                    tablero[i, 1] == marcaActual &&
                    tablero[i, 2] == marcaActual)
                {
                    return true;
                }
            }

            // Verificar columnas
            for (int i = 0; i < 3; i++)
            {
                if (!string.IsNullOrEmpty(tablero[0, i]) &&
                    tablero[0, i] == marcaActual &&
                    tablero[1, i] == marcaActual &&
                    tablero[2, i] == marcaActual)
                {
                    return true;
                }
            }

            // Diagonal principal
            if (!string.IsNullOrEmpty(tablero[0, 0]) &&
                tablero[0, 0] == marcaActual &&
                tablero[1, 1] == marcaActual &&
                tablero[2, 2] == marcaActual)
            {
                return true;
            }

            // Diagonal secundaria
            if (!string.IsNullOrEmpty(tablero[0, 2]) &&
                tablero[0, 2] == marcaActual &&
                tablero[1, 1] == marcaActual &&
                tablero[2, 0] == marcaActual)
            {
                return true;
            }

            return false;
        }

        private bool VerificarEmpate()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (string.IsNullOrEmpty(tablero[i, j]))
                        return false;
                }
            }
            return true;
        }
        public void MarcarCasilla(object parameter)
        {
            if (!JuegoActivo) return;

            if (parameter is not string posicion)
                return;

            // Validar formato del movimiento usando regex del servicio de sesión
            if (!Sesion.ValidarMovimiento(posicion))
            {
                MessageBox.Show("Formato de movimiento inválido. Debe ser 'fila,columna' donde ambos valores estén entre 0 y 2.");
                return;
            }

            string[] coordenadas = posicion.Split(',');
            if (coordenadas.Length != 2)
                return;

            if (!int.TryParse(coordenadas[0], out int fila) || !int.TryParse(coordenadas[1], out int columna))
                return;

            if (!string.IsNullOrEmpty(tablero[fila, columna]))
            {
                MessageBox.Show("Casilla ocupada, elige otra.");
                return;
            }

            string marca = TurnoActual == Turno.X ? "X" : "O";
            tablero[fila, columna] = marca;
            ActualizarCelda(fila, columna);

            if (VerificarVictoria())
            {
                Ganador = TurnoActual == Turno.X ? (J1?.Nombre ?? "Jugador X") : (J2?.Nombre ?? "Jugador O");
                JuegoActivo = false;

                if (TurnoActual == Turno.X)
                {
                    VictoriasJ1++;
                    if (J1 != null) J1.Victorias++;
                }
                else
                {
                    VictoriasJ2++;
                    if (J2 != null) J2.Victorias++;
                }

                Sesion.SerializarCuentas();
                MessageBox.Show($"¡{Ganador} gana!", "Fin del juego");
                return;
            }

            if (VerificarEmpate())
            {
                Ganador = "Empate";
                JuegoActivo = false;
                Empates++;
                MessageBox.Show("¡Es un empate!", "Fin del juego");
                return;
            }

            TurnoActual = TurnoActual == Turno.X ? Turno.O : Turno.X;

            if (!EsMultijugador && TurnoActual == Turno.O)
            {
                System.Threading.Tasks.Task.Delay(500).ContinueWith(_ =>
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        TurnoBot();
                    });
                });
            }
        }

        private void TurnoBot()
        {
            if (!JuegoActivo) return;

            int fil = r.Next(0, 3);
            int col = r.Next(0, 3);

            while (!string.IsNullOrEmpty(tablero[fil, col]))
            {
                fil = r.Next(0, 3);
                col = r.Next(0, 3);
            }

            tablero[fil, col] = "O";
            ActualizarCelda(fil, col);

            if (VerificarVictoria())
            {
                Ganador = "Gala";
                JuegoActivo = false;
                VictoriasJ2++;
                Sesion.SerializarCuentas();
                MessageBox.Show($"¡{Ganador} gana!", "Fin del juego");
                return;
            }

            if (VerificarEmpate())
            {
                Ganador = "Empate";
                JuegoActivo = false;
                Empates++;
                MessageBox.Show("¡Es un empate!", "Fin del juego");
                return;
            }

            TurnoActual = Turno.X;
        }

        private void InicializarTablero()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    tablero[i, j] = "";
                }
            }
            JuegoActivo = true;
            TurnoActual = Turno.X;
            Ganador = "";
            ActualizarTodasLasCeldas();
        }

        public void ReiniciarJuego()
        {
            InicializarTablero();
        }

        // Propiedades para cada celda individual
        public string Celda00 => tablero[0, 0] ?? "";
        public string Celda01 => tablero[0, 1] ?? "";
        public string Celda02 => tablero[0, 2] ?? "";
        public string Celda10 => tablero[1, 0] ?? "";
        public string Celda11 => tablero[1, 1] ?? "";
        public string Celda12 => tablero[1, 2] ?? "";
        public string Celda20 => tablero[2, 0] ?? "";
        public string Celda21 => tablero[2, 1] ?? "";
        public string Celda22 => tablero[2, 2] ?? "";

        public Brush ColorCelda00 => ObtenerColorCelda(0, 0);
        public Brush ColorCelda01 => ObtenerColorCelda(0, 1);
        public Brush ColorCelda02 => ObtenerColorCelda(0, 2);
        public Brush ColorCelda10 => ObtenerColorCelda(1, 0);
        public Brush ColorCelda11 => ObtenerColorCelda(1, 1);
        public Brush ColorCelda12 => ObtenerColorCelda(1, 2);
        public Brush ColorCelda20 => ObtenerColorCelda(2, 0);
        public Brush ColorCelda21 => ObtenerColorCelda(2, 1);
        public Brush ColorCelda22 => ObtenerColorCelda(2, 2);

        private Brush ObtenerColorCelda(int fila, int columna)
        {
            if (string.IsNullOrEmpty(tablero[fila, columna]))
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F5F5F5"));

            return tablero[fila, columna] == "X"
                ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF8C00"))
                : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4ADE80"));
        }

        private void ActualizarCelda(int fila, int columna)
        {
            string celdaNombre = $"Celda{fila}{columna}";
            Notificar(celdaNombre);
            Notificar($"Color{celdaNombre}");
        }

        private void ActualizarTodasLasCeldas()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    ActualizarCelda(i, j);
                }
            }
        }
    }
}

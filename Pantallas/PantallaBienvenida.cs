using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Tetris2D.Graficos;
using Tetris2D.UI;

namespace Tetris2D.Pantallas
{
    /// <summary>
    /// Pantalla de bienvenida estilo arcade:
    ///  - Fondo oscuro con estrellas ("cielo" de sala de máquinas).
    ///  - Título "TETRIS2D" con resplandor neón.
    ///  - Caja para escribir el nombre del jugador.
    ///  - Botón INICIAR JUEGO (se habilita al escribir el nombre).
    /// </summary>
    public class PantallaBienvenida : Pantalla
    {
        // Evento que notifica a TetrisGame para cambiar a PantallaJuego
        public new event Action<string>? JugarSolicitado;

        private readonly Boton _botonIniciar;
        private readonly CuadroTexto _cuadroNombre;
        private bool _iniciado;

        // Estrellas decorativas: posición en fracciones de pantalla (0..1),
        // tamaño y brillo fijos para que no cambien de lugar entre frames.
        private readonly Estrella[] _estrellas;

        private readonly struct Estrella
        {
            public readonly float Nx, Ny, Tam, Brillo;
            public Estrella(float nx, float ny, float tam, float brillo)
            {
                Nx = nx; Ny = ny; Tam = tam; Brillo = brillo;
            }
        }

        public PantallaBienvenida(GestorShader shaders, DibujadorCuadros cuadros, RenderizadorTexto texto)
            : base(shaders, cuadros, texto)
        {
            _botonIniciar = new Boton(cuadros, texto);
            _botonIniciar.Texto = "INICIAR JUEGO";
            _botonIniciar.AlPresionar += IniciarJuego;

            _cuadroNombre = new CuadroTexto(cuadros, texto);

            // Semilla fija: mismas estrellas en cada ejecución.
            Random rnd = new(2026);
            _estrellas = new Estrella[60];
            for (int i = 0; i < _estrellas.Length; i++)
            {
                _estrellas[i] = new Estrella(
                    (float)rnd.NextDouble(),
                    (float)rnd.NextDouble() * 0.85f,
                    0.5f + (float)rnd.NextDouble() * 2.0f,
                    0.12f + (float)rnd.NextDouble() * 0.6f);
            }
        }

        public override void Cargar()
        {
            base.Cargar();
            _cuadroNombre.Limpiar();     // Empezamos con el nombre vacío
            _iniciado = false;
        }

        public override void Actualizar(float dt, Vector2 raton)
        {
            base.Actualizar(dt, raton);
            _botonIniciar.Actualizar(raton);
            _cuadroNombre.Actualizar(dt);

            // El botón solo funciona cuando hay nombre escrito.
            _botonIniciar.Habilitado = !_iniciado && _cuadroNombre.Nombre.Length > 0;
        }

        public override void AlClick(Vector2 posicion, MouseButton boton)
        {
            base.AlClick(posicion, boton);
            if (boton == MouseButton.Left)
                _botonIniciar.AlClick(posicion);
        }

        public override void AlTecla(Keys tecla)
        {
            base.AlTecla(tecla);
            if (!_iniciado)
            {
                _cuadroNombre.AlTecla(tecla);
                if (tecla == Keys.Enter && _cuadroNombre.Nombre.Length > 0)
                    IniciarJuego();
            }
        }

        public override void AlTexto(string caracter)
        {
            base.AlTexto(caracter);
            if (!_iniciado)
                _cuadroNombre.AlTexto(caracter);
        }

        /// <summary>El jugador confirmó su nombre: dispara el evento de inicio.</summary>
        private void IniciarJuego()
        {
            if (_iniciado)
                return;

            _iniciado = true;

            // Transición hacia el tablero enviando el nombre al evento.
            string nombreJugador = string.IsNullOrWhiteSpace(_cuadroNombre.Nombre) ? "JUGADOR" : _cuadroNombre.Nombre;
            JugarSolicitado?.Invoke(nombreJugador);
        }

        public override void Renderizar(float ancho, float alto)
        {
            // --- Fondo + estrellas decorativas -------------------------------
            Cuadros.DibujarRectangulo(TemaArcade.Fondo, 0, 0, ancho, alto);
            foreach (Estrella e in _estrellas)
            {
                float tam = e.Tam * alto * 0.004f;
                Vector4 blanco = new(TemaArcade.Blanco.X, TemaArcade.Blanco.Y,
                    TemaArcade.Blanco.Z, e.Brillo);
                Cuadros.DibujarRectangulo(blanco, e.Nx * ancho, e.Ny * alto,
                    e.Nx * ancho + tam, e.Ny * alto + tam);
            }

            float cx = ancho * 0.5f;
            string titulo = "TETRIS2D";

            // --- Título con resplandor neón ----------------------------------
            float tituloAlto = alto * 0.10f;
            float tituloY = alto * 0.16f;
            float resplandor = alto * 0.008f;

            Vector4 cianSuave = new(TemaArcade.Cian.X, TemaArcade.Cian.Y,
                TemaArcade.Cian.Z, 0.16f);
            Vector2[] direcciones =
            {
                new(-resplandor, 0), new(resplandor, 0),
                new(0, -resplandor), new(0, resplandor),
                new(-resplandor, -resplandor), new(resplandor, resplandor)
            };

            foreach (Vector2 d in direcciones)
            {
                float anchoTitulo = Texto.MedirTexto(titulo, tituloAlto);
                float x = cx - anchoTitulo * 0.5f + d.X;
                Texto.DibujarTexto(titulo, x, tituloY + d.Y, tituloAlto, cianSuave);
            }

            float anchoTotal = Texto.MedirTexto(titulo, tituloAlto);
            Texto.DibujarTexto(titulo, cx - anchoTotal * 0.5f, tituloY, tituloAlto, TemaArcade.Cian);

            // --- Subtítulo ---------------------------------------------------
            string subtitulo = "— GRAFICACIÓN POR COMPUTADORA —";
            float subAlto = alto * 0.028f;
            float subAncho = Texto.MedirTexto(subtitulo, subAlto);
            Texto.DibujarTexto(subtitulo, cx - subAncho * 0.5f, alto * 0.30f, subAlto, TemaArcade.TextoSuave);

            // --- Etiqueta + caja del nombre ----------------------------------
            string etiqueta = "INGRESA TU NOMBRE:";
            float etqAlto = alto * 0.025f;
            float etqAncho = Texto.MedirTexto(etiqueta, etqAlto);
            Texto.DibujarTexto(etiqueta, cx - etqAncho * 0.5f, alto * 0.42f, etqAlto, TemaArcade.Magenta);

            float cajaAncho = ancho * 0.44f;
            float cajaAlto = alto * 0.075f;
            float cajaY = alto * 0.48f;
            _cuadroNombre.Renderizar(cx - cajaAncho * 0.5f, cajaY, cajaAncho, cajaAlto);

            // --- Botón INICIAR JUEGO -----------------------------------------
            float botonAncho = ancho * 0.36f;
            float botonAlto = alto * 0.085f;
            _botonIniciar.X0 = cx - botonAncho * 0.5f;
            _botonIniciar.Y0 = cajaY + cajaAlto + alto * 0.05f;
            _botonIniciar.X1 = _botonIniciar.X0 + botonAncho;
            _botonIniciar.Y1 = _botonIniciar.Y0 + botonAlto;
            _botonIniciar.Renderizar(alto * 0.042f);

            // --- Pie de pantalla ---------------------------------------------
            string pie = "TETRIS2D v1.0 — Graficación por Computadora";
            float pieAlto = alto * 0.018f;
            float pieAncho = Texto.MedirTexto(pie, pieAlto);
            Texto.DibujarTexto(pie, cx - pieAncho * 0.5f, alto - pieAlto * 2.2f, pieAlto, TemaArcade.TextoSuave);
        }
    }
}
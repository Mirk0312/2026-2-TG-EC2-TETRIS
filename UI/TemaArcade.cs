using OpenTK.Mathematics;

namespace Tetris2D.UI
{
    public static class TemaArcade
    {
        // Colores de UI generales
        public static Vector4 Fondo { get; } = new Vector4(0.05f, 0.05f, 0.08f, 1.0f);
        public static Vector4 Tarjetas { get; } = new Vector4(0.12f, 0.12f, 0.18f, 1.0f);
        public static Vector4 Superficie { get; } = new Vector4(0.08f, 0.08f, 0.12f, 1.0f);
        public static Vector4 PanelOscuro { get; } = new Vector4(0.06f, 0.06f, 0.09f, 1.0f);

        // Colores de texto y botones
        public static Vector4 Blanco { get; } = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
        public static Vector4 Gris { get; } = new Vector4(0.5f, 0.5f, 0.5f, 1.0f);
        public static Vector4 TextoSuave { get; } = new Vector4(0.8f, 0.8f, 0.85f, 1.0f);

        // Colores de las piezas de Tetris
        public static Vector4 Cian { get; } = new Vector4(0.0f, 1.0f, 1.0f, 1.0f);
        public static Vector4 Amarillo { get; } = new Vector4(1.0f, 1.0f, 0.0f, 1.0f);
        public static Vector4 Magenta { get; } = new Vector4(0.6f, 0.0f, 0.8f, 1.0f);
        public static Vector4 Verde { get; } = new Vector4(0.0f, 1.0f, 0.0f, 1.0f);
        public static Vector4 Rojo { get; } = new Vector4(1.0f, 0.0f, 0.0f, 1.0f);
        public static Vector4 Azul { get; } = new Vector4(0.0f, 0.0f, 1.0f, 1.0f);
        public static Vector4 Naranja { get; } = new Vector4(1.0f, 0.5f, 0.0f, 1.0f);
    }
}
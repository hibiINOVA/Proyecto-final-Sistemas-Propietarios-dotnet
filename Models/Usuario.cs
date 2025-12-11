using System;

namespace proyectoSistemaPropietarios
{
    public class Usuario
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Contrasena { get; set; }
        public string Foto { get; set; }
    }
}

namespace proyectoSistemaPropietarios;

public class Filamento
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Tipo { get; set; }
    public string Marca { get; set; }
    public string Color { get; set; }
    public decimal Diametro { get; set; }
    public decimal PrecioPorKg { get; set; }
    public string Foto { get; set; }
    public Guid UsuarioId { get; set; }
}
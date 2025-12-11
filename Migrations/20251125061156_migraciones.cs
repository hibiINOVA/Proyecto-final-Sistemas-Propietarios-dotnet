using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace proyectoSistemaPropietarios.Migrations
{
    /// <inheritdoc />
    public partial class migraciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cotizaciones",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImpresoraId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FilamentoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TiempoHoras = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FilamentoGr = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CostoFilamento = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CostoLuz = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MargenError15 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CostoDesgarteImpresora = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IVA = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ArchivoGcode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cotizaciones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Filamentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Marca = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Diametro = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecioPorKg = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Foto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filamentos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Impresoras",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Modelo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Marca = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TipoExtrusion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiametroBoquilla = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VelocidadDeImpresion = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CostoDeLuzPorHora = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Foto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Impresoras", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contrasena = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Foto = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cotizaciones");

            migrationBuilder.DropTable(
                name: "Filamentos");

            migrationBuilder.DropTable(
                name: "Impresoras");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}

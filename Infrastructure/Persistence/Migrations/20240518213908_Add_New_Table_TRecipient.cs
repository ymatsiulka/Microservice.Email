using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Microservice.Email.Persistence
{
    /// <inheritdoc />
    public partial class Add_New_Table_TRecipient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_email",
                table: "Email");

            migrationBuilder.DropColumn(
                name: "recipients",
                table: "Email");

            migrationBuilder.RenameTable(
                name: "Email",
                newName: "TEmail");

            migrationBuilder.AddPrimaryKey(
                name: "pk_t_email",
                table: "TEmail",
                column: "id");

            migrationBuilder.CreateTable(
                name: "TRecipient",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    email = table.Column<string>(type: "text", nullable: false),
                    email_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_t_recipient", x => x.id);
                    table.ForeignKey(
                        name: "fk_t_recipient_t_email_email_id",
                        column: x => x.email_id,
                        principalTable: "TEmail",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_t_recipient_email_id",
                table: "TRecipient",
                column: "email_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TRecipient");

            migrationBuilder.DropPrimaryKey(
                name: "pk_t_email",
                table: "TEmail");

            migrationBuilder.RenameTable(
                name: "TEmail",
                newName: "Email");

            migrationBuilder.AddColumn<string>(
                name: "recipients",
                table: "Email",
                type: "character varying(8000)",
                maxLength: 8000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "pk_email",
                table: "Email",
                column: "id");
        }
    }
}

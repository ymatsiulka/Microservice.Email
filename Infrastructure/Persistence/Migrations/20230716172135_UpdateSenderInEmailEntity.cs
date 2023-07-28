using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microservice.Email.Persistence
{
    /// <inheritdoc />
    public partial class UpdateSenderInEmailEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "sender",
                table: "Email",
                newName: "sender_name");

            migrationBuilder.AlterColumn<string>(
                name: "body",
                table: "Email",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "sender_email",
                table: "Email",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "sender_email",
                table: "Email");

            migrationBuilder.RenameColumn(
                name: "sender_name",
                table: "Email",
                newName: "sender");

            migrationBuilder.AlterColumn<string>(
                name: "body",
                table: "Email",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}

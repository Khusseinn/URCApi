using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace URCApi.Migrations
{
    /// <inheritdoc />
    public partial class EmptyMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageIs",
                table: "News",
                newName: "ImageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageId",
                table: "News",
                newName: "ImageIs");
        }
    }
}

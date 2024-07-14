using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MohiuddinCoreMasterDetailCrud.Migrations
{
    /// <inheritdoc />
    public partial class courseinstructorupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseInstructorId",
                table: "CourseInstructor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourseInstructorId",
                table: "CourseInstructor",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

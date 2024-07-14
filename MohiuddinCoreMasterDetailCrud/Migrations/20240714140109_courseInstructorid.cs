using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MohiuddinCoreMasterDetailCrud.Migrations
{
    /// <inheritdoc />
    public partial class courseInstructorid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "CourseInstructor",
                newName: "CourseInstructorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CourseInstructorId",
                table: "CourseInstructor",
                newName: "Id");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MohiuddinCoreMasterDetailCrud.Migrations
{
    /// <inheritdoc />
    public partial class updatemodules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Modules__Student__29572725",
                table: "Modules");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "Modules",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Modules_StudentId",
                table: "Modules",
                newName: "IX_Modules_CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_Courses_CourseId",
                table: "Modules",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modules_Courses_CourseId",
                table: "Modules");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "Modules",
                newName: "StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Modules_CourseId",
                table: "Modules",
                newName: "IX_Modules_StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK__Modules__Student__29572725",
                table: "Modules",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "StudentId");
        }
    }
}

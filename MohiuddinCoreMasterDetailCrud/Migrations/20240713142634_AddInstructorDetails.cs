using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MohiuddinCoreMasterDetailCrud.Migrations
{
    /// <inheritdoc />
    public partial class AddInstructorDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InstructorPicture",
                table: "Instructors");

            migrationBuilder.AddColumn<string>(
                name: "Mobile",
                table: "Instructors",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "InstructorDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InstructorID = table.Column<int>(type: "int", nullable: false),
                    PresentAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PermanentAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dob = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Salary = table.Column<decimal>(type: "money", nullable: false),
                    InstructorPicture = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstructorDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstructorDetails_Instructors_InstructorID",
                        column: x => x.InstructorID,
                        principalTable: "Instructors",
                        principalColumn: "InstructorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InstructorDetails_InstructorID",
                table: "InstructorDetails",
                column: "InstructorID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InstructorDetails");

            migrationBuilder.DropColumn(
                name: "Mobile",
                table: "Instructors");

            migrationBuilder.AddColumn<string>(
                name: "InstructorPicture",
                table: "Instructors",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}

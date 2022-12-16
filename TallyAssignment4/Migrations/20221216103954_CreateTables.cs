using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TallyAssignment4.Migrations
{
    /// <inheritdoc />
    public partial class CreateTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    StudId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Class = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.StudId);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    SubId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxMarks = table.Column<int>(type: "int", nullable: false),
                    MarksObtained = table.Column<int>(type: "int", nullable: false),
                    StudentStudId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.SubId);
                    table.ForeignKey(
                        name: "FK_Subjects_Students_StudentStudId",
                        column: x => x.StudentStudId,
                        principalTable: "Students",
                        principalColumn: "StudId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_StudentStudId",
                table: "Subjects",
                column: "StudentStudId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "Students");
        }
    }
}

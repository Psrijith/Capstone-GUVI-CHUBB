using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace demobkend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProgressAndLearningPathRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseIds",
                table: "LearningPaths");

            migrationBuilder.CreateTable(
                name: "LearningPathCourses",
                columns: table => new
                {
                    CoursesCourseId = table.Column<int>(type: "int", nullable: false),
                    LearningPathsPathId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningPathCourses", x => new { x.CoursesCourseId, x.LearningPathsPathId });
                    table.ForeignKey(
                        name: "FK_LearningPathCourses_Courses_CoursesCourseId",
                        column: x => x.CoursesCourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LearningPathCourses_LearningPaths_LearningPathsPathId",
                        column: x => x.LearningPathsPathId,
                        principalTable: "LearningPaths",
                        principalColumn: "PathId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LearningPathCourses_LearningPathsPathId",
                table: "LearningPathCourses",
                column: "LearningPathsPathId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LearningPathCourses");

            migrationBuilder.AddColumn<string>(
                name: "CourseIds",
                table: "LearningPaths",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

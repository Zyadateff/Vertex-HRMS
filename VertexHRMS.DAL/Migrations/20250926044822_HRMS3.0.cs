using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VertexHRMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class HRMS30 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PayrollRuns_AspNetUsers_RunByUserId",
                table: "PayrollRuns");

            migrationBuilder.DropIndex(
                name: "IX_PayrollRuns_RunByUserId",
                table: "PayrollRuns");

            migrationBuilder.DropColumn(
                name: "RunByUserId",
                table: "PayrollRuns");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RunByUserId",
                table: "PayrollRuns",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollRuns_RunByUserId",
                table: "PayrollRuns",
                column: "RunByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PayrollRuns_AspNetUsers_RunByUserId",
                table: "PayrollRuns",
                column: "RunByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

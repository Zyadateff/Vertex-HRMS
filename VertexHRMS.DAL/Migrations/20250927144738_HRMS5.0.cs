using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VertexHRMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class HRMS50 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LeaveRequestEmails",
                columns: table => new
                {
                    LeaveRequestEmailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    FromEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceivedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HasAttachment = table.Column<bool>(type: "bit", nullable: false),
                    LeaveRequestId = table.Column<int>(type: "int", nullable: false),
                    ReplySubject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReplyBody = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveRequestEmails", x => x.LeaveRequestEmailId);
                    table.ForeignKey(
                        name: "FK_LeaveRequestEmails_LeaveRequests_LeaveRequestId",
                        column: x => x.LeaveRequestId,
                        principalTable: "LeaveRequests",
                        principalColumn: "LeaveRequestId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequestEmails_LeaveRequestId",
                table: "LeaveRequestEmails",
                column: "LeaveRequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeaveRequestEmails");
        }
    }
}

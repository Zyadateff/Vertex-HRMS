using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VertexHRMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class HRMStest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applicant_AspNetUsers_IdentityUserId",
                table: "Applicant");

            migrationBuilder.DropForeignKey(
                name: "FK_Applicant_JobOpenings_JobOpeningId",
                table: "Applicant");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_AspNetUsers_IdentityUserId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_ExitClearance_AspNetUsers_FinanceClearedByUserId",
                table: "ExitClearance");

            migrationBuilder.DropForeignKey(
                name: "FK_ExitClearance_AspNetUsers_HRClearedByUserId",
                table: "ExitClearance");

            migrationBuilder.DropForeignKey(
                name: "FK_ExitClearance_AspNetUsers_ITClearedByUserId",
                table: "ExitClearance");

            migrationBuilder.DropForeignKey(
                name: "FK_ExitClearance_Resignations_ResignationId",
                table: "ExitClearance");

            migrationBuilder.DropForeignKey(
                name: "FK_Interview_Applicant_ApplicantId",
                table: "Interview");

            migrationBuilder.DropForeignKey(
                name: "FK_Interview_AspNetUsers_InterviewerUserId",
                table: "Interview");

            migrationBuilder.DropForeignKey(
                name: "FK_Interview_Employees_InterviewerId",
                table: "Interview");

            migrationBuilder.DropForeignKey(
                name: "FK_Interview_JobOpenings_JobOpeningId",
                table: "Interview");

            migrationBuilder.DropForeignKey(
                name: "FK_LeaveApproval_AspNetUsers_ApproverUserId",
                table: "LeaveApproval");

            migrationBuilder.DropForeignKey(
                name: "FK_LeaveApproval_Employees_ApproverEmployeeId",
                table: "LeaveApproval");

            migrationBuilder.DropForeignKey(
                name: "FK_LeaveApproval_LeaveRequests_LeaveRequestId",
                table: "LeaveApproval");

            migrationBuilder.DropForeignKey(
                name: "FK_LeaveEntitlements_LeaveType_LeaveTypeId",
                table: "LeaveEntitlements");

            migrationBuilder.DropForeignKey(
                name: "FK_LeaveLedgers_LeaveType_LeaveTypeId",
                table: "LeaveLedgers");

            migrationBuilder.DropForeignKey(
                name: "FK_LeavePolicy_LeaveType_LeaveTypeId",
                table: "LeavePolicy");

            migrationBuilder.DropForeignKey(
                name: "FK_LeaveRequestDay_LeaveRequests_LeaveRequestId",
                table: "LeaveRequestDay");

            migrationBuilder.DropForeignKey(
                name: "FK_LeaveRequests_LeaveType_LeaveTypeID",
                table: "LeaveRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Offer_Applicant_ApplicantId",
                table: "Offer");

            migrationBuilder.DropForeignKey(
                name: "FK_Offer_AspNetUsers_IssuedByUserId",
                table: "Offer");

            migrationBuilder.DropForeignKey(
                name: "FK_Offer_JobOpenings_JobOpeningId",
                table: "Offer");

            migrationBuilder.DropForeignKey(
                name: "FK_Onboarding_Applicant_ApplicantId",
                table: "Onboarding");

            migrationBuilder.DropForeignKey(
                name: "FK_Onboarding_AspNetUsers_ResponsibleUserId",
                table: "Onboarding");

            migrationBuilder.DropForeignKey(
                name: "FK_Onboarding_Employees_EmployeeId",
                table: "Onboarding");

            migrationBuilder.DropForeignKey(
                name: "FK_PayrollDeduction_Deduction_DeductionId",
                table: "PayrollDeduction");

            migrationBuilder.DropForeignKey(
                name: "FK_PayrollDeduction_Payrolls_PayrollId",
                table: "PayrollDeduction");

            migrationBuilder.DropForeignKey(
                name: "FK_PayrollRun_AspNetUsers_RunByUserId",
                table: "PayrollRun");

            migrationBuilder.DropForeignKey(
                name: "FK_Payrolls_PayrollRun_PayrollRunId",
                table: "Payrolls");

            migrationBuilder.DropIndex(
                name: "IX_Employees_Email",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_EmployeeCode",
                table: "Employees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PayrollRun",
                table: "PayrollRun");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PayrollDeduction",
                table: "PayrollDeduction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Onboarding",
                table: "Onboarding");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Offer",
                table: "Offer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LeaveType",
                table: "LeaveType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LeaveRequestDay",
                table: "LeaveRequestDay");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LeavePolicy",
                table: "LeavePolicy");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LeaveApproval",
                table: "LeaveApproval");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Interview",
                table: "Interview");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExitClearance",
                table: "ExitClearance");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Deduction",
                table: "Deduction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Applicant",
                table: "Applicant");

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Positions",
                keyColumn: "PositionId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Positions",
                keyColumn: "PositionId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Positions",
                keyColumn: "PositionId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Positions",
                keyColumn: "PositionId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Positions",
                keyColumn: "PositionId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Positions",
                keyColumn: "PositionId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Positions",
                keyColumn: "PositionId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Positions",
                keyColumn: "PositionId",
                keyValue: 8);

            migrationBuilder.RenameTable(
                name: "PayrollRun",
                newName: "PayrollRuns");

            migrationBuilder.RenameTable(
                name: "PayrollDeduction",
                newName: "PayrollDeductions");

            migrationBuilder.RenameTable(
                name: "Onboarding",
                newName: "Onboardings");

            migrationBuilder.RenameTable(
                name: "Offer",
                newName: "Offers");

            migrationBuilder.RenameTable(
                name: "LeaveType",
                newName: "LeaveTypes");

            migrationBuilder.RenameTable(
                name: "LeaveRequestDay",
                newName: "LeaveRequestDays");

            migrationBuilder.RenameTable(
                name: "LeavePolicy",
                newName: "LeavePolicies");

            migrationBuilder.RenameTable(
                name: "LeaveApproval",
                newName: "LeaveApprovals");

            migrationBuilder.RenameTable(
                name: "Interview",
                newName: "Interviews");

            migrationBuilder.RenameTable(
                name: "ExitClearance",
                newName: "ExitClearances");

            migrationBuilder.RenameTable(
                name: "Deduction",
                newName: "Deductions");

            migrationBuilder.RenameTable(
                name: "Applicant",
                newName: "Applicants");

            migrationBuilder.RenameIndex(
                name: "IX_PayrollRun_RunByUserId",
                table: "PayrollRuns",
                newName: "IX_PayrollRuns_RunByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_PayrollDeduction_PayrollId",
                table: "PayrollDeductions",
                newName: "IX_PayrollDeductions_PayrollId");

            migrationBuilder.RenameIndex(
                name: "IX_PayrollDeduction_DeductionId",
                table: "PayrollDeductions",
                newName: "IX_PayrollDeductions_DeductionId");

            migrationBuilder.RenameIndex(
                name: "IX_Onboarding_ResponsibleUserId",
                table: "Onboardings",
                newName: "IX_Onboardings_ResponsibleUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Onboarding_EmployeeId",
                table: "Onboardings",
                newName: "IX_Onboardings_EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Onboarding_ApplicantId",
                table: "Onboardings",
                newName: "IX_Onboardings_ApplicantId");

            migrationBuilder.RenameIndex(
                name: "IX_Offer_JobOpeningId",
                table: "Offers",
                newName: "IX_Offers_JobOpeningId");

            migrationBuilder.RenameIndex(
                name: "IX_Offer_IssuedByUserId",
                table: "Offers",
                newName: "IX_Offers_IssuedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Offer_ApplicantId",
                table: "Offers",
                newName: "IX_Offers_ApplicantId");

            migrationBuilder.RenameIndex(
                name: "IX_LeaveRequestDay_LeaveRequestId",
                table: "LeaveRequestDays",
                newName: "IX_LeaveRequestDays_LeaveRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_LeavePolicy_LeaveTypeId",
                table: "LeavePolicies",
                newName: "IX_LeavePolicies_LeaveTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_LeaveApproval_LeaveRequestId",
                table: "LeaveApprovals",
                newName: "IX_LeaveApprovals_LeaveRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_LeaveApproval_ApproverUserId",
                table: "LeaveApprovals",
                newName: "IX_LeaveApprovals_ApproverUserId");

            migrationBuilder.RenameIndex(
                name: "IX_LeaveApproval_ApproverEmployeeId",
                table: "LeaveApprovals",
                newName: "IX_LeaveApprovals_ApproverEmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Interview_JobOpeningId",
                table: "Interviews",
                newName: "IX_Interviews_JobOpeningId");

            migrationBuilder.RenameIndex(
                name: "IX_Interview_InterviewerUserId",
                table: "Interviews",
                newName: "IX_Interviews_InterviewerUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Interview_InterviewerId",
                table: "Interviews",
                newName: "IX_Interviews_InterviewerId");

            migrationBuilder.RenameIndex(
                name: "IX_Interview_ApplicantId",
                table: "Interviews",
                newName: "IX_Interviews_ApplicantId");

            migrationBuilder.RenameIndex(
                name: "IX_ExitClearance_ResignationId",
                table: "ExitClearances",
                newName: "IX_ExitClearances_ResignationId");

            migrationBuilder.RenameIndex(
                name: "IX_ExitClearance_ITClearedByUserId",
                table: "ExitClearances",
                newName: "IX_ExitClearances_ITClearedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_ExitClearance_HRClearedByUserId",
                table: "ExitClearances",
                newName: "IX_ExitClearances_HRClearedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_ExitClearance_FinanceClearedByUserId",
                table: "ExitClearances",
                newName: "IX_ExitClearances_FinanceClearedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Applicant_JobOpeningId",
                table: "Applicants",
                newName: "IX_Applicants_JobOpeningId");

            migrationBuilder.RenameIndex(
                name: "IX_Applicant_IdentityUserId",
                table: "Applicants",
                newName: "IX_Applicants_IdentityUserId");

            migrationBuilder.AlterColumn<string>(
                name: "PositionName",
                table: "Positions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ImagePath",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "EmploymentType",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeCode",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "DepartmentName",
                table: "Departments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PayrollRuns",
                table: "PayrollRuns",
                column: "PayrollRunId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PayrollDeductions",
                table: "PayrollDeductions",
                column: "PayrollDeductionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Onboardings",
                table: "Onboardings",
                column: "OnboardingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Offers",
                table: "Offers",
                column: "OfferId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LeaveTypes",
                table: "LeaveTypes",
                column: "LeaveTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LeaveRequestDays",
                table: "LeaveRequestDays",
                column: "LeaveRequestDayId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LeavePolicies",
                table: "LeavePolicies",
                column: "LeavePolicyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LeaveApprovals",
                table: "LeaveApprovals",
                column: "LeaveApprovalId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Interviews",
                table: "Interviews",
                column: "InterviewId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExitClearances",
                table: "ExitClearances",
                column: "ExitClearanceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Deductions",
                table: "Deductions",
                column: "DeductionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Applicants",
                table: "Applicants",
                column: "ApplicantId");

            migrationBuilder.CreateTable(
                name: "HolidayCalendars",
                columns: table => new
                {
                    HolidayCalendarId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HolidayCalendars", x => x.HolidayCalendarId);
                });

            migrationBuilder.CreateTable(
                name: "WorkSchedules",
                columns: table => new
                {
                    WorkScheduleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkSchedules", x => x.WorkScheduleId);
                });

            migrationBuilder.CreateTable(
                name: "Holidays",
                columns: table => new
                {
                    HolidayId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HolidayCalendarId = table.Column<int>(type: "int", nullable: false),
                    HolidayDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holidays", x => x.HolidayId);
                    table.ForeignKey(
                        name: "FK_Holidays_HolidayCalendars_HolidayCalendarId",
                        column: x => x.HolidayCalendarId,
                        principalTable: "HolidayCalendars",
                        principalColumn: "HolidayCalendarId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Holidays_HolidayCalendarId",
                table: "Holidays",
                column: "HolidayCalendarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applicants_AspNetUsers_IdentityUserId",
                table: "Applicants",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Applicants_JobOpenings_JobOpeningId",
                table: "Applicants",
                column: "JobOpeningId",
                principalTable: "JobOpenings",
                principalColumn: "JobOpeningId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_AspNetUsers_IdentityUserId",
                table: "Employees",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExitClearances_AspNetUsers_FinanceClearedByUserId",
                table: "ExitClearances",
                column: "FinanceClearedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExitClearances_AspNetUsers_HRClearedByUserId",
                table: "ExitClearances",
                column: "HRClearedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExitClearances_AspNetUsers_ITClearedByUserId",
                table: "ExitClearances",
                column: "ITClearedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExitClearances_Resignations_ResignationId",
                table: "ExitClearances",
                column: "ResignationId",
                principalTable: "Resignations",
                principalColumn: "ResignationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Interviews_Applicants_ApplicantId",
                table: "Interviews",
                column: "ApplicantId",
                principalTable: "Applicants",
                principalColumn: "ApplicantId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Interviews_AspNetUsers_InterviewerUserId",
                table: "Interviews",
                column: "InterviewerUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Interviews_Employees_InterviewerId",
                table: "Interviews",
                column: "InterviewerId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Interviews_JobOpenings_JobOpeningId",
                table: "Interviews",
                column: "JobOpeningId",
                principalTable: "JobOpenings",
                principalColumn: "JobOpeningId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveApprovals_AspNetUsers_ApproverUserId",
                table: "LeaveApprovals",
                column: "ApproverUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveApprovals_Employees_ApproverEmployeeId",
                table: "LeaveApprovals",
                column: "ApproverEmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveApprovals_LeaveRequests_LeaveRequestId",
                table: "LeaveApprovals",
                column: "LeaveRequestId",
                principalTable: "LeaveRequests",
                principalColumn: "LeaveRequestId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveEntitlements_LeaveTypes_LeaveTypeId",
                table: "LeaveEntitlements",
                column: "LeaveTypeId",
                principalTable: "LeaveTypes",
                principalColumn: "LeaveTypeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveLedgers_LeaveTypes_LeaveTypeId",
                table: "LeaveLedgers",
                column: "LeaveTypeId",
                principalTable: "LeaveTypes",
                principalColumn: "LeaveTypeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LeavePolicies_LeaveTypes_LeaveTypeId",
                table: "LeavePolicies",
                column: "LeaveTypeId",
                principalTable: "LeaveTypes",
                principalColumn: "LeaveTypeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveRequestDays_LeaveRequests_LeaveRequestId",
                table: "LeaveRequestDays",
                column: "LeaveRequestId",
                principalTable: "LeaveRequests",
                principalColumn: "LeaveRequestId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveRequests_LeaveTypes_LeaveTypeID",
                table: "LeaveRequests",
                column: "LeaveTypeID",
                principalTable: "LeaveTypes",
                principalColumn: "LeaveTypeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Applicants_ApplicantId",
                table: "Offers",
                column: "ApplicantId",
                principalTable: "Applicants",
                principalColumn: "ApplicantId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_AspNetUsers_IssuedByUserId",
                table: "Offers",
                column: "IssuedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_JobOpenings_JobOpeningId",
                table: "Offers",
                column: "JobOpeningId",
                principalTable: "JobOpenings",
                principalColumn: "JobOpeningId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Onboardings_Applicants_ApplicantId",
                table: "Onboardings",
                column: "ApplicantId",
                principalTable: "Applicants",
                principalColumn: "ApplicantId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Onboardings_AspNetUsers_ResponsibleUserId",
                table: "Onboardings",
                column: "ResponsibleUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Onboardings_Employees_EmployeeId",
                table: "Onboardings",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PayrollDeductions_Deductions_DeductionId",
                table: "PayrollDeductions",
                column: "DeductionId",
                principalTable: "Deductions",
                principalColumn: "DeductionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PayrollDeductions_Payrolls_PayrollId",
                table: "PayrollDeductions",
                column: "PayrollId",
                principalTable: "Payrolls",
                principalColumn: "PayrollId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PayrollRuns_AspNetUsers_RunByUserId",
                table: "PayrollRuns",
                column: "RunByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payrolls_PayrollRuns_PayrollRunId",
                table: "Payrolls",
                column: "PayrollRunId",
                principalTable: "PayrollRuns",
                principalColumn: "PayrollRunId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applicants_AspNetUsers_IdentityUserId",
                table: "Applicants");

            migrationBuilder.DropForeignKey(
                name: "FK_Applicants_JobOpenings_JobOpeningId",
                table: "Applicants");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_AspNetUsers_IdentityUserId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_ExitClearances_AspNetUsers_FinanceClearedByUserId",
                table: "ExitClearances");

            migrationBuilder.DropForeignKey(
                name: "FK_ExitClearances_AspNetUsers_HRClearedByUserId",
                table: "ExitClearances");

            migrationBuilder.DropForeignKey(
                name: "FK_ExitClearances_AspNetUsers_ITClearedByUserId",
                table: "ExitClearances");

            migrationBuilder.DropForeignKey(
                name: "FK_ExitClearances_Resignations_ResignationId",
                table: "ExitClearances");

            migrationBuilder.DropForeignKey(
                name: "FK_Interviews_Applicants_ApplicantId",
                table: "Interviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Interviews_AspNetUsers_InterviewerUserId",
                table: "Interviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Interviews_Employees_InterviewerId",
                table: "Interviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Interviews_JobOpenings_JobOpeningId",
                table: "Interviews");

            migrationBuilder.DropForeignKey(
                name: "FK_LeaveApprovals_AspNetUsers_ApproverUserId",
                table: "LeaveApprovals");

            migrationBuilder.DropForeignKey(
                name: "FK_LeaveApprovals_Employees_ApproverEmployeeId",
                table: "LeaveApprovals");

            migrationBuilder.DropForeignKey(
                name: "FK_LeaveApprovals_LeaveRequests_LeaveRequestId",
                table: "LeaveApprovals");

            migrationBuilder.DropForeignKey(
                name: "FK_LeaveEntitlements_LeaveTypes_LeaveTypeId",
                table: "LeaveEntitlements");

            migrationBuilder.DropForeignKey(
                name: "FK_LeaveLedgers_LeaveTypes_LeaveTypeId",
                table: "LeaveLedgers");

            migrationBuilder.DropForeignKey(
                name: "FK_LeavePolicies_LeaveTypes_LeaveTypeId",
                table: "LeavePolicies");

            migrationBuilder.DropForeignKey(
                name: "FK_LeaveRequestDays_LeaveRequests_LeaveRequestId",
                table: "LeaveRequestDays");

            migrationBuilder.DropForeignKey(
                name: "FK_LeaveRequests_LeaveTypes_LeaveTypeID",
                table: "LeaveRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Applicants_ApplicantId",
                table: "Offers");

            migrationBuilder.DropForeignKey(
                name: "FK_Offers_AspNetUsers_IssuedByUserId",
                table: "Offers");

            migrationBuilder.DropForeignKey(
                name: "FK_Offers_JobOpenings_JobOpeningId",
                table: "Offers");

            migrationBuilder.DropForeignKey(
                name: "FK_Onboardings_Applicants_ApplicantId",
                table: "Onboardings");

            migrationBuilder.DropForeignKey(
                name: "FK_Onboardings_AspNetUsers_ResponsibleUserId",
                table: "Onboardings");

            migrationBuilder.DropForeignKey(
                name: "FK_Onboardings_Employees_EmployeeId",
                table: "Onboardings");

            migrationBuilder.DropForeignKey(
                name: "FK_PayrollDeductions_Deductions_DeductionId",
                table: "PayrollDeductions");

            migrationBuilder.DropForeignKey(
                name: "FK_PayrollDeductions_Payrolls_PayrollId",
                table: "PayrollDeductions");

            migrationBuilder.DropForeignKey(
                name: "FK_PayrollRuns_AspNetUsers_RunByUserId",
                table: "PayrollRuns");

            migrationBuilder.DropForeignKey(
                name: "FK_Payrolls_PayrollRuns_PayrollRunId",
                table: "Payrolls");

            migrationBuilder.DropTable(
                name: "Holidays");

            migrationBuilder.DropTable(
                name: "WorkSchedules");

            migrationBuilder.DropTable(
                name: "HolidayCalendars");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PayrollRuns",
                table: "PayrollRuns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PayrollDeductions",
                table: "PayrollDeductions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Onboardings",
                table: "Onboardings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Offers",
                table: "Offers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LeaveTypes",
                table: "LeaveTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LeaveRequestDays",
                table: "LeaveRequestDays");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LeavePolicies",
                table: "LeavePolicies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LeaveApprovals",
                table: "LeaveApprovals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Interviews",
                table: "Interviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExitClearances",
                table: "ExitClearances");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Deductions",
                table: "Deductions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Applicants",
                table: "Applicants");

            migrationBuilder.RenameTable(
                name: "PayrollRuns",
                newName: "PayrollRun");

            migrationBuilder.RenameTable(
                name: "PayrollDeductions",
                newName: "PayrollDeduction");

            migrationBuilder.RenameTable(
                name: "Onboardings",
                newName: "Onboarding");

            migrationBuilder.RenameTable(
                name: "Offers",
                newName: "Offer");

            migrationBuilder.RenameTable(
                name: "LeaveTypes",
                newName: "LeaveType");

            migrationBuilder.RenameTable(
                name: "LeaveRequestDays",
                newName: "LeaveRequestDay");

            migrationBuilder.RenameTable(
                name: "LeavePolicies",
                newName: "LeavePolicy");

            migrationBuilder.RenameTable(
                name: "LeaveApprovals",
                newName: "LeaveApproval");

            migrationBuilder.RenameTable(
                name: "Interviews",
                newName: "Interview");

            migrationBuilder.RenameTable(
                name: "ExitClearances",
                newName: "ExitClearance");

            migrationBuilder.RenameTable(
                name: "Deductions",
                newName: "Deduction");

            migrationBuilder.RenameTable(
                name: "Applicants",
                newName: "Applicant");

            migrationBuilder.RenameIndex(
                name: "IX_PayrollRuns_RunByUserId",
                table: "PayrollRun",
                newName: "IX_PayrollRun_RunByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_PayrollDeductions_PayrollId",
                table: "PayrollDeduction",
                newName: "IX_PayrollDeduction_PayrollId");

            migrationBuilder.RenameIndex(
                name: "IX_PayrollDeductions_DeductionId",
                table: "PayrollDeduction",
                newName: "IX_PayrollDeduction_DeductionId");

            migrationBuilder.RenameIndex(
                name: "IX_Onboardings_ResponsibleUserId",
                table: "Onboarding",
                newName: "IX_Onboarding_ResponsibleUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Onboardings_EmployeeId",
                table: "Onboarding",
                newName: "IX_Onboarding_EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Onboardings_ApplicantId",
                table: "Onboarding",
                newName: "IX_Onboarding_ApplicantId");

            migrationBuilder.RenameIndex(
                name: "IX_Offers_JobOpeningId",
                table: "Offer",
                newName: "IX_Offer_JobOpeningId");

            migrationBuilder.RenameIndex(
                name: "IX_Offers_IssuedByUserId",
                table: "Offer",
                newName: "IX_Offer_IssuedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Offers_ApplicantId",
                table: "Offer",
                newName: "IX_Offer_ApplicantId");

            migrationBuilder.RenameIndex(
                name: "IX_LeaveRequestDays_LeaveRequestId",
                table: "LeaveRequestDay",
                newName: "IX_LeaveRequestDay_LeaveRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_LeavePolicies_LeaveTypeId",
                table: "LeavePolicy",
                newName: "IX_LeavePolicy_LeaveTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_LeaveApprovals_LeaveRequestId",
                table: "LeaveApproval",
                newName: "IX_LeaveApproval_LeaveRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_LeaveApprovals_ApproverUserId",
                table: "LeaveApproval",
                newName: "IX_LeaveApproval_ApproverUserId");

            migrationBuilder.RenameIndex(
                name: "IX_LeaveApprovals_ApproverEmployeeId",
                table: "LeaveApproval",
                newName: "IX_LeaveApproval_ApproverEmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Interviews_JobOpeningId",
                table: "Interview",
                newName: "IX_Interview_JobOpeningId");

            migrationBuilder.RenameIndex(
                name: "IX_Interviews_InterviewerUserId",
                table: "Interview",
                newName: "IX_Interview_InterviewerUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Interviews_InterviewerId",
                table: "Interview",
                newName: "IX_Interview_InterviewerId");

            migrationBuilder.RenameIndex(
                name: "IX_Interviews_ApplicantId",
                table: "Interview",
                newName: "IX_Interview_ApplicantId");

            migrationBuilder.RenameIndex(
                name: "IX_ExitClearances_ResignationId",
                table: "ExitClearance",
                newName: "IX_ExitClearance_ResignationId");

            migrationBuilder.RenameIndex(
                name: "IX_ExitClearances_ITClearedByUserId",
                table: "ExitClearance",
                newName: "IX_ExitClearance_ITClearedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_ExitClearances_HRClearedByUserId",
                table: "ExitClearance",
                newName: "IX_ExitClearance_HRClearedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_ExitClearances_FinanceClearedByUserId",
                table: "ExitClearance",
                newName: "IX_ExitClearance_FinanceClearedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Applicants_JobOpeningId",
                table: "Applicant",
                newName: "IX_Applicant_JobOpeningId");

            migrationBuilder.RenameIndex(
                name: "IX_Applicants_IdentityUserId",
                table: "Applicant",
                newName: "IX_Applicant_IdentityUserId");

            migrationBuilder.AlterColumn<string>(
                name: "PositionName",
                table: "Positions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Employees",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Employees",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Employees",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ImagePath",
                table: "Employees",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Employees",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "EmploymentType",
                table: "Employees",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeCode",
                table: "Employees",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Employees",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DepartmentName",
                table: "Departments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PayrollRun",
                table: "PayrollRun",
                column: "PayrollRunId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PayrollDeduction",
                table: "PayrollDeduction",
                column: "PayrollDeductionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Onboarding",
                table: "Onboarding",
                column: "OnboardingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Offer",
                table: "Offer",
                column: "OfferId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LeaveType",
                table: "LeaveType",
                column: "LeaveTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LeaveRequestDay",
                table: "LeaveRequestDay",
                column: "LeaveRequestDayId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LeavePolicy",
                table: "LeavePolicy",
                column: "LeavePolicyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LeaveApproval",
                table: "LeaveApproval",
                column: "LeaveApprovalId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Interview",
                table: "Interview",
                column: "InterviewId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExitClearance",
                table: "ExitClearance",
                column: "ExitClearanceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Deduction",
                table: "Deduction",
                column: "DeductionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Applicant",
                table: "Applicant",
                column: "ApplicantId");

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "DepartmentId", "DepartmentName", "ParentDepartmentId" },
                values: new object[,]
                {
                    { 1, "Human Resources", null },
                    { 2, "Information Technology", null },
                    { 3, "Finance", null },
                    { 4, "Marketing", null },
                    { 5, "Operations", null }
                });

            migrationBuilder.InsertData(
                table: "Positions",
                columns: new[] { "PositionId", "BaseSalary", "PositionName" },
                values: new object[,]
                {
                    { 1, 75000m, "Software Developer" },
                    { 2, 65000m, "HR Manager" },
                    { 3, 60000m, "Financial Analyst" },
                    { 4, 55000m, "Marketing Specialist" },
                    { 5, 70000m, "Operations Manager" },
                    { 6, 68000m, "System Administrator" },
                    { 7, 50000m, "Accountant" },
                    { 8, 40000m, "HR Assistant" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Email",
                table: "Employees",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmployeeCode",
                table: "Employees",
                column: "EmployeeCode",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Applicant_AspNetUsers_IdentityUserId",
                table: "Applicant",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Applicant_JobOpenings_JobOpeningId",
                table: "Applicant",
                column: "JobOpeningId",
                principalTable: "JobOpenings",
                principalColumn: "JobOpeningId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_AspNetUsers_IdentityUserId",
                table: "Employees",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExitClearance_AspNetUsers_FinanceClearedByUserId",
                table: "ExitClearance",
                column: "FinanceClearedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExitClearance_AspNetUsers_HRClearedByUserId",
                table: "ExitClearance",
                column: "HRClearedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExitClearance_AspNetUsers_ITClearedByUserId",
                table: "ExitClearance",
                column: "ITClearedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExitClearance_Resignations_ResignationId",
                table: "ExitClearance",
                column: "ResignationId",
                principalTable: "Resignations",
                principalColumn: "ResignationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Interview_Applicant_ApplicantId",
                table: "Interview",
                column: "ApplicantId",
                principalTable: "Applicant",
                principalColumn: "ApplicantId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Interview_AspNetUsers_InterviewerUserId",
                table: "Interview",
                column: "InterviewerUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Interview_Employees_InterviewerId",
                table: "Interview",
                column: "InterviewerId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Interview_JobOpenings_JobOpeningId",
                table: "Interview",
                column: "JobOpeningId",
                principalTable: "JobOpenings",
                principalColumn: "JobOpeningId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveApproval_AspNetUsers_ApproverUserId",
                table: "LeaveApproval",
                column: "ApproverUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveApproval_Employees_ApproverEmployeeId",
                table: "LeaveApproval",
                column: "ApproverEmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveApproval_LeaveRequests_LeaveRequestId",
                table: "LeaveApproval",
                column: "LeaveRequestId",
                principalTable: "LeaveRequests",
                principalColumn: "LeaveRequestId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveEntitlements_LeaveType_LeaveTypeId",
                table: "LeaveEntitlements",
                column: "LeaveTypeId",
                principalTable: "LeaveType",
                principalColumn: "LeaveTypeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveLedgers_LeaveType_LeaveTypeId",
                table: "LeaveLedgers",
                column: "LeaveTypeId",
                principalTable: "LeaveType",
                principalColumn: "LeaveTypeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LeavePolicy_LeaveType_LeaveTypeId",
                table: "LeavePolicy",
                column: "LeaveTypeId",
                principalTable: "LeaveType",
                principalColumn: "LeaveTypeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveRequestDay_LeaveRequests_LeaveRequestId",
                table: "LeaveRequestDay",
                column: "LeaveRequestId",
                principalTable: "LeaveRequests",
                principalColumn: "LeaveRequestId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveRequests_LeaveType_LeaveTypeID",
                table: "LeaveRequests",
                column: "LeaveTypeID",
                principalTable: "LeaveType",
                principalColumn: "LeaveTypeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Offer_Applicant_ApplicantId",
                table: "Offer",
                column: "ApplicantId",
                principalTable: "Applicant",
                principalColumn: "ApplicantId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Offer_AspNetUsers_IssuedByUserId",
                table: "Offer",
                column: "IssuedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Offer_JobOpenings_JobOpeningId",
                table: "Offer",
                column: "JobOpeningId",
                principalTable: "JobOpenings",
                principalColumn: "JobOpeningId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Onboarding_Applicant_ApplicantId",
                table: "Onboarding",
                column: "ApplicantId",
                principalTable: "Applicant",
                principalColumn: "ApplicantId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Onboarding_AspNetUsers_ResponsibleUserId",
                table: "Onboarding",
                column: "ResponsibleUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Onboarding_Employees_EmployeeId",
                table: "Onboarding",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PayrollDeduction_Deduction_DeductionId",
                table: "PayrollDeduction",
                column: "DeductionId",
                principalTable: "Deduction",
                principalColumn: "DeductionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PayrollDeduction_Payrolls_PayrollId",
                table: "PayrollDeduction",
                column: "PayrollId",
                principalTable: "Payrolls",
                principalColumn: "PayrollId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PayrollRun_AspNetUsers_RunByUserId",
                table: "PayrollRun",
                column: "RunByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payrolls_PayrollRun_PayrollRunId",
                table: "Payrolls",
                column: "PayrollRunId",
                principalTable: "PayrollRun",
                principalColumn: "PayrollRunId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

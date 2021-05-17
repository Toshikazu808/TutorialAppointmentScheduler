using Microsoft.EntityFrameworkCore.Migrations;

namespace AppointmentScheduling.Migrations
{
    public partial class AddAppointmentToDbFixedSpelling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PatrientId",
                table: "Appointments",
                newName: "PatientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PatientId",
                table: "Appointments",
                newName: "PatrientId");
        }
    }
}

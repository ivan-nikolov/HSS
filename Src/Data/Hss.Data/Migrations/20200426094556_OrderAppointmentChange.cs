using Microsoft.EntityFrameworkCore.Migrations;

namespace Hss.Data.Migrations
{
    public partial class OrderAppointmentChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Appointments_AppointmetnId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_AppointmetnId",
                table: "Orders");

            migrationBuilder.AlterColumn<string>(
                name: "AppointmetnId",
                table: "Orders",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OrderId",
                table: "Appointments",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_OrderId",
                table: "Appointments",
                column: "OrderId",
                unique: true,
                filter: "[OrderId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Orders_OrderId",
                table: "Appointments",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Orders_OrderId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_OrderId",
                table: "Appointments");

            migrationBuilder.AlterColumn<string>(
                name: "AppointmetnId",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OrderId",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AppointmetnId",
                table: "Orders",
                column: "AppointmetnId",
                unique: true,
                filter: "[AppointmetnId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Appointments_AppointmetnId",
                table: "Orders",
                column: "AppointmetnId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

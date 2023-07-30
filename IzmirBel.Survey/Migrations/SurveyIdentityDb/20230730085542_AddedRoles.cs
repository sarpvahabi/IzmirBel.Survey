using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IzmirBel.Survey.Migrations.SurveyIdentityDb
{
    /// <inheritdoc />
    public partial class AddedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ec2a9f95-5334-4024-84a7-88d3fce30da4", "1", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "23d15752-e4f1-4baa-b061-e30bd9302fb1", 0, "1", "admin@izmir.bel.tr", true, false, null, "ADMIN@IZMIR.BEL.TR", "ADMIN@IZMIR.BEL.TR", null, null, false, "f9138c55-697e-4c6c-b7e8-8503049df58e", false, "admin@izmir.bel.tr" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ec2a9f95-5334-4024-84a7-88d3fce30da4");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "23d15752-e4f1-4baa-b061-e30bd9302fb1");
        }
    }
}

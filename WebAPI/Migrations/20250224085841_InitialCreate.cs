using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    IDCardNumber = table.Column<string>(type: "TEXT", maxLength: 18, nullable: false),
                    Photo = table.Column<string>(type: "TEXT", nullable: true),
                    Education = table.Column<string>(type: "TEXT", nullable: true),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    Level = table.Column<string>(type: "TEXT", nullable: true),
                    LevelJobType = table.Column<string>(type: "TEXT", nullable: true),
                    Position = table.Column<string>(type: "TEXT", nullable: true),
                    UnitName = table.Column<string>(type: "TEXT", nullable: true),
                    RuzhiDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SchoolName = table.Column<string>(type: "TEXT", nullable: true),
                    ZhuanYe = table.Column<string>(type: "TEXT", nullable: true),
                    RuzhiDateStart = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RuzhiDateEnd = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.UniqueConstraint("AK_Employees_IDCardNumber", x => x.IDCardNumber);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Role = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    Permissions = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImportHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IDCardNumber = table.Column<string>(type: "TEXT", maxLength: 18, nullable: false),
                    ImportCount = table.Column<string>(type: "TEXT", nullable: false),
                    ImportTime = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImportHistories_Employees_IDCardNumber",
                        column: x => x.IDCardNumber,
                        principalTable: "Employees",
                        principalColumn: "IDCardNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhotoTables",
                columns: table => new
                {
                    IDCardNumber = table.Column<string>(type: "TEXT", maxLength: 18, nullable: false),
                    Photo = table.Column<byte[]>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhotoTables", x => x.IDCardNumber);
                    table.ForeignKey(
                        name: "FK_PhotoTables_Employees_IDCardNumber",
                        column: x => x.IDCardNumber,
                        principalTable: "Employees",
                        principalColumn: "IDCardNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SerialNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    TrainingDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TrainingContent = table.Column<string>(type: "TEXT", nullable: true),
                    TrainingUnit = table.Column<string>(type: "TEXT", nullable: true),
                    TrainingLocation = table.Column<string>(type: "TEXT", nullable: true),
                    Assessment = table.Column<string>(type: "TEXT", nullable: true),
                    Cost = table.Column<decimal>(type: "TEXT", nullable: true),
                    Remarks = table.Column<string>(type: "TEXT", nullable: true),
                    EmployeeId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingRecords_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_IDCardNumber",
                table: "Employees",
                column: "IDCardNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImportHistories_IDCardNumber",
                table: "ImportHistories",
                column: "IDCardNumber");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingRecords_EmployeeId",
                table: "TrainingRecords",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImportHistories");

            migrationBuilder.DropTable(
                name: "PhotoTables");

            migrationBuilder.DropTable(
                name: "TrainingRecords");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}

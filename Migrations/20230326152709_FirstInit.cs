using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Invoices_Manager_API.Migrations
{
    /// <inheritdoc />
    public partial class FirstInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Notebook",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notebook", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Note",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    Value = table.Column<string>(type: "longtext", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastEditDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    NotebookModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Note", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Note_Notebook_NotebookModelId",
                        column: x => x.NotebookModelId,
                        principalTable: "Notebook",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(type: "longtext", nullable: false),
                    Password = table.Column<string>(type: "longtext", nullable: false),
                    FirstName = table.Column<string>(type: "longtext", nullable: false),
                    LastName = table.Column<string>(type: "longtext", nullable: false),
                    Email = table.Column<string>(type: "longtext", nullable: false),
                    NotebookId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Notebook_NotebookId",
                        column: x => x.NotebookId,
                        principalTable: "Notebook",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BackUp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    NotebookId = table.Column<int>(type: "int", nullable: false),
                    UserModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackUp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BackUp_Notebook_NotebookId",
                        column: x => x.NotebookId,
                        principalTable: "Notebook",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BackUp_User_UserModelId",
                        column: x => x.UserModelId,
                        principalTable: "User",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Invoice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    FileID = table.Column<string>(type: "longtext", nullable: false),
                    CaptureDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ExhibitionDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Reference = table.Column<string>(type: "longtext", nullable: true),
                    DocumentType = table.Column<string>(type: "longtext", nullable: true),
                    Organization = table.Column<string>(type: "longtext", nullable: true),
                    InvoiceNumber = table.Column<string>(type: "longtext", nullable: true),
                    Tags = table.Column<string>(type: "longtext", nullable: false),
                    ImportanceState = table.Column<int>(type: "int", nullable: false),
                    MoneyState = table.Column<int>(type: "int", nullable: false),
                    PaidState = table.Column<int>(type: "int", nullable: false),
                    MoneyTotal = table.Column<double>(type: "double", nullable: false),
                    UserModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoice_User_UserModelId",
                        column: x => x.UserModelId,
                        principalTable: "User",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BackUpInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    BackUpId = table.Column<int>(type: "int", nullable: false),
                    BackUpName = table.Column<string>(type: "longtext", nullable: false),
                    DateOfCreation = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    BackUpSize = table.Column<string>(type: "longtext", nullable: false),
                    EntityCountInvoices = table.Column<int>(type: "int", nullable: false),
                    EntityCountNotes = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackUpInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BackUpInfo_BackUp_BackUpId",
                        column: x => x.BackUpId,
                        principalTable: "BackUp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "InvoiceBackUp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    InvoiceId = table.Column<int>(type: "int", nullable: false),
                    Base64 = table.Column<string>(type: "longtext", nullable: false),
                    BackUpModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceBackUp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceBackUp_BackUp_BackUpModelId",
                        column: x => x.BackUpModelId,
                        principalTable: "BackUp",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InvoiceBackUp_Invoice_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_BackUp_NotebookId",
                table: "BackUp",
                column: "NotebookId");

            migrationBuilder.CreateIndex(
                name: "IX_BackUp_UserModelId",
                table: "BackUp",
                column: "UserModelId");

            migrationBuilder.CreateIndex(
                name: "IX_BackUpInfo_BackUpId",
                table: "BackUpInfo",
                column: "BackUpId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_UserModelId",
                table: "Invoice",
                column: "UserModelId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceBackUp_BackUpModelId",
                table: "InvoiceBackUp",
                column: "BackUpModelId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceBackUp_InvoiceId",
                table: "InvoiceBackUp",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Note_NotebookModelId",
                table: "Note",
                column: "NotebookModelId");

            migrationBuilder.CreateIndex(
                name: "IX_User_NotebookId",
                table: "User",
                column: "NotebookId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BackUpInfo");

            migrationBuilder.DropTable(
                name: "InvoiceBackUp");

            migrationBuilder.DropTable(
                name: "Note");

            migrationBuilder.DropTable(
                name: "BackUp");

            migrationBuilder.DropTable(
                name: "Invoice");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Notebook");
        }
    }
}

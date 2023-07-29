using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IzmirBel.Survey.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerSurveys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SurveyCompleteMessage = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerSurveys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerSurveysResponses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SurveyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerSurveysResponses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SurveyQuestions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SurveyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PossibleAnswers = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurveyQuestions_CustomerSurveys_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "CustomerSurveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SurveyAnswer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SurveyResponseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurveyAnswer_CustomerSurveysResponses_SurveyResponseId",
                        column: x => x.SurveyResponseId,
                        principalTable: "CustomerSurveysResponses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CustomerSurveys",
                columns: new[] { "Id", "SurveyCompleteMessage", "Title" },
                values: new object[] { new Guid("8f8afc29-228d-4508-9f7a-7d17c4ae9900"), "You completed the survey, THANKS!!!", "Staff Survey - İzmir Metropolitan Municipality" });

            migrationBuilder.InsertData(
                table: "CustomerSurveysResponses",
                columns: new[] { "Id", "SurveyId" },
                values: new object[,]
                {
                    { new Guid("31470369-f098-4ab8-8e0c-bf98134c61a9"), new Guid("8f8afc29-228d-4508-9f7a-7d17c4ae9900") },
                    { new Guid("d8cd6f82-50f1-4ebf-95c3-4fd1956d62f5"), new Guid("8f8afc29-228d-4508-9f7a-7d17c4ae9900") }
                });

            migrationBuilder.InsertData(
                table: "SurveyAnswer",
                columns: new[] { "Id", "Answer", "Question", "SurveyResponseId" },
                values: new object[,]
                {
                    { new Guid("1f970bf1-3c5d-4a6a-94ec-77adb79d84a9"), "No", "Do you enjoy working at İzmir Metropolitan Municipality?", new Guid("31470369-f098-4ab8-8e0c-bf98134c61a9") },
                    { new Guid("3e67aa19-0fe2-4a84-85d6-1844be9d7a8f"), "It's really cool here!", "Any comments on how you find working for İzmir Metropolitan Municipality?", new Guid("d8cd6f82-50f1-4ebf-95c3-4fd1956d62f5") },
                    { new Guid("5bc2f91e-dcd3-481e-9712-8794431b6f97"), "More than 5 years", "How long have you worked at İzmir Metropolitan Municipality?", new Guid("31470369-f098-4ab8-8e0c-bf98134c61a9") },
                    { new Guid("7724ce32-427d-46d5-8033-07a8756083eb"), "Less than 1 year", "How long have you worked at İzmir Metropolitan Municipality?", new Guid("d8cd6f82-50f1-4ebf-95c3-4fd1956d62f5") },
                    { new Guid("9cbaa84f-a1b2-4557-9b0a-8f16e615c1b6"), "Yes", "Do you enjoy working at İzmir Metropolitan Municipality?", new Guid("d8cd6f82-50f1-4ebf-95c3-4fd1956d62f5") },
                    { new Guid("e8a13a1e-42d2-4608-bd8e-de3a916d12a3"), "My computer is really slow", "Any comments on how you find working for İzmir Metropolitan Municipality?", new Guid("31470369-f098-4ab8-8e0c-bf98134c61a9") }
                });

            migrationBuilder.InsertData(
                table: "SurveyQuestions",
                columns: new[] { "Id", "Answer", "PossibleAnswers", "Question", "SurveyId" },
                values: new object[,]
                {
                    { new Guid("8f8afc29-228d-4508-9f7a-7d17c4ae9901"), "", "Less than 1 year|1-5 years|More than 5 years", "How long have you worked at İzmir Metropolitan Municipality?", new Guid("8f8afc29-228d-4508-9f7a-7d17c4ae9900") },
                    { new Guid("8f8afc29-228d-4508-9f7a-7d17c4ae9902"), "", "Yes|No|Sometimes", "Do you enjoy working at İzmir Metropolitan Municipality?", new Guid("8f8afc29-228d-4508-9f7a-7d17c4ae9900") },
                    { new Guid("8f8afc29-228d-4508-9f7a-7d17c4ae9903"), "", "", "Any comments on how you find working for İzmir Metropolitan Municipality?", new Guid("8f8afc29-228d-4508-9f7a-7d17c4ae9900") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_SurveyAnswer_SurveyResponseId",
                table: "SurveyAnswer",
                column: "SurveyResponseId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyQuestions_SurveyId",
                table: "SurveyQuestions",
                column: "SurveyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SurveyAnswer");

            migrationBuilder.DropTable(
                name: "SurveyQuestions");

            migrationBuilder.DropTable(
                name: "CustomerSurveysResponses");

            migrationBuilder.DropTable(
                name: "CustomerSurveys");
        }
    }
}

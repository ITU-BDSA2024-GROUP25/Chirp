using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chirp.Web.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authors_Authors_AuthorId1",
                table: "Authors");

            migrationBuilder.DropIndex(
                name: "IX_Cheeps_CheepId",
                table: "Cheeps");

            migrationBuilder.DropIndex(
                name: "IX_Authors_AuthorId",
                table: "Authors");

            migrationBuilder.DropIndex(
                name: "IX_Authors_AuthorId1",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "AuthorId1",
                table: "Authors");

            migrationBuilder.CreateTable(
                name: "AuthorFollowings",
                columns: table => new
                {
                    AuthorId = table.Column<int>(type: "INTEGER", nullable: false),
                    FollowingAuthorId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorFollowings", x => new { x.AuthorId, x.FollowingAuthorId });
                    table.ForeignKey(
                        name: "FK_AuthorFollowings_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "AuthorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorFollowings_Authors_FollowingAuthorId",
                        column: x => x.FollowingAuthorId,
                        principalTable: "Authors",
                        principalColumn: "AuthorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorFollowings_FollowingAuthorId",
                table: "AuthorFollowings",
                column: "FollowingAuthorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorFollowings");

            migrationBuilder.AddColumn<int>(
                name: "AuthorId1",
                table: "Authors",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cheeps_CheepId",
                table: "Cheeps",
                column: "CheepId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Authors_AuthorId",
                table: "Authors",
                column: "AuthorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Authors_AuthorId1",
                table: "Authors",
                column: "AuthorId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Authors_Authors_AuthorId1",
                table: "Authors",
                column: "AuthorId1",
                principalTable: "Authors",
                principalColumn: "AuthorId");
        }
    }
}

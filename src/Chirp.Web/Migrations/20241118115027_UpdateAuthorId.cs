﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chirp.Web.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAuthorId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuthorId1",
                table: "Authors",
                type: "INTEGER",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authors_Authors_AuthorId1",
                table: "Authors");

            migrationBuilder.DropIndex(
                name: "IX_Authors_AuthorId1",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "AuthorId1",
                table: "Authors");
        }
    }
}

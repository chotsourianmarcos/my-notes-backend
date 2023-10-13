using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "t_tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdWeb = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "t_user_rols",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    IdWeb = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_user_rols", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "t_users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdWeb = table.Column<Guid>(type: "uuid", nullable: false),
                    IdRol = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    HashedPassword = table.Column<string>(type: "text", nullable: false),
                    HashedAccessToken = table.Column<string>(type: "text", nullable: false),
                    HashedRefreshToken = table.Column<string>(type: "text", nullable: false),
                    FailedLogins = table.Column<int>(type: "integer", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    State = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_t_users_t_user_rols_IdRol",
                        column: x => x.IdRol,
                        principalTable: "t_user_rols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "t_notes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdWeb = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    InsertedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_notes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_t_notes_t_users_UserId",
                        column: x => x.UserId,
                        principalTable: "t_users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "notes_tags",
                columns: table => new
                {
                    NoteItemId = table.Column<int>(type: "integer", nullable: false),
                    TagsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notes_tags", x => new { x.NoteItemId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_notes_tags_t_notes_NoteItemId",
                        column: x => x.NoteItemId,
                        principalTable: "t_notes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_notes_tags_t_tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "t_tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_notes_tags_TagsId",
                table: "notes_tags",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_t_notes_UserId",
                table: "t_notes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_t_users_IdRol",
                table: "t_users",
                column: "IdRol");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "notes_tags");

            migrationBuilder.DropTable(
                name: "t_notes");

            migrationBuilder.DropTable(
                name: "t_tags");

            migrationBuilder.DropTable(
                name: "t_users");

            migrationBuilder.DropTable(
                name: "t_user_rols");
        }
    }
}

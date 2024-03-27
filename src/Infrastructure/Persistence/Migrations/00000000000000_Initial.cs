﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "block",
                columns: table => new
                {
                    idx = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nonce = table.Column<long>(type: "INTEGER", nullable: false),
                    hash = table.Column<string>(type: "TEXT", nullable: false),
                    mrkl_root = table.Column<string>(type: "TEXT", nullable: false),
                    previous_hash = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_block", x => x.idx);
                });

            migrationBuilder.CreateTable(
                name: "outbox_message",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    type = table.Column<string>(type: "TEXT", nullable: false),
                    content = table.Column<string>(type: "TEXT", nullable: false),
                    occured_on_utc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    processed_on_utc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    error = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_outbox_message", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "vote",
                columns: table => new
                {
                    hash = table.Column<string>(type: "TEXT", nullable: false),
                    voter_address = table.Column<string>(type: "TEXT", nullable: false),
                    voter_pub_key = table.Column<string>(type: "TEXT", nullable: false),
                    signature = table.Column<string>(type: "TEXT", nullable: false),
                    party_id = table.Column<int>(type: "INTEGER", nullable: false),
                    timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    nonce = table.Column<long>(type: "INTEGER", nullable: false),
                    block_index = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_vote", x => x.hash);
                    table.ForeignKey(
                        name: "f_k_vote_block_block_index",
                        column: x => x.block_index,
                        principalTable: "block",
                        principalColumn: "idx",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "i_x_block_hash",
                table: "block",
                column: "hash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_vote_block_index",
                table: "vote",
                column: "block_index");

            migrationBuilder.CreateIndex(
                name: "i_x_vote_party_id",
                table: "vote",
                column: "party_id");

            migrationBuilder.CreateIndex(
                name: "i_x_vote_voter_address",
                table: "vote",
                column: "voter_address",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "outbox_message");

            migrationBuilder.DropTable(
                name: "vote");

            migrationBuilder.DropTable(
                name: "block");
        }
    }
}

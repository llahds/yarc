using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    public partial class refactorpostscores : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostScores",
                schema: "Social");

            migrationBuilder.AddColumn<int>(
                name: "CommentCount",
                schema: "Social",
                table: "Posts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Downs",
                schema: "Social",
                table: "Posts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Hot",
                schema: "Social",
                table: "Posts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "New",
                schema: "Social",
                table: "Posts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Rising",
                schema: "Social",
                table: "Posts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Top",
                schema: "Social",
                table: "Posts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Ups",
                schema: "Social",
                table: "Posts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2022, 7, 6, 12, 24, 22, 404, DateTimeKind.Utc).AddTicks(4796));

            migrationBuilder.CreateIndex(
                name: "IX_Posts_Hot",
                schema: "Social",
                table: "Posts",
                column: "Hot");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_New",
                schema: "Social",
                table: "Posts",
                column: "New");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_Rising",
                schema: "Social",
                table: "Posts",
                column: "Rising");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_Top",
                schema: "Social",
                table: "Posts",
                column: "Top");

            migrationBuilder.Sql(@"ALTER procedure [Social].[UpdatePostScores]
as
set nocount on

declare @now datetime = getutcdate();
declare @last48 datetime = dateadd(d, -2, getutcdate());
declare @lastRun datetime = '1970-01-01';

IF not EXISTS (SELECT * 
    FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_SCHEMA = 'Social' 
    AND  TABLE_NAME = '_Jobs')
BEGIN
    
	create table Social._Jobs (Id int, LastRun datetime);

END

if not exists (select id from social._jobs where id = 1)
begin

	insert Social._jobs (id, lastrun) 
		values (1, getutcdate());

end
else
begin

	select @lastRun = lastRun from social._jobs where id = 1;

	update social._jobs set LastRun = getutcdate() where id = 1;

end

;

WITH s AS
(
	select
		id,
		[hot] = round([sign] * [order] + seconds / 45000, 7),
		[top] = round([sign] * [order], 7),
		[new] = seconds * -1,
		[rising] = round([sign] * [order] + last48, 7),
		commentCount = (select count(*) from Comments where postId = r.id),
		ups = (select count(*) from social.PostVotes with (nolock) where PostId = r.Id and vote > 0),
		downs = (select count(*) from social.PostVotes with (nolock) where PostId = r.Id and vote < 0)
	from 
		(select
			id,
			[order] = log (iif(abs(score) < 1, 1, abs(score)), 10),
			[sign] = case 
				when score > 0 then 1
				when score < 0 then -1
				when score = 0 then 0
			end,
			seconds,
			last48
		from 
			(select 
				id,
				score = (select count(*) from social.PostVotes with (nolock) where PostId = p.Id and vote > 0)
					- (select count(*) from social.PostVotes with (nolock) where PostId = p.Id and vote < 0),
				seconds = datediff(s, '1970-01-01', p.CreatedOn),
				hours = datediff(hh, p.CreatedOn, @now),
				last48 = (select count(vote) from social.PostVotes with (nolock) where postId = p.id and CreatedOn >= @last48)
			from 
				social.Posts p
			where p.id in (select postId from social.PostVotes where createdon >= @lastRun)) scores
		) r
)

update social.posts set
		hot = s.hot,
		[top] = s.[top],
		[new] = s.[new],
		rising = s.rising,
		commentCount = s.commentCount,
		ups = s.ups,
		downs = s.downs
	from s join social.posts p on s.id = p.id;", true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Posts_Hot",
                schema: "Social",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_New",
                schema: "Social",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_Rising",
                schema: "Social",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_Top",
                schema: "Social",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "CommentCount",
                schema: "Social",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Downs",
                schema: "Social",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Hot",
                schema: "Social",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "New",
                schema: "Social",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Rising",
                schema: "Social",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Top",
                schema: "Social",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Ups",
                schema: "Social",
                table: "Posts");

            migrationBuilder.CreateTable(
                name: "PostScores",
                schema: "Social",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "int", nullable: false),
                    Hot = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    New = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Rising = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Top = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostScores", x => x.PostId);
                    table.ForeignKey(
                        name: "FK_PostScores_Posts_PostId",
                        column: x => x.PostId,
                        principalSchema: "Social",
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2022, 6, 28, 11, 54, 47, 4, DateTimeKind.Utc).AddTicks(5629));

            migrationBuilder.CreateIndex(
                name: "IX_PostScores_Hot",
                schema: "Social",
                table: "PostScores",
                column: "Hot");

            migrationBuilder.CreateIndex(
                name: "IX_PostScores_New",
                schema: "Social",
                table: "PostScores",
                column: "New");

            migrationBuilder.CreateIndex(
                name: "IX_PostScores_Rising",
                schema: "Social",
                table: "PostScores",
                column: "Rising");

            migrationBuilder.CreateIndex(
                name: "IX_PostScores_Top",
                schema: "Social",
                table: "PostScores",
                column: "Top");

            migrationBuilder.Sql(@"alter procedure Social.UpdatePostScores
as
set nocount on

declare @now datetime = getutcdate();
declare @last48 datetime = dateadd(d, -2, getutcdate());
declare @lastRun datetime = '1970-01-01';

IF not EXISTS (SELECT * 
    FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_SCHEMA = 'Social' 
    AND  TABLE_NAME = '_Jobs')
BEGIN
    
	create table Social._Jobs (Id int, LastRun datetime);

END

if not exists (select id from social._jobs where id = 1)
begin

	insert Social._jobs (id, lastrun) 
		values (1, getutcdate());

end
else
begin

	select @lastRun = lastRun from social._jobs where id = 1;

	update social._jobs set LastRun = getutcdate() where id = 1;

end

;

WITH s AS
(
	select
		id,
		[hot] = round([sign] * [order] + seconds / 45000, 7),
		[top] = round([sign] * [order], 7),
		[new] = seconds * -1,
		[rising] = round([sign] * [order] + last48, 7)
	from 
		(select
			id,
			[order] = log (iif(abs(score) < 1, 1, abs(score)), 10),
			[sign] = case 
				when score > 0 then 1
				when score < 0 then -1
				when score = 0 then 0
			end,
			seconds,
			last48
		from 
			(select 
				id,
				score = (select count(*) from social.PostVotes with (nolock) where PostId = p.Id and vote > 0)
					- (select count(*) from social.PostVotes with (nolock) where PostId = p.Id and vote < 0),
				seconds = datediff(s, '1970-01-01', p.CreatedOn),
				hours = datediff(hh, p.CreatedOn, @now),
				last48 = (select count(vote) from social.PostVotes with (nolock) where postId = p.id and CreatedOn >= @last48)
			from 
				social.Posts p
			where p.id in (select postId from social.PostVotes where createdon >= @lastRun)) scores
		) r
)
MERGE social.postscores AS target
USING s AS source  
ON (target.postId = source.id)
WHEN MATCHED THEN     
    UPDATE SET 
		target.hot = source.hot,
		target.[top] = source.[top],
		target.[new] = source.[new],
		target.rising = source.rising
WHEN NOT MATCHED THEN
    INSERT (PostId, hot, [top], [new], rising) VALUES (Source.id, source.hot, source.[top], source.[new], source.rising);", true);
        }
    }
}

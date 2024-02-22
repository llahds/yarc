using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    public partial class fixcommentcount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Users",
                keyColumn: "Id",
                keyValue: -1,
                column: "CreatedOn",
                value: new DateTime(2024, 2, 22, 5, 40, 20, 318, DateTimeKind.Utc).AddTicks(8717));

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2024, 2, 22, 5, 40, 20, 318, DateTimeKind.Utc).AddTicks(8630));

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CreatedOn",
                schema: "Social",
                table: "Comments",
                column: "CreatedOn")
                .Annotation("SqlServer:Include", new[] { "PostId" });

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
			where 
				p.id in (select postId from social.PostVotes where createdon >= @lastRun)
			) scores				 
		) r
)

update social.posts set
		hot = s.hot,
		[top] = s.[top],
		[new] = s.[new],
		rising = s.rising,
		ups = s.ups,
		downs = s.downs
	from s join social.posts p on s.id = p.id;

update social.posts set
		CommentCount = (select count(*) from social.Comments where postId = p.id)
	from social.posts p join social.Comments c on p.Id = c.PostId
	where c.CreatedOn >= @lastRun;
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Comments_CreatedOn",
                schema: "Social",
                table: "Comments");

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Users",
                keyColumn: "Id",
                keyValue: -1,
                column: "CreatedOn",
                value: new DateTime(2022, 7, 15, 19, 0, 6, 839, DateTimeKind.Utc).AddTicks(4470));

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2022, 7, 15, 19, 0, 6, 839, DateTimeKind.Utc).AddTicks(4455));
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    public partial class updatepostscores : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(@"create procedure Social.UpdatePostScores
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql("drop procedure Social.UpdatePostScores");
		}
    }
}

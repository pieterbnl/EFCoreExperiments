using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCoreMovies.Migrations
{
    public partial class ViewMovieCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE VIEW dbo.MoviesWithCounts
            as

            Select Id, Title,
            (Select count(*) FROM GenreMovie where MoviesId = Movies.Id) as AmountGenres,
            (Select count(distinct moviesId) from CinemaHallMovie
	            INNER JOIN CinemaHalls
	            ON CinemaHalls.Id = CinemaHallMovie.CinemaHallsId
	            WHERE MoviesID = movies.Id) as AmountCinemas,
            (Select count(*) from MoviesActors where MovieId = movies.Id) as AmountActors
            FROM Movies");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW dbo.MoviesWithCounts");
        }
    }
}

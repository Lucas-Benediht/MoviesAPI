using AutoMapper;
using FilmsAPI.Data;
using FilmsAPI.Data.Dtos;
using FilmsAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FilmsAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class MovieController: ControllerBase
{
    private MovieContext _context;
    private IMapper _mapper;

    public MovieController(MovieContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpPost]
    public IActionResult AddMovie([FromBody] CreateMovieDto movieDto)
    {
        // Usando o Dto
        Movie movie = _mapper.Map<Movie>(movieDto);
        _context.Movies.Add(movie);
        _context.SaveChanges(); // Salvar as alterações após inserir um filme 
        return CreatedAtAction(nameof(RecoverMoviesByID), 
            new {id = movie.Id},
            movie );
    }

    [HttpGet]
    public IEnumerable<Movie> RecoverMovies([FromQuery] int skip = 0,[FromQuery] int take = 50) // Limitando o quanto irá aparecer na consulta do postman
    {
        return _context.Movies.Skip(skip).Take(take);
    }

    [HttpGet("{id}")]
    public IActionResult RecoverMoviesByID(int id)
    {
        var movie = _context.Movies.FirstOrDefault(movie => movie.Id == id);
        if (movie == null) return NotFound();
        var movieDto = _mapper.Map<ReadMovieDto>(movie);
        return Ok(movie);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateMovie(int id, [FromBody] UpdateMovieDto movieDto)
    {
        var movie = _context.Movies.FirstOrDefault(movie => movie.Id ==id);
        if (movie == null) return NotFound();
        _mapper.Map(movieDto, movie);
        _context.SaveChanges();
        return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult UpdatePartialMovie(int id, JsonPatchDocument <UpdateMovieDto> patch)
    {
        var movie = _context.Movies.FirstOrDefault(movie => movie.Id == id);
        if (movie == null) return NotFound();

        var movieForUpdate = _mapper.Map<UpdateMovieDto>(movie);

        patch.ApplyTo(movieForUpdate, ModelState);

        if (!TryValidateModel(movieForUpdate))
        {
            return ValidationProblem(ModelState);
        }
        _mapper.Map(movieForUpdate, movie);
        _context.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteMovie(int id) 
    {
        var movie = _context.Movies.FirstOrDefault(movie => movie.Id == id);
        if (movie == null) return NotFound();
        _context.Movies.Remove(movie);
        _context.SaveChanges();
        return NoContent();
    }


}


using System.ComponentModel.DataAnnotations;

namespace FilmsAPI.Data.Dtos;

public class CreateMovieDto
{
    [Required(ErrorMessage = "O titulo do filme é obrigadotório")]
    public string Title { get; set; }
    [Required(ErrorMessage = "O gênero do filme é obrigatório")]
    [StringLength(100, ErrorMessage = "O tamanho do gênero não pode exceder 100 caracteres")]
    public string Genre { get; set; }
    [Required]
    [Range(70, 600, ErrorMessage = "A duraçaõ do filme deve ter entre 70 e 600 minutos")]
    public int Duration { get; set; }
}

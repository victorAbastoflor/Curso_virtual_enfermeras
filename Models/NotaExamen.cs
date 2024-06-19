using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NurseCourse.Models;

public partial class NotaExamen
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int NotaExamenId { get; set; }

    [ForeignKey("Usuario")]
    public int UsuarioId { get; set; }

    [ForeignKey("Examen")]
    public int ExamenId { get; set; }

    [Range(1, 100, ErrorMessage = "La nota debe estar entre 1 y 100.")]
    public double Calificacion { get; set; }

    public virtual Usuario Usuario { get; set; }
    public virtual Examene Examen { get; set; }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace villaApi.Models.Dto
{
  public class VillaDTO
  {
    public int Id { get; set; }

    [Required]
    [MaxLength(30)]
    public string Name { get; set; }
    public string Ocupancy { get; set; }
    public int Sqft { get; set; }
  }
}
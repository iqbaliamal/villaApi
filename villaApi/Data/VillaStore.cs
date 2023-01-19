using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using villaApi.Models.Dto;

namespace villaApi.Data
{
  public class VillaStore
  {
    public static List<VillaDTO> villaList = new List<VillaDTO>
        {
            new VillaDTO {Id = 1, Name = "Pool View", Ocupancy = "3", Sqft = 1000},
            new VillaDTO {Id = 2, Name = "Beach View", Ocupancy = "3", Sqft = 1000},
        };
  }
}
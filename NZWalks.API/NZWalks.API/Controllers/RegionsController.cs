using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("controller")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository regionRepository,IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        public IRegionRepository RegionRepository { get; }

        [HttpGet]
        public async Task<IActionResult>GetAllRegions()
        {
           var regions=await regionRepository.GetAllAsync();
            //return DTO regions
            /* var regionsDTO=new List<Models.DTO.Region>();
             regions.ToList().ForEach(region =>
             {
                 var regionDTO = new Models.DTO.Region()

                 {
                     Id = region.Id,
                     Code=region.Code,
                     Name = region.Name,
                     Area= region.Area,
                     Lat= region.Lat,
                     Long= region.Long,
                     Population= region.Population,
                };
                 regionsDTO.Add(regionDTO);
             });*/
            /* var regions = new List<Region>()
             {
                 new Region()
                 {
                     Id=Guid.NewGuid(),
                     Name="Willington",
                     Code="WLG",
                     Area=22550,
                     Lat=-1.76939,
                     Long=5.890468,
                     Population=7800000
                 } ,
                 new Region()
                 {
                     Id=Guid.NewGuid(),
                     Name="Acqualand",
                     Code="AUCK",
                     Area=82755,
                     Lat=-1.69636,
                     Long=5.12821,
                     Population=9648000
                 }
             };
            return Ok(regions);*/
            // return Ok(regions);
            //return Ok(regionsDTO);

            var regionsDTO =mapper.Map<List<Models.DTO.Region>>(regions);
            return Ok(regionsDTO);
         }
    }
}

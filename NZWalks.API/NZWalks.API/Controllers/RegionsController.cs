﻿using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
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
        public async Task<IActionResult>GetAllRegionsAsync()
        {
           var regions=await regionRepository.GetAllAsync();
            //return DTO regions
            var regionsDTO = new List<Models.DTO.Region>();
            regions.ToList().ForEach(region =>
            {
                var regionDTO = new Models.DTO.Region()

                {
                    Id = region.Id,
                    Code = region.Code,
                    Name = region.Name,
                    Area = region.Area,
                    Lat = region.Lat,
                    Long = region.Long,
                    Population = region.Population,
                };
                regionsDTO.Add(regionDTO);
            });
          /*  var regions = new List<Region>()
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
            return Ok(regionsDTO);
            /*
                        var regionsDTO = mapper.Map<List<Models.DTO.Region>>(regions);
                        return Ok(regionsDTO);*/
            
        }
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            var region = await regionRepository.GetAsync(id);
            if (region == null)
            {
                return NotFound();
            }
            var regionDTO = mapper.Map<Models.DTO.Region>(region);
            return Ok(regionDTO);
        }
        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            //Request(DTO) to Domain Model
            var region = new Models.Domain.Region()
            {
                Code = addRegionRequest.Code,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Name = addRegionRequest.Name,
                Population = addRegionRequest.Population,
            };
            //pass details to repository
            region = await regionRepository.AddAsync(region);
            //convert back to DTO
            var regionDTO = new Models.DTO.Region
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population
            };
            return CreatedAtAction(nameof(GetRegionAsync), new { id = regionDTO.Id }, regionDTO);
        }
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            //Get the region from the database
           var region=await regionRepository.DeleteAsync(id);

            //if null NotFound
            if(region==null)
            {
                return NotFound();
            }

            //Convert response back to DTO
            var regionDTO = new Models.DTO.Region
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population
            };
            //return Ok Response
            return Ok(regionDTO);
        }
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id,[FromBody]Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            //Convert DTO to Domain Model
            var region = new Models.Domain.Region()
            {

                Code = updateRegionRequest.Code,
                Area = updateRegionRequest.Area,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Name = updateRegionRequest.Name,
                Population = updateRegionRequest.Population,
            };
            //Update Region using respository
            region=await regionRepository.UpdateAsync(id, region);
            //If null then NotFound
            if(region==null)
            {
                return NotFound();
            }
            //Convert Domain back to DTO
            var regionDTO = new Models.DTO.Region
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population
            };
            //Return Ok Response
            return Ok(regionDTO);
        }
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories.IRepository;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }
        //public IWalkRepository WalkRepository { get; }
        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            //Fetch data from database-domain walks
            var walksDomain = await walkRepository.GetAllAsync();
            //Convert domain walks to DTO walks
            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walksDomain);
            //Return response
            return Ok(walksDTO);

            //return DTO Walks
        }
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            //Get Walk Domain object from database
            var walkDomain = await walkRepository.GetAsync(id);
            //convert domain object to DTO
            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);
            //return response
            return Ok(walkDTO);
        }
        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody] Models.DTO.AddWalkRequest addWalkRequest)
        {
            //Convert DTO Domain Object
            var walkDomain = new Models.Domain.Walk
            {
                Length = addWalkRequest.Length,
                Name = addWalkRequest.Name,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId
            };
            //Pass domain object to Repository to persist this
            walkDomain = await walkRepository.AddAsync(walkDomain);
            //Convert the Domain object back to DTO
            var walkDTO = new Models.DTO.Walk
            {
                Id = walkDomain.Id,
                Length = walkDomain.Length,
                Name = walkDomain.Name,
                RegionId = walkDomain.RegionId,
                WalkDifficultyId = walkDomain.WalkDifficultyId
            };
            //Send DTO response back to Client
            return CreatedAtAction(nameof(GetWalkAsync), new { id = walkDTO.Id }, walkDTO);
        }
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            //Convert DTO to Domain Object
            var walkDomain = new Models.Domain.Walk
            {
                Length = updateWalkRequest.Length,
                Name = updateWalkRequest.Name,
                RegionId = updateWalkRequest.RegionId,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId
            };
            //Pass Details to Repository-Get Domain Object in response(or null)
            walkDomain = await walkRepository.UpdateAsync(id, walkDomain);
            //Handle null (not found)
            if (walkDomain == null)
            {
                return NotFound();
            }

            //Convert back Domain to DTO
            var walkDTO = new Models.DTO.Walk
            {
                Id = walkDomain.Id,
                Length = walkDomain.Length,
                Name = walkDomain.Name,
                RegionId = walkDomain.RegionId,
                WalkDifficultyId = walkDomain.WalkDifficultyId
            };
            //Return Response
            return Ok(walkDTO);
        }
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            //call repository to delete walk
            var walkDomain=await walkRepository.DeleteAsync(id);
            if(walkDomain==null)
            {
                return NotFound();  
            }
            var walkDTO=mapper.Map<Models.DTO.Walk>(walkDomain);
            return Ok(walkDTO);
        }
    }
}

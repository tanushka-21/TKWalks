using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TKWalks.API.Models.DTO;
using TKWalks.API.Repositories;

namespace TKWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultiesController : Controller
    {
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;

        public WalkDifficultiesController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficulties()
        {
            var walkDiff = await walkDifficultyRepository.GetAllAsync();
            var walksDiffDTO = mapper.Map<List<Models.DTO.WalkDifficulty>>(walkDiff);

            return Ok(walksDiffDTO);
            
        }
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficultyByIdAsync")]
        public async Task<IActionResult> GetWalkDifficultyByIdAsync(Guid id)
        {
            var walkDif = await walkDifficultyRepository.GetAsync(id);
            if (walkDif == null)
                return NotFound();
            var walkDiffDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDif);
            return Ok(walkDiffDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkDifficultyAsync([FromBody] Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            if (ValidateAddWalkDifficultyAsync(addWalkDifficultyRequest))
                return BadRequest(ModelState);
            var walkDiffDomain = new Models.Domain.WalkDifficulty
            {
                Code = addWalkDifficultyRequest.Code
            };
            walkDiffDomain = await walkDifficultyRepository.AddAsync(walkDiffDomain);
            var walkDiffDTO = new Models.DTO.WalkDifficulty
            {
                Id=walkDiffDomain.Id,
                Code= walkDiffDomain.Code
            };

            return CreatedAtAction(nameof(GetWalkDifficultyByIdAsync), new { id = walkDiffDTO.Id }, walkDiffDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkDifficultyAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkDiffRequest updateWalkDiffRequest)
        {
            if (ValidateUpdateWalkDifficultyAsync(updateWalkDiffRequest))
                return BadRequest(ModelState);
            //convert dto to domain
            var walkDiffDomain = new Models.Domain.WalkDifficulty
            {
                Code = updateWalkDiffRequest.Code
            };

            //update region
            walkDiffDomain = await walkDifficultyRepository.UpdateAsync(id, walkDiffDomain);

            //if null notfound
            if (walkDiffDomain == null)
                return NotFound();
            //convert domain to DTo
            var walkDiffDTO = new Models.DTO.WalkDifficulty
            {
                Id=walkDiffDomain.Id,
                Code=walkDiffDomain.Code
            };
            //ok response
            return Ok(walkDiffDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkDifficultyAsync(Guid id)
        {
            var walkDiffDomain = await walkDifficultyRepository.DeleteAsync(id);
            if (walkDiffDomain == null)
                return NotFound();
            var walkDiffDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDiffDomain);
            return Ok(walkDiffDTO);
        }

        #region Private methods
        private bool ValidateAddWalkDifficultyAsync(Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            if (addWalkDifficultyRequest == null)
            {
                ModelState.AddModelError(nameof(addWalkDifficultyRequest), $"{nameof(addWalkDifficultyRequest)} cannot be empty.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(addWalkDifficultyRequest.Code))
            {
                ModelState.AddModelError(nameof(addWalkDifficultyRequest.Code), 
                    $"{nameof(addWalkDifficultyRequest.Code)} cannot be empty.");
                
            }
            if (ModelState.ErrorCount > 0)
                return false;
            return true;
        }
        private bool ValidateUpdateWalkDifficultyAsync(Models.DTO.UpdateWalkDiffRequest updateWalkDiffRequest)
        {
            if (updateWalkDiffRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkDiffRequest), $"{nameof(updateWalkDiffRequest)} cannot be empty.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(updateWalkDiffRequest.Code))
            {
                ModelState.AddModelError(nameof(updateWalkDiffRequest.Code),
                    $"{nameof(updateWalkDiffRequest.Code)} cannot be empty.");

            }
            if (ModelState.ErrorCount > 0)
                return false;
            return true;
        }
        #endregion
    }
}

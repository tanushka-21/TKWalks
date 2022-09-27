﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TKWalks.API.Models.DTO;
using TKWalks.API.Repositories;

namespace TKWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;
        private readonly IRegionRepository regionRepository;
        private readonly IWalkDifficultyRepository walkDifficultyRepository;

        public WalksController(IWalkRepository walkRepository, IMapper mapper, IRegionRepository regionRepository, IWalkDifficultyRepository walkDifficultyRepository)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
            this.regionRepository = regionRepository;
            this.walkDifficultyRepository = walkDifficultyRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            var walks = await walkRepository.GetAllAsync();
            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walks);

            return Ok(walksDTO);
        }
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            var walkDomain = await walkRepository.GetAsync(id);

            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);
            return Ok(walkDTO);
        }

        [HttpPost]

        public async Task<IActionResult> AddWalkAsync([FromBody] Models.DTO.AddWalkRequest addWalkRequest)
        {
            if(!(await ValidateAddWalkAsync(addWalkRequest)))
            {
                return BadRequest(ModelState);
            }
            var walkDomain = new Models.Domain.Walk
            {
                Length = addWalkRequest.Length,
                Name = addWalkRequest.Name,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId
            };

            walkDomain = await walkRepository.AddAsync(walkDomain);

            var walkDTO = new Models.DTO.Walk
            {
                Id = walkDomain.Id,
                Length = walkDomain.Length,
                Name = walkDomain.Name,
                RegionId = walkDomain.RegionId,
                WalkDifficultyId = walkDomain.WalkDifficultyId
            };

            return CreatedAtAction(nameof(GetWalkAsync), new { id = walkDTO.Id }, walkDTO);
        }
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            if(!(await ValidateUpdateWalkAsync(updateWalkRequest)))
            {
                return BadRequest(ModelState);
            }
            //convert dto to domain
            var walkDomain = new Models.Domain.Walk
            {
                Length = updateWalkRequest.Length,
                Name = updateWalkRequest.Name,
                RegionId = updateWalkRequest.RegionId,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId
            };

            //update region
            walkDomain = await walkRepository.UpdateAsync(id, walkDomain);

            //if null notfound
            if (walkDomain == null)
                return NotFound();
            //convert domain to DTo
            var walkDTO = new Models.DTO.Walk
            {
                Id = walkDomain.Id,
                Length = walkDomain.Length,
                Name = walkDomain.Name,
                RegionId = walkDomain.RegionId,
                WalkDifficultyId = walkDomain.WalkDifficultyId
            };
            //ok response
            return Ok(walkDTO);
        }


        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            var walkDomain = await walkRepository.DeleteAsync(id);
            if (walkDomain == null)
                return NotFound();
            var walkDTO=mapper.Map<Models.DTO.Walk>(walkDomain);
            return Ok(walkDTO);
        }


        #region Private methods
        private async Task<bool> ValidateAddWalkAsync(Models.DTO.AddWalkRequest addWalkRequest)
        {
            if (addWalkRequest == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest), $"{nameof(addWalkRequest)} cannot be empty.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(addWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(addWalkRequest.Name), $"{nameof(addWalkRequest.Name)} is required.");
            }
            if (addWalkRequest.Length<=0)
            {
                ModelState.AddModelError(nameof(addWalkRequest.Length), $"{nameof(addWalkRequest.Length)} should be greater than 0.");
            }
            var region = await regionRepository.GetAsync(addWalkRequest.RegionId);
            if(region==null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.RegionId), $"{nameof(addWalkRequest.RegionId)} is an invalid region id.");
            }
            var walkDiff = await walkDifficultyRepository.GetAsync(addWalkRequest.WalkDifficultyId);
            if(walkDiff==null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.WalkDifficultyId), $"{nameof(addWalkRequest.WalkDifficultyId)} is an invalid walk difficulty id.");
            }

            if (ModelState.ErrorCount > 0)
                return false;
            return true;
        }
        private async Task<bool> ValidateUpdateWalkAsync(Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            if (updateWalkRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest), $"{nameof(updateWalkRequest)} cannot be empty.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(updateWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Name), $"{nameof(updateWalkRequest.Name)} is required.");
            }
            if (updateWalkRequest.Length <= 0)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Length), $"{nameof(updateWalkRequest.Length)} should be greater than 0.");
            }
            var region = await regionRepository.GetAsync(updateWalkRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.RegionId), $"{nameof(updateWalkRequest.RegionId)} is an invalid region id.");
            }
            var walkDiff = await walkDifficultyRepository.GetAsync(updateWalkRequest.WalkDifficultyId);
            if (walkDiff == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.WalkDifficultyId), $"{nameof(updateWalkRequest.WalkDifficultyId)} is an invalid walk difficulty id.");
            }

            if (ModelState.ErrorCount > 0)
                return false;
            return true;
        }
        #endregion
    }
}

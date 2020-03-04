﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shortstories.Data;
using shortstories.Models;

namespace shortstories.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileModelsController : ControllerBase
    {
        private readonly ShortstoriesContext _context;

        public ProfileModelsController(ShortstoriesContext context)
        {
            _context = context;
        }

        // GET: api/ProfileModels/{profileUsername}
        [HttpGet("{profileUsername}")]
        public async Task<ActionResult<ProfileModel>> GetProfile(string profileUsername)
        {
            var profile = await _context.Profile.SingleAsync(a => a.ProfileUsername == profileUsername);

            if (profile == null)
            {
               return NotFound();
            }
            

            return new JsonResult(new { username = profile.ProfileUsername, typeOfWriter = profile.ProfileTypeOfWriter, description = profile.ProfileDescription });
        }

        // POST: api/ProfileModels
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<ProfileModel>> PostProfileModel([FromBody] ProfileModel profile)
        {
            _context.Profile.Add(profile);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return Ok(profile.ProfileModelId);
        }

        // PUT: api/ProfileModels/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("update/type-of-writer/{userId}/{profileTypeOfWriter}")]
        [Authorize]
        public async Task<IActionResult> PutProfileTypeOfWriter([FromRoute] string userId, [FromRoute] string profileTypeOfWriter)
        {
            var profileModel = await _context.Profile.SingleAsync(a => a.UserId == userId);

            if (profileModel == null)
            {
                return BadRequest();
            }

            profileModel.ProfileTypeOfWriter = profileTypeOfWriter;

            _context.Entry(profileModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (profileModel == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPut("update/description/{userId}/{profileDescription}")]
        [Authorize]
        public async Task<IActionResult> PutProfileDescription(string userId, string profileDescription)
        {
            var profileModel = await _context.Profile.SingleAsync(a => a.UserId == userId);

            if (profileModel == null)
            {
                return BadRequest();
            }

            profileModel.ProfileDescription = profileDescription;

            _context.Entry(profileModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (profileModel == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/ProfileModels/5
        [HttpDelete("delete/{userId}")]
        [Authorize]
        public async Task<ActionResult<ProfileModel>> DeleteProfileModel(string userId)
        {
            var profileModel = await _context.Profile.SingleAsync(c => c.UserId == userId);

            if (profileModel == null)
            {
                return NotFound();
            }

            _context.Profile.Remove(profileModel);

            await _context.SaveChangesAsync();

            return Ok("Profile deleted.");
        }
    }
}
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
    public class FollowersModelsController : ControllerBase
    {
        private readonly ShortstoriesContext _context;

        public FollowersModelsController(ShortstoriesContext context)
        {
            _context = context;
        }

        // GET: api/FollowersModels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FollowersModel>>> GetFollowers()
        {
            return await _context.Followers.ToListAsync();
        }

        // GET: api/FollowersModels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FollowersModel>> GetFollowersModel(int id)
        {
            var followersModel = await _context.Followers.FindAsync(id);

            if (followersModel == null)
            {
                return NotFound();
            }

            return followersModel;
        }

        [HttpGet("{profileId}/{userProfileId}")]
        [Authorize]
        public async Task<ActionResult<FollowersModel>> CheckIfUserIsAFriend([FromRoute] string profileId, [FromRoute] string userProfileId) {
            try
            {
                FollowersModel follower = await _context.Followers.Where(a => a.ProfileId == profileId).Where(b => b.FollowersId == userProfileId).SingleOrDefaultAsync();

                if (follower == null)
                {
                    return NotFound();
                }

                return Ok("friend");

            } catch(Exception)
            {
                throw;
            }
        }

        // PUT: api/FollowersModels/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFollowersModel(int id, FollowersModel followersModel)
        {
            if (id != followersModel.FollowersModelId)
            {
                return BadRequest();
            }

            _context.Entry(followersModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FollowersModelExists(id))
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

        // POST: api/FollowersModels
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<FollowersModel>> PostFollowersModel(FollowersModel followersModel)
        {
            _context.Followers.Add(followersModel);
            await _context.SaveChangesAsync();

            return Ok("Follower Added.");
        }

        // DELETE: api/FollowersModels/...
        [HttpDelete("{userId}/{followerId}")]
        [Authorize]
        public async Task<IActionResult> DeleteFollowersModel([FromRoute] string userId, [FromRoute] string followerId)
        {
            try
            {
                ProfileModel profile = await _context.Profile.Where(a => a.UserId == userId).SingleOrDefaultAsync();

                if (profile == null)
                {
                    return NotFound();
                }

                FollowersModel follower = await _context.Followers.Where(b => b.FollowersId == followerId).Where(c => c.ProfileId == profile.ProfileModelId).SingleOrDefaultAsync();

                if (follower == null)
                {
                    return NotFound();
                }

                _context.Followers.Remove(follower);

                await _context.SaveChangesAsync();

                return Ok(new { Deleted = "Unfollowed." });
            } catch(Exception e)
            {
                throw;
            }
        }

        private bool FollowersModelExists(int id)
        {
            return _context.Followers.Any(e => e.FollowersModelId == id);
        }
    }
}

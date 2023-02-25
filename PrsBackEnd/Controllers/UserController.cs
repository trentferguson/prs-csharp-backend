﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrsBackEnd.Models;

namespace PrsBackEnd.Controllers
{
    [Route("users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly PrsDbContext _context;

        public UserController(PrsDbContext context)
        {
            _context = context;
        }

        // Login JSON Format:
        //  {
        //    "username": "string",
        //    "password": "string"
        //  {

        [Route("/login")]
        [HttpPost]
        public async Task<ActionResult<User>> LoginUser([FromBody] UserPasswordObject upo)
        {
            var user = await _context.Users.Where(u => u.Username == upo.username && u.Password == upo.password).FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();  // 404
            }

            return user;  
        }


        // GET: /users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: /users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: /users/
        [HttpPut]
        public async Task<IActionResult> PutUser([FromBody] User user)
        {
            /*if (id != user.Id)
            {
                return BadRequest();
            }
            */

            _context.Entry(user).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            /*
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            */

            return NoContent();
        }

        // POST: /users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser([FromBody] User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: /users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }

    public class UserPasswordObject
    {
        public string username { get; set; }
        public string password { get; set; }
    }

}

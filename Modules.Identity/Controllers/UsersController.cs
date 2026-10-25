using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modules.Identity.Infrastructure.Persistence;
using Modules.Identity.Application.DTOs;
using Modules.Identity.Application.Services;

namespace Modules.Identity.Controllers
{
    [ApiController]
    [Route("api/identity/users")]
    public class UsersController : ControllerBase
    {
        private readonly IdentityDbContext _context;
        private readonly UserService _userService;
        public UsersController(IdentityDbContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
        {
            var users = await _context.Users.AsNoTracking().Select(x=> new
            {
                x.Id,
                x.Email,
                x.FullName,
                x.IsActive,
                x.CreatedAtUtc
            }).ToListAsync(cancellationToken);
            return Ok(users);
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
        {
            var result = await _userService.CreateAsync(
                request,
                cancellationToken
            );

            if (result.Conflict)
            {
                return Conflict(new
                {
                    message = "Энэ имэйл бүртгэлтэй байна."
                });
            }

            var user = result.User!;

            return Created(
                $"/api/identity/users/{user.Id}",
                new
                {
                    user.Id,
                    user.Email,
                    user.FullName,
                    user.IsActive,
                    user.CreatedAtUtc
                }
            );
        }
    }
}

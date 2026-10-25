using Microsoft.EntityFrameworkCore;
using Modules.Identity.Application.DTOs;
using Modules.Identity.Domain.Entities;
using Modules.Identity.Infrastructure.Persistence;

namespace Modules.Identity.Application.Services;

public class UserService
{
    private readonly IdentityDbContext _context;

    public UserService(IdentityDbContext context)
    {
        _context = context;
    }

    public async Task<(User? User, bool Conflict)> CreateAsync(
        CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var exists = await _context.Users
            .AnyAsync(x => x.Email == email, cancellationToken);

        if (exists)
            return (null, true);

        var user = new User
        {
            Email = email,
            FullName = request.FullName.Trim()
        };

        _context.Users.Add(user);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is Npgsql.PostgresException
            {
                SqlState: "23505"
            })
        {
            return (null, true);
        }

        return (user, false);
    }
}
using System.Security.Cryptography;
using Bookanizer.REST.API.DTOs;
using Bookanizer.REST.DAL.Models;
using Bookanizer.REST.DAL.Repositories.Interfaces;
using Bookanizer.REST.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Bookanizer.REST.Services
{
    public class AuthService : IAuthService
    {
        #region Repositories, Services & Helpers
        private readonly IUserRepository _userRepo;
        private readonly ITokenService _tokenSvc;
        private readonly IPasswordHasher<UserModel> _passwordHasher;
        #endregion

        #region Constructors
        public AuthService(IUserRepository userRepo, ITokenService tokenSvc) 
        {
            _userRepo = userRepo;
            _tokenSvc = tokenSvc;
            _passwordHasher = new PasswordHasher<UserModel>();
        }
        #endregion

        #region Methods
        public async Task RegisterAsync(RegisterRequestDto req, CancellationToken ct)
        {            
            // Username
            string username = req.Username.Trim().ToLower(); // Usernames saved as lowercase to prevent same-letter usernames
            UserModel? userInDb = await _userRepo.ReadSingleByNameAsync(username, ct);
            if (userInDb is not null) { throw new InvalidOperationException("Username already exists."); }

            // UserId
            UserModel user = new UserModel { Username = username };
            user.UserId = await GenerateUniqueUserIdAsync(ct);

            // Password
            user.PasswordHash = _passwordHasher.HashPassword(user, req.Password);

            // DB
            await _userRepo.CreateSingleAsync(user, ct);
        }

        public async Task<string> LoginAsync(LoginRequestDto req, CancellationToken ct)
        {
            // Check username
            UserModel? user = await _userRepo.ReadSingleByNameAsync(req.Username.Trim().ToLower(), ct);
            if (user is null) { throw new InvalidOperationException("Username not found."); }

            // Check password
            PasswordVerificationResult verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, req.Password);
            if (verificationResult == PasswordVerificationResult.Failed) { throw new InvalidOperationException("Password does not match."); }

            // Return JWT
            return _tokenSvc.GenerateJwt(user);

        }
        #endregion

        #region Helpers
        private static string GenerateUserId()
        {
            byte[] input = Guid.NewGuid().ToByteArray();
            byte[] hash = MD5.HashData(input);
            return Convert.ToHexString(hash).ToLowerInvariant();
        }

        private async Task<string> GenerateUniqueUserIdAsync(CancellationToken ct)
        {
            const int maxAttempts = 5;
            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                string candidate = GenerateUserId();
                UserModel? existing = await _userRepo.ReadSingleByIdAsync(candidate, ct);
                if (existing is null) { return candidate; }
            }
            throw new InvalidOperationException("Failed to generate a unique user ID after multiple attempts.");
        }
        #endregion
    }
}

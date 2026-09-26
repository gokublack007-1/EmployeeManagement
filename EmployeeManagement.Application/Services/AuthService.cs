using EmployeeManagement.Application.Contracts.Repositories;
using EmployeeManagement.Application.Contracts.Services;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Exceptions;
using EmployeeManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IpasswordHasher _passwordHasher;
        private readonly IJWTTokenService _jwtTokenService;
        public AuthService(IUnitOfWork unitOfWork, IpasswordHasher hasher,IJWTTokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = hasher;
            _jwtTokenService = tokenService;
        }
        public async Task RegisterAsync(RegisterDTO dto)
        {
            var existingUser = await _unitOfWork.Users.GetByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                throw new UserAlreadyExistsException(dto.Email);
            }
            var user = new User
            {
                Email = dto.Email,
                UserName = dto.UserName,
                PasswordHash = _passwordHasher.Hash(dto.Password)
            };
            _unitOfWork.Users.Add(user);
            await _unitOfWork.SaveChangesAsync();

        }
        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(request.Email);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }
                bool isPasswordValid =_passwordHasher.Verify(request.Password,user.PasswordHash);
                if (!isPasswordValid)
                {
                    throw new UnauthorizedAccessException("Invalid email or password.");
                }
                var token = _jwtTokenService.GenerateToken(user);
                return new LoginResponse
                {
                    Token = token,
                    Expiration= DateTime.UtcNow.AddHours(1),
                    Email = user.Email,
                    FullName=user.UserName
                };
            }
        }
    }


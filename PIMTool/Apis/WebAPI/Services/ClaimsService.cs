﻿using Application.Interfaces;
using System.Security.Claims;

namespace WebAPI.Services
{
    public class ClaimsService : IClaimsService
    {
        public ClaimsService(IHttpContextAccessor httpContextAccessor)
        {
            // todo implementation to get the current userId
            var Id = httpContextAccessor.HttpContext?.User?.FindFirstValue("userId");
            GetCurrentUserId = string.IsNullOrEmpty(Id) ? 0 : Convert.ToInt32(Id);
        }

        public int GetCurrentUserId { get; }
    }
}

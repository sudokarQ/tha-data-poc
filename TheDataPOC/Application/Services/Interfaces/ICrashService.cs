﻿namespace Application.Services.Interfaces
{
    using Domain.Models;

    using Microsoft.AspNetCore.Http;

    public interface ICrashService
	{
        public Task<ProcessingResult> DataProcessing(IFormFile file);
    }
}


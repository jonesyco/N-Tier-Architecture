﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using N_Tier.DataAccess.Persistence;
using System.Threading.Tasks;

namespace N_Tier.API.Middleware
{
    public class TranscationMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ILogger<TranscationMiddleware> _logger;

        public TranscationMiddleware(RequestDelegate next, ILogger<TranscationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, DatabaseContext databaseContext)
        {
            await using var transaction = await databaseContext.Database.BeginTransactionAsync();

            try
            {
                await _next(context);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
            }
        }
    }
}

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GetAPI_Response
{
    public class Worker : BackgroundService
    {

        private readonly CheckAndFillDBService _checkservice;
        private readonly ILogger<Worker> _logger;

        public Worker(CheckAndFillDBService checkservice,
            ILogger<Worker> logger) => (_checkservice, _logger) = (checkservice, logger);
       

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                var infoDictionary = await _checkservice.CheckForNewMatches();
                var logInfo = infoDictionary["Result"]=="Success"? $"Account updated: {infoDictionary["Update accounts count"]}\nMatches added: {infoDictionary["Added mathces count"]} \nUpdating done: {infoDictionary["Result"]}" : "Updating done: Failed";
                _logger.LogInformation(logInfo);
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}

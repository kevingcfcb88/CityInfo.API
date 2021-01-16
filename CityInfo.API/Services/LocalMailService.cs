using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Services
{
    public class LocalMailService : IMailService
    {
        private readonly IConfiguration _configuration;

        public LocalMailService(IConfiguration conf)
        {
            _configuration = conf ?? throw new ArgumentNullException(nameof(conf));
        }

        public void SendMail(string subject, string message)
        {
            Debug.WriteLine($"From: {_configuration["mailSettings:mailFromAddress"]} To: {_configuration["mailSettings:mailToAddress"]}");
            Debug.WriteLine($"Subject: {subject}");
            Debug.WriteLine($"Message: {message}");
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TestProject.DataAccess;
using TestProject.Models;

namespace TestProject.Services
{
    public class ResponseService
    {
        private readonly DataContext _context;

        public ResponseService(DataContext context)
        {
            _context = context;
        }

        public ResponseServiceInformation GetRequestTimeServiceInformation(string url, string accessKey)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Headers.Add("AccessKey", accessKey);

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var response = (HttpWebResponse)request.GetResponse();
            stopWatch.Stop();

            TimeSpan timeSpan = stopWatch.Elapsed;

            var responseInformation = new ResponseServiceInformation
            {
                NameService = url,
                RequestTime = timeSpan.Milliseconds
            };

            response.Close();

            return responseInformation;
        }

        public ResponseServiceInformation GetAvailableServiceInformation(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            var response = (HttpWebResponse)request.GetResponse();

            var responseInformation = new ResponseServiceInformation
            {
                IsAvailable = response.StatusCode == HttpStatusCode.OK ? true : false,
                NameService = url
            };

            response.Close();


            return responseInformation;
        }

        public async Task<int> GetCountFailuresPerHours(int hour, string name)
        {
            var result = await _context.ResponseServiceInformations
                .Where(information => information.IsAvailable == false
                && (DateTime.Now - information.CreatedDate).TotalHours <= hour
                && information.NameService == name)
                .ToListAsync();

            return result.Count;
        }

        public async Task<double> CheckForDiscrepancyPerHours(int hour, string name)
        {
            var s = _context.ResponseServiceInformations.ToList();

            var result = await _context.ResponseServiceInformations
               .Where(information => (DateTime.Now - information.CreatedDate).TotalHours <= hour
               && information.RequestTime != null
               && information.NameService == name)
               .ToListAsync();

            if (result.Count == 0)
                return 0;

            var averageRequestTime = result.Average(information => information.RequestTime.Value);

            return averageRequestTime;
        }

        public List<ResponseServiceInformation> GetAllInformation()
        {
            const string FIRST_URL = "http://ibonus.1c-work.net/api/ibonus/version";
            const string SECOND_URL = "http://iswiftdata.1c-work.net/api/refdata/version";
            const string THIRD_URL = "http://iswiftdata.1c-work.net/api/catalog/catalog";
            const string ACCESS_KEY = "test_05fc5ed1-0199-4259-92a0-2cd58214b29c";

            var availableServiceInformation = GetAvailableServiceInformation(FIRST_URL);
            var secondAvailableServiceInformation = GetAvailableServiceInformation(SECOND_URL);
            var requestTimeServiceInformation = GetRequestTimeServiceInformation(THIRD_URL, ACCESS_KEY);

            return new List<ResponseServiceInformation>()
            { availableServiceInformation,  secondAvailableServiceInformation, requestTimeServiceInformation };
        }

        public async Task AddResponseInformation()
        {
            var responseServiceInformations = GetAllInformation();

            _context.ResponseServiceInformations.AddRange(responseServiceInformations);

            await _context.SaveChangesAsync();
        }
    }
}

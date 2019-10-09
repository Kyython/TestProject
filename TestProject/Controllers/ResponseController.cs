using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestProject.Models;
using TestProject.Services;

namespace TestProject.Controllers
{
    public class ResponseController : Controller
    {
        private readonly ResponseService _responseService;

        public ResponseController(ResponseService responseService)
        {
            _responseService = responseService;
        }

        public async Task<ActionResult> ResponseView()
        {
            const int ONE_HOUR = 1;
            const int ONE_DAY = 24;

            var results = _responseService.GetAllInformation();
            var information = new List<ResponseInformationViewModel>();

            foreach (var item in results)
            {
                information.Add(new ResponseInformationViewModel
                {
                    IsAvailable = item.IsAvailable,
                    NameService = item.NameService,
                    RequestTime = item.RequestTime,
                    CountFailuresPerHour = await _responseService.GetCountFailuresPerHours(ONE_HOUR, item.NameService),
                    CountFailuresPerDay = await _responseService.GetCountFailuresPerHours(ONE_DAY, item.NameService),
                    DiscrepancyPerHour = await _responseService.CheckForDiscrepancyPerHours(ONE_HOUR, item.NameService),
                    DiscrepancyPerDay = await _responseService.CheckForDiscrepancyPerHours(ONE_DAY, item.NameService)
                });
            }


            return View(information);
        }
    }
}
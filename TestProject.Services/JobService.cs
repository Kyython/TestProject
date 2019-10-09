using Quartz;
using System.Threading.Tasks;
using TestProject.DataAccess;

namespace TestProject.Services
{
    class JobService : IJob
    {
        private readonly ResponseService _responseService;

        public JobService(ResponseService responseService)
        {
            _responseService = responseService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _responseService.AddResponseInformation();
        }
    }
}

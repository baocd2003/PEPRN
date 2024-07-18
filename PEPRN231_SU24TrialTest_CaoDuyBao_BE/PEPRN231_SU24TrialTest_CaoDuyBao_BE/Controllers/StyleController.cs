using Microsoft.AspNetCore.Mvc;
using Repository.UnitOfWork;

namespace PEPRN231_SU24TrialTest_CaoDuyBao_BE.Controllers
{
    [ApiController]
    [Route("api/styles")]
    public class StyleController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public StyleController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}

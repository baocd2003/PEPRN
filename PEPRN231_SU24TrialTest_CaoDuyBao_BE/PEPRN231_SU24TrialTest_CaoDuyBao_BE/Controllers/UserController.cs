using Microsoft.AspNetCore.Mvc;
using PEPRN231_SU24TrialTest_CaoDuyBao_BE.DTO;
using Repository.Models;
using Repository.UnitOfWork;

namespace PEPRN231_SU24TrialTest_CaoDuyBao_BE.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public IActionResult Login(UserRequest request)
        {
            try
            {
                UserAccount user = _unitOfWork.AccountRepository.GetQueryable().FirstOrDefault(u => u.UserEmail == request.Email && u.UserPassword == request.Password);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                String token = _unitOfWork.AccountRepository.GenerateToken(user);
                return Ok(token);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PEPRN231_SU24TrialTest_CaoDuyBao_BE.DTO;
using Repository.Models;
using Repository.UnitOfWork;

namespace PEPRN231_SU24TrialTest_CaoDuyBao_BE.Controllers
{
    [ApiController]
    [Route("api/paintings")]
    public class PaintingController : Controller
    {

        private IUnitOfWork _unitOfWork;
        public PaintingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult GetAllPaintings()
        {
            List<WatercolorsPainting> paintings = _unitOfWork.PaintingRepository.GetQueryable().Include(p => p.Style).ToList();
            List<PaintingDTO> results = new List<PaintingDTO>();
            foreach (var painting in paintings)
            {
                var dto = new PaintingDTO
                {
                    PaintingId = painting.PaintingId,
                    PaintingName = painting.PaintingName,
                    PaintingDescription = painting.PaintingDescription,
                    PaintingAuthor = painting.PaintingAuthor,
                    Price = painting.Price,
                    PublishYear = painting.PublishYear,
                    StyleId = painting.StyleId,
                    CreatedDate = painting.CreatedDate,
                    StyleName = painting.Style.StyleName
                };
                results.Add(dto);
            }
            return Ok(results);
        }

        [HttpGet("{id}")]
        public IActionResult GetPaintingById(string id)
        {
            WatercolorsPainting painting = _unitOfWork.PaintingRepository.GetQueryable().Include(p => p.Style).FirstOrDefault(p => p.PaintingId == id);
            if (painting == null)
            {
                return NotFound("Paint not found");
            }
            var dto = new PaintingDTO
            {
                PaintingId = painting.PaintingId,
                PaintingName = painting.PaintingName,
                PaintingDescription = painting.PaintingDescription,
                PaintingAuthor = painting.PaintingAuthor,
                Price = painting.Price,
                PublishYear = painting.PublishYear,
                CreatedDate = painting.CreatedDate,
                StyleId = painting.StyleId,
                StyleName = painting.Style.StyleName
            };
            return Ok(dto);
        }

        [HttpGet("/search")]
        public IActionResult SearchPainting([FromQuery] string? paintingAuthor, [FromQuery] int? publishYear)
        {
            var query = _unitOfWork.PaintingRepository.GetQueryable().Include(p => p.Style).AsQueryable();

            if (!string.IsNullOrEmpty(paintingAuthor))
            {
                query = query.Where(p => p.PaintingAuthor.Contains(paintingAuthor));
            }

            if (publishYear.HasValue)
            {
                query = query.Where(p => p.PublishYear == publishYear.Value);
            }

            var paintings = query.Select(p => new PaintingDTO
            {
                PaintingId = p.PaintingId,
                PaintingName = p.PaintingName,
                PaintingDescription = p.PaintingDescription,
                PaintingAuthor = p.PaintingAuthor,
                Price = p.Price,
                PublishYear = p.PublishYear,
                CreatedDate = p.CreatedDate,
                StyleId = p.StyleId,
                StyleName = p.Style.StyleName
            }).ToList();

            return Ok(paintings);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CreatePaint([FromBody] PaintingRequest paintingDto)
        {
            if (paintingDto == null ||
             string.IsNullOrWhiteSpace(paintingDto.PaintingName) ||
             string.IsNullOrWhiteSpace(paintingDto.PaintingDescription) ||
             string.IsNullOrWhiteSpace(paintingDto.PaintingAuthor) ||
             string.IsNullOrWhiteSpace(paintingDto.StyleId) ||
             paintingDto.Price < 0 ||
             paintingDto.PublishYear < 1000)
            {
                return BadRequest("Invalid input data");
            }

            if (!ValidatePaintingName(paintingDto.PaintingName))
            {
                return BadRequest("Invalid PaintingName format");
            }
            var painting = new WatercolorsPainting
            {
                PaintingId = Guid.NewGuid().ToString(),
                PaintingName = paintingDto.PaintingName,
                PaintingDescription = paintingDto.PaintingDescription,
                PaintingAuthor = paintingDto.PaintingAuthor,
                Price = paintingDto.Price,
                PublishYear = paintingDto.PublishYear,
                CreatedDate = DateTime.Now,
                Style = _unitOfWork.StyleRepository.GetQueryable().FirstOrDefault(s => s.StyleId == paintingDto.StyleId)
            };

            _unitOfWork.PaintingRepository.Add(painting);
            return GetPaintingById(painting.PaintingId);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdatePainting(PaintingDTO paintingDto)
        {
            if (paintingDto == null ||
             string.IsNullOrWhiteSpace(paintingDto.PaintingId) ||
             string.IsNullOrWhiteSpace(paintingDto.PaintingName) ||
             string.IsNullOrWhiteSpace(paintingDto.PaintingDescription) ||
             string.IsNullOrWhiteSpace(paintingDto.PaintingAuthor) ||
             string.IsNullOrWhiteSpace(paintingDto.StyleId) ||
             paintingDto.Price < 0 ||
             paintingDto.PublishYear < 1000)
            {
                return BadRequest("Price must be > 0 and publish year must > 1000");
            }

            if (!ValidatePaintingName(paintingDto.PaintingName))
            {
                return BadRequest("Invalid PaintingName format");
            }

            var painting = _unitOfWork.PaintingRepository.GetQueryable().Include(p => p.Style).FirstOrDefault(p => p.PaintingId == paintingDto.PaintingId);
            painting.PaintingName = paintingDto.PaintingName;
            painting.PaintingDescription = paintingDto.PaintingDescription;
            painting.PaintingAuthor = paintingDto.PaintingAuthor;
            painting.Price = paintingDto.Price;
            painting.PublishYear = paintingDto.PublishYear;
            painting.Style = _unitOfWork.StyleRepository.GetQueryable().FirstOrDefault(s => s.StyleId == paintingDto.StyleId);

            if (painting == null)
            {
                return NotFound("Paint not found");
            }
            _unitOfWork.PaintingRepository.Update(painting);
            return GetPaintingById(painting.PaintingId);

        }

        private bool ValidatePaintingName(string paintingName)
        {
            var words = paintingName.Split(' ');
            foreach(var word in words) {
                if (string.IsNullOrWhiteSpace(word) || !char.IsUpper(word[0]) || !word.Skip(1).All(char.IsLetterOrDigit))
                {
                    return false;
                }
            }
            return true;
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeletePainting(string id)
        {
            var painting = _unitOfWork.PaintingRepository.GetQueryable().FirstOrDefault(p => p.PaintingId == id);
            if (painting == null)
            {
                return NotFound("Paint not found");
            }
            _unitOfWork.PaintingRepository.Delete(id);
            return Ok("Delete successfully");
        }

    }
}

using FileManagement.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FileManagement.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ClassificationsController : ControllerBase
    {
        private readonly IClassificationService _classificationService;

        public ClassificationsController(IClassificationService classificationService)
        {
            _classificationService = classificationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetClassifications()
        {
            var classifications = await _classificationService.GetClassificationsAsync();
            return Ok(classifications);
        }
    }
} 
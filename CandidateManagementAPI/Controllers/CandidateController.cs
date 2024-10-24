using CandidateManagementAPI.Models;
using CandidateManagementAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CandidateManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateController : ControllerBase
    {
        private readonly ICandidateService _candidateService;

        public CandidateController(ICandidateService candidateService)
        {
            _candidateService = candidateService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrUpdateCandidate([FromBody] Candidate candidate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _candidateService.AddOrUpdateCandidateAsync(candidate);
            return Ok(result);
        }
    }
}

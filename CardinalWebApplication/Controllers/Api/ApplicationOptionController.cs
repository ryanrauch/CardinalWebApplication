using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardinalLibrary.DataContracts;
using CardinalWebApplication.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CardinalWebApplication.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/ApplicationOption")]
    [Authorize]
    public class ApplicationOptionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApplicationOptionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ApplicationOption
        [HttpGet]
        public async Task<IActionResult> GetLatestApplicationOption()
        {
            var option = await _context.ApplicationOptions
                                       .OrderByDescending(a => a.OptionsDate)
                                       .FirstOrDefaultAsync();
            if (option == null)
            {
                return NoContent();
            }
            var contract = new ApplicationOptionContract()
            {
                EndUserLicenseAgreementSource = option.EndUserLicenseAgreementSource,
                TermsConditionsSource = option.TermsConditionsSource,
                PrivacyPolicySource = option.PrivacyPolicySource,
                DataTimeWindow = option.DataTimeWindow,
                Version = option.Version,
                VersionMajor = option.VersionMajor,
                VersionMinor = option.VersionMinor
            };
            return Ok(contract);
        }
    }
}
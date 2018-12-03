using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace _101mngr.WebApp.Controllers
{
    [Route("api/[controller]")]
    public class DictionariesController : Controller
    {
        [HttpGet("countries")]
        public IActionResult GetCountries()
        {
            return Ok(CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                .Select(culture => new RegionInfo(culture.Name))
                .Select(x => new {Name = x.DisplayName, Code = x.TwoLetterISORegionName}));
        }

        [HttpGet("currencies")]
        public IActionResult GetCurrencies()
        {
            return Ok(new object[0]);
        }
    }
}
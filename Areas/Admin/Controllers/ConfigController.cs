using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using IntricomMVC.Models;
using IntricomMVC.Services;
using IntricomMVC.Validations;
using JW;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IntricomMVC.Areas.Admin.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    //[Authorize]
    [Area("Admin")]
    public class ConfigController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IConfigurationService _configurationService;

        public ConfigController(
                                IMapper mapper,
                                IConfigurationService configurationService
            )
        {
            _mapper = mapper;
            _configurationService = configurationService;
        }

        [Route("~/admin/Config/index")]
        public async Task<ActionResult> Index(int numPage = 1)
        {
            var documentList = await _configurationService.GetConfigurationsAsync();
            ViewBag.ShowNewConfiguration = false;
            if (documentList.Count() == 0) ViewBag.ShowNewConfiguration = true;
            return View(documentList);
        }


        [HttpGet]
        [Route("Admin/Config/GetConfiguration")]
        public async Task<IActionResult> GetConfiguration(int id)
        {
            var item = await _configurationService.GetConfigurationAsync(id);
            
            return Json(item);
        }

        [HttpPost]
        [Route("Admin/Config/SaveConfiguration")]
        public async Task<IActionResult> SaveConfiguration(Configuration configuration)
        {
            ResponseObject rs = new(false, "Error, intentelo en breve!!!");
            try
            {
                ConfigurationValidator oValidator = new();
                ValidationResult result = oValidator.Validate(configuration);
                if (result.IsValid)
                {
                    if (configuration.Id == 0)
                    {
                        var newConfig = await _configurationService.PostConfigurationAsync(configuration);
                        if (newConfig != null) rs.success = true;
                    }else rs.success = await _configurationService.PutConfigurationAsync(configuration);
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        rs.message += item.ErrorMessage.Replace("'", "") + "<br/>";
                    }
                }
            }
            catch (Exception) { rs.message = "Erroraco"; }
            return Json(rs);
        }
    }
}

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
    public class ClientController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IClientService _clientService;

        public ClientController(
                                IMapper mapper,
                                IClientService clientService
            )
        {
            _mapper = mapper;
            _clientService = clientService;
        }

        [Route("~/admin/client/index")]
        [Route("~/admin/client/index/{numPage}")]
        public async Task<ActionResult> Index(int numPage = 1)
        {
            //var documentList = await _context.Clients.ToListAsync();
            var documentList = await _clientService.GetClientsAsync();
            var pag = new PaginationDoc<Client>
            {
                TotalItems = documentList.ToList().Count,
                PageSize = 25,
                MaxPages = 3
            };
            pag.Pager = new Pager(documentList.ToList().Count, numPage, pag.PageSize, pag.MaxPages);
            pag.Documents = documentList.Skip((pag.Pager.CurrentPage - 1) * pag.Pager.PageSize).Take(pag.Pager.PageSize);
            return View(pag);
        }


        [HttpGet]
        [Route("Admin/Client/GetClient")]
        public async Task<IActionResult> GetClient(int id)
        {
            var item = await _clientService.GetClientAsync(id);
            return Json(item);
        }

        [HttpPost]
        [Route("Admin/Client/SaveClient")]
        public async Task<IActionResult> SaveClient(Client client)
        {
            ResponseObject rs = new(false, "Error, intentelo en breve!!!");
            try
            {
                string basePath = @"C:\IntricomDB";
                FileSystemRepository.SaveEntityToFileSystem(client, basePath);


                ClientValidator oValidator = new();
                ValidationResult result = oValidator.Validate(client);
                if (result.IsValid)
                {
                    var itemDTO = _mapper.Map<ClientDTO>(client);
                    if (client.Id == 0)
                    {
                        var newClient = await _clientService.PostClientAsync(itemDTO);
                        if (newClient != null) rs.success = true;
                    }else rs.success = await _clientService.PutClientAsync(1, itemDTO);
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

        [HttpGet]
        [Route("Admin/Client/RemoveClient")]
        public async Task<IActionResult> RemoveClient(int id)
        {
            ResponseObject rs = new(false, "Error, intentelo en breve!!!");
            rs.success = await _clientService.DeleteClientAsync(id);
            return Json(rs);
        }
    }
}

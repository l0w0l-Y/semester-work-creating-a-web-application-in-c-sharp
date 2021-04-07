using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Pages.Transfer
{
    public class DetailsByUserFromModel : PageModel
    {
        private readonly IPaymentService _paymentService;
        public IEnumerable<TransferModel> TransferModels { get; set; }

        public DetailsByUserFromModel(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task<ActionResult> OnGetAsync(int userId)
        {
            TransferModels = await _paymentService.GetTransfersByUserFrom(userId);
            return Page();
        }
    }
}
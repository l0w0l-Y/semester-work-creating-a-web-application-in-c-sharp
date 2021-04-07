using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Pages.BankAccount
{
    public class IndexModel : PageModel
    {
        private readonly IPaymentService _paymentService;
        public IEnumerable<BankAccountModel> BankAccountModels { get; set; }

        public IndexModel(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task<ActionResult> OnGetAsync()
        {
            BankAccountModels = await _paymentService.GetBankAccounts();
            return Page();
        }
    }
}
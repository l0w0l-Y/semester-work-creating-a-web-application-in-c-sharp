﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Pages
{
    public class CompanyProfile : PageModel
    {
        private readonly IDeveloperService _developerService;

        public CompanyProfile(IDeveloperService developerService)
        {
            _developerService = developerService;
        }

        public CompanyModel CompanyModel { get; set; }
        
        public async Task<ActionResult> OnGetAsync(int id)
        {
            CompanyModel = await _developerService.GetCompany(id);
            CompanyModel.Tags = await _developerService.GetTags(CompanyModel);
            CompanyModel.Users = await _developerService.GetCompanyUsers(id);
            CompanyModel.Projects = await _developerService.GetCompanyProjects(id);
            return Page();
        }
    }
}
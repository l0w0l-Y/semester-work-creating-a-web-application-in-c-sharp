﻿using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.DTOs;
using WebApp.Models.Developer;
using WebApp.Pages;

namespace WebApp.Services.Developer
{
    public interface IDeveloperService
    {
        //Users
        Task<IEnumerable<UserModel>> GetUsers();
        Task<IEnumerable<UserModel>> GetUsersByName(string name);
        Task<UserModel> GetUser(int id);
        Task<IEnumerable<ProjectModel>> GetUserProjects(int userId);
        Task<IEnumerable<CompanyModel>> GetUserCompanies(int userId);
        Task CreateUser(UserModel user);
        Task UpdateUser(UserModel user);

        //Projects
        Task<IEnumerable<ProjectModel>> GetProjects();
        Task<IEnumerable<ProjectModel>> GetProjectsByName(string name);
        Task<ProjectModel> GetProject(int id);
        Task<ProjectModel> GetProject(string name);
        Task<IEnumerable<UserModel>> GetProjectUsers(int projectId);
        Task<CompanyModel> GetProjectCompany(int projectId);
        Task<string> CreateProject(ProjectForm projectForm);
        Task UpdateProject(ProjectModel project);
        Task AddUserToProject(int userId, int projectId);

        //Companies
        Task<IEnumerable<CompanyModel>> GetCompanies();
        Task<IEnumerable<CompanyModel>> GetCompaniesByName(string name);
        Task<CompanyModel> GetCompany(int id);
        Task<CompanyModel> GetCompany(string name);
        Task<IEnumerable<UserModel>> GetCompanyUsers(int companyId);
        Task<IEnumerable<ProjectModel>> GetCompanyProjects(int companyId);
        Task<string> CreateCompany(CompanyForm companyForm);
        Task UpdateCompanyName(CompanyModel company);
        Task AddUserToCompany(int userId, int companyId);
        Task UpdateCoordinates(CoordinatesDto dto);

        //Tags
        Task<IEnumerable<TagModel>> GetTags(ICreator creator);
    }
}
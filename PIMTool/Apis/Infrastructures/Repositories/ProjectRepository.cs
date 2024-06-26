using System.Formats.Asn1;
using Application.Interfaces.Repositories;
using Application.ViewModels.EmployeeViewModels;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        private readonly PimContext _context;
        public ProjectRepository(PimContext pimContext) : base(pimContext)
        {
            _context = pimContext;
        }

        public async Task<IEnumerable<Employee>> GetEmployeInProjectAsync(int projectId)
        {
            var project = await _context.Projects
                            .Include(p => p.ProjectEmployees)
                            .ThenInclude(pe => pe.Employee)
                            .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project != null)
            {
                var employeeInProject = project.ProjectEmployees
                                        .Select(pe => pe.Employee);

                return employeeInProject;
            }
            return null;
        }

        public bool IsProjectNumberExists(int projectNumber)
        {
            IQueryable<Project> query = base.Get();
            var isExist = query.Any(x => x.ProjectNumber == projectNumber);

            return isExist;
        }

        public async Task<IEnumerable<Project>> FilterProjectsAsync(string? searchTerm, StatusEnum? status)
        {
            IQueryable<Project> query = _context.Projects;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(x => x.Name.Contains(searchTerm) ||
                                        x.ProjectNumber.ToString().Contains(searchTerm) ||
                                        x.Customer.Contains(searchTerm));
            }

            if (status.HasValue)
            {
                query = query.Where(x => x.Status == status.Value);
            }

            return await query
                            .OrderBy(x => x.ProjectNumber)
                            .ToListAsync();
        }
    }
}
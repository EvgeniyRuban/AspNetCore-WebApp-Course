﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Timesheets.Entities;
using Timesheets.DataBase.Repositories.Interfaces;

namespace Timesheets.DataBase.Repositories
{
    public class EmployeesRepository : IEmployeesRepository
    {
        private readonly AppDbContext _context;

        public EmployeesRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Employee> GetAsync(Guid id, CancellationToken cancelToken)
        {
            return await Task.Run(() =>
            {
                try
                {
                    return _context.Employees
                                    .FirstOrDefault(e => e.Id == id && e.IsDeleted == false);
                }
                catch
                {
                    return null;
                }
            }, cancelToken);
        }
        public async Task<List<Employee>> GetRangeAsync(
            int skip, 
            int take, 
            CancellationToken cancelToken)
        {
            return await Task.Run(() =>
            {
                try
                {
                    return _context.Employees.Skip(skip).Take(take).ToList();
                }
                catch
                {
                    return null;
                }
            }, cancelToken);
        }
        public async Task AddAsync(Employee employee, CancellationToken cancelToken)
        {
            await _context.Employees.AddAsync(employee, cancelToken);
            await _context.SaveChangesAsync(cancelToken);
        }
        public async Task UpdateAsync(Employee newEmployee, CancellationToken cancelToken)
        {
            var employee = await Task.Run(() =>
            {
                return _context.Employees
                                .FirstOrDefault(e => e.Id == newEmployee.Id && e.IsDeleted == false);
            }, cancelToken);
            if (employee != null)
            {
                employee.UserId = newEmployee.UserId;
            }
            await _context.SaveChangesAsync(cancelToken);
        }
        public async Task DeleteAsync(Guid id, CancellationToken cancelToken)
        {
            var employeeToDelete = await Task.Run(() =>
            {
                return _context.Employees
                                .FirstOrDefault(e => e.Id == id && e.IsDeleted == false);
            }, cancelToken);

            if (employeeToDelete != null)
            {
                employeeToDelete.IsDeleted = true;
            }

            await _context.SaveChangesAsync(cancelToken);
        }
    }
}

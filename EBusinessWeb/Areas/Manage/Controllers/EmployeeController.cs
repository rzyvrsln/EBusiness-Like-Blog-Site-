﻿using EBusinessData.DAL;
using EBusinessEntity.Entities;
using EBusinessService.Services.Abstraction;
using EBusinessViewModel.Entities.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EBusinessWeb.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService employeeService;
        private readonly AppDbContext context;

        public EmployeeController(AppDbContext context, IEmployeeService service)
        {
            this.context = context;
            this.employeeService = service;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            return View(await employeeService.PaginationForEmployeeAsync(page));
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            ViewBag.Positions = new SelectList(context.Positions, nameof(Position.Id), nameof(Position.Name));
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeVM employeeVM)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Positions = new SelectList(context.Positions, nameof(Position.Id), nameof(Position.Name));
                return View(nameof(Add));
            }
            await employeeService.AddEmployeeAsync(employeeVM);
            return RedirectToAction(nameof(Add));
        }

        [HttpGet]
        public async Task<IActionResult> Remove(int id)
        {
            if(!ModelState.IsValid) BadRequest();
            await employeeService.RemoveEmployeeAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!ModelState.IsValid) return BadRequest();
            ViewBag.Positions = new SelectList(context.Positions, nameof(Position.Id), nameof(Position.Name));
            return View(await employeeService.EditEmployeeAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdateEmployeeVM employeeVM)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Positions = new SelectList(context.Positions, nameof(Position.Id), nameof(Position.Name));
                return View();
            }

            await employeeService.EditPostEmployeeAsync(id, employeeVM);
            return RedirectToAction(nameof(Index));
        }

    }
}

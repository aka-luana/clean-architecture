﻿using CleanArchMvc.Application.DTOs;
using CleanArchMvc.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchMvc.WebUI.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetCategories();

            return View(categories);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryDTO category)
        {
            if(ModelState.IsValid)
            {
                await _categoryService.Add(category);

                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if(id is null)
                return NotFound();
            
            var categoryDto = await _categoryService.GetById(id);

            if (categoryDto is null)
                return NotFound();

            return View(categoryDto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryDTO category)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    await _categoryService.Update(category);
                }
                catch (System.Exception)
                {
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
                return NotFound();

            var categoryDto = await _categoryService.GetById(id);

            if (categoryDto is null)
                return NotFound();
            
            return View(categoryDto);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.Remove(id);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
                return NotFound();
            
            var categoryDto = await _categoryService.GetById(id);

            if (categoryDto is null)
                return NotFound();

            return View(categoryDto);
        }
    }
}

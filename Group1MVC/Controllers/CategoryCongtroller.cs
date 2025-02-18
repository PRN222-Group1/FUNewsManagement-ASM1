using BusinessServiceLayer.DTOs;
using BusinessServiceLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Specifications.Categories;

namespace Group1MVC.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoriesService _categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        public async Task<IActionResult> Index(CategorySpecParams specParams)
        {
            specParams.Status = null;

            var categories = await _categoriesService.GetCategoriesAsync(specParams);

            var cleanSpec = new CategorySpecParams();
            cleanSpec.Status = null;

            var parentCategories = await _categoriesService.GetCategoriesAsync(cleanSpec);

            ViewData["CurrentFilter"] = specParams.Search;

            ViewBag.Categories = parentCategories;
            ViewData["SelectedCategory"] = specParams.CategoryId;

            return View(categories);
        }

        public async Task<IActionResult> Create()
        {
            var specParams = new CategorySpecParams();
            specParams.Status = true;
            var parentCategories = await _categoriesService.GetCategoriesAsync(specParams);
            ViewBag.Categories = parentCategories;
            ViewData["Action"] = "Create";

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("CategoryName,CategoryDescription,CategoryId")] CategoryToAddOrUpdateDTO category)
        {
            var result = false;

            category.Status = true;

            if (ModelState.IsValid)
            {
                result = await _categoriesService.CreateCategoryAsync(category);
            }

            if (result)
            {
                TempData["SuccessMessage"] = "Create Category Successfully!";
                return RedirectToAction("Index");
            }

            return View(category);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var specParams = new CategorySpecParams();
            specParams.Status = true;
            var parentCategories = await _categoriesService.GetCategoriesAsync(specParams);

            var category = await _categoriesService.GetCategoryByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            ViewBag.Categories = parentCategories;
            ViewData["Action"] = "Edit";
            ViewBag.SelectedCategoryId = category.ParentCategoryId.Value;

            var categoryToUpdate = new CategoryToAddOrUpdateDTO
            {
                CategoryId = id,
                CategoryName = category.CategoryName,
                CategoryDescription = category.CategoryDescription,
                Status = category.IsActive
            };

            return View(categoryToUpdate); 
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryName,CategoryDescription,CategoryId,Status")] CategoryToAddOrUpdateDTO category)
        {
            var result = false;
            if (ModelState.IsValid)
            {
                result = await _categoriesService.EditCategoryAsync(id, category);
            }

            if (result)
            {
                TempData["SuccessMessage"] = "Cập nhật danh mục thành công!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Cập nhật danh mục thất bại!";
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoriesService.DeleteCategoryAsync(id);

            if (result)
            {
                TempData["SuccessMessage"] = "Delete successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = result;
            }

            return View();
        }
    }
}
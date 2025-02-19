using BusinessServiceLayer.DTOs;
using BusinessServiceLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Specifications.Categories;

namespace Group1MVC.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoriesService;

        public CategoriesController(ICategoryService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        [Authorize(Roles = "Staff")]
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

        [Authorize(Roles = "Staff")]
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
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Create([Bind("CategoryName,CategoryDescription,CategoryId")] CategoryToAddOrUpdateDTO category)
        {
            var result = false;

            // Set status to true by default
            category.Status = true;

            if (!ModelState.IsValid)
            {
                // Reload the categories in case of validation errors
                var specParams = new CategorySpecParams { Status = true };
                var parentCategories = await _categoriesService.GetCategoriesAsync(specParams);
                ViewBag.Categories = parentCategories;

                // Return view with validation errors
                return View(category);
            }

            // Proceed with category creation if valid
            result = await _categoriesService.CreateCategoryAsync(category);

            if (result)
            {
                TempData["SuccessMessage"] = "Create Category Successfully!";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Error creating Category!";
            return View(category);
        }

        [HttpGet]
        [Authorize(Roles = "Staff")]
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
                CategoryId = category.ParentCategoryId.Value,
                CategoryName = category.CategoryName,
                CategoryDescription = category.CategoryDescription,
                Status = category.IsActive
            };

            return View(categoryToUpdate); 
        }

        [HttpPost]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryName,CategoryDescription,CategoryId,Status")] CategoryToAddOrUpdateDTO category)
        {
            var result = false;

            // Validate the model
            if (!ModelState.IsValid)
            {
                // Reload the categories list if validation fails
                var specParams = new CategorySpecParams { Status = true };
                var parentCategories = await _categoriesService.GetCategoriesAsync(specParams);
                ViewBag.Categories = parentCategories;
                ViewData["Action"] = "Edit";

                // Return the view with validation errors
                return View(category);
            }

            // Proceed with updating the category if the model is valid
            result = await _categoriesService.EditCategoryAsync(id, category);

            if (result)
            {
                TempData["SuccessMessage"] = "Update successfully!";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = "Update failed!";
            return View(category); // Return the view with the current category data
        }


        [HttpPost]
        [Authorize(Roles = "Staff")]
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
                TempData["ErrorMessage"] = "Delete failed due to the category is being applied to one or more news articles!";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
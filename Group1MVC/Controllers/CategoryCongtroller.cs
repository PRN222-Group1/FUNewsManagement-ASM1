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

        public async Task<IActionResult> Index()
        {
            var categories = await _categoriesService.getAll();
            return View(categories);
        }

        public async Task<IActionResult> NewsByCategory(int id)
        {
            var newsArticles = await _categoriesService.getNewsArticlesByID(id);
            return View(newsArticles);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EditCategoriesentity model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _categoriesService.createCategory(model);
            TempData["SuccessMessage"] = "Thêm danh mục thành công!";

            return RedirectToAction("Index"); // Load lại danh sách
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var categoryList = await _categoriesService.getAll();
            var category = categoryList.FirstOrDefault(c => c.ParentCategoryId == id);

            if (category == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy danh mục!";
                return RedirectToAction("Index");
            }

            var editModel = new EditCategoriesentity
            {
                ParentCategoryId = id,
                CategoryName = category.CategoryName ?? "Chưa có tên",
                CategoryDescription = category.CategoryDescription ?? "Không có mô tả"
            };

            return View(editModel); 
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditCategoriesentity model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string result = await _categoriesService.editCategory(id, model);

            if (result == "Edit category successfully")
            {
                TempData["SuccessMessage"] = "Cập nhật danh mục thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = "Cập nhật danh mục thất bại!";
            }

            return RedirectToAction("Index"); 
        }




        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoriesService.deleteCategory(id);

            if (result == "Delete Category successfully")
            {
                TempData["SuccessMessage"] = "Xóa danh mục thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = result;
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Search(string keyword)
        {
            var result = await _categoriesService.searchCategory(keyword);
            return View("Index", result);
        }
    }
}
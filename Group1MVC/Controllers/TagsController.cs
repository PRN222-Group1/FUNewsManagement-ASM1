using BusinessServiceLayer.DTOs;
using BusinessServiceLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Group1MVC.Controllers
{
    public class TagsController : Controller
    {
        private readonly ITagService _tagsService;

        public TagsController(ITagService tagsService)
        {
            _tagsService = tagsService;
        }

        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Index(string? search)
        {
            var tags = await _tagsService.GetTagsAsync(search);

            ViewData["CurrentFilter"] = search;

            return View(tags);
        }

        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Create()
        {
            ViewData["Action"] = "Create";

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Create([Bind("TagName,Note")] TagToAddOrUpdateDTO tag)
        {
            var result = false;

            if (!ModelState.IsValid)
            {
                // Return view with validation errors
                return View(tag);
            }

            // Proceed with category creation if valid
            result = await _tagsService.CreateTagAsync(tag);

            if (result)
            {
                TempData["SuccessMessage"] = "Create Tag Successfully!";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Error creating Tag!";
            return View(tag);
        }



        [HttpGet]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Edit(int id)
        {
            var tag = await _tagsService.GetTagByIdAsync(id);

            if (tag == null)
            {
                return NotFound();
            }

            ViewData["Action"] = "Edit";

            var tagToUpdate = new TagToAddOrUpdateDTO
            {
                TagName = tag.TagName,
                Note = tag.Note
            };

            return View(tagToUpdate);
        }

        [HttpPost]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Edit(int id, [Bind("TagName,Note")] TagToAddOrUpdateDTO tag)
        {
            var result = false;

            // Validate the model
            if (!ModelState.IsValid)
            {
                ViewData["Action"] = "Edit";

                // Return the view with validation errors
                return View(tag);
            }

            // Proceed with updating the category if the model is valid
            result = await _tagsService.UpdateTagAsync(id, tag);

            if (result)
            {
                TempData["SuccessMessage"] = "Update successfully!";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = "Update failed!";
            return View(tag);
        }
    }
}

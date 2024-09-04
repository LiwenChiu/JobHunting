using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JobHunting.Areas.Admins.Models;
using JobHunting.Areas.Admins.ViewModels;

namespace JobHunting.Areas.Admins.Controllers
{
    [Area("Admins")]
    public class TagManagementController : Controller
    {
        private readonly DuckAdminsContext _context;

        public TagManagementController(DuckAdminsContext context)
        {
            _context = context;
        }

        // GET: Admins/TagManagement
        public async Task<IActionResult> Index()
        {
            return View();
        }

        // GET: Admins/TagManagement/IndexTags
        public JsonResult IndexTags()
        {
            return Json(_context.Tags.Select(t => new TagsViewModel
            {
                TagID = t.TagID,
                TagClassID = t.TagClassID,
                TagName = t.TagName,
            }));
        }

        // GET: Admins/TagManagement/IndexTagClasses
        public JsonResult IndexTagClasses()
        {
            return Json(_context.TagClasses.Where(t => t.TagClassID > 0).Select(t => new TagClassesViewModel
            {
                TagClassID = t.TagClassID,
                TagClassName = t.TagClassName,
            }));
        }

        //POST: Admins/TagManagement/CreateTag
       [HttpPost]
       [ValidateAntiForgeryToken]
        public async Task<string> CreateTag([Bind("TagID,TagClassID,TagName")] TagsViewModel tvm)
        {
            if (ModelState.IsValid)
            {
                Tag tadd = new Tag
                {
                    TagClassID = tvm.TagClassID,
                    TagName = tvm.TagName,
                };

                _context.Tags.Add(tadd);
                await _context.SaveChangesAsync();
                return "新增標籤成功";
            }
            return "新增標籤錯誤";
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<string> DeleteTags(Array arr)
        {
            if (arr.Length == 0)
            {
                return "未選擇標籤";
            }

            TagsViewModel[] delarr = new TagsViewModel[arr.Length];
            for (int i = 0; i < arr.Length; i++) { 
                    
            }

            return "標籤刪除失敗";
        }

        //    // GET: Admins/TagManagement/Details/5
        //    public async Task<IActionResult> Details(int? id)
        //    {
        //        if (id == null)
        //        {
        //            return NotFound();
        //        }

        //        var tag = await _context.Tags
        //            .Include(t => t.TagClass)
        //            .FirstOrDefaultAsync(m => m.TagID == id);
        //        if (tag == null)
        //        {
        //            return NotFound();
        //        }

        //        return View(tag);
        //    }

        //    // GET: Admins/TagManagement/Edit/5
        //    public async Task<IActionResult> Edit(int? id)
        //    {
        //        if (id == null)
        //        {
        //            return NotFound();
        //        }

        //        var tag = await _context.Tags.FindAsync(id);
        //        if (tag == null)
        //        {
        //            return NotFound();
        //        }
        //        ViewData["TagClassID"] = new SelectList(_context.TagClasses, "TagClassID", "TagClass1", tag.TagClassID);
        //        return View(tag);
        //    }

        //    // POST: Admins/TagManagement/Edit/5
        //    // To protect from overposting attacks, enable the specific properties you want to bind to.
        //    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public async Task<IActionResult> Edit(int id, [Bind("TagID,TagClassID,TagName")] Tag tag)
        //    {
        //        if (id != tag.TagID)
        //        {
        //            return NotFound();
        //        }

        //        if (ModelState.IsValid)
        //        {
        //            try
        //            {
        //                _context.Update(tag);
        //                await _context.SaveChangesAsync();
        //            }
        //            catch (DbUpdateConcurrencyException)
        //            {
        //                if (!TagExists(tag.TagID))
        //                {
        //                    return NotFound();
        //                }
        //                else
        //                {
        //                    throw;
        //                }
        //            }
        //            return RedirectToAction(nameof(Index));
        //        }
        //        ViewData["TagClassID"] = new SelectList(_context.TagClasses, "TagClassID", "TagClass1", tag.TagClassID);
        //        return View(tag);
        //    }

        //    // GET: Admins/TagManagement/Delete/5
        //    public async Task<IActionResult> Delete(int? id)
        //    {
        //        if (id == null)
        //        {
        //            return NotFound();
        //        }

        //        var tag = await _context.Tags
        //            .Include(t => t.TagClass)
        //            .FirstOrDefaultAsync(m => m.TagID == id);
        //        if (tag == null)
        //        {
        //            return NotFound();
        //        }

        //        return View(tag);
        //    }

        //    // POST: Admins/TagManagement/Delete/5
        //    [HttpPost, ActionName("Delete")]
        //    [ValidateAntiForgeryToken]
        //    public async Task<IActionResult> DeleteConfirmed(int id)
        //    {
        //        var tag = await _context.Tags.FindAsync(id);
        //        if (tag != null)
        //        {
        //            _context.Tags.Remove(tag);
        //        }

        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }

        //    private bool TagExists(int id)
        //    {
        //        return _context.Tags.Any(e => e.TagID == id);
        //    }
    }
}

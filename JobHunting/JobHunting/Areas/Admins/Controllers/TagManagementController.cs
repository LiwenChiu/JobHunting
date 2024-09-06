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

        // GET: Admins/TagManagement/IndexTagManagement
        //public JsonResult IndexTagManagement()
        //{
        //    return Json(_context.Tags.Include(t => t.TagClass).Select(t => new TagManagementViewModel
        //    {
        //        TagClassID = t.TagClassID,
        //        TagClassName = t.TagClass.TagClassName,
        //        TagID = t.TagID,
        //        TagName = t.TagName,
        //    }));
        //}

        // GET: Admins/TagManagement/IndexTags
        public JsonResult IndexTags()
        {
            return Json(_context.Tags.Select(t => new
            {
                TagID = t.TagID,
                TagClassID = t.TagClassID,
                TagName = t.TagName,
            }));
        }

        // GET: Admins/TagManagement/IndexTagClasses
        public JsonResult IndexTagClasses()
        {
            return Json(_context.TagClasses.Where(t => t.TagClassID > 0).Select(t => new
            {
                TagClassID = t.TagClassID,
                TagClassName = t.TagClassName,
            }));
        }

        //POST: Admins/TagManagement/CreateTag
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<Array> CreateTag([FromBody][Bind("TagID,TagClassID,TagName")] TagsViewModel tvm)
        {
            string[] returnStatus = new string[2];

            if (ModelState.IsValid)
            {
                Tag tadd = new Tag
                {
                    TagClassID = tvm.TagClassID,
                    TagName = tvm.TagName,
                };

                var arr = await _context.Tags.Where(t => t.TagClassID == tadd.TagClassID).Where(t => t.TagName == tadd.TagName).ToArrayAsync();
                if(arr.Length > 0)
                {
                    returnStatus = ["此類型已存在相同標籤","失敗"];
                    return returnStatus;
                }

                returnStatus = ["新增標籤成功", "成功"];
                _context.Tags.Add(tadd);
                await _context.SaveChangesAsync();
                return returnStatus;
            }

            returnStatus = ["新增標籤失敗", "失敗"];
            return returnStatus;
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<Array> DeleteTags([FromBody]int[] arr)
        {
            string[] returnStatus = new string[2];

            if (arr.Length == 0)
            {
                returnStatus = ["未選擇標籤", "失敗"];
                return returnStatus;
            }

            for (int i = 0; i < arr.Length; i++)
            {
                int id = arr[i];
                var tag = await _context.Tags.FindAsync(id);
                if(tag != null)
                {
                    _context.Tags.Remove(tag);
                }

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateException ex){
                    returnStatus = [$"刪除ID為{arr[i]}的關聯標籤失敗!", "失敗"];
                    return returnStatus;
                }
            }

            returnStatus = ["刪除標籤成功", "成功"];
            return returnStatus;
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

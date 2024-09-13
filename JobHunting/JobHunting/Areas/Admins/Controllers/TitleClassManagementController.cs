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
    public class TitleClassManagementController : Controller
    {
        private readonly DuckAdminsContext _context;

        public TitleClassManagementController(DuckAdminsContext context)
        {
            _context = context;
        }

        // GET: Admins/TitleClassManagement
        public async Task<IActionResult> Index()
        {
            return View();
        }

        // GET: Admins/TitleClassManagement/IndexTitleClasses
        public JsonResult IndexTitleClasses()
        {
            return Json(_context.TitleClasses.AsNoTracking().Select(t => new
            {
                TitleClassId = t.TitleClassId,
                TitleCategoryId = t.TitleCategoryId,
                TitleClassName = t.TitleClassName,
            }));
        }

        // GET: Admins/TitleClassManagement/IndexTitleCategories
        public JsonResult IndexTitleCategories()
        {
            return Json(_context.TitleCategories.AsNoTracking().Where(t => t.TitleCategoryId > 0).Select(t => new
            {
                TitleCategoryId = t.TitleCategoryId,
                TitleCategoryName = t.TitleCategoryName,
            }));
        }

        //POST: Admins/TitleClassManagement/CreateTitleClass
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<Array> CreateTitleClass([FromBody][Bind("TitleClassId,TitleCategoryId,TitleClassName")] TitleClassViewModel tcvm)
        {
            string[] returnStatus = new string[2];

            if (ModelState.IsValid)
            {
                if (tcvm.TitleClassName == "")
                {
                    returnStatus = ["職業類型名稱不可為空", "失敗"];
                    return returnStatus;
                }

                TitleClass tcadd = new TitleClass
                {
                    TitleCategoryId = tcvm.TitleCategoryId,
                    TitleClassName = tcvm.TitleClassName,
                };

                var arr = await _context.TitleClasses.Where(t => t.TitleCategoryId == tcadd.TitleCategoryId).Where(t => t.TitleClassName == tcadd.TitleClassName).ToArrayAsync();
                if (arr.Length > 0)
                {
                    returnStatus = [$"此類別已存在相同職業類型{tcadd.TitleClassName}", "失敗"];
                    return returnStatus;
                }

                returnStatus = [$"新增職業類型{tcadd.TitleClassName}成功", "成功"];
                _context.TitleClasses.Add(tcadd);
                await _context.SaveChangesAsync();
                return returnStatus;
            }

            returnStatus = ["新增標籤失敗", "失敗"];
            return returnStatus;
        }

        //POST: Admins/TitleClassManagement/CreateTitleCategory
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<Array> CreateTitleCategory([FromBody] TitleCategoryNameViewModel titleCategoryName)
        {
            string[] returnStatus = new string[2];

            if (ModelState.IsValid)
            {
                if (titleCategoryName.TitleCategoryName == "")
                {
                    returnStatus = ["職業職別名稱不可為空", "失敗"];
                    return returnStatus;
                }

                TitleCategory tcadd = new TitleCategory
                {
                    TitleCategoryName = titleCategoryName.TitleCategoryName,
                };

                var arr = await _context.TitleCategories.Where(tc => tc.TitleCategoryName == tcadd.TitleCategoryName).ToArrayAsync();
                if (arr.Length > 0)
                {
                    returnStatus = [$"已存在相同職業類別{tcadd.TitleCategoryName}", "失敗"];
                    return returnStatus;
                }

                returnStatus = [$"新增職業類別{tcadd.TitleCategoryName}成功", "成功"];
                _context.TitleCategories.Add(tcadd);
                await _context.SaveChangesAsync();
                return returnStatus;
            }

            returnStatus = ["新增職業類別失敗", "失敗"];
            return returnStatus;
        }

        //POST: Admins/TitleClassManagement/MoveTags
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<Array> MoveTitleClasses([FromBody][Bind("TitleCategoryId,TitleClassChecked")] MoveTitleClassesViewModel mtcvm)
        {
            string[] returnStatus = new string[2];
            string str = "";

            if (!ModelState.IsValid)
            {
                returnStatus = ["移動職業類型失敗", "失敗"];
                return returnStatus;
            }

            if (mtcvm.TitleClassChecked.Length == 0)
            {
                returnStatus = ["未選擇職業類型", "失敗"];
                return returnStatus;
            }

            for (int i = 0; i < mtcvm.TitleClassChecked.Length; i++)
            {
                int id = mtcvm.TitleClassChecked[i];
                var titleClass = await _context.TitleClasses.FindAsync(id);
                if (titleClass != null)
                {
                    titleClass.TitleCategoryId = mtcvm.TitleCategoryId;
                    if (i != mtcvm.TitleClassChecked.Length - 1)
                    {
                        str += $"{titleClass.TitleClassName}, ";
                    }
                    else
                    {
                        str += $"{titleClass.TitleClassName}";
                    }

                    _context.Entry(titleClass).State = EntityState.Modified;
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateException ex)
                    {
                        returnStatus = [$"移動ID為{mtcvm.TitleClassChecked[i]}的關聯職業類型失敗!", "失敗"];
                        return returnStatus;
                    }
                }
            }

            returnStatus = [$"移動職業類型{str}成功", "成功"];
            return returnStatus;
        }

        //POST: Admins/TitleClassManagement/EditTitleClass
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<Array> EditTitleClass([FromBody][Bind("TitleClassId,TitleCategoryId,TitleClassName")] TitleClassViewModel tcvm)
        {
            string[] returnStatus = new string[2];

            if (!ModelState.IsValid)
            {
                returnStatus = [$"修改職業類型失敗", "失敗"];
                return returnStatus;
            }

            if (tcvm.TitleClassId == null)
            {
                returnStatus = ["請選擇欲修改職業類型", "失敗"];
                return returnStatus;
            }

            if (tcvm.TitleClassName == "")
            {
                returnStatus = ["職業類型名稱不可為空", "失敗"];
                return returnStatus;
            }

            var arr = await _context.TitleClasses.Where(t => t.TitleCategoryId == tcvm.TitleCategoryId).Where(t => t.TitleClassName == tcvm.TitleClassName).ToArrayAsync();
            if (arr.Length > 0)
            {
                returnStatus = [$"此類別已存在相同職業類型{tcvm.TitleClassName}", "失敗"];
                return returnStatus;
            }

            var titleClass = await _context.TitleClasses.FindAsync(tcvm.TitleClassId);
            if (titleClass == null)
            {
                returnStatus = [$"不存在可修改職業類型", "失敗"];
                return returnStatus;
            }
            titleClass.TitleClassName = tcvm.TitleClassName;

            _context.Entry(titleClass).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                returnStatus = ["修改職業類型失敗", "失敗"];
                return returnStatus;
            }

            returnStatus = [$"修改職業類型ID:{tcvm.TitleClassId}為{tcvm.TitleClassName}成功", "成功"];
            return returnStatus;
        }

        //POST: Admins/TitleClassManagement/EditTitleCategory
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<Array> EditTitleCategory([FromBody][Bind("TitleCategoryId,TitleCategoryName")] TitleCategoryViewModel tcvm)
        {
            string[] returnStatus = new string[2];

            if (!ModelState.IsValid)
            {
                returnStatus = [$"修改職業類別失敗", "失敗"];
                return returnStatus;
            }

            if (tcvm.TitleCategoryId == null)
            {
                returnStatus = ["請選擇欲修改職業類別", "失敗"];
                return returnStatus;
            }

            if (tcvm.TitleCategoryName == "")
            {
                returnStatus = ["職業類別名稱不可為空", "失敗"];
                return returnStatus;
            }

            var arr = await _context.TitleCategories.Where(tc => tc.TitleCategoryName == tcvm.TitleCategoryName).ToArrayAsync();
            if (arr.Length > 0)
            {
                returnStatus = [$"已存在相同職業類別{tcvm.TitleCategoryName}", "失敗"];
                return returnStatus;
            }

            var titleCategory = await _context.TitleCategories.FindAsync(tcvm.TitleCategoryId);
            if (titleCategory == null)
            {
                returnStatus = [$"不存在可修改職業類別", "失敗"];
                return returnStatus;
            }
            titleCategory.TitleCategoryName = tcvm.TitleCategoryName;

            _context.Entry(titleCategory).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                returnStatus = ["修改職業類別失敗", "失敗"];
                return returnStatus;
            }

            returnStatus = [$"修改職業類別ID:{tcvm.TitleCategoryId}為{tcvm.TitleCategoryName}成功", "成功"];
            return returnStatus;
        }

        //POST: Admins/TitleClassManagement/DeleteTitleClasses
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<Array> DeleteTitleClasses([FromBody] int[] arr)
        {
            string[] returnStatus = new string[2];
            string str = "";

            if (!ModelState.IsValid)
            {
                returnStatus = ["刪除職業類型失敗", "失敗"];
                return returnStatus;
            }

            if (arr.Length == 0)
            {
                returnStatus = ["未選擇職業類型", "失敗"];
                return returnStatus;
            }

            for (int i = 0; i < arr.Length; i++)
            {
                int id = arr[i];
                var titleClass = await _context.TitleClasses.FindAsync(id);
                if (titleClass != null)
                {
                    _context.TitleClasses.Remove(titleClass);
                    if (i != arr.Length - 1)
                    {
                        str += $"{titleClass.TitleClassName}, ";
                    }
                    else
                    {
                        str += $"{titleClass.TitleClassName}";
                    }
                }

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    returnStatus = [$"刪除ID為{arr[i]}的關聯職業類型失敗!", "失敗"];
                    return returnStatus;
                }
            }

            returnStatus = [$"刪除職業類型{str}成功", "成功"];
            return returnStatus;
        }

        //POST: Admins/TitleClassManagement/DeleteTitleCategory
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<Array> DeleteTitleCategory([FromBody] int titleCategoryId)
        {
            string[] returnStatus = new string[2];

            if (!ModelState.IsValid)
            {
                returnStatus = ["刪除職業類別失敗", "失敗"];
                return returnStatus;
            }

            var titleCategory = await _context.TitleCategories.FindAsync(titleCategoryId);
            if (titleCategory != null)
            {
                _context.TitleCategories.Remove(titleCategory);
            }
            else
            {
                returnStatus = [$"不存在此職業類別ID:{titleCategoryId}", "失敗"];
                return returnStatus;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                returnStatus = [$"刪除ID為{titleCategoryId}的關聯職業類別失敗!", "失敗"];
                return returnStatus;
            }

            returnStatus = [$"刪除職業類別{titleCategory.TitleCategoryName}成功", "成功"];
            return returnStatus;
        }
    }
}

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
                if(tvm.TagName == "")
                {
                    returnStatus = ["標籤名稱不可為空", "失敗"];
                    return returnStatus;
                }

                Tag tadd = new Tag
                {
                    TagClassID = tvm.TagClassID,
                    TagName = tvm.TagName,
                };

                var arr = await _context.Tags.Where(t => t.TagClassID == tadd.TagClassID).Where(t => t.TagName == tadd.TagName).ToArrayAsync();
                if(arr.Length > 0)
                {
                    returnStatus = [$"此類型已存在相同標籤{tadd.TagName}","失敗"];
                    return returnStatus;
                }

                returnStatus = [$"新增標籤{tadd.TagName}成功", "成功"];
                _context.Tags.Add(tadd);
                await _context.SaveChangesAsync();
                return returnStatus;
            }

            returnStatus = ["新增標籤失敗", "失敗"];
            return returnStatus;
        }

        //POST: Admins/TagManagement/CreateTagClass
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<Array> CreateTagClass([FromBody]string tagClassName)
        {
            string[] returnStatus = new string[2];

            if (ModelState.IsValid)
            {
                if(tagClassName == "")
                {
                    returnStatus = ["標籤類型名稱不可為空", "失敗"];
                    return returnStatus;
                }

                TagClass tcadd = new TagClass
                {
                    TagClassName = tagClassName,
                };

                var arr = await _context.TagClasses.Where(tc => tc.TagClassName == tcadd.TagClassName).ToArrayAsync();
                if (arr.Length > 0)
                {
                    returnStatus = [$"已存在相同標籤類型{tcadd.TagClassName}", "失敗"];
                    return returnStatus;
                }

                returnStatus = [$"新增標籤類型{tcadd.TagClassName}成功", "成功"];
                _context.TagClasses.Add(tcadd);
                await _context.SaveChangesAsync();
                return returnStatus;
            }

            returnStatus = ["新增標籤類型失敗", "失敗"];
            return returnStatus;
        }

        public class moveTagsModel
        {
            public int TagClassID { get; set; }

            public int[] TagChecked { get; set; }
        }

        //POST: Admins/TagManagement/MoveTags
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<Array> MoveTags([FromBody][Bind("TagClassID,TagChecked")] moveTagsModel mtm)
        {
            string[] returnStatus = new string[2];
            string str = "";

            if (!ModelState.IsValid)
            {
                returnStatus = ["移動標籤失敗", "失敗"];
                return returnStatus;
            }

            if (mtm.TagChecked.Length == 0)
            {
                returnStatus = ["未選擇標籤", "失敗"];
                return returnStatus;
            }

            for (int i = 0; i < mtm.TagChecked.Length; i++)
            {
                int id = mtm.TagChecked[i];
                var tag = await _context.Tags.FindAsync(id);
                if (tag != null)
                {
                    tag.TagClassID = mtm.TagClassID;
                    if (i != mtm.TagChecked.Length - 1)
                    {
                        str += $"{tag.TagName}, ";
                    }
                    else
                    {
                        str += $"{tag.TagName}";
                    }

                    _context.Entry(tag).State = EntityState.Modified;
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateException ex)
                    {
                        returnStatus = [$"移動ID為{mtm.TagChecked[i]}的關聯標籤失敗!", "失敗"];
                        return returnStatus;
                    }
                }
            }

            returnStatus = [$"移動標籤{str}成功", "成功"];
            return returnStatus;
        }

        //POST: Admins/TagManagement/EditTag
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<Array> EditTag([FromBody][Bind("TagID,TagClassID,TagName")] TagsViewModel tvm)
        {
            string[] returnStatus = new string[2];

            if (!ModelState.IsValid)
            {
                returnStatus = [$"修改標籤失敗", "失敗"];
                return returnStatus;
            }

            if(tvm.TagID == null)
            {
                returnStatus = ["請選擇欲修改標籤", "失敗"];
                return returnStatus;
            }

            if (tvm.TagName == "")
            {
                returnStatus = ["標籤名稱不可為空", "失敗"];
                return returnStatus;
            }

            var arr = await _context.Tags.Where(t => t.TagClassID == tvm.TagClassID).Where(t => t.TagName == tvm.TagName).ToArrayAsync();
            if (arr.Length > 0)
            {
                returnStatus = [$"此類型已存在相同標籤{tvm.TagName}", "失敗"];
                return returnStatus;
            }

            var tag = await _context.Tags.FindAsync(tvm.TagID);
            if (tag == null)
            {
                returnStatus = [$"不存在可修改標籤", "失敗"];
                return returnStatus;
            }
            tag.TagName = tvm.TagName;

            _context.Entry(tag).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException ex)
            {
                returnStatus = ["修改標籤失敗", "失敗"];
                return returnStatus;
            }

            returnStatus = [$"修改標籤ID:{tvm.TagID}為{tvm.TagName}成功", "成功"];
            return returnStatus;
        }

        //POST: Admins/TagManagement/EditTagClass
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<Array> EditTagClass([FromBody][Bind("TagClassID,TagClassName")] TagClassesViewModel tcvm)
        {
            string[] returnStatus = new string[2];

            if (!ModelState.IsValid)
            {
                returnStatus = [$"修改標籤類型失敗", "失敗"];
                return returnStatus;
            }

            if (tcvm.TagClassID == null)
            {
                returnStatus = ["請選擇欲修改標籤類型", "失敗"];
                return returnStatus;
            }

            if (tcvm.TagClassName == "")
            {
                returnStatus = ["標籤類型名稱不可為空", "失敗"];
                return returnStatus;
            }

            var arr = await _context.TagClasses.Where(tc => tc.TagClassName == tcvm.TagClassName).ToArrayAsync();
            if (arr.Length > 0)
            {
                returnStatus = [$"已存在相同標籤類型{tcvm.TagClassName}", "失敗"];
                return returnStatus;
            }

            var tagClass = await _context.TagClasses.FindAsync(tcvm.TagClassID);
            if (tagClass == null)
            {
                returnStatus = [$"不存在可修改標籤類型", "失敗"];
                return returnStatus;
            }
            tagClass.TagClassName = tcvm.TagClassName;

            _context.Entry(tagClass).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                returnStatus = ["修改標籤類型失敗", "失敗"];
                return returnStatus;
            }

            returnStatus = [$"修改標籤類型ID:{tcvm.TagClassID}為{tcvm.TagClassName}成功", "成功"];
            return returnStatus;
        }

        //POST: Admins/TagManagement/DeleteTags
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<Array> DeleteTags([FromBody]int[] arr)
        {
            string[] returnStatus = new string[2];
            string str = "";

            if (!ModelState.IsValid) {
                returnStatus = ["刪除標籤失敗", "失敗"];
                return returnStatus;
            }

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
                    if (i != arr.Length - 1)
                    {
                        str += $"{tag.TagName}, ";
                    }
                    else
                    {
                        str += $"{tag.TagName}";
                    }
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

            returnStatus = [$"刪除標籤{str}成功", "成功"];
            return returnStatus;
        }

        //POST: Admins/TagManagement/DeleteTagClass
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<Array> DeleteTagClass([FromBody] int tagClassID)
        {
            string[] returnStatus = new string[2];

            if (!ModelState.IsValid)
            {
                returnStatus = ["刪除標籤類型失敗", "失敗"];
                return returnStatus;
            }

            var tagClass = await _context.TagClasses.FindAsync(tagClassID);
            if (tagClass != null) { 
                _context.TagClasses.Remove(tagClass);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) 
            {
                returnStatus = [$"刪除ID為{tagClassID}的關聯標籤類型失敗!", "失敗"];
                return returnStatus;
            }

            returnStatus = [$"刪除標籤類型{tagClass.TagClassName}成功", "成功"];
            return returnStatus;
        }

    }
}

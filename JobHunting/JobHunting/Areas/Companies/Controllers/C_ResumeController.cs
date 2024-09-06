﻿using JobHunting.Areas.Companies.Models;
using JobHunting.Areas.Companies.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace JobHunting.Areas.Companies.Controllers
{
    [Area("Companies")]
    public class C_ResumeController : Controller
    {
        private readonly DuckCompaniesContext _context;

        public C_ResumeController(DuckCompaniesContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ResumeStorage()
        {
            return View();
        }
        //GET:compaines/C_Resume/ResumeStorageJson
        [HttpGet]
        public JsonResult ResumeStorageJson()
        {
            var Candidate = _context.Candidates;
            return Json(_context.Resumes.Select(p => new
            {
                jobTitle = _context.ResumeOpeningRecords.Where(ror => ror.ResumeID == p.ResumeID).Select(ror => ror.OpeningTitle).Single(),
                candidateName = Candidate.Where(c => c.CandidateID == p.CandidateID).Select(c => c.Name).Single(),
                candidateSex = Candidate.Where(c => c.CandidateID == p.CandidateID).Select(c => c.Sex).Single(),
                candidateDegree = Candidate.Where(c => c.CandidateID == p.CandidateID).Select(c => c.Degree).Single(),
                candidateEmpStatus = Candidate.Where(c => c.CandidateID == p.CandidateID).Select(c => c.EmploymentStatus).Single(),

            }));
        }
        //GET:Compaines/C_Resume/ReceiveResume
        [HttpGet]
        public IActionResult ReceiveResume()
        {
            return View();
        }
        //GET:compaines/C_Resume/ReceiveResumeJson
        [HttpGet]
        public JsonResult ReceiveResumeJson()
        {
            var Openings = _context.Openings;
            var TitleClasses = _context.TitleClasses;
            var ResumeOpeningRecords = _context.ResumeOpeningRecords;
            ViewBag.TitleClass = new SelectList(TitleClasses, "TitleClassID", "TitleClassName");
            return Json(Openings.Select(p => new
            {
                Title = p.Title,
                TitleId=p.OpeningID,
                TitleClassName = TitleClasses.Where(s => s.TitleClassID == p.TitleClassID).Select(s => s.TitleClassName).Single(),
                TitleClassId = TitleClasses.Where(s => s.TitleClassID == p.TitleClassID).Select(s => s.TitleClassID).Single(),
                ApplyDate = ResumeOpeningRecords.Where(a => a.OpeningID == p.OpeningID).Select(a => a.ApplyDate).Single(),
            }));
        }
        //GET:compaines/C_Resume/ResumeOpClassTitle
        [HttpGet]
        public JsonResult OpClassTitle()
        {
            return Json(_context.TitleClasses.Select(t => new
            {
                titleClassID = t.TitleClassID,
                titleClassName = t.TitleClassName
            }));
        }

        public JsonResult OpTitle()
        {
            return Json(_context.Openings.Select(o => new
            {
                OpID = o.OpeningID,
                OpTitle = o.Title,
                titleClassID=o.TitleClassID
            }));
        }
    }

}

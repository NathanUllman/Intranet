using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using IntranetApplication.Engines;
using IntranetApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IntranetApplication.Models.DashboardItem;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace IntranetApplication.Controllers
{
/*===========================================================================================================*/
/*============================ Add Dashboard ================================================================*/
/*===========================================================================================================*/
    [AllowAnonymous]
    [Route("[controller]/[action]")]
    public class DashboardFormsController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> AddDashboard()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddDashboard(Dashboard newDashboard)
        {
            DbAccessor accessor = new DbAccessor();
            accessor.InsertDashboard(newDashboard);

            return Redirect("/AdminTools/DashManager");
        }
/*===========================================================================================================*/
/*============================ Add item, HTML scrapping =====================================================*/
/*===========================================================================================================*/
        [HttpGet]
        public async Task<IActionResult> ImageHtmlScrapping(string DashboardId) //Input forms and Edit forms are same************,                                                                                                         //we know which action is being taken depending on what arguments are provided
        {
            string id = TempData["EditID"]?.ToString(); // has value if we are editing

            if (DashboardId == null & id == null) // error
            {
                return Redirect("/AdminTools/DashManager");

            }
            if (id == null) // we are creating a new item
            {
                int Id = Int32.Parse(DashboardId); // todo use tryparse to check if we can actually
                return View(new DashItemScrap {DashboardID = Id}); // creating new

            }

            // we are editing a item
            DbAccessor accessor = new DbAccessor();
            DashItemScrap ItemToEdit = (DashItemScrap)accessor.GetDashItem(id); // get item to fill model with

            return View(ItemToEdit); // editing pre-exisiting
        }

        [HttpPost]
        public async Task<IActionResult> ImageHtmlScrapping(DashItemScrap newItem)
        {

            if (!ModelState.IsValid)
            {
                return View(newItem);
            }
            DbAccessor accessor = new DbAccessor();
            if (newItem.DashboardItemID == 0) // creating new item
            {
                TransformEngine yeah = new TransformEngine();
                newItem.ImageURI = yeah.Convert(newItem); // this will take awhile TODO: make background process do this             
                accessor.InsertDashItem(newItem);
            }
            else // editing pre-existing item
            {
                accessor.UpdateDashItem((DashboardItem) newItem);
            }

            return Redirect("/AdminTools/DashManager");
        }

        [HttpPost]
        public async Task<IActionResult> TestImage(string url, string selector, string userName, string password)
        {
            TransformEngine yeah = new TransformEngine();
            string location = yeah.ConvertOneTEMP(userName, password, url,
                selector);
            return Content(JsonConvert.SerializeObject(location));
        }
/*===========================================================================================================*/
/*============================ Add item, Image Upload =======================================================*/
/*===========================================================================================================*/
        [HttpGet]
        public async Task<IActionResult> ImageUpload(string DashboardId)
        {

            string id = TempData["EditID"]?.ToString();

            if (DashboardId == null & id == null)
            {
                return Redirect("/AdminTools/DashManager");

            }
            if (id == null) // we are going to create something
            {
                int Id = Int32.Parse(DashboardId); // todo use tryparse to check if we can actually
                return View(new DashItemUpload { DashboardID = Id });

            }

            // we are editing
            DbAccessor accessor = new DbAccessor();
            DashItemUpload ItemToEdit = (DashItemUpload)accessor.GetDashItem(id); // get item to fill model with
            return View(ItemToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> ImageUpload(DashItemUpload newItem, List<IFormFile> files)
        {
            
            if (!ModelState.IsValid)
            {
                return View(newItem);
            }

            if (files.Count != 0) // only need to upload photos if the user uploaded any
            {
                newItem.ImageURI = UploadImage(files, newItem.Title);
            }
        
            DbAccessor accessor = new DbAccessor();

            if (newItem.DashboardItemID == 0) // creating new item
            {         
                accessor.InsertDashItem(newItem);
            }
            else // editing pre-existing item
            {
                accessor.UpdateDashItem((DashboardItem)newItem);
            }

            return Redirect("/AdminTools/DashManager");       
        }

        public string UploadImage(List<IFormFile> files, string fileName)
        {

            string location = "wwwroot/images/UploadedImages/" + fileName;

            Random rnd = new Random();
            while (System.IO.File.Exists(location + ".jpg")) // dont want to overwrite any pictures, so we keep adding random numbers until we get an unused name
            {
                location += rnd.Next(0, 10).ToString();
            }


            long size = files.Sum(f => f.Length);

            // var domain = Request.GetUri().ToString().Replace("/DashboardForms/UploadImage", ""); // used to get the domain of the website
            //location = location.Replace(domain, "wwwroot");
            var filePath = location + ".jpg";

            foreach (var formFile in files)
            {

                if (formFile.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        formFile.CopyTo(stream);
                    }
                }
            }

            location = location.Replace("wwwroot", ""); // so that img src will point to correct area
            return location + ".jpg";
        }
/*===========================================================================================================*/
/*============================ Add item, Text only ==========================================================*/
/*===========================================================================================================*/

        [HttpGet]
        public async Task<IActionResult> TextOnly( string DashboardId)
        {
            string id = TempData["EditID"]?.ToString(); // only non null when we are editing
           
            if (DashboardId == null & id == null) // error
            {
                return Redirect("/AdminTools/DashManager");
                
            }
            if (id == null) // we are creating
            {
                int Id = Int32.Parse(DashboardId); // todo use tryparse to check if we can actually
                return View(new DashItemText { DashboardID = Id });

            }

            // we are editing
            DbAccessor accessor = new DbAccessor();
            DashItemText ItemToEdit = (DashItemText) accessor.GetDashItem(id); // get item to fill model with
            return View(ItemToEdit);

            
        }

        [HttpPost]
        public async Task<IActionResult> TextOnly(DashItemText newItem)
        {

            if (!ModelState.IsValid)
            {
                return View(newItem);
            }

            DbAccessor accessor = new DbAccessor();
            if (newItem.DashboardItemID == 0) // creating new item
            {
                accessor.InsertDashItem(newItem);
            }
            else // editing pre-existing item
            {
                accessor.UpdateDashItem((DashboardItem)newItem);
            }

            return Redirect("/AdminTools/DashManager");
        }
        /*===========================================================================================================*/
        /*============================ Edit Item ====================================================================*/
        /*===========================================================================================================*/
        [HttpGet]
        public async Task<IActionResult> EditDashboardItem(string DashboardItemId)
        {
            if (DashboardItemId == null)
            {

            }

            TempData["EditID"] = DashboardItemId;
            DbAccessor accessor = new DbAccessor();
            DashboardItem item = accessor.GetDashItem(DashboardItemId);


            switch (item.DashboardTypeID)
            {

                case 1: //Url Scrapping    
                    return RedirectToAction("ImageHtmlScrapping");
                case 2: //Image Upload
                    return RedirectToAction("ImageUpload");
                case 3: // Text/HTML based
                    return RedirectToAction("TextOnly");
                default:                    
                    break;
            }

            return Redirect("/AdminTools/DashManager");
        }
        /*===========================================================================================================*/
        /*============================ Edit Dashboard ===============================================================*/
        /*===========================================================================================================*/
        [HttpGet]
        public async Task<IActionResult> EditDashboard(string DashboardId)
        {
            if (DashboardId == null)
            {

            }

            TempData["EditID"] = DashboardId;
            DbAccessor accessor = new DbAccessor();
            Dashboard item = accessor.GetDashboard(DashboardId);

/*
            switch (item.DashboardTypeID)
            {

                case 1: //Url Scrapping    
                    return RedirectToAction("ImageHtmlScrapping");
                case 2: //Image Upload
                    return RedirectToAction("ImageUpload");
                case 3: // Text/HTML based
                    return RedirectToAction("TextOnly");
                default:
                    break;
            }
            */
            return Redirect("/AdminTools/DashManager");
        }

    }
}
﻿using ClosedXML.Excel;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using topGlove.Data;
using topGlove.Extension;
using topGlove.Model;
using TopGlove.TrayTracking.Api.Model;

namespace topGlove.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class TrackingController : ControllerBase
    {
        public readonly UserDbContext dataContext;

        public TrackingController(UserDbContext userData)
        {
            dataContext = userData;
        }

        [HttpPost("AddTrayDetail")]

        public IActionResult AddTrayDetail([FromBody] TrayTrackinInput inputOperatorData)
        {
            if (inputOperatorData == null)
            {
                return BadRequest();
            }
            else
            {
                dataContext.TrayDetails.Add(inputOperatorData);
                dataContext.SaveChanges();
                return Ok(inputOperatorData);
              
            }           
        }

        [HttpPut("UpdateTrayDetail")]

        public IActionResult UpdateTrayDetail([FromBody] TrayTrackinInput inputData)
        {
            if(inputData == null)
            {
                return NotFound();
            }
            var details = dataContext.TrayDetails.AsNoTracking().FirstOrDefault(x=>x.UserId == inputData.UserId);
            if (details == null)
            {
                return BadRequest();
            }
            else
            {
                dataContext.Entry(inputData).State = EntityState.Modified;
                dataContext.SaveChanges();
                return Ok(inputData);
            }
        }

        [HttpDelete("DeleteTrayDetail")]

        public IActionResult DeleteTrayDetail(int id)
        {
            var inputData = dataContext.TrayDetails.Find(id);
            if(inputData == null)
            {
                return BadRequest();
            }
            else
            {
                dataContext.TrayDetails.Remove(inputData);
                dataContext.SaveChanges();
                return Ok();
            }
        }

        [HttpPost("FilteredItems")]
        public IActionResult GetProductQualityWithUser(RequestModel requestModel)
        {
            var response = GetFilter(requestModel);
            return Ok(response);
        }

        [HttpPost("GenerateExcel")]
        public IActionResult GenerateExcel(RequestModel requestModel)
        {
            var response = GetFilter(requestModel);
            response = response.OrderByDescending(x => x.UserId).ToList();
            var excelStream = response.CreateExcel<TrayTrackinInput>();

            var fileName = $"TopGlove_{DateTime.Now}.xlsx";
            return File(excelStream,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        fileName);
        }

       
        private List<TrayTrackinInput> GetFilter(RequestModel requestModel)
        {
            var response = dataContext.TrayDetails.Where(a => a.DataTime.Date >= requestModel.FromDate.Date && a.DataTime.Date
            <= requestModel.ToDate.Date);
            if(!string.IsNullOrWhiteSpace(requestModel.User) && response.Any())
            {
                response = response.Where(a => a.User == requestModel.User);
            }
            if (!string.IsNullOrWhiteSpace(requestModel.Process) && response.Any())
            {
                response = response.Where(a => a.Proces == requestModel.User);
            }

            return response.ToList();
        }     
    }
}


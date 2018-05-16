using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Automation.Common;
using System.Data;
using System.Data.OleDb;
using System.IO;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using SiteManagement.SiteHelper;
using SiteManagement.Models;
using System.Configuration;

namespace SiteManagement.Controllers
{
    public class SiteCoreController : Controller
    {
        string excludeSite = ConfigurationManager.AppSettings["ExcludeSiteName"].ToString();
        // GET: PassengerModifyPhone
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult QuerySite(SiteQueryDto condition)
        {
            var result = new Automation.Common.ResponseModel()
            {
                Success = true
            };
            try
            {
                var siteHandler = UnionSoftSiteHelper.GetSites(true);
                List<SiteDto> sites = new List<SiteDto>();
                foreach (var item in siteHandler)
                {
                    sites.Add(new SiteDto() { SiteID = item.IisId, SiteName = item.Name, SiteFullPath = item.SitePath, BindingUrls = item.Bindings, SiteStatus = item.SiteState.ToString() });
                }

                IList<SiteDto> SearchTemp = null;
                IList<SiteDto> PageResult = null;
                if (condition.SiteName != string.Empty && condition.SiteName != null)
                    SearchTemp = sites.Where(x => x.SiteName.ToUpper().Contains(condition.SiteName.ToUpper()) && x.SiteName != excludeSite).ToList<SiteDto>();
                else
                    SearchTemp = sites.Where(x => x.SiteName != excludeSite).ToList<SiteDto>();

                PageResult = SearchTemp.Skip(condition.PageSize * (condition.PageIndex - 1)).Take(condition.PageSize).OrderBy(x => x.SiteID).ToList<SiteDto>();
                condition.Total = SearchTemp.Count();
                condition.Data = PageResult;
                //result.Result = sites;
                return Json(condition, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Automation.Common.LogHelper.Log(Automation.Common.LogHelper.LogLevel.Error, ex.ToString());
                result.Success = false;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult StartSite(int SiteID)
        {
            var result = new Automation.Common.ResponseModel()
            {
                Success = true
            };
            try
            {
                var FinalMessage = UnionSoftSiteHelper.StartSite(new IisSite() { IisId = SiteID });
                result.Message = FinalMessage.ToString();
            }
            catch (Exception)
            {
                result.Success = false;
                result.Message = SiteStartResult.IISError.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult StopSite(int SiteID)
        {
            var result = new Automation.Common.ResponseModel()
            {
                Success = true
            };
            try
            {
                UnionSoftSiteHelper.StopSite(new IisSite() { IisId = SiteID });
                result.Message = "Stop Success";
            }
            catch (Exception)
            {
                result.Success = false;
                result.Message = SiteStartResult.IISError.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RestartSite(int SiteID)
        {
            var result = new Automation.Common.ResponseModel()
            {
                Success = true
            };
            try
            {
                UnionSoftSiteHelper.RestartSite( SiteID );
                result.Message = "Restart Success";
            }
            catch (Exception)
            {
                result.Success = false;
                result.Message = SiteStartResult.IISError.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ExportExcelFile()
        {
            //var result = new Automation.Common.ResponseModel()
            //{
            //    Success = true
            //};
            //MemoryStream ms = ExportExcel();
            //string fileName = "乘客修改手机号.xlsx";
            //Session[fileName] = ms;
            //result.Result = fileName;
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DownloadExportReport(string fileName)
        {
            var ms = Session[fileName] as MemoryStream;
            if (ms == null)
                return new EmptyResult();
            Session[fileName] = null;
            return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <returns></returns>
        public MemoryStream ExportExcel()
        {
            MemoryStream ms = new MemoryStream();
            //string templatePath = Server.MapPath("~/Template/乘客修改手机号.xlsx");
            ////打开excel 模板文件
            //using (FileStream fs = new FileStream(templatePath, FileMode.Open, FileAccess.ReadWrite))
            //{
            //    IWorkbook wk = new XSSFWorkbook(fs);
            //    // 日期的样式
            //    ICellStyle dateStyle = wk.CreateCellStyle();
            //    // 日期的格式化
            //    IDataFormat format = wk.CreateDataFormat();
            //    dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd HH:mm");
            //    // 字体样式
            //    IFont datafont = wk.CreateFont();
            //    // 字体大小
            //    datafont.FontHeightInPoints = 8;
            //    datafont.FontName = "Arial";
            //    dateStyle.SetFont(datafont);
            //    // 边框
            //    dateStyle.BorderBottom = BorderStyle.Thin;
            //    dateStyle.BorderLeft = BorderStyle.Thin;
            //    dateStyle.BorderRight = BorderStyle.Thin;
            //    dateStyle.BorderTop = BorderStyle.Thin;
            //    dateStyle.Alignment = HorizontalAlignment.Center;
            //    // 数据的样式
            //    ICellStyle cellStyle = wk.CreateCellStyle();
            //    cellStyle.Alignment = HorizontalAlignment.General;//【General】数字、时间默认：右对齐；BOOL：默认居中；字符串：默认左对齐
            //                                                      // 字体样式
            //    cellStyle.Alignment = HorizontalAlignment.CenterSelection;
            //    cellStyle.VerticalAlignment = VerticalAlignment.Center;
            //    IFont cellfont = wk.CreateFont();
            //    // 字体大小
            //    cellfont.FontHeightInPoints = 8;
            //    cellfont.FontName = "Arial";
            //    cellStyle.SetFont(cellfont);
            //    // 边框
            //    cellStyle.BorderBottom = BorderStyle.Thin;
            //    cellStyle.BorderLeft = BorderStyle.Thin;
            //    cellStyle.BorderRight = BorderStyle.Thin;
            //    cellStyle.BorderTop = BorderStyle.Thin;

            //    List<Model.PassengerModifyPhone> columns = _passengerModifyPhone.GetAll().ToList();

            //    #region 生成Sheet1--WorkCalendar
            //    //3.获取Sheet
            //    ISheet CalanderSheet = wk.GetSheetAt(0);
            //    int rowIndex = 0;
            //    IRow week = CalanderSheet.GetRow(rowIndex);

            //    int j = 1;
            //    foreach (var colume in columns)
            //    {
            //        int i = 0;
            //        CalanderSheet.GetRow(j).GetCell(i).SetCellValue(colume.Date.ToString("D"));
            //        CalanderSheet.GetRow(j).GetCell(i + 1).SetCellValue(colume.LogonPhoneNum);
            //        CalanderSheet.GetRow(j).GetCell(i + 2).SetCellValue(colume.ModifyPhoneNum);
            //        if (colume.Comment != null)
            //        {
            //            CalanderSheet.GetRow(j).GetCell(i + 3).SetCellValue(colume.Comment);
            //        }
            //        CalanderSheet.GetRow(j).GetCell(i + 4).SetCellValue(colume.Workplace);
            //        j++;
            //    }
            //    #endregion
            //    wk.Write(ms);
            //    ms.Flush();
            //    ms.Position = 0;
            //}
            return ms;

        }
    }

}
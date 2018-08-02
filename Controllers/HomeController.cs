using Newtonsoft.Json;
using SampleSite.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SampleSite.Controllers
{
    public class HomeController : Controller
    {
        public string connection { get; set; }
        // GET: Home
        [SessionAuthorize]
        public ActionResult Home()
        {
            return View();
        }
        public ActionResult Product()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetTaskName()
        {
            try
            {
                connection = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
                string jResult = string.Empty;
                using (SqlConnection con = new SqlConnection(connection))
                {
                    using (SqlCommand cmd = new SqlCommand("USPDropDownBind"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@Action", 1);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        SqlDataReader dataReader = cmd.ExecuteReader();
                        DataTable dataTable = new DataTable();
                        dataTable.Load(dataReader);
                        con.Close();
                        if (dataTable.Rows.Count > 0)
                        {
                            jResult = JsonConvert.SerializeObject(dataTable, Newtonsoft.Json.Formatting.Indented);
                            if (jResult != string.Empty)
                                return Json(jResult, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                Dispose();
            }
            return Json(new { flag = false }, JsonRequestBehavior.AllowGet);
        }
    }
}
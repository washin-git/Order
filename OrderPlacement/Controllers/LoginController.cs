using OrderPlacement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Web.Security;

namespace OrderPlacement.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public string status;

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(User u)
        {
            String SqlCon = ConfigurationManager.ConnectionStrings["ConnDB"].ConnectionString;
            SqlConnection con = new SqlConnection(SqlCon);
            string SqlQuery = "select UserName,Password from User where UserName=@UserName and Password=@Password";
            con.Open();
            SqlCommand cmd = new SqlCommand(SqlQuery, con); ;
            cmd.Parameters.AddWithValue("@UserName", u.UserName);
            cmd.Parameters.AddWithValue("@Password", u.Password);
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.Read())
            {
                Session["UserName"] = u.UserName.ToString();
                return RedirectToAction("Welcome");
            }
            else
            {
                ViewData["Message"] = "User Login Details Failed!!";
            }
            if (u.UserName.ToString() != null)
            {
                Session["UserName"] = u.UserName.ToString();
                status = "1";
            }
            else
            {
                status = "3";
            }

            con.Close();
            return View();
            //return new JsonResult { Data = new { status = status } };  
        }

        [HttpGet]
        public ActionResult Welcome(User u)
        {
            User user = new User();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection("Data Source=(local);Initial Catalog=orderplacement;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework"))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetEnrollmentDetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@UserName", SqlDbType.VarChar, 30).Value = Session["UserName"].ToString();
                    con.Open();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(ds);
                    List<User> userlist = new List<User>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        User uobj = new User();
                        uobj.id = Convert.ToInt32(ds.Tables[0].Rows[i]["ID"].ToString());
                        uobj.FName = ds.Tables[0].Rows[i]["FirstName"].ToString();
                        uobj.LName = ds.Tables[0].Rows[i]["LastName"].ToString();
                        uobj.Email = ds.Tables[0].Rows[i]["Email"].ToString();
                        uobj.UserName = ds.Tables[0].Rows[i]["UserName"].ToString();
                        uobj.Address = ds.Tables[0].Rows[i]["Address"].ToString();
                        uobj.Password = ds.Tables[0].Rows[i]["Password"].ToString();
                        uobj.ConfirmPassword = ds.Tables[0].Rows[i]["ConfirmPassword"].ToString();
                        uobj.NIC = ds.Tables[0].Rows[i]["NIC"].ToString();
                        uobj.Phone = ds.Tables[0].Rows[i]["Phone"].ToString();
                   
                       

                        userlist.Add(uobj);

                    }
                    
                }
                con.Close();

            }
            return View(user);
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }


    }
}
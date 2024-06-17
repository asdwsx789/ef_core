#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Web;
using System.Xml;

namespace EF_WebApi.Controllers
{
    public class MoodleSyncUserController : Controller
    {
        internal static string _token = "c34f4e2707448ac8f94e845ff7329af0";
        internal static string commString = "data source='10.0.9.76';initial catalog='FlowSync';User Id='sa';Password='cj06xj/6';Max Pool Size=300";

        [HttpGet(Name = "SyncAPI")]
        public async Task<IActionResult> Get()
        {
            string User_sql = @"SELECT a.ACCOUNTID, a.FULLNAME, a.EMAIL, a.AD_ACCOUNT, a.PASSWORD, b.DEPTNAME, a.AREA_FLAG, a.STATUS FROM UMEC_AFS_ACCOUNT AS a 
                        JOIN (
                            SELECT ACCOUNTID,CASE WHEN AD_ACCOUNT !='NULL' THEN AD_ACCOUNT ELSE 'x'  END AS noAD FROM UMEC_AFS_ACCOUNT
                            ) x on a.ACCOUNTID = x.ACCOUNTID 
                        JOIN 
                            UMEC_AFS_DEPT AS b on a.DEPTID = b.DEPTID 
                        WHERE a.AREA_FLAG = 'TW'";
            
            MoodleUser user = new MoodleUser();

            using(SqlConnection conn = new SqlConnection(commString))
            {
                try
                {
                    // WriteLog("Openning Connection ...");
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(User_sql, conn);
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    DataTable dt = new DataTable();
                    dt.Load(dataReader);

                    foreach(DataRow dr in dt.Rows)
                    {
                        user = GetUserInfo(dr);

                        string id = await UserCheck(user);
                        if(id == string.Empty)
                        {
                            await CreateUser(user); 
                        }else{
                            await UpdateUser(user, id);  
                        }

                        return Ok();
                    }

                    WriteLog("Connection successful!");
                    conn.Close();
                }
                catch(SqlException ex)
                {
                    WriteLog("[Error] : " + ex);
                }
            }

            return Ok();
        }

        static async Task<JsonNode> RestCall(string serviceType, string postData)
        {
            string  requestUrl = string.Format("http://e-learning-test//moodle/webservice/rest/server.php?wstoken={0}&wsfunction={1}&moodlewsrestformat=json", _token, serviceType);

            using(HttpClient client = new HttpClient())
            {
                try
                {
                    //Create Data Content.
                    byte[] formData = Encoding.UTF8.GetBytes(postData);
                    ByteArrayContent byteContent = new ByteArrayContent(formData);
                    byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");

                    //setting connect time close, Post Data & Read return Data.
                    client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                    client.DefaultRequestHeaders.Add("Keep-Alive", "300");
                    var responce = await client.PostAsync(requestUrl, byteContent);
                    string result = await responce.Content.ReadAsStringAsync();

                    //ReadData to json 
                    JsonNode jsonData = JsonNode.Parse(result);
                    return jsonData;
                }
                catch(HttpRequestException ex)
                {
                    WriteLog("[Error] : " + ex);
                }
            }

            return "";
        }

        static async Task<string> UserCheck(MoodleUser user)
        {
            string postData = string.Format("criteria[0][key]=username&criteria[0][value]={0}", user.username);
            JsonNode jsonData = await RestCall("core_user_get_users", postData);

            JsonArray jsonusers = jsonData["users"].AsArray();
            if(jsonusers.Count != 1)
            {
                return string.Empty;
            }else{
                JsonNode userid = jsonusers[0];
                return userid["id"].ToString();
            }
        }

        static async Task CreateUser(MoodleUser user)
        {
            string postData_CreateUser = string.Empty;

            postData_CreateUser = string.Format("users[0][username]={0}&users[0][password]={1}&users[0][auth]={2}&users[0][firstname]={3}&users[0][lastname]={4}&users[0][email]={5}&users[0][department]={6}", 
                                                user.username, user.pwd, user.auth, user.firstname, user.lastname, user.email, user.department);
            
            try
            {
                JsonNode jsonData = await RestCall("core_user_create_users", postData_CreateUser);

                // 判斷回傳Json的類型，有包錯誤訊息的是JsonObject
                if(jsonData.GetType().ToString() == "System.Text.Json.Nodes.JsonArray")
                {
                    string id =  jsonData[0]["id"].ToString();
                    Console.WriteLine("[Note] Create user " + jsonData[0]["username"].ToString());
                    await UpdateUser(user, id);
                }
                if(jsonData.GetType().ToString() == "System.Text.Json.Nodes.JsonObject")
                {
                    JsonObject jsonobject = jsonData.AsObject();
                    WriteLog("[Create Error] : " + user.username + " " + jsonData["debuginfo"]);
                }
            }
            catch(Exception ex)
            {
                WriteLog("[Error] " + ex);
            }
        }

        static async Task UpdateUser(MoodleUser user, string id)
        {
            // Moodle account suspended can only be change by Update.
            string postData_UpdateUser = string.Format("users[0][id]={0}&users[0][suspended]={1}", id, user.suspended);
            JsonNode jsonData = await RestCall("core_user_update_users", postData_UpdateUser);

            JsonObject jsonobject = jsonData.AsObject();
            if(jsonobject.ContainsKey("exception"))
            {
                WriteLog("[Update Error] : " + user.username + " " + jsonData["debuginfo"]);
            }
        }

        public static MoodleUser GetUserInfo(DataRow dr)
        {
            MoodleUser user = new MoodleUser();

            user.username = dr["ACCOUNTID"].ToString();
            user.pwd = dr["PASSWORD"].ToString();
            user.auth = "manual";
            user.suspended = dr["STATUS"].ToString() == "1" ? "0" : "1";
            user.firstname = dr["FULLNAME"].ToString();
            user.lastname = dr["ACCOUNTID"].ToString().Substring(2);
            user.email = dr["EMAIL"].ToString() == string.Empty ? dr["ACCOUNTID"].ToString() + "@umec.com.tw" : dr["EMAIL"].ToString();
            user.department = dr["DEPTNAME"].ToString();

            return user;
        }

        public class MoodleUser
        {
            public string username { get; set;}
            public string pwd { get; set;}
            public string auth{ get; set;}
            public string suspended { get; set;}
            public string firstname { get; set;}
            public string lastname { get; set;}
            public string email { get; set;}
            public string department { get; set;}
        }

        public static void WriteLog(string strlog)
        {
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            using(StreamWriter outputFile = new StreamWriter("D:\\Sync.log", true, UTF8Encoding.UTF8))
            {
                outputFile.WriteLine(strlog);
            }

            // Console.WriteLine(strlog);
        }
    }
}
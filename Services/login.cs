using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;

namespace COMMON_PROJECT_STRUCTURE_API.services
{
    public class login
    {
        dbServices ds = new dbServices();
        decryptService cm = new decryptService();
        
        private readonly Dictionary<string, string> jwt_config = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _service_config = new Dictionary<string, string>();

        IConfiguration appsettings = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        
        public login()
        {
            jwt_config["Key"] = appsettings["jwt_config:Key"].ToString();
            jwt_config["Issuer"] = appsettings["jwt_config:Issuer"].ToString();
            jwt_config["Audience"] = appsettings["jwt_config:Audience"].ToString();
            jwt_config["Subject"] = appsettings["jwt_config:Subject"].ToString();
            jwt_config["ExpiryDuration_app"] = appsettings["jwt_config:ExpiryDuration_app"].ToString();
            jwt_config["ExpiryDuration_web"] = appsettings["jwt_config:ExpiryDuration_web"].ToString();
        }
        public async Task<responseData> Login(requestData req)
        {
            responseData resData= new responseData();
             resData.rStatus = 200;
            resData.rData["rCode"]=0;
            resData.eventID = req.eventID;  
            try
            {
                 string input = req.addInfo["UserId"].ToString();
                bool isEmail = IsValidEmail(input);
                bool isMobileNumber = IsValidMobileNumber(input);
                string columnName;
                if (isEmail)
                {
                    columnName = "email";
                }
                else if (isMobileNumber)
                {
                    columnName = "phone";
                }
                else
                {
                    columnName = "";
                }

                MySqlParameter[] myParams = new MySqlParameter[] {
                new MySqlParameter("@UserId", input),
                new MySqlParameter("@roleId", 6),
                new MySqlParameter("@Password", req.addInfo["Password"].ToString())
                };
                var sq = $"SELECT * FROM GData.et_register WHERE {columnName} = @UserId AND password = @Password ";
                var data = ds.ExecuteSQLName(sq, myParams);
                
                if (data==null || data[0].Count()==0)
                {
                    resData.rData["rCode"] = 1;
                    resData.rStatus = 404;
                    resData.rData["rMessage"] = "Invalid Credentials";
                }
                else
                {
                      resData.rData["rCode"] = 0;
                    resData.rData["rMessage"] = "Login Successfully";
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message.ToString());
                resData.rStatus = 199;
                resData.rData["rCode"]=1;
                resData.rData["rMessage"]=ex.Message.ToString();
            }
            return resData;
        }

        // Method to check if phone number exists in dependra_signup table
        // private bool CheckPhoneNumberExists(string phoneNumber)
        // {
        //     try
        //     {
        //         using (MySqlConnection connection = new MySqlConnection("server=127.0.0.1;user=root;password=@Rattan1175;port=3306;database=GData;"))
        //         {
        //             connection.Open();
        //             string query = "SELECT COUNT(*) FROM GData.et_register WHERE phone = @phone";
        //             MySqlCommand command = new MySqlCommand(query, connection);
        //             command.Parameters.AddWithValue("@phone", phoneNumber);
        //             int count = Convert.ToInt32(command.ExecuteScalar());
        //             return count > 0;
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         // Handle exception, log, or return false
        //         Console.WriteLine("Error while executing query: " + ex.Message);
        //         return false;
        //     }
        // }

          public static bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }
        public static bool IsValidMobileNumber(string phoneNumber)
        {
            string pattern = @"^[0-9]{7,15}$";
            return Regex.IsMatch(phoneNumber, pattern);
        }
    }
}
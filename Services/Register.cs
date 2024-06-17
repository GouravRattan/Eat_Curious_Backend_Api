using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Ocsp;


namespace MyCommonStructure.Services
{
    public class Register
    {
        dbServices ds = new dbServices();

        public async Task<responseData> Registration(requestData req)
        {
            responseData resData = new responseData();
            resData.rData["rCode"] = 0;
            resData.rData["rMessage"] = "Student registered successfully";

            try
            {
                MySqlParameter[] para = new MySqlParameter[] {
            new MySqlParameter("@password", req.addInfo["password"].ToString()),
            new MySqlParameter("@name", req.addInfo["name"].ToString()),
            new MySqlParameter("@phone", req.addInfo["phone"].ToString()),
            new MySqlParameter("@email", req.addInfo["email"].ToString()),


        };

                var checkSql = $"SELECT * FROM GData.et_register WHERE phone=@phone OR email=@email;";
                var checkResult = ds.executeSQL(checkSql, para);


                if (checkResult[0].Count() != 0)
                {
                    resData.rData["rCode"] = 2;
                    resData.rData["rMessage"] = "Duplicate data found";
                }
                else
                {
                    var insertSql = $"INSERT INTO GData.et_register( name, phone, email, password) VALUES(@name, @phone, @email, @password);";
                    var insertId = ds.ExecuteInsertAndGetLastId(insertSql, para);

                    if (insertId != null)
                    {
                        resData.eventID = req.eventID;
                        resData.rData["rMessage"] = "Registration Successfully";
                    }
                }
            }
            catch (Exception ex)
            {
                resData.rData["rCode"] = 1;
                resData.rData["rMessage"] = $"Error: {ex.Message}";
            }
            return resData;
        }

        public async Task<responseData> GetUserRegistrationByEmail(requestData req)
        {
            responseData resData = new responseData();
            resData.eventID = req.eventID;
            resData.rData["rCode"] = 0;
            resData.rData["rMessage"] = "User Details Retrieved Successfully";

            try
            {
                string input = req.addInfo["email"].ToString(); // Assuming the email is in "email" field
                MySqlParameter[] myParams = new MySqlParameter[] {
            new MySqlParameter("@Email", input)
        };

                var sql = "SELECT * FROM GData.et_register WHERE email=@Email;";
                var data = ds.ExecuteSQLName(sql, myParams);

                if (data == null || data[0].Count() == 0)
                {
                    resData.rData["rCode"] = 1;
                    resData.rData["rMessage"] = "User not found";
                }
                else
                {
                    // Assuming you have specific keys in responseData for user details
                    // resData.rData["Id"] = data[0][0]["id"];
                     resData.rData["name"] = data[0][0]["name"];
                    resData.rData["phone"] = data[0][0]["phone"];
                    resData.rData["email"] = data[0][0]["email"];
                    resData.rData["image"] = data[0][0]["image"];
                     resData.rData["dob"] = data[0][0]["dob"];
                      resData.rData["country"] = data[0][0]["country"];
                    // Add more properties as needed
                }
            }
            catch (Exception ex)
            {
                resData.rData["rCode"] = 1;
                resData.rData["rMessage"] = $"Error: {ex.Message}";
            }

            return resData;
        }



    }

}
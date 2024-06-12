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

                var checkSql = $"SELECT * FROM pc_student.et_register WHERE phone=@phone OR email=@email;";
                var checkResult = ds.executeSQL(checkSql, para);


                if (checkResult[0].Count() != 0)
                {
                    resData.rData["rCode"] = 2;
                    resData.rData["rMessage"] = "Duplicate data found";
                }
                else
                {
                    var insertSql = $"INSERT INTO pc_student.et_register( name, phone, email, password) VALUES(@name, @phone, @email, @password);";
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

        
    }
}
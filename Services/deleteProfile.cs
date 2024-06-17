using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace MyCommonStructure.Services
{
    public class deleteProfile
    {
        dbServices ds = new dbServices();

          public async Task<responseData> DeleteProfile(requestData req)
        {
            responseData resData = new responseData();

            try
            {

                string password = req.addInfo["password"].ToString();
                string phone =  req.addInfo["phone"].ToString();


                MySqlParameter[] para = new MySqlParameter[] {
                new MySqlParameter("@Password",password),
                new MySqlParameter("@Phone",phone),
        };
                var delSql = $"DELETE FROM pc_student.et_register WHERE phone=@Phone AND password=@Password;";
                var checkResult = ds.ExecuteSQLName(delSql, para);

                if (checkResult == null || checkResult[0].Count() == 0)
                {

                    resData.rData["rCode"] = 1;
                    resData.rData["rMessage"] = "Invalid Credentials";
                }

                else{
                    resData.rData["rCode"] = 0;
                    resData.rData["rMessage"] = "Profile Deleted Successfull";
                }
            }
            catch (Exception ex)
            {
                resData.rStatus = 199;
                resData.rData["rCode"] = 1;
                resData.rData["rMessage"] = ex.Message.ToString();
            }

            return resData;

        }
    }
}

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;

namespace COMMON_PROJECT_STRUCTURE_API.services
{
    public class changePassword
    {
        dbServices ds = new dbServices();
        public async Task<responseData> ChangePassword(requestData req)
        {
            responseData resData = new responseData();
            resData.eventID = req.eventID;
            resData.rData["rCode"] = 0;
            try
            {
                string id = req.addInfo["id"].ToString();
                string password = req.addInfo["password"].ToString();
                string NewPassword = req.addInfo["NewPassword"].ToString();

                if (password == NewPassword)
                {
                    resData.rData["rCode"] = 1;
                    resData.rData["rMessage"] = "New password must be different from the current password";
                }
                else
                {
                    MySqlParameter[] parameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@id", id),
                        new MySqlParameter("@password", password),
                        new MySqlParameter("@NewPassword", NewPassword),
                    };

                    var checkSql = $"SELECT * FROM pc_student.et_register WHERE id=@id AND password=@password;";
                    var checkResult = ds.executeSQL(checkSql, parameters);
                    if (checkResult[0].Count() == 0)
                    {
                        resData.rData["rCode"] = 2;
                        resData.rData["rMessage"] = "Wrong credentials, enter valid details!";
                    }
                    else
                    {
                        string updateSql = $"UPDATE pc_student.et_register SET password = @NewPassword WHERE id = @id";
                        var rowsAffected = ds.executeSQL(updateSql, parameters);
                        if (rowsAffected[0].Count() != 0)
                        {
                            resData.rData["rCode"] = 3;
                            resData.rData["rMessage"] = "Password didnt changed!";
                        }
                        else
                        {
                            resData.rData["rCode"] = 0;
                            resData.rData["rMessage"] = "Password changed successfully";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                resData.rStatus = 404;
                resData.rData["rCode"] = 1;
                resData.rData["rMessage"] = $"Error: {ex.Message}";
            }
            return resData;
        }
    }
}
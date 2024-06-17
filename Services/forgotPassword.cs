using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace MyCommonStructure
{
    public class forgotPassword
    {
        dbServices ds = new dbServices();

        public async Task<responseData> ForgotPassword(requestData req)
        {
            responseData resData = new responseData();
            resData.rData["rCode"] = 0;
            resData.eventID = req.eventID;

            try
            {
                string userId = req.addInfo["UserId"].ToString();
                string newPassword = req.addInfo["newPassword"].ToString();
                string confirmNewPassword = req.addInfo["confirmNewPassword"].ToString();

                // Check if newPassword and confirmNewPassword match
                if (newPassword != confirmNewPassword)
                {
                    resData.rData["rCode"] = 1;
                    resData.rData["rMessage"] = "New password and confirm password do not match";
                    return resData;
                }

                // Update the password in the database
                MySqlParameter[] myParams = new MySqlParameter[] {
            new MySqlParameter("@UserId", userId),
            new MySqlParameter("@NewPassword", newPassword)
        };

<<<<<<< HEAD
                var sq = $"UPDATE GData.et_register SET password = @NewPassword WHERE phone = @UserId";
=======
                var sq = $"UPDATE pc_student.et_register SET password = @NewPassword WHERE phone = @UserId";
>>>>>>> af050699d26dc41c5345d47111495612def26342
                var affectedRows = ds.ExecuteSQLName(sq, myParams);

                // Check if any rows were affected
                if (affectedRows != null && affectedRows.Count > 0)
                {
                    resData.rData["rCode"] = 0;
                    resData.rData["rMessage"] = "Password updated successfully";
                }
                else
                {
                    resData.rData["rCode"] = 1;
                    resData.rData["rMessage"] = "Failed to update password";
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message.ToString());
                resData.rStatus = 199;
                resData.rData["rMessage"] = "REMOVE THIS ERROR IN PRODUCTION !!!  " + ex.Message.ToString();
            }
<<<<<<< HEAD
=======

>>>>>>> af050699d26dc41c5345d47111495612def26342
            return resData;
        }
    }
}

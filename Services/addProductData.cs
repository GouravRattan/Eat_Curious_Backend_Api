using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace MyCommonStructure.Services
{
    public class addProductData
    {
        dbServices ds = new dbServices();

        public async Task<responseData> AddProductData(requestData req)
        {
            responseData resData = new responseData();
            resData.rData["rCode"] = 0;
            resData.rData["rMessage"] = "Student registered successfully";

            try
            {

                string base64ImageData = req.addInfo["image"].ToString();

                byte[] imageData = Convert.FromBase64String(base64ImageData);

            MySqlParameter[] para = new MySqlParameter[] {
            new MySqlParameter("@product_id", req.addInfo["product_id"].ToString()),
            new MySqlParameter("@product_name", req.addInfo["product_name"].ToString()),
            new MySqlParameter("@description", req.addInfo["description"].ToString()),
            new MySqlParameter("@price", Convert.ToDecimal(req.addInfo["price"])),
            new MySqlParameter("@image", imageData),
            new MySqlParameter("@rating", Convert.ToDecimal(req.addInfo["rating"])),


        };
                    var insertSql = $"INSERT INTO pc_student.et_products    ( product_id, product_name, description, price, image, rating) VALUES(@product_id, @product_name, @description, @price, @image, @rating);";
                    var insertId = ds.ExecuteInsertAndGetLastId(insertSql, para);

                    if (insertId != null)
                    {
                        resData.eventID = req.eventID;
                        resData.rData["rMessage"] = "Product added to the Database Successfully";
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
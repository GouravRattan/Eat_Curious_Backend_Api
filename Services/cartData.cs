using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace MyCommonStructure.Services
{
    public class cartData
    {
        dbServices ds = new dbServices();

        public async Task<responseData> AddItemIntoCart(requestData req)
        {
            responseData resData = new responseData();
            resData.rData["rCode"] = 0;
            resData.rData["rMessage"] = "Item added to the cart";

            try
            {
                MySqlParameter[] para = new MySqlParameter[] {
                    new MySqlParameter("@product_id", req.addInfo["product_id"].ToString())
                };

                var checkSql = $"SELECT product_id FROM pc_student.et_products WHERE product_id=@product_id;";
                var checkResult = ds.executeSQL(checkSql, para);

                if (checkResult[0].Count() != 0)
                {
                    resData.rData["rCode"] = 2;
                    resData.rData["rMessage"] = "Item already exists in the cart";
                }
                else
                {
                    var insertSql = $"INSERT INTO pc_student.et_cart (product_id, product_name, description, price, image, rating) VALUES (@product_id, @product_name, @description, @price, @image, @rating);";
                    
                    // Assuming you need to retrieve product details from req.addInfo
                    var insertParams = new MySqlParameter[] {
                        new MySqlParameter("@product_id", req.addInfo["product_id"].ToString()),
                        new MySqlParameter("@product_name", req.addInfo["product_name"].ToString()),
                        new MySqlParameter("@description", req.addInfo["description"].ToString()),
                        new MySqlParameter("@price", req.addInfo["price"].ToString()),
                        new MySqlParameter("@image", req.addInfo["image"].ToString()),
                        new MySqlParameter("@rating", req.addInfo["rating"].ToString())
                    };

                    var insertId = ds.ExecuteInsertAndGetLastId(insertSql, insertParams);

                    if (insertId == null)
                    {
                        resData.rData["rCode"] = 1;
                        resData.rData["rMessage"] = "Failed to add item to the cart";
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

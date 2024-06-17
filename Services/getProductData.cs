using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Ocsp;

namespace MyCommonStructure.Services
{
    public class getProductData
    {
        dbServices ds = new dbServices();

        public async Task<responseData> GetProductData(requestData req)
        {
            responseData resData = new responseData();
            resData.eventID = req.eventID;

            try
            {
                string product_id = req.addInfo["product_id"].ToString();

            MySqlParameter[] myParams = new MySqlParameter[] {
            new MySqlParameter("@product_id", product_id)
        };
                // Query to fetch product data from the database
                var query = $"SELECT * FROM pc_student.et_products WHERE product_id = @product_id;";

                // Execute the query using the dbServices instance
                var data =  ds.ExecuteSQLName(query, myParams);

                // Set response data
                if (data == null || data[0].Count() == 0)
                {
                    resData.rData["rCode"] = 1;
                    resData.rData["rMessage"] = "Product Not Found...";
                }
                else
                {

                    var product = data[0][0];
                    resData.rData["product_id"] = product["product_id"];
                    resData.rData["product_name"] = product["product_name"];
                    resData.rData["description"] = product["description"];
                    resData.rData["price"] = product["price"];
                    resData.rData["image"] = product["image"];
                    resData.rData["rating"] = product["rating"];

                    
                    resData.rData["rCode"] = 0;
                    resData.rData["rMessage"] = "Product found";
                    
                }

            }
            catch (Exception ex)
            {
                resData.rData["rCode"] = 1;
                resData.rData["rMessage"] = ex.Message;

            }
            return resData;
        }
    }
}

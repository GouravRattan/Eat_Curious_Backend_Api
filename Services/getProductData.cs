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
                // Query to fetch product data from the database
                string query = "SELECT * FROM pc_student.et_products;";

                // Execute the query using the dbServices instance
                var productData = await ds.ExecuteSQLAsync(query, null);

                // Set response data
                resData.rStatus = 200;
                resData.rData["products"] = productData;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                resData.rStatus = 500;
                resData.rData["rCode"] = 1;
                resData.rData["rMessage"] = "Error occurred while fetching product data: " + ex.Message;
            }
            return resData;
        }
    }
}

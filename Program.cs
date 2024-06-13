using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using COMMON_PROJECT_STRUCTURE_API.services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using MyCommonStructure;
using MyCommonStructure.Services;

WebHost.CreateDefaultBuilder().
ConfigureServices(s =>
{
    IConfiguration appsettings = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
    s.AddSingleton<login>();
    s.AddSingleton<Register>();
    s.AddSingleton<forgotPassword>();
    s.AddSingleton<deleteProfile>();
    s.AddSingleton<addProductData>();
    s.AddSingleton<getProductData>();
    s.AddSingleton<cartData>();

    s.AddAuthorization();
    s.AddControllers();
    s.AddCors();
    s.AddAuthentication("SourceJWT").AddScheme<SourceJwtAuthenticationSchemeOptions, SourceJwtAuthenticationHandler>("SourceJWT", options =>
        {
            options.SecretKey = appsettings["jwt_config:Key"].ToString();
            options.ValidIssuer = appsettings["jwt_config:Issuer"].ToString();
            options.ValidAudience = appsettings["jwt_config:Audience"].ToString();
            options.Subject = appsettings["jwt_config:Subject"].ToString();
        });
}).Configure(app =>
{
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseCors(options =>
             options.WithOrigins("https://localhost:5001", "http://localhost:5002")
            // options.WithOrigins("*")
            .AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
    app.UseRouting();
    app.UseStaticFiles();

    app.UseEndpoints(e =>
    {
        var login = e.ServiceProvider.GetRequiredService<login>();

        e.MapPost("login",
            [AllowAnonymous] async (HttpContext http) =>
            {
                var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
                requestData rData = JsonSerializer.Deserialize<requestData>(body);
                if (rData.eventID == "1002") // update
                    await http.Response.WriteAsJsonAsync(await login.Login(rData));

            });


        var register = e.ServiceProvider.GetRequiredService<Register>();
        e.MapPost("registration",
        [AllowAnonymous] async (HttpContext http) =>
        {
            var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
            requestData rData = JsonSerializer.Deserialize<requestData>(body);
            if (rData.eventID == "1006") // update
                await http.Response.WriteAsJsonAsync(await register.Registration(rData));
        });


        var forgotPassword = e.ServiceProvider.GetRequiredService<forgotPassword>();
        e.MapPost("forgotPassword", [AllowAnonymous] async (HttpContext http) =>
        {
            var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
            requestData rData = JsonSerializer.Deserialize<requestData>(body);
            if (rData.eventID == "1007") // forgot Password
                await http.Response.WriteAsJsonAsync(await forgotPassword.ForgotPassword(rData));
        });


        var deleteProfile = e.ServiceProvider.GetRequiredService<deleteProfile>();
        e.MapPost("deleteProfile", [AllowAnonymous] async (HttpContext http) =>
        {
            var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
            requestData rData = JsonSerializer.Deserialize<requestData>(body);
            if (rData.eventID == "1009") await http.Response.WriteAsJsonAsync(await deleteProfile.DeleteProfile(rData));
        });



        var addProductData = e.ServiceProvider.GetRequiredService<addProductData>();
        e.MapPost("addProductData", [AllowAnonymous] async (HttpContext http) =>
        {
            var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
            requestData rData = JsonSerializer.Deserialize<requestData>(body);
            if (rData.eventID == "1001") await http.Response.WriteAsJsonAsync(await addProductData.AddProductData(rData));
        });


        // var getProductData = e.ServiceProvider.GetRequiredService<getProductData>();
        // e.MapPost("getProductData", [AllowAnonymous] async (HttpContext http) =>
        // {
        //     try
        //     {
        //         var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
        //         requestData rData = JsonSerializer.Deserialize<requestData>(body);
        //         if (rData != null && rData.eventID == "1004")
        //         {
        //             var response = await getProductData.GetProductData(rData);
        //             await http.Response.WriteAsJsonAsync(response);
        //         }
        //         else
        //         {
        //             http.Response.StatusCode = 400; // Bad Request
        //             await http.Response.WriteAsJsonAsync(new { rStatus = 400, rMessage = "Invalid request data" });
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine(ex.Message);
        //         http.Response.StatusCode = 500; // Internal Server Error
        //         await http.Response.WriteAsJsonAsync(new { rStatus = 500, rMessage = "Internal server error" });
        //     }
        // });



        var cartData = e.ServiceProvider.GetRequiredService<cartData>();
        e.MapPost("cartData ",
        [AllowAnonymous] async (HttpContext http) =>
        {
            var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
            requestData rData = JsonSerializer.Deserialize<requestData>(body);
            if (rData.eventID == "1001") // update
                await http.Response.WriteAsJsonAsync(await cartData.AddItemIntoCart(rData));
        });




        e.MapGet("/bing",
          async c => await c.Response.WriteAsJsonAsync("{'Name':'Anish','Age':'26','Project':'COMMON_PROJECT_STRUCTURE_API'}"));
    });
}).Build().Run();

public record requestData
{
    [Required]
    public string eventID { get; set; }
    [Required]  
    public IDictionary<string, object> addInfo { get; set; }
}

public record responseData
{
    public responseData()
    {
        eventID = "";
        rStatus = 0;
        rData = new Dictionary<string, object>();
    }
    [Required]
    public int rStatus { get; set; } = 0;
    public string eventID { get; set; }
    public IDictionary<string, object> addInfo { get; set; }
    public IDictionary<string, object> rData { get; set; }
}

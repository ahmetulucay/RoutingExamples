
using RoutingExample.CustomConstraints;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRouting(options =>
{
    options.ConstraintMap.Add("months", typeof(MonthsCustomConstraints));
});
var app = builder.Build();

//enable routing
app.UseRouting();

//create endpoints
app.UseEndpoints(endpoints =>
{
    //add your end points -- endpoints.Map(), endpoints.MapGet(), endpoints.MapPost() etc..

    #region Extension
    endpoints.Map("files/{filename}.{extension}", async context =>
    {
        string? filename = Convert.ToString(context.Request.RouteValues["filename"]);
        string? extension = Convert.ToString(context.Request.RouteValues["extension"]);

        await context.Response.WriteAsync($"In files - {filename}.{extension}");
    });
    #endregion

    #region Exercise-1 (Default value)
    endpoints.Map("employee/profile/{EmployeeName=Jack}", async context =>
    {
        string? employeeName = Convert.ToString(context.Request.RouteValues["EmployeeName"]);
        await context.Response.WriteAsync($"In Employee profile - {employeeName}");
    });
    endpoints.Map("products/details/{id=1}", async context =>
    {
        int id = Convert.ToInt32(context.Request.RouteValues["id"]);
        await context.Response.WriteAsync($"Product id is {id}");
    });
    endpoints.Map("daily-digest-report/{reportdate:datetime}",
    async context =>
    {
    DateTime reportDate = Convert.ToDateTime(context.Request.RouteValues["reportdate"]);
        await context.Response.WriteAsync($"In-daily-digest-report - {reportDate.ToShortDateString()}");
    });

    #endregion

    #region Exercise-2 (Optional Parameter-1)
    endpoints.Map("products/details/{id?}", async context =>
    {
        if (context.Request.RouteValues.ContainsKey("id"))
        {
            int id = Convert.ToInt32(context.Request.RouteValues["id"]);
            await context.Response.WriteAsync($"Product details -  {id}");
        }
        else
        {
            await context.Response.WriteAsync("Product details is not supplied.");
        }
    });
    #endregion

    #region Exercise-3 (Optional Parameter-2)
    endpoints.Map("employee/profile/{EmployeeName?}", async context =>
    {
        if (context.Request.RouteValues.ContainsKey("EmployeeName"))
        {
            string? employeeName = Convert.ToString(context.Request.RouteValues["EmployeeName"]);
            await context.Response.WriteAsync($"In Employee profile - {employeeName}");
        }
        else
        {
            await context.Response.WriteAsync("Employee name is not supplied.");
        }
    });
    endpoints.Map("products/details/{id?}", async context =>
    {
        if (context.Request.RouteValues.ContainsKey("id"))
        {
            int id = Convert.ToInt32(context.Request.RouteValues["id"]);
            await context.Response.WriteAsync($"Product details -  {id}");
        }
        else
        {
            await context.Response.WriteAsync("Product details is not supplied.");
        }
    });
    #endregion

    #region Exercise-4 (Route Constraints)
    //Instead of "regex(^(apr|jul|oct)$)" we used "months" predefined constraint here.
    endpoints.Map("sales-report/{year:int:min(1900)}/{month:months}", async context =>
    {
        int year = Convert.ToInt32(context.Request.RouteValues["year"]);
        string? month = Convert.ToString(context.Request.RouteValues["month"]);
        await context.Response.WriteAsync($"Sales-report - {year} - {month}");
    });
    #endregion

    #region Exercise-5 (EndPoint Selection Order)
    //sales-report/2024/jan 
    //Url template a/b/c is higher than(>) a/b, a/b is > a/{parameter}, a/{b}:int > a/{b}, a/{b} > a/***
    endpoints.Map("sales-report/2024/jan", async context =>
    {
        await context.Response.WriteAsync("Sales-report for 2024 - jan");
    });
    #endregion
});

app.Run(async context =>
{
    //await context.Response.WriteAsync($"Request received at {context.Request.Path}");
    await context.Response.WriteAsync($"No route matched at {context.Request.Path}");
});

app.Run();

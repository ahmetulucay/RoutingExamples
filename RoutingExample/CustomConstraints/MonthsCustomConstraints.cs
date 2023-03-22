using System.Text.RegularExpressions;

#region //Exercise-5 (Custom Constraints)

namespace RoutingExample.CustomConstraints;
public class MonthsCustomConstraints : IRouteConstraint
{
    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
    {
        //Check value exists
        if (!values.ContainsKey(routeKey)) //month
        {
            return false; //not a match
        }

        Regex regex = new Regex("^(apr|jul|oct|jan)$");
        string? monthValue = Convert.ToString(values[routeKey]);

        if (regex.IsMatch(monthValue))
        {
            return true; // it is a match
        }
        return false; // not match
    }
}
#endregion

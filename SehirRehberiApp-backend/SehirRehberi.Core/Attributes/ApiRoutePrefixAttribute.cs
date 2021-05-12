using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SehirRehberi.Core.Attributes
{
    public class ApiRoutePrefixAttribute : RouteAttribute
    {
        private const string RouteBase = "api";
        private const string PrefixRouteBase = RouteBase + "/";

        public ApiRoutePrefixAttribute(string routePostfix) 
            : base(string.IsNullOrWhiteSpace(routePostfix) ? RouteBase : PrefixRouteBase + routePostfix)
        {
        }
    }
}

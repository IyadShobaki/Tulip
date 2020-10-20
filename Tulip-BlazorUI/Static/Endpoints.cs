using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tulip_BlazorUI.Static
{
    public static class Endpoints
    {
        public static string BaseUrl = "https://localhost:44353/";
        public static string CategoriesEndpoint = $"{BaseUrl}api/categories/";
        public static string ProductsEndpoint = $"{BaseUrl}api/products/";
        public static string RegisterEndpoint = $"{BaseUrl}api/users/register/";
        public static string LoginEndpoint = $"{BaseUrl}api/users/login/";

    }
}

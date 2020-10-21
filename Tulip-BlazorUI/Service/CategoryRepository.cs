using Blazored.LocalStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Tulip_BlazorUI.Contratcs;
using Tulip_BlazorUI.Models;

namespace Tulip_BlazorUI.Service
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        private readonly IHttpClientFactory _client;
        private readonly ILocalStorageService _localStorage;

        public CategoryRepository(IHttpClientFactory client, 
            ILocalStorageService localStorage) : base(client, localStorage)
        {
            _client = client;
            _localStorage = localStorage;
        }
    }
}

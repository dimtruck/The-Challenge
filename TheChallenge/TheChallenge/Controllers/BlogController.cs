using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace TheChallenge.Controllers
{
    public class BlogController : ApiController
    {
        // GET /api/blog
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET /api/blog/5
        public string Get(int id)
        {
            return "value";
        }

        // POST /api/blog
        public void Post(string value)
        {
        }

        // PUT /api/blog/5
        public void Put(int id, string value)
        {
        }

        // DELETE /api/blog/5
        public void Delete(int id)
        {
        }
    }
}

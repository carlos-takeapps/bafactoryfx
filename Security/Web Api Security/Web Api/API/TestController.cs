using BAFactory.Fx.Samples.WebApiSecurity.WebApi.Filters;
using BAFactory.Fx.Samples.WebApiSecurity.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace BAFactory.Fx.Samples.WebApiSecurity.WebApi
{
    public class TestController : ApiController
    {
        [HttpGet]
        [Authorize]
        [Route("api/test/{name}")]
        public string Get([FromUri]string name)
        {
            return string.Concat("Hello, ", name);
        }

        [HttpPost]
        [Authorize]
        [HMacSignedMessgeFilter]
        [Route("api/test")]
        public string Post(PersonModel person)
        {
            return string.Concat("Hello, ", person.FirstName, " ", person.LastName);
        }
    }
}

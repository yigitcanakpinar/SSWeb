using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaatveSaatWeb.Controllers
{
    public class RestServices
    {

        private RestClient client = new RestClient("http://localhost:1994/api/");
        public class reqParameters
        {
            public string name { get; set; }
            public string value { get; set; }
        }


        public IRestResponse RestServicesExexReq(string apiPath, RestSharp.Method method)
        {

            try
            {
                var request = new RestRequest(apiPath, method);

                return client.Execute(request);
            }
            catch (Exception)
            {
                return null;
            }

        }

        public IRestResponse RestServicesExexReqParam(string apiPath, RestSharp.Method method, List<reqParameters> reqParameters)
        {

            try
            {
                var request = new RestRequest(apiPath, method);

                foreach (var i in reqParameters)
                {
                    request.AddQueryParameter(i.name, i.value);
                }

                return client.Execute(request);
            }
            catch (Exception)
            {
                return null;
            }

        }

    }
}
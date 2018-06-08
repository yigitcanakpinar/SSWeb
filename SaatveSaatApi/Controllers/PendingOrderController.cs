using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SaatveSaatApi.Controllers
{
    [RoutePrefix("pendingorder")]
    public class PendingOrderController : ApiController
    {

        private SaatvesaatDBEntities db = new SaatvesaatDBEntities();

        private HttpResponseMessage GetForPendingOrderToId(int id)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;

                if (id <= 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, db.PendingOrder.Include("product").ToList());
                }
                else
                {
                    var q = db.PendingOrder.Include("product").FirstOrDefault(X => X.OrderId == id);

                    if (q == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Pending Order (Id = {id}) is not finded !");
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, q);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex);
            }
        }

        [HttpGet]
        public HttpResponseMessage Get(int ? id = 0)
        {

            return GetForPendingOrderToId(id.Value);

        }

    }
}

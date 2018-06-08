using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SaatveSaatApi.Controllers
{
    [RoutePrefix("completedorder")]
    public class CompletedOrderController : ApiController
    {

        private SaatvesaatDBEntities db = new SaatvesaatDBEntities();

        private HttpResponseMessage GetForCompletedOrderAll()
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;


                var q = db.CompletedOrder.Include("Product").ToList();

                if (q.Count() <= 0)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Completed Order is not finded !");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, q);
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex);
            }
        }

        private HttpResponseMessage GetForCompletedOrderToId(int id)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;

                if (id <= 0)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Completed Order (Id = {id}) is invalid !");
                }
                else
                {
                    var q = db.CompletedOrder.Include("Product").FirstOrDefault(x => x.OrderId == id);

                    if (q == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Completed Order (Id = {id}) is not finded !");
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
        public HttpResponseMessage Get(int? id = 0)
        {
            if (id.Value <= 0)
            {
                return GetForCompletedOrderAll();
            }
            else
            {
                return GetForCompletedOrderToId(id.Value);
            }

        }

        [HttpPost]
        public HttpResponseMessage Post([FromUri]int? id = 0)
        {

            try
            {
                var tempquery = db.PendingOrder.FirstOrDefault(x => x.OrderId == id);

                db.PendingOrder.Remove(tempquery);

                db.CompletedOrder.Add(new CompletedOrder()
                {
                    OrderId = tempquery.OrderId,
                    CompletedOrderProductId = tempquery.PendingOrderProductId,
                    CompletedOrderCreateDate = System.DateTime.Now,
                    CompletedOrderProductPiece = tempquery.PendingOrderProductPiece
                });

                if (db.SaveChanges() >= 2)
                {
                    var query = db.CompletedOrder.Include("Product").FirstOrDefault(x => x.OrderId == id);
                    return Request.CreateResponse(HttpStatusCode.OK, query);            
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadGateway, "Completed Order error !");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex);
            }
            
        }

    }
}

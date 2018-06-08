using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SaatveSaatApi.Controllers
{
    
    [RoutePrefix("api/brand")]
    [EnableCors( origins: "*", headers: "*", methods: "*")]
    public class BrandController : ApiController
    {

        private SaatvesaatDBEntities db = new SaatvesaatDBEntities();

        private HttpResponseMessage GetForBrandToId(int id)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;

                if (id <= 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, db.Brand.ToList());
                }
                else
                {
                    var q = db.Brand.FirstOrDefault(X => X.BrandId == id);

                    if (q == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Brand (Id = {id}) is not finded !");
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

        private HttpResponseMessage GetForBrandToName(string name)
        {

            try
            {
                db.Configuration.ProxyCreationEnabled = false;

                if (name == null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, db.Brand.ToList());
                }
                else
                {
                    var q = db.Brand.FirstOrDefault(X => X.BrandName.ToLower() == name.ToLower());

                    if (q == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Brand (Name = {name}) is not finded !");
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
        public HttpResponseMessage Get([FromUri]int? id = 0, [FromUri]string name = null)
        {

            if (id >= 1)
            {
                return GetForBrandToId(id.Value);
            }
            else
            {
                return GetForBrandToName(name);
            }

        }

        [HttpGet]
        [Route("{id:int:min(1)}")]
        public HttpResponseMessage Get(int id)
        {
            return GetForBrandToId(id);
        }

        [HttpGet]
        [Route("{name}")]
        public HttpResponseMessage Get(string name)
        {
            return GetForBrandToName(name);
        }

        public HttpResponseMessage Post([FromBody]Brand brand)
        {

            try
            {
                if (brand == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Data is not finded at request body !");
                }
                else
                {
                    db.Brand.Add(brand);

                    if (db.SaveChanges() > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, brand);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Brand (Name = {brand.BrandName}) is not added !");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex);
            }

        }

        private HttpResponseMessage PutForBrandToId(int id, Brand brand)
        {

            try
            {
                if (brand == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Data is not finded at request body !");
                }
                else
                {
                    if (id <= 0)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Brand (BrandId = {id}) is not valid !");
                    }
                    else
                    {
                        var query = db.Brand.FirstOrDefault(x => x.BrandId == id);

                        if (query == null)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Brand (Id = {id}) is not finded !");
                        }
                        else
                        {
                            if (brand.BrandName != null) { query.BrandName = brand.BrandName; }
                            if (brand.BrandImage != null) { query.BrandImage = brand.BrandImage; }

                            if (db.SaveChanges() > 0)
                            {
                                return Request.CreateResponse(HttpStatusCode.OK, query);
                            }
                            else
                            {
                                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Brand (Id = {id}) is not updated !");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex);
            }

        }

        private HttpResponseMessage PutForBrandToName(string name, Brand brand)
        {

            try
            {
                if (brand == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Data is not finded at request body !");
                }
                else
                {
                    if (name == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Brand (BrandName = {name}) is not valid !");
                    }
                    else
                    {
                        var query = db.Brand.FirstOrDefault(x => x.BrandName == name);

                        if (query == null)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Brand (name = {name}) is not finded !");
                        }
                        else
                        {
                            if (brand.BrandName != null) { query.BrandName = brand.BrandName; }
                            if (brand.BrandImage != null) { query.BrandImage = brand.BrandImage; }

                            if (db.SaveChanges() > 0)
                            {
                                return Request.CreateResponse(HttpStatusCode.OK, query);
                            }
                            else
                            {
                                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Brand (name = {name}) is not updated !");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex);
            }

        }

        [HttpPut]
        public HttpResponseMessage Put([FromBody]Brand brand, [FromUri]int? id = 0, [FromUri]string name = null)
        {
            if (id >= 1)
            {
                return PutForBrandToId(id.Value, brand);
            }
            else if (name != null)
            {
                return PutForBrandToName(name, brand);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Parameter is not valid ! Please use Brand Id (id) or Brand Name (name) .");
            }
        }

        [HttpPut]
        [Route("{id:int:min(1)}")]
        public HttpResponseMessage Put(int id, [FromBody]Brand brand)
        {
            return PutForBrandToId(id, brand);
        }

        [HttpPut]
        [Route("{name}")]
        public HttpResponseMessage Put(string name, [FromBody]Brand brand)
        {
            return PutForBrandToName(name, brand);
        }

        private HttpResponseMessage DeleteForBrandToId(int id)
        {

            try
            {
                if (id <= 0)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Brand (BrandId = {id}) is not valid !");
                }
                else
                {
                    var query = db.Brand.FirstOrDefault(x => x.BrandId == id);

                    if (query == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Brand (Id = {id}) is not finded !");
                    }
                    else
                    {
                        db.Brand.Remove(query);
                        db.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, $"Brand (id = {id}) is deleted");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex);
            }

        }

        private HttpResponseMessage DeleteForBrandToName(string name)
        {

            try
            {
                if (name == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Brand (BrandName = {name}) is not valid !");
                }
                else
                {
                    var query = db.Brand.FirstOrDefault(x => x.BrandName == name);

                    if (query == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Brand (Name = {name}) is not finded !");
                    }
                    else
                    {
                        db.Brand.Remove(query);
                        db.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, $"Brand (Name = {name}) is deleted");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex);
            }

        }

        [HttpDelete]
        public HttpResponseMessage Delete([FromBody]Brand brand, [FromUri]int? id = 0, [FromUri]string name = null)
        {
            if (brand == null)
            {
                if (id >= 1)
                {
                    return DeleteForBrandToId(id.Value);
                }
                else if (name != null)
                {
                    return DeleteForBrandToName(name);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Parameter is not valid !Please use Brand Id(id = {id}) or Brand Name(name = {name}).");
                }
            }
            else
            {
                if (brand.BrandId >= 1)
                {
                    return DeleteForBrandToId(brand.BrandId);
                }
                else if (brand.BrandName != null)
                {
                    return DeleteForBrandToName(brand.BrandName);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Parameter is not valid !Please use Brand Id(id = {brand.BrandId}) or Brand Name(name = {brand.BrandName}).");
                }
            }


        }

        [HttpDelete]
        [Route("{id:int:min(1)}")]
        public HttpResponseMessage Delete(int id)
        {
            return DeleteForBrandToId(id);
        }

        [HttpDelete]
        [Route("{name}")]
        public HttpResponseMessage Delete(string name)
        {
            return DeleteForBrandToName(name);
        }

    }
}

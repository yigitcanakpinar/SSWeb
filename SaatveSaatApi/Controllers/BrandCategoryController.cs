using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SaatveSaatApi.Controllers
{
    [RoutePrefix("api/brandcategory")]
    public class BrandCategoryController : ApiController
    {

        private SaatvesaatDBEntities db = new SaatvesaatDBEntities();

        private HttpResponseMessage GetForBrandCategoryToId(int id)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;

                if (id <= 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, db.BrandCategory.Include("Brand").Include("Category").ToList());
                }
                else
                {
                    var query = db.BrandCategory.Include("Brand").Include("Category").FirstOrDefault(x => x.BrandCategoryId == id);

                    if (query == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"BrandCategory (Id = {id}) is not finded !");
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, query);
                    }
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex);
            }
        }

        private HttpResponseMessage GetForBrandCategoryToBrandId(int id)
        {

            try
            {
                if (id <= 0)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"BrandCategory (BrandId = {id}) is not valid !");
                }
                else
                {
                    db.Configuration.ProxyCreationEnabled = false;

                    var query = db.BrandCategory.Include("Brand").Include("Category").Where(x => x.BrandCategoryBrandId == id);

                    if (query.ToArray<BrandCategory>().Count() == 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, $"BrandCategory (BrandId = {id}) is not finded !)");
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, query);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex);
            }

        }

        private HttpResponseMessage GetForBrandCategoryToCategoryId(int id)
        {

            try
            {
                if (id <= 0)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"BrandCategory (CAtegoryId = {id}) is not valid !");
                }
                else
                {
                    var query = db.BrandCategory.Include("Brand").Include("Category").Where(x => x.BrandCategoryCategoryId == id);

                    if (query.ToArray<BrandCategory>().Count() == 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, $"BrandCategory (CategoryId = {id}) is not finded !");
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, query);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex);
            }

        }

        [HttpGet]
        public HttpResponseMessage Get([FromUri]int? id = 0, [FromUri]int? bid = 0, [FromUri]int? cid = 0)
        {

            if (id >= 1)
            {
                return GetForBrandCategoryToId(id.Value);
            }
            else if (bid >= 1)
            {
                return GetForBrandCategoryToBrandId(bid.Value);
            }
            else if (cid >= 1)
            {
                return GetForBrandCategoryToCategoryId(cid.Value);
            }
            else
            {
                var allData = Request.RequestUri.LocalPath.Replace("/api/brandcategory", null).ToString();

                if (allData.Equals("") == true || allData.Equals("/") == true)
                {
                    return GetForBrandCategoryToId(0);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Parameter is not valid !Please use BrandCategory Id(id = {id}) or Brand Id (bid = {bid}) or Category Id (cid = {cid})");
                }
            }

        }

        [HttpGet]
        [Route("{id:int:min(1)}")]
        public HttpResponseMessage Get(int id)
        {
            return GetForBrandCategoryToId(id);
        }

        [HttpGet]
        [Route("brand/{id:int:min(1)}")]
        public HttpResponseMessage GetBrand(int id)
        {
            return GetForBrandCategoryToBrandId(id);
        }

        [HttpGet]
        [Route("category/{id:int:min(1)}")]
        public HttpResponseMessage GetCategory(int id)
        {
            return GetForBrandCategoryToCategoryId(id);
        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody]BrandCategory brandcategory)
        {

            try
            {
                if (brandcategory == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Data is not finded at request body !");
                }
                else
                {
                    db.BrandCategory.Add(brandcategory);

                    if (db.SaveChanges() > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, brandcategory);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"BrandCategory is not added !");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex);
            }

        }

        private HttpResponseMessage PutForBrandCategoryToId(int id, BrandCategory brandcategory)
        {

            try
            {
                if (brandcategory == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Data is not finded at request body !");
                }
                else
                {
                    if (id <= 0)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"BrandCategory (Id = {id}) is not valid !");
                    }
                    else
                    {
                        var query = db.BrandCategory.FirstOrDefault(x => x.BrandCategoryId == id);

                        if (query == null)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"BrandCategory (Id = {id}) is not finded !");
                        }
                        else
                        {
                            query.BrandCategoryCategoryId = brandcategory.BrandCategoryCategoryId;
                            query.BrandCategoryBrandId = brandcategory.BrandCategoryBrandId;

                            if (db.SaveChanges() > 0)
                            {
                                return Request.CreateResponse(HttpStatusCode.OK, query);
                            }
                            else
                            {
                                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"BrandCategory (Id = {id}) is not updated !");
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

        private HttpResponseMessage PutForBrandCategoryToBrandId(int id, BrandCategory brandcategory)
        {
            try
            {
                if (brandcategory == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Data is not finded at request body !");
                }
                else
                {
                    if (id <= 0)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"BrandCategory (BrandId = {id}) is not valid !");
                    }
                    else
                    {
                        var query = db.BrandCategory.FirstOrDefault(x => x.BrandCategoryBrandId == id);

                        if (query == null)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"BrandCategory (BrandId = {id}) is not finded !");
                        }
                        else
                        {
                            query.BrandCategoryCategoryId = brandcategory.BrandCategoryCategoryId;
                            query.BrandCategoryBrandId = brandcategory.BrandCategoryBrandId;

                            if (db.SaveChanges() > 0)
                            {
                                return Request.CreateResponse(HttpStatusCode.OK, query);
                            }
                            else
                            {
                                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"BrandCategory (BrandId = {id}) is not updated !");
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

        private HttpResponseMessage PutForBrandCategoryToCategoryId(int id, BrandCategory brandcategory)
        {

            try
            {
                if (brandcategory == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Data is not finded at request body !");
                }
                else
                {
                    if (id <= 0)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"BrandCategory (BrandId = {id}) is not valid !");
                    }
                    else
                    {
                        var query = db.BrandCategory.FirstOrDefault(x => x.BrandCategoryCategoryId == id);

                        if (query == null)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"BrandCategory (BrandId = {id}) is not finded !");
                        }
                        else
                        {
                            query.BrandCategoryCategoryId = brandcategory.BrandCategoryCategoryId;
                            query.BrandCategoryBrandId = brandcategory.BrandCategoryBrandId;

                            if (db.SaveChanges() > 0)
                            {
                                return Request.CreateResponse(HttpStatusCode.OK, query);
                            }
                            else
                            {
                                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"BrandCategory (BrandId = {id}) is not updated !");
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
        public HttpResponseMessage Put([FromBody]BrandCategory brandcategory, [FromUri]int? id = 0, [FromUri]int? bid = 0, [FromUri]int? cid = 0)
        {

            if (id >= 1)
            {
                return PutForBrandCategoryToId(id.Value, brandcategory);
            }
            else if (bid >= 1)
            {
                return PutForBrandCategoryToBrandId(bid.Value, brandcategory);
            }
            else if (cid >= 1)
            {
                return PutForBrandCategoryToCategoryId(cid.Value, brandcategory);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Parameter is not valid !Please use BrandCategory Id(id = {id}) or Brand Id (bid = {bid}) or Category Id (cid = {cid})");
            }

        }

        [HttpPut]
        [Route("brand/{bid}")]
        public HttpResponseMessage PutBrand([FromBody]BrandCategory brandcategory, [FromUri]String bid = null)
        {

            int res;

            if (Int32.TryParse(bid, out res) == true)
            {
                return PutForBrandCategoryToBrandId(res, brandcategory);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Parameter is not valid !Please use Brand Id (bid = {bid}) ");
            }

        }

        [HttpPut]
        [Route("category/{cid}")]
        public HttpResponseMessage PutCategory([FromBody]BrandCategory brandcategory, [FromUri]String cid = null)
        {

            int res;

            if (Int32.TryParse(cid, out res) == true)
            {
                return PutForBrandCategoryToCategoryId(res, brandcategory);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Parameter is not valid !Please use Category Id (bid = {cid}) ");
            }

        }

        private HttpResponseMessage DeleteForBrandCategoryToId(int id)
        {

            try
            {
                if (id <= 0)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"BrandCategory (Id = {id}) is not valid !");
                }
                else
                {
                    db.Configuration.ProxyCreationEnabled = false;

                    var query = db.BrandCategory.FirstOrDefault(x => x.BrandCategoryId == id);

                    if (query == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"BrandCategory (Id = {id}) is not finded !");
                    }
                    else
                    {
                        db.BrandCategory.Remove(query);
                        db.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, $"BrandCategory (Id = {id}) is deleted");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex);
            }

        }

        private HttpResponseMessage DeleteForBrandCategoryToBrandId(int id)
        {

            try
            {
                if (id <= 0)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"BrandCategory (Brand Id = {id}) is not valid !");
                }
                else
                {
                    var query = db.BrandCategory.FirstOrDefault(x => x.BrandCategoryBrandId == id);

                    if (query == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"BrandCategory (Brand Id = {id}) is not finded !");
                    }
                    else
                    {
                       // foreach (BrandCategory a in query)
                       // {
                            db.BrandCategory.Remove(query);
                            db.SaveChanges();
                        //}


                        return Request.CreateResponse(HttpStatusCode.OK, $"BrandCategory (Brand Id = {id}) is deleted");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex);
            }

        }

        private HttpResponseMessage DeleteForBrandCategoryToCategoryId(int id)
        {

            try
            {
                if (id <= 0)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"BrandCategory (Category Id = {id}) is not valid !");
                }
                else
                {
                    var query = db.BrandCategory.FirstOrDefault(x => x.BrandCategoryCategoryId == id);

                    if (query == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"BrandCategory (Category Id = {id}) is not finded !");
                    }
                    else
                    {
                        db.BrandCategory.Remove(query);
                        db.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, $"BrandCategory (Category Id = {id}) is deleted");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex);
            }

        }

        [HttpDelete]
        public HttpResponseMessage Delete([FromBody]BrandCategory brandcategory, [FromUri]int? id = 0, [FromUri]int? bid = 0, [FromUri]int? cid = 0)
        {
            if (brandcategory == null)
            {
                if (id.Value >= 1)
                {
                    return DeleteForBrandCategoryToId(id.Value);
                }
                else if (bid >= 1)
                {
                    return DeleteForBrandCategoryToBrandId(bid.Value);
                }
                else if (cid >= 1)
                {
                    return DeleteForBrandCategoryToCategoryId(cid.Value);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Parameter is not valid !Please use BrandCategoryId (id = {id}) or BrandId (bid = {bid}) or CategoryId (cid = {cid}) .");
                }
            }
            else
            {
                if (brandcategory.BrandCategoryId >= 1)
                {
                    return DeleteForBrandCategoryToId(brandcategory.BrandCategoryId);
                }
                else if (brandcategory.BrandCategoryBrandId >= 1)
                {
                    return DeleteForBrandCategoryToBrandId(brandcategory.BrandCategoryBrandId);
                }
                else if (brandcategory.BrandCategoryCategoryId >= 1)
                {
                    return DeleteForBrandCategoryToCategoryId(brandcategory.BrandCategoryCategoryId);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Parameter is not valid !Please use BrandCategoryId (id = {brandcategory.BrandCategoryId}) or BrandId (bid = {brandcategory.BrandCategoryBrandId}) or CategoryId (cid = {brandcategory.BrandCategoryCategoryId}) .");
                }
            }

        }

        [HttpDelete]
        [Route("brand/{bid}")]
        public HttpResponseMessage DeleteBrand([FromUri]String bid = null)
        {

            int res;

            if (Int32.TryParse(bid, out res) == true)
            {
                return DeleteForBrandCategoryToBrandId(res);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Parameter is not valid !Please use Brand Id (bid = {bid}) ");
            }

        }

        [HttpDelete]
        [Route("category/{cid}")]
        public HttpResponseMessage DeleteCategory([FromUri]String cid = null)
        {

            int res;

            if (Int32.TryParse(cid, out res) == true)
            {
                return DeleteForBrandCategoryToCategoryId(res);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Parameter is not valid !Please use Category Id (bid = {cid}) ");
            }

        }

    }
}

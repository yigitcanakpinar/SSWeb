using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SaatveSaatApi.Controllers
{
    [RoutePrefix("api/category")]
    public class CategoryController : ApiController
    {

        SaatvesaatDBEntities db = new SaatvesaatDBEntities();

        private HttpResponseMessage GetForCategoryToId(int id)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;

                if (id <= 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, db.Category.ToList());
                }
                else
                {
                    var q = db.Category.FirstOrDefault(X => X.CategoryId == id);

                    if (q == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Category (Id = {id}) is not finded !");
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

        private HttpResponseMessage GetForCategoryToName(string name)
        {

            try
            {
                db.Configuration.ProxyCreationEnabled = false;

                if (name == null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, db.Category.ToList());
                }
                else
                {
                    var q = db.Category.FirstOrDefault(X => X.CategoryName.ToLower() == name.ToLower());

                    if (q == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Category (Name = {name}) is not finded !");
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
                return GetForCategoryToId(id.Value);
            }
            else
            {
                return GetForCategoryToName(name);
            }

        }

        [HttpGet]
        [Route("{id:int:min(1)}")]
        public HttpResponseMessage Get(int id)
        {
            return GetForCategoryToId(id);
        }

        [HttpGet]
        [Route("{name}")]
        public HttpResponseMessage Get(string name)
        {
            return GetForCategoryToName(name);
        }

        [HttpGet]
        [Route("brand/{name}")]
        public HttpResponseMessage GetBrandName(string name)
        {

            try
            {
                var query = from c in db.Category
                            join bc in db.BrandCategory on c.CategoryId equals bc.BrandCategoryCategoryId
                            join b in db.Brand on bc.BrandCategoryBrandId equals b.BrandId
                            where b.BrandName == name
                            select c;
                if (query == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"BrandName (Name = {name}) is not found Categorys !");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, query);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex);
            }

        }

        public HttpResponseMessage Post([FromBody]Category category)
        {

            try
            {
                if (category == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Data is not finded at request body !");
                }
                else
                {
                    db.Category.Add(category);

                    if (db.SaveChanges() > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, category);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Category (Name = {category.CategoryName}) is not added !");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex);
            }

        }

        private HttpResponseMessage PutForCategoryToId(int id, Category category)
        {

            try
            {
                if (category == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Data is not finded at request body !");
                }
                else
                {
                    if (id <= 0)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Category (CategoryId = {id}) is not valid !");
                    }
                    else
                    {
                        var query = db.Category.FirstOrDefault(x => x.CategoryId == id);

                        if (query == null)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Category (Id = {id}) is not finded !");
                        }
                        else
                        {
                            if (category.CategoryName != null) { query.CategoryName = category.CategoryName; }

                            if (db.SaveChanges() > 0)
                            {
                                return Request.CreateResponse(HttpStatusCode.OK, query);
                            }
                            else
                            {
                                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Category (Id = {id}) is not updated !");
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

        private HttpResponseMessage PutForCategoryToName(string name, Category category)
        {

            try
            {
                if (category == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Data is not finded at request body !");
                }
                else
                {
                    if (name == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Category (CategoryName = {name}) is not valid !");
                    }
                    else
                    {
                        var query = db.Category.FirstOrDefault(x => x.CategoryName == name);

                        if (query == null)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Category (name = {name}) is not finded !");
                        }
                        else
                        {
                            if (category.CategoryName != null) { query.CategoryName = category.CategoryName; }

                            if (db.SaveChanges() > 0)
                            {
                                return Request.CreateResponse(HttpStatusCode.OK, query);
                            }
                            else
                            {
                                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Category (name = {name}) is not updated !");
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
        public HttpResponseMessage Put([FromBody]Category category, [FromUri]int? id = 0, [FromUri]string name = null)
        {
            if (id >= 1)
            {
                return PutForCategoryToId(id.Value, category);
            }
            else if (name != null)
            {
                return PutForCategoryToName(name, category);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Parameter is not valid ! Please use Category Id (id) or Category Name (name) .");
            }
        }

        [HttpPut]
        [Route("{id:int:min(1)}")]
        public HttpResponseMessage Put(int id, [FromBody]Category category)
        {
            return PutForCategoryToId(id, category);
        }

        [HttpPut]
        [Route("{name}")]
        public HttpResponseMessage Put(string name, [FromBody]Category category)
        {
            return PutForCategoryToName(name, category);
        }

        private HttpResponseMessage DeleteForCategoryToId(int id)
        {

            try
            {
                if (id <= 0)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Category (CategoryId = {id}) is not valid !");
                }
                else
                {
                    var query = db.Category.FirstOrDefault(x => x.CategoryId == id);

                    if (query == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Category (Id = {id}) is not finded !");
                    }
                    else
                    {
                        db.Category.Remove(query);
                        db.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, $"Category (id = {id}) is deleted");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex);
            }

        }

        private HttpResponseMessage DeleteForCategoryToName(string name)
        {

            try
            {
                if (name == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Category (CategoryName = {name}) is not valid !");
                }
                else
                {
                    var query = db.Category.FirstOrDefault(x => x.CategoryName == name);

                    if (query == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Category (Name = {name}) is not finded !");
                    }
                    else
                    {
                        db.Category.Remove(query);
                        db.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, $"Category (Name = {name}) is deleted");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex);
            }

        }

        [HttpDelete]
        public HttpResponseMessage Delete([FromBody]Category category, [FromUri]int? id = 0, [FromUri]string name = null)
        {
            if (category == null)
            {
                if (id >= 1)
                {
                    return DeleteForCategoryToId(id.Value);
                }
                else if (name != null)
                {
                    return DeleteForCategoryToName(name);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Parameter is not valid !Please use Category Id(id = {id}) or Category Name(name = {name}).");
                }
            }
            else
            {
                if (category.CategoryId >= 1)
                {
                    return DeleteForCategoryToId(category.CategoryId);
                }
                else if (category.CategoryName != null)
                {
                    return DeleteForCategoryToName(category.CategoryName);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Parameter is not valid !Please use Category Id(id = {category.CategoryId}) or Category Name(name = {category.CategoryName}).");
                }
            }


        }

        [HttpDelete]
        [Route("{id:int:min(1)}")]
        public HttpResponseMessage Delete(int id)
        {
            return DeleteForCategoryToId(id);
        }

        [HttpDelete]
        [Route("{name}")]
        public HttpResponseMessage Delete(string name)
        {
            return DeleteForCategoryToName(name);
        }

    }
}

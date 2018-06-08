using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SaatveSaatApi.Controllers
{
    [RoutePrefix("api/product")]
    public class ProductController : ApiController
    {

        public class BuyProduct
        {
            public int id { get; set; } = 0;
            public string name { get; set; } = null;
            public int piece { get; set; }
        }

        private SaatvesaatDBEntities db = new SaatvesaatDBEntities();

        private HttpResponseMessage GetForProductToId(int id)
        {

            try
            {
                db.Configuration.ProxyCreationEnabled = false;

                if (id <= 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, db.Product.Include("BrandCategory").ToList());
                }
                else
                {
                    var query = db.Product.Include("BrandCategory").First(x => x.ProductId == id);

                    if (query == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Product (id = {id}) is not finded");
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

        private HttpResponseMessage GetForProductToName(string name)
        {

            try
            {
                db.Configuration.ProxyCreationEnabled = false;

                if (name == null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, db.Product.Include("BrandCategory").ToList());
                }
                else
                {
                    var query = db.Product.Include("BrandCategory").Where(x => x.ProductName == name);

                    if (query.ToArray<Product>().Count() == 0)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Product (name = {name}) is not finded");
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
        public HttpResponseMessage Get([FromUri]int? id = 0, [FromUri]string name = null)
        {

            if (id >= 1)
            {
                return GetForProductToId(id.Value);
            }
            else
            {
                return GetForProductToName(name);
            }

        }

        [HttpGet]
        [Route("{id:int:min(1)}")]
        public HttpResponseMessage Get(int id)
        {
            return GetForProductToId(id);
        }

        [HttpGet]
        [Route("{name}")]
        public HttpResponseMessage Get(string name)
        {
            return GetForProductToName(name);
        }

        [HttpGet]
        [Route("brandcategory")]
        public HttpResponseMessage Get([FromUri]string bname = null, [FromUri]string cname = null)
        {

            //db.Configuration.ProxyCreationEnabled = false;

            try
            {
                IQueryable<Product> query = null;

                if (bname != null && cname == null)
                {
                    query = from p in db.Product
                            join bc in db.BrandCategory on p.ProductBrandCategoryId equals bc.BrandCategoryId
                            join b in db.Brand on bc.BrandCategoryBrandId equals b.BrandId
                            where b.BrandName == bname
                            select p;
                }
                else if (bname == null && cname != null)
                {
                    query = from p in db.Product
                            join bc in db.BrandCategory on p.ProductBrandCategoryId equals bc.BrandCategoryId
                            join c in db.Category on bc.BrandCategoryCategoryId equals c.CategoryId
                            where c.CategoryName == cname
                            select p;
                }
                else if (bname != null && cname != null)
                {
                    query = from p in db.Product
                            join bc in db.BrandCategory on p.ProductBrandCategoryId equals bc.BrandCategoryId
                            join b in db.Brand on bc.BrandCategoryBrandId equals b.BrandId
                            join c in db.Category on bc.BrandCategoryCategoryId equals c.CategoryId
                            where b.BrandName == bname && c.CategoryName == cname
                            select p;
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Parameter or Parameters is invalid ! Please use BrandName (bname = {bname}) or/and CategoryName(cname = {cname}) !");
                }

                if (query.Count() > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, query);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Product is not finded !");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex);
            }

        }

        //[HttpGet]
        //[Route("brand/{id:int:min(1)}")]
        //public HttpResponseMessage GetBrandId(int id)
        //{
        //    //return GetForProductToName(name);

        //    var query = from p in db.Product join bc in db.BrandCategory on p.ProductBrandCategoryId equals bc.BrandCategoryId join b in db.Brand on bc.BrandCategoryBrandId
        //                equals b.BrandId where b.BrandName == "Gant" select{  };
        //}

        //[HttpGet]
        //[Route("brand/{name}")]
        //public HttpResponseMessage GetBrandName(string name)
        //{
        //    return GetForProductToName(name);
        //}

        public HttpResponseMessage Post([FromBody]Product product)
        {

            try
            {
                if (product == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Data is not finded at request body !");
                }
                else
                {
                    db.Product.Add(product);

                    if (db.SaveChanges() > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, product);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Brand (Name = {product.ProductName}) is not added !");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex);
            }

        }

        private HttpResponseMessage PutForProductToId(int id, Product product)
        {
            try
            {
                if (product == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Data is not finded at request body !");
                }
                else
                {
                    if (id <= 0)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Product (ProductId = {id}) is not valid !");
                    }
                    else
                    {
                        var query = db.Product.FirstOrDefault(x => x.ProductId == id);

                        if (query == null)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Product (Id = {id}) is not finded !");
                        }
                        else
                        {
                            if (product.ProductName != null) { query.ProductName = product.ProductName; }
                            if (product.ProductPrice > 0) { query.ProductPrice = product.ProductPrice; }
                            if (product.ProductStock > 0) { query.ProductStock = product.ProductStock; }
                            if (product.ProductDesc != null) { query.ProductDesc = product.ProductDesc; }
                            if (product.ProductBrandCategoryId > 0) { query.ProductBrandCategoryId = product.ProductBrandCategoryId; }
                            if (product.ProductImage != null) { query.ProductImage = product.ProductImage; }

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

        private HttpResponseMessage PutForProductToName(string name, Product product)
        {

            try
            {
                if (product == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Data is not finded at request body !");
                }
                else
                {
                    if (name == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Product (ProductName = {name}) is not valid !");
                    }
                    else
                    {
                        var query = db.Product.FirstOrDefault(x => x.ProductName == name);

                        if (query == null)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Product (name = {name}) is not finded !");
                        }
                        else
                        {
                            if (product.ProductName != null) { query.ProductName = product.ProductName; }
                            if (product.ProductPrice > 0) { query.ProductPrice = product.ProductPrice; }
                            if (product.ProductStock > 0) { query.ProductStock = product.ProductStock; }
                            if (product.ProductDesc != null) { query.ProductDesc = product.ProductDesc; }
                            if (product.ProductBrandCategoryId > 0) { query.ProductBrandCategoryId = product.ProductBrandCategoryId; }
                            if (product.ProductImage != null) { query.ProductImage = product.ProductImage; }

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
        public HttpResponseMessage Put([FromBody]Product product, [FromUri]int? id = 0, [FromUri]string name = null)
        {
            if (id >= 1)
            {
                return PutForProductToId(id.Value, product);
            }
            else if (name != null)
            {
                return PutForProductToName(name, product);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Parameter is not valid ! Please use Brand Id (id) or Brand Name (name) .");
            }
        }

        [HttpPut]
        [Route("{id:int:min(1)}")]
        public HttpResponseMessage Put(int id, [FromBody]Product product)
        {
            return PutForProductToId(id, product);
        }

        [HttpPut]
        [Route("{name}")]
        public HttpResponseMessage Put(string name, [FromBody]Product product)
        {
            return PutForProductToName(name, product);
        }

        private HttpResponseMessage DeleteForProductToId(int id)
        {

            try
            {
                if (id <= 0)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Poduct (ProductId = {id}) is not valid !");
                }
                else
                {
                    var query = db.Product.FirstOrDefault(x => x.ProductId == id);

                    if (query == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Product (Id = {id}) is not finded !");
                    }
                    else
                    {
                        db.Product.Remove(query);
                        db.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, $"Product (id = {id}) is deleted");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex);
            }

        }

        private HttpResponseMessage DeleteForProductToName(string name)
        {

            try
            {
                if (name == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"ProductCategory (ProductName = {name}) is not valid !");
                }
                else
                {
                    var query = db.Product.FirstOrDefault(x => x.ProductName == name);

                    if (query == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Product (Name = {name}) is not finded !");
                    }
                    else
                    {
                        db.Product.Remove(query);
                        db.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, $"Product (Name = {name}) is deleted");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex);
            }

        }

        [HttpDelete]
        public HttpResponseMessage Delete([FromBody]Product product, [FromUri]int? id = 0, [FromUri]string name = null)
        {
            if (product == null)
            {
                if (id >= 1)
                {
                    return DeleteForProductToId(id.Value);
                }
                else if (name != null)
                {
                    return DeleteForProductToName(name);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Parameter is not valid !Please use Brand Id(id = {id}) or Brand Name(name = {name}).");
                }
            }
            else
            {
                if (product.ProductId >= 1)
                {
                    return DeleteForProductToId(product.ProductId);
                }
                else if (product.ProductName != null)
                {
                    return DeleteForProductToName(product.ProductName);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Parameter is not valid !Please use Brand Id(id = {product.ProductId}) or Brand Name(name = {product.ProductName}).");
                }
            }

        }

        [HttpDelete]
        [Route("{id:int:min(1)}")]
        public HttpResponseMessage Delete(int id)
        {
            return DeleteForProductToId(id);
        }

        [HttpDelete]
        [Route("{name}")]
        public HttpResponseMessage Delete(string name)
        {
            return DeleteForProductToName(name);
        }

        private HttpResponseMessage BuyForProductToId(int id, int piece)
        {

            try
            {
                var query = db.Product.FirstOrDefault(x => x.ProductId == id);

                if (query == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Product (Id = {id}) is not finded !");
                }
                else
                {
                    if (query.ProductStock >= piece)
                    {
                        query.ProductStock = query.ProductStock - piece;

                        db.PendingOrder.Add(new PendingOrder()
                        {
                            PendingOrderProductId = id,
                            PendingOrderProductPiece = piece,
                            PendingOrderCreateDate = System.DateTime.Now,
                            PendingOrderUpdateDate = System.DateTime.Now
                        });

                        if (db.SaveChanges() >= 2)
                        {

                            return Request.CreateResponse(HttpStatusCode.OK, query);

                        }
                        else
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"{piece} piece product (Id = {id}) is not buyed !");
                        }
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Out of stock ! Maximum stock piece {query.ProductStock}");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex);
            }

        }

        private HttpResponseMessage BuyForProductToName(string name, int piece)
        {

            try
            {
                var query = db.Product.FirstOrDefault(x => x.ProductName == name);

                if (query == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Product (Name = {name}) is not finded !");
                }
                else
                {
                    if (query.ProductStock >= piece)
                    {
                        query.ProductStock = query.ProductStock - piece;

                        db.PendingOrder.Add(new PendingOrder()
                        {
                            PendingOrderProductId = query.ProductId,
                            PendingOrderProductPiece = piece,
                            PendingOrderCreateDate = System.DateTime.Now,
                            PendingOrderUpdateDate = System.DateTime.Now
                        });

                        if (db.SaveChanges() >= 2)
                        {

                            return Request.CreateResponse(HttpStatusCode.OK, query);

                        }
                        else
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"{piece} piece product (Name = {name}) is not buyed !");
                        }
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Out of stock ! Maximum stock piece {query.ProductStock}");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex);
            }

        }

        [HttpPost]
        [Route("buy")]
        public HttpResponseMessage Put([FromBody]BuyProduct buyproduct, [FromUri]int? id = 0, [FromUri]string name = null, [FromUri]int? piece = 0)
        {

            BuyProduct bp = new BuyProduct();

            if (buyproduct == null)
            {
                bp.id = id.Value;
                bp.name = name;
                bp.piece = piece.Value;
            }
            else
            {
                bp = buyproduct;
            }

            if (bp.piece >= 1)
            {
                if (bp.id >= 1)
                {
                    return BuyForProductToId(bp.id, bp.piece);
                }
                else if (bp.name != null)
                {
                    return BuyForProductToName(bp.name, bp.piece);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Parameter is not valid ! Please use Brand Id (id) or Brand Name (name) .");
                }

            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Product piece is not valid !");
            }

        }

        [HttpPost]
        [Route("{id:int:min(1)}/buy")]
        public HttpResponseMessage Put(int id, [FromBody]BuyProduct buyproduct, [FromUri]int? piece = 0)
        {

            if (buyproduct.piece >= 1)
            {
                return BuyForProductToId(id, buyproduct.piece);
            }
            else if (piece.Value >= 1)
            {
                return BuyForProductToId(id, piece.Value);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Product piece is not valid !");
            }

        }

        [HttpPost]
        [Route("{name}/buy")]
        public HttpResponseMessage Put(string name, [FromBody]BuyProduct buyproduct, [FromUri]int? piece = 0)
        {

            if (buyproduct.piece >= 1)
            {
                return BuyForProductToName(name, buyproduct.piece);
            }
            else if (piece.Value >= 1)
            {
                return BuyForProductToName(name, piece.Value);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Product piece is not valid !");
            }

        }

    }
}

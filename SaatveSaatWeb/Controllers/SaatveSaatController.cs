using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Runtime.Serialization.Json;
using SaatveSaatWeb.Models;


namespace SaatveSaatWeb.Controllers
{
    [RoutePrefix("SaatveSaat/Markalar")]

    public class SaatveSaatController : BaseController
    {

        private RestServices rs = new RestServices();
        private IRestResponse response = null;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Hata(Error e)
        {

            if (e.statuscode == null)
            {
                ViewBag.statuscode = "404";
                ViewBag.message = "HATA İSTENİLEN SAYFA YOK VEYA GEÇERSİZ.";
            }
            else
            {
                ViewBag.statuscode = e.statuscode;
                ViewBag.message = e.message;
            }

            return View();

        }

        [HttpGet]
        public ActionResult SatinAl()
        {

            try
            {
                var bsid = TempData["basarilisiparisid"];

                if (bsid == null)
                {
                    return RedirectToAction("Hata", "SaatveSaat");
                }
                else
                {
                    response = rs.RestServicesExexReqParam("product/buy", Method.POST, new List<RestServices.reqParameters>
                    {
                        new RestServices.reqParameters(){ name = "id" , value = bsid.ToString() },
                        new RestServices.reqParameters(){ name = "piece" , value = "1" }
                    });

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return View();
                    }
                    else
                    {
                        return RedirectToAction("Hata", "SaatveSaat", new
                        {
                            statuscode = response.StatusCode.GetHashCode(),
                            message = response.Content.ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Hata", "SaatveSaat", new { message = ex.Message.ToString() });
            }

        }

        [HttpGet]
        public ActionResult Detaylar(int? id = 0)
        {

            try
            {
                if (id > 0)
                {
                    response = rs.RestServicesExexReq("product/" + id.ToString(), Method.GET);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        Product product = (Product)Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content, typeof(Product));
                        return View(product);
                    }
                    else
                    {
                        return RedirectToAction("Hata", "SaatveSaat", new
                        {
                            statuscode = response.StatusCode.GetHashCode(),
                            message = response.StatusDescription.ToString()
                        });
                    }
                }
                else
                {
                    return RedirectToAction("Urunler", "SaatveSaat");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Hata", "SaatveSaat", new { message = ex.Message.ToString() });
            }

        }

        [HttpPost]
        public ActionResult SatinAl(string id)
        {
            TempData["basarilisiparisid"] = id;

            return RedirectToAction("SatinAl");
        }

        public ActionResult Kategoriler(string id)
        {

            try
            {
                string queryString = null;
                if (id != null)
                {
                    TempData["marka"] = id;
                    queryString = "brand/" + id;
                }
                else
                {
                    TempData["marka"] = "";
                }

                response = rs.RestServicesExexReq("category/" + queryString, Method.GET);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    List<Category> categoryList = (List<Category>)Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content, typeof(List<Category>));
                    return View(categoryList);
                }
                else
                {
                    return RedirectToAction("Hata", "SaatveSaat", new
                    {
                        statuscode = response.StatusCode.GetHashCode(),
                        message = response.StatusDescription.ToString()
                    });
                }


            }
            catch (Exception ex)
            {
                return RedirectToAction("Hata", "SaatveSaat", new { message = ex.Message.ToString() });
            }
        }

        public ActionResult Urunler(string bname, string cname)
        {

            try
            {
                if (bname == null && cname == null)
                {
                    response = rs.RestServicesExexReq("product/", Method.GET);
                }
                else
                {
                    response = rs.RestServicesExexReqParam("product/brandcategory", Method.GET, new List<RestServices.reqParameters>
                    {
                        new RestServices.reqParameters(){ name = "bname" , value = bname },
                        new RestServices.reqParameters(){ name = "cname" , value = cname }
                    });
                }

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    List<Product> productList = (List<Product>)Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content, typeof(List<Product>));
                    return View(productList);
                }
                else
                {
                    return RedirectToAction("Hata", "SaatveSaat", new
                    {
                        statuscode = response.StatusCode.GetHashCode(),
                        message = response.Content.ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Hata", "SaatveSaat", new { message = ex.Message.ToString() });
            }

        }

        public ActionResult Markalar(string id)
        {

            try
            {
                response = rs.RestServicesExexReq("brand/", Method.GET);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    List<Brand> brandlist = (List<Brand>)Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content, typeof(List<Brand>));
                    return View(brandlist);
                }
                else
                {
                    return RedirectToAction("Hata", "SaatveSaat", new
                    {
                        statuscode = response.StatusCode.GetHashCode(),
                        message = response.Content.ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Hata", "SaatveSaat", new { message = ex.Message.ToString() });
            }

        }

    }

}
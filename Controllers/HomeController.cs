using AAA_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Xml.Schema;

namespace AAA_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AaaDbContext aaaDbContext;

        public HomeController(ILogger<HomeController> logger, AaaDbContext aaaDbContext)
        {
            _logger = logger;
            this.aaaDbContext = aaaDbContext;
        }

        public IActionResult Index()
        {
            //No. of Mobile Models
            var noOfModels = (from s in aaaDbContext.AaaMobileModels
                              select s.ModelName).Count();

            //Most sold mobile models across all the cities
            var topSalesInfo = (
                                 from x in (
                                             from a in aaaDbContext.AaaSalesInfos
                                             group a by a.ModelId into aGroup
                                             select new
                                             { 
                                                 MobileModelId = aGroup.Key,
                                                 TotalSalesAmount = aGroup.Sum(s => s.SalesCount)
                                             }
                                           ) 
                                 join c in aaaDbContext.AaaMobileModels
                                 on x.MobileModelId equals c.Id
                                 select new
                                 { 
                                    ModelName = c.ModelName,
                                    TotalSales = (x.TotalSalesAmount/c.Amount),
                                    TotalSalesAmount = x.TotalSalesAmount 
                                 }
                               ).OrderByDescending( x => x.TotalSales ).ToList();

            ViewBag.topSalesInfo = topSalesInfo[0];

            //Most sold mobile models - city-wise
            var topCityWiseSales = (
                                    from x in (
                                                from a in aaaDbContext.AaaSalesInfos
                                                group a by new { a.ModelId, a.CityId } into aGroup
                                                select new
                                                {
                                                    MobileModelId = aGroup.Key.ModelId,
                                                    CityId = aGroup.Key.CityId,
                                                    TotalSalesAmount = aGroup.Sum(s => s.SalesCount)
                                                }
                                              )
                                    join b in aaaDbContext.AaaMobileModels
                                    on x.MobileModelId equals b.Id
                                    join c in aaaDbContext.AaaCities
                                    on x.CityId equals c.Id
                                    select new
                                    {
                                        MobileModel = b.ModelName,
                                        City = c.CityName,
                                        TotalSales = (x.TotalSalesAmount / b.Amount),
                                        TotalSalesAmount = x.TotalSalesAmount
                                    }
                                    ).OrderByDescending(x => x.City).ThenByDescending(y => y.TotalSales).ToList();

            List<TopModelSalesCityWise> topSalesCityWise = new List<TopModelSalesCityWise>();

            for (var i = 0; i < topCityWiseSales.Count; i += noOfModels)
            {
                var a = new TopModelSalesCityWise();
                a.City = topCityWiseSales[i].City;
                a.ModelName = topCityWiseSales[i].MobileModel;
                a.TotalSales = topCityWiseSales[i].TotalSales;
                a.TotalSalesAmount = topCityWiseSales[i].TotalSalesAmount;

                topSalesCityWise.Add(a);
            }

            ViewBag.topSoldModelsCitywise = topSalesCityWise;

            //Most least sold mobile models across all cities
            var leastSalesInfo = (
                                 from x in (
                                             from a in aaaDbContext.AaaSalesInfos
                                             group a by a.ModelId into aGroup
                                             select new
                                             {
                                                 MobileModelId = aGroup.Key,
                                                 TotalSalesAmount = aGroup.Sum(s => s.SalesCount)
                                             }
                                           )
                                 join c in aaaDbContext.AaaMobileModels
                                 on x.MobileModelId equals c.Id
                                 select new
                                 {
                                     ModelName = c.ModelName,
                                     TotalSales = (x.TotalSalesAmount / c.Amount),
                                     TotalSalesAmount = x.TotalSalesAmount
                                 }
                               ).OrderBy(x => x.TotalSales).ToList();

            ViewBag.leastSalesInfo = leastSalesInfo[0];

            //Total sales amount across all cities and all mobile models
            var totalSalesAmount = (from x in aaaDbContext.AaaSalesInfos
                                    select x.SalesCount).Sum();

            ViewBag.totalSalesAmount = totalSalesAmount;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();

        }
        public IActionResult SalesChart()
        {
            return View();
        }

        [HttpGet]
        public JsonResult SalesChartData()
        {
            //No. of Mobile Models
            var noOfModels = (from s in aaaDbContext.AaaMobileModels
                              select s.ModelName).Count();

            var topCityWiseSales = (
                                    from x in (
                                                from a in aaaDbContext.AaaSalesInfos
                                                group a by new { a.ModelId, a.CityId } into aGroup
                                                select new
                                                {
                                                    MobileModelId = aGroup.Key.ModelId,
                                                    CityId = aGroup.Key.CityId,
                                                    TotalSalesAmount = aGroup.Sum(s => s.SalesCount)
                                                }
                                              )
                                    join b in aaaDbContext.AaaMobileModels
                                    on x.MobileModelId equals b.Id
                                    join c in aaaDbContext.AaaCities
                                    on x.CityId equals c.Id
                                    select new
                                    {
                                        MobileModel = b.ModelName,
                                        City = c.CityName,
                                        TotalSales = (x.TotalSalesAmount / b.Amount),
                                        TotalSalesAmount = x.TotalSalesAmount
                                    }
                                    ).OrderBy(x => x.City).ThenBy(y => y.MobileModel).ToList();

            var cityModelSales = new List<CitySalesModel>();

            for (var i = 0; i < topCityWiseSales.Count; i += noOfModels)
            {
                var s = new CitySalesModel();
                s.City = topCityWiseSales[i].City;
                s.VA1 = topCityWiseSales[i].TotalSales;
                s.VA2 = topCityWiseSales[i + 1].TotalSales;
                s.VA3 = topCityWiseSales[i + 2].TotalSales;

                cityModelSales.Add(s);
            }

            return Json(cityModelSales);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
using ChainStoreApi.Data;
using ChainStoreApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ChainStoreApi.Services;

public class RevenueService
{
    private readonly IMongoCollection<Order> _OrderCollection;
    public RevenueService(
        IOptions<DatabaseSetting> databaseSetting)
    {
        var mongoClient = new MongoClient(
            databaseSetting.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(databaseSetting.Value.DatabaseName);

        _OrderCollection = mongoDatabase.GetCollection<Order>(databaseSetting.Value.OrderCollectionName);
    }
    private static int profit = 0;
    private static int productNumber = 0;
    // lấy ra dữ liệu sản phẩm và lợi nhuận cho Chart
    public async Task<List<revenue>> GetRevenueByYears()
    {
        // lấy ra thông tin trước tiên của toàn bộ hóa đơn
        var query = await _OrderCollection.Find(_ => true).ToListAsync();
        // khai báo một anonymous Dictonary
        //Dictionary<string, object> revenue = new Dictionary<string, object>();
        List<revenue> revenueYears = new List<revenue>();
        // lấy ra 3 năm gần đây        
        var last3Years = (from n in Enumerable.Range(0, 3) select DateTime.Now.Year - n).OrderBy(p => p).ToList();
        // // lấy ra dữ liệu trong vòng 3 năm gần đây property List Last n Years
        foreach (var result in last3Years)
        {
            DateTime start = DateTime.ParseExact(result + "-01-01", "yyyy-MM-dd", CultureInfo.GetCultureInfo("vi-VN"));
            DateTime end = DateTime.ParseExact(result + "-12-31", "yyyy-MM-dd", CultureInfo.GetCultureInfo("vi-VN"));
            var compareQuery = query.Where(x => x.OrderDate >= start && x.OrderDate <= end).ToList();
            profit = 0;
            productNumber = 0;
            if (compareQuery != null)
            {
                foreach (var val in compareQuery)
                {
                    foreach (var pf in val.OrderDetails)
                    {
                        var purchasePrice = pf.Product.purchasePrice;
                        if (pf.Product.purchasePrice == null)
                        {
                            purchasePrice = 0;
                        }
                        profit = profit + (pf.Product.price - purchasePrice ?? 0);
                        productNumber = productNumber + pf.count ?? 0;
                    }
                }
            }
            revenueYears.Add(
                    new revenue
                    {
                        rYear = result + string.Empty,
                        rprofit = profit,
                        rproductNumber = productNumber
                    });
        }
        return revenueYears.ToList();
    }

    public async Task<List<revenue>> GetRevenueByWeek(string? startDate, string? endDate)
    {
        List<revenue> revenueWeek = new List<revenue>();
        // Get current date
        // Convert it to an integer
        // Subtract days from the current date and Subtract 1.That gives you Monday.
        // Make a list, starting with monday.
        var query = await _OrderCollection.Find(_ => true).ToListAsync();
        if (startDate == null && endDate == null)
        {
            var now = DateTime.Now;
            var currentDay = now.DayOfWeek;
            int days = (int)currentDay;
            // ngày bắt đầu từ thứ 2
            DateTime monday = now.AddDays(-days);
            var daysThisWeek = Enumerable.Range(0, 8)
                .Select(d => monday.AddDays(d))
                .ToList();
            var dataInWeek = query.Where(x => x.OrderDate > monday).ToList();
            //List<string> myDaysOfWeek = daysThisWeek.Select(d => d.DayOfWeek.ToString()).ToList();
            foreach (var curdays in daysThisWeek)
            {
                var dataindays = dataInWeek.Where(x => DateTime.Parse(x.OrderDate + string.Empty).ToString("d") == curdays.ToString("d")).ToList();
                profit = 0;
                productNumber = 0;
                if (dataindays.Count > 0)
                {
                    foreach (var val in dataindays)
                    {
                        foreach (var pf in val.OrderDetails)
                        {
                            var purchasePrice = pf.Product.purchasePrice;
                            if (pf.Product.purchasePrice == null)
                            {
                                purchasePrice = 0;
                            }
                            profit = profit + (pf.Product.price - purchasePrice ?? 0);
                            productNumber = productNumber + pf.count ?? 0;
                        }
                    }
                    revenueWeek.Add(
                    new revenue
                    {
                        rYear = curdays.ToString("d"),
                        rprofit = profit,
                        rproductNumber = productNumber
                    });
                }
            }
        }
        else
        {
            var SD = DateTime.Parse(startDate + string.Empty);
            var ED = DateTime.Parse(endDate + string.Empty);
            var dateinweekfromto = query.Where(x =>
            DateTime.Parse(DateTime.Parse(x.OrderDate + string.Empty).ToString("d")) >=
            DateTime.Parse(DateTime.Parse(startDate + string.Empty).ToString("d")) &&
            DateTime.Parse(DateTime.Parse(x.OrderDate + string.Empty).ToString("d")) <=
            DateTime.Parse(DateTime.Parse(endDate + string.Empty).ToString("d"))
            ).ToList();
            TimeSpan totalD = (ED.Date - SD.Date);
            var daysfromto = Enumerable.Range(0, Convert.ToInt32(totalD.TotalDays))
                .Select(d => SD.AddDays(d))
                .ToList();

            foreach (var curdays in daysfromto)
            {
                var dataindays = dateinweekfromto.Where(x => DateTime.Parse(x.OrderDate + string.Empty).ToString("d") == curdays.ToString("d")).ToList();
                profit = 0;
                productNumber = 0;
                if (dataindays.Count > 0)
                {
                    foreach (var val in dataindays)
                    {
                        foreach (var pf in val.OrderDetails)
                        {
                            var purchasePrice = pf.Product.purchasePrice;
                            if (pf.Product.purchasePrice == null)
                            {
                                purchasePrice = 0;
                            }
                            profit = profit + (pf.Product.price - purchasePrice ?? 0);
                            productNumber = productNumber + pf.count ?? 0;
                        }
                    }
                    revenueWeek.Add(
                    new revenue
                    {
                        rYear = curdays.ToString("d"),
                        rprofit = profit,
                        rproductNumber = productNumber
                    });
                }
            }
        }
        // lấy ra thông tin trước tiên của toàn bộ hóa đơn

        return revenueWeek.ToList();
    }
    public class revenue
    {
        public string rYear { get; set; }
        public int rprofit { get; set; }
        public int rproductNumber { get; set; }
    }

}
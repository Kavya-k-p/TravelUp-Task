using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TravelUp_Task.Data;
using TravelUp_Task.Models;

public class ItemsController : Controller
{
    private readonly AppDbContext _context;
    public ItemsController(AppDbContext context) => _context = context;

    public IActionResult Index()
    {
        var items = _context.Items.ToList();
        return View(items);
    }

    public IActionResult Create() => View();

    [HttpPost]
    public JsonResult CreateAjax([FromForm] string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Json(new { success = false, message = "Name is required" });

        var item = new Item { Name = name };
        _context.Items.Add(item);
        _context.SaveChanges();
        return Json(new { success = true, item });
    }
}

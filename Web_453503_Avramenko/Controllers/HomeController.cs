using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web_453503_Avramenko.Controllers;

public class HomeController : Controller
{
    readonly List<ListDemo> _list = new List<ListDemo>
    {
        new ListDemo{Id = 1, Name = "Item 1"},
        new ListDemo{Id = 2, Name = "Item 2"},
        new ListDemo{Id = 3, Name = "Item 3"}
    };
    public IActionResult Index()
    {
        ViewData["Message"] = "Лабораторная работа №2";
        return View(new SelectList(
            _list,
            "Id", 
            "Name"
            ));
    }
}

public class ListDemo
{
    public int Id { get; set; }
    public string Name { get; set; }
}
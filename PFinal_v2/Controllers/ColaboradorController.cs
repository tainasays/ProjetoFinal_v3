using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;



[Authorize(Roles = "Colaborador")]

public class ColaboradorController : Controller

{

    public IActionResult Index()

    {

        return View();

    }

}
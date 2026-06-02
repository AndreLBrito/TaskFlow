using System.Diagnostics;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Features.Dashboard.GetDashboard;
using TaskFlow.Web.Mapping;
using TaskFlow.Web.Models;
using TaskFlow.Web.ViewModels.Dashboard;

namespace TaskFlow.Web.Controllers;

public class HomeController : Controller
{
    private readonly IMediator _mediator;

    public HomeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> Index(
        CancellationToken cancellationToken)
    {
        var dashboard = await _mediator.Send(
            new GetDashboardQuery(),
            cancellationToken);

        return View(
            dashboard.To<DashboardViewModel>());
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

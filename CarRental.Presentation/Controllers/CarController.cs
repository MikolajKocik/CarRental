using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CarRental.Presentation.Models;
using CarRental.Application.Dto;
using CarRental.Application.Dto.CreateCar;
using CarRental.Application.Dto.Queries.GetAllCars;
using CarRental.Application.Dto.Queries.CarDetails;
using CarRental.Application.Dto.EditCar;
using CarRental.Application.Dto.DeleteCar;
using CarRental.Domain.Entities;

namespace CarRental.Presentation.Controllers;


[Authorize(Roles = "Admin")]
public class CarController : Controller
{

    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public CarController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var cars = await _mediator.Send(new GetAllCarsQuery(), cancellationToken);

        return View(cars);
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {

        var details = await _mediator.Send(new CarDetailsQuery(id), cancellationToken);
        if (details == null)
        {
            return NotFound();
        }

        return View(details);
    }

    [HttpGet] // Admin
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken] // Admin
    public async Task<IActionResult> Create(CreateCarViewModel carViewModel,
        CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            var command = new CreateCarCommand
            {
                Car = _mapper.Map<CarDto>(carViewModel)
            };

            await _mediator.Send(command, cancellationToken);

            return RedirectToAction("Index");
        }

        return View(carViewModel);
    }


    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellation)
    {
;
        var car =  await _mediator.Send(new CarDetailsQuery(id), cancellation);

        var editViewModel = _mapper.Map<EditCarViewModel>(car);

        return View(editViewModel);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EditCarViewModel editCarViewModel,
        CancellationToken cancellation)
    {
        if (ModelState.IsValid)
        {
            var command = new EditCarCommand
            {
                Id = id,
                Car = _mapper.Map<CarDto>(editCarViewModel)
            };

            await _mediator.Send(command, cancellation);

            return RedirectToAction(nameof(Index));
        }
        return View(editCarViewModel);
    }
  

    [HttpGet]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellation)
    {
        var car = await _mediator.Send(new CarDetailsQuery(id), cancellation);

        return View(car);
    }

  
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellation)
    {

        if (!ModelState.IsValid)
        {
            var car = await _mediator.Send(new CarDetailsQuery(id), cancellation);

            return View("Delete", car);
        }

        var command = new DeleteCarCommand() { Id = id };

        await _mediator.Send(command, cancellation);

        return RedirectToAction(nameof(Index));
    }
}
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CarRental.Presentation.Models;
using CarRental.Application.Dto;
using CarRental.Application.Dto.CreateCar;
using CarRental.Application.Dto.EditCar;
using CarRental.Application.Dto.DeleteCar;
using CarRental.Application.Dto.Queries.CarQueries.GetAllCars;
using CarRental.Application.Dto.Queries.CarQueries.GetCarDetails;

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
    public async Task<IActionResult> Index(CancellationToken cancellation)
    {
        var carDtos = await _mediator.Send(new GetAllCarsQuery(), cancellation);

        var viewModel = _mapper.Map<ICollection<CreateCarViewModel>>(carDtos);

        return View(viewModel);
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken cancellation)
    {
        var detailsDto = await _mediator.Send(new CarDetailsQuery(id), cancellation);
        if (detailsDto == null)
        {
            return NotFound();
        }
        var viewModel = _mapper.Map<CreateCarViewModel>(detailsDto);

        return View(viewModel);
    }

    [HttpGet] // Admin
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken] // Admin
    public async Task<IActionResult> Create(CreateCarViewModel carViewModel,
        CancellationToken cancellation)
    {
        if (ModelState.IsValid)
        {
            var command = new CreateCarCommand
            {
                CarDto = _mapper.Map<CarDto>(carViewModel)
            };

            command.CarDto.Images = carViewModel.Images ?? new List<IFormFile>();

            await _mediator.Send(command, cancellation);

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
                CarDto = _mapper.Map<CarDto>(editCarViewModel)
            };

            command.CarDto.Images = editCarViewModel.NewImages ?? new List<IFormFile>();

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
        var car = await _mediator.Send(new CarDetailsQuery(id), cancellation);

        var command = new DeleteCarCommand() { Id = id };

        await _mediator.Send(command, cancellation);

        return RedirectToAction(nameof(Index));
    }
}
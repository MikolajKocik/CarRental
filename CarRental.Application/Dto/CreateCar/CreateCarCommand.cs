using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application.Dto.CreateCar
{
    public class CreateCarCommand : CarDto, IRequest
    {
    }
}

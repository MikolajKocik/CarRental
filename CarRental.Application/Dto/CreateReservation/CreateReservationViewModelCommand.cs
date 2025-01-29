using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application.Dto.CreateReservation
{
    public class CreateReservationViewModelCommand : ReservationDto, IRequest
    {
    }
}

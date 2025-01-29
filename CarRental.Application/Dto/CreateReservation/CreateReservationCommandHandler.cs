using AutoMapper;
using CarRental.Application.Dto.CreateCar;
using CarRental.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application.Dto.CreateReservation
{
    public class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand>
    {
        private readonly IMapper _mapper;
        private readonly IReservationRepository _reservationRepository;

        public CreateReservationCommandHandler(IMapper mapper, IReservationRepository reservationRepository)
        {
            _mapper = mapper;
            _reservationRepository = reservationRepository;
        }
        public async Task<Unit> Handle(CreateReservationCommand command, CancellationToken cancellationToken)
        {

        }

    }
}

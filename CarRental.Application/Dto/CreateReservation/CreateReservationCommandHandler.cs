using AutoMapper;
using CarRental.Domain.Interfaces;
using MediatR;


namespace CarRental.Application.Dto.CreateReservation
{
    public class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand>
    {
        private readonly IMapper _mapper;
        private readonly IReservationRepository _repository;

        public CreateReservationCommandHandler(IMapper mapper, IReservationRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }
        public async Task<Unit> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
        {

            

        }
    }
}

using AutoMapper;
using CarRental.Application.Dto;
using CarRental.Domain.Interfaces.Repositories;
using CarRental.Domain.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Errors.Model;

namespace CarRental.Application.CQRS.Queries.ReservationQueries.GetReservationDetails
{
    public class GetReservationDetailsQueryHandler : IRequest<ReservationDto>
    {
        private readonly IReservationRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _userManager;
        private readonly ILogger<GetReservationDetailsQueryHandler> _logger;

        public GetReservationDetailsQueryHandler(IReservationRepository repository, IMapper mapper,
            ICurrentUserService userManager, ILogger<GetReservationDetailsQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<ReservationDto> Handle(GetReservationDetailsQuery request, CancellationToken cancellation)
        {
            cancellation.ThrowIfCancellationRequested();

            var currentUserId = _userManager.UserId;

            var reservation = await _repository.GetReservationByIdAsync(request.Id, cancellation);

            if (reservation == null || reservation.UserId != currentUserId) // compare currentUserId with reservation.UserId
            {
                _logger.LogError($"Reservation with Id = {request.Id} was not found");
                throw new NotFoundException();
            }

            return _mapper.Map<ReservationDto>(reservation);
        }
    }
}

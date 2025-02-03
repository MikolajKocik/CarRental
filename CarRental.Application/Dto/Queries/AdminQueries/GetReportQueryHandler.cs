using AutoMapper;
using CarRental.Domain.Interfaces;
using MediatR;

namespace CarRental.Application.Dto.Queries.AdminQueries;

public class GetReportQueryHandler : IRequestHandler<GetReportQuery, AdminReportsDto>
{
    private readonly IAdminRepository _repository;
    private readonly IMapper _mapper;

    public GetReportQueryHandler(IAdminRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<AdminReportsDto> Handle(GetReportQuery request, CancellationToken cancellation)
    {
        try
        {
            // total reservations count
            var totalReservations = await _repository.GetReportsCountAsync(cancellation);

            // popular cars

            var popularCars = await _repository.GetPopularCars(cancellation);

            // not confirmed reservations
            var notConfirmedReservations = await _repository.GetNotConfirmedReservationsAsync(cancellation);

            // total income
            var totalIncome = await _repository.GetTotalIncomeAsync();

            return new AdminReportsDto
            {
                TotalReservations = totalReservations,
                TotalIncome = totalIncome,
                PopularCars = _mapper.Map<IEnumerable<ReportDto>>(popularCars),
                NotConfirmedReservations = _mapper.Map<IEnumerable<ReservationDto>>(notConfirmedReservations)
            };
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error fetching admin reports.", ex);
        }

    }
}

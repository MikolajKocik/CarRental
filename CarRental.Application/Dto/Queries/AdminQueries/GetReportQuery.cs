using CarRental.Domain.Entities;
using MediatR;

namespace CarRental.Application.Dto.Queries.AdminQueries
{
    public class GetReportQuery : IRequest<AdminReportsDto>
    {
    }
}

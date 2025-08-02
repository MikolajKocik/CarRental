using CarRental.Application.Dto;
using CarRental.Domain.Entities;
using MediatR;

namespace CarRental.Application.CQRS.Queries.AdminQueries
{
    public class GetReportQuery : IRequest<AdminReportsDto>
    {
    }
}

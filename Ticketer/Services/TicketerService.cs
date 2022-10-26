using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Ticket;
using Microsoft.Extensions.Logging;

namespace Ticketer.Services
{
    public class TicketerService : Ticket.Ticketer.TicketerBase
    {
        private readonly TicketRepository _ticketRepository;
        private readonly ILogger<TicketRepository> _logger;

        public TicketerService(TicketRepository ticketRepository, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<TicketRepository>();
            _ticketRepository = ticketRepository;

        }
        [Authorize]
        public override Task<AvailableTicketsResponse> GetAvailableTickets(Empty request, ServerCallContext context)
        {
            return Task.FromResult(new AvailableTicketsResponse
            {
                Count = _ticketRepository.GetAvailableTickets()
            });
        }

        [Authorize]
        public override Task<BuyTicketsResponse> BuyTickets(BuyTicketsRequest request, ServerCallContext context)
        {
            _logger.LogInformation(context.GetHttpContext().ToString());
            var user = context.GetHttpContext().User;

            return Task.FromResult(new BuyTicketsResponse
            {
                Success = _ticketRepository.BuyTickets(user.Identity.Name!, request.Count)
            });
        }
    }
}

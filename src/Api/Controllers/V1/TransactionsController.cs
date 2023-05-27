using AutoMapper;
using Greentube.DemoWallet.Api.Filters;
using Greentube.DemoWallet.Api.Models.V1;
using Greentube.DemoWallet.Application.Abstractions;
using Greentube.DemoWallet.Application.Transactions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Greentube.DemoWallet.Api.Controllers.V1;

/// <summary>
/// View and manage players
/// </summary>
[ApiController]
[Route("v{version:apiVersion}/[controller]")]
[ApiVersion("1")]
[TypeFilter(typeof(ApplicationErrorsFilter))]
public class TransactionsController
{
    private readonly IMapper _mapper;

    /// <summary>
    /// Top ups player's balance.
    /// </summary>
    /// <response code="404">Player with the specified ID was not found</response>
    [HttpPost("topup")]
    [ProducesResponseType(typeof(BalanceModel), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public async Task<BalanceModel> TopupAsync(
        [FromServices] ICommandHandler<TopUpCommand, decimal> handler,
        [FromBody] PlayerAmountModel amount,
        CancellationToken ct)
    {
        var command = _mapper.Map<TopUpCommand>(amount);
        var newBalance = await handler.HandleAsync(command, ct);
        return new BalanceModel { Balance = newBalance };
    }

    /// <summary>
    /// Creates Win transaction which increases player's balance.
    /// </summary>
    /// <response code="404">Player with the specified ID was not found</response>
    [HttpPost("win")]
    [ProducesResponseType(typeof(BalanceModel), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public async Task<BalanceModel> WinAsync(
        [FromServices] ICommandHandler<WinCommand, decimal> handler,
        [FromBody] PlayerAmountModel amount,
        CancellationToken ct)
    {
        var command = _mapper.Map<WinCommand>(amount);
        var newBalance = await handler.HandleAsync(command, ct);
        return new BalanceModel { Balance = newBalance };
    }

    /// <summary>
    /// Creates Bet transaction which decreases player's balance.
    /// </summary>
    /// <remarks>
    /// Note that this endpoint fails with 402 status code if player's balance is not sufficient to make a requested
    /// bet. Note, that Player's `BalanceLimit` is taken into account when checking this requirement. The balance is
    /// considered insufficient if current `Balance` minus bet `Amount` is less than `BalanceLimit`.
    /// </remarks>
    /// <response code="402">Player doesn't have enough balance to perform the operation</response>
    /// <response code="404">Player with the specified ID was not found</response>
    [HttpPost("bet")]
    [ProducesResponseType(typeof(BalanceModel), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 402)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public async Task<Results<Ok<BalanceModel>, ProblemHttpResult>> BetAsync(
        [FromServices] ICommandHandler<BetCommand, BetCommandResult> handler,
        [FromBody] PlayerAmountModel amount,
        CancellationToken ct)
    {
        var command = _mapper.Map<BetCommand>(amount);
        var result = await handler.HandleAsync(command, ct);
        return result.Succeeded
            ? TypedResults.Ok(new BalanceModel { Balance = result.PlayerBalance })
            : TypedResults.Problem(
                statusCode: 402,
                title: "Not Enough Balance",
                detail: $"Player with id {amount.PlayerId} doesn't have sufficient balance to make this bet");
    }

    /// <summary />
    public TransactionsController(IMapper mapper)
    {
        _mapper = mapper;
    }
}

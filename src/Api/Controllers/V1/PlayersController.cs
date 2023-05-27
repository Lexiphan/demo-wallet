using AutoMapper;
using Greentube.DemoWallet.Api.Filters;
using Greentube.DemoWallet.Api.Models.V1;
using Greentube.DemoWallet.Application.Abstractions;
using Greentube.DemoWallet.Application.Models;
using Greentube.DemoWallet.Application.Players;
using Greentube.DemoWallet.Application.Transactions;
using Greentube.DemoWallet.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Greentube.DemoWallet.Api.Controllers.V1;

/// <summary>
/// View and manage players
/// </summary>
[ApiController]
[Route("v{version:apiVersion}/[controller]")]
[ApiVersion("1")]
[TypeFilter(typeof(ApplicationErrorsFilter))]
public class PlayersController : ControllerBase
{
    private readonly IMapper _mapper;

    /// <summary>
    /// Retrieve the list of all players
    /// </summary>
    [HttpGet("")]
    public async Task<PagedResult<PlayerModel>> ListPlayersAsync(
        [FromServices] IQueryHandler<PagedEntityQuery<Player>, PagedResult<Player>> handler,
        [FromQuery] PageArgs pageArgs,
        CancellationToken ct)
    {
        var players = await handler.HandleAsync(new(pageArgs), ct);
        return _mapper.Map<PagedResult<PlayerModel>>(players);
    }

    /// <summary>
    /// Retrieve the player with the specified ID
    /// </summary>
    /// <param name="id">ID of the player to retrieve</param>
    /// <param name="handler" />
    /// <param name="ct" />
    /// <response code="404">Player with the specified ID was not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PlayerModel), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public async Task<PlayerModel> GetPlayerAsync(
        [FromServices] IQueryHandler<SingleEntityQuery<Player>, Player> handler,
        long id,
        CancellationToken ct)
    {
        var player = await handler.HandleAsync(new(id), ct);
        return _mapper.Map<PlayerModel>(player);
    }

    /// <summary>
    /// Add a player
    /// </summary>
    /// <response code="400">Input data was incorrect</response>
    [HttpPut("")]
    [ProducesResponseType(typeof(PlayerModel), 200)]
    [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
    public async Task<PlayerModel> AddPlayerAsync(
        [FromServices] ICommandHandler<AddPlayerCommand, Player> handler,
        [FromBody] CreatePlayerModel createModel,
        CancellationToken ct)
    {
        var command = _mapper.Map<AddPlayerCommand>(createModel);
        var newPlayer = await handler.HandleAsync(command, ct);
        return _mapper.Map<PlayerModel>(newPlayer);
    }

    /// <summary>
    /// Patch player with the specified ID
    /// </summary>
    /// <param name="id">ID of the player to patch</param>
    /// <param name="handler" />
    /// <param name="patchModel" />
    /// <param name="ct" />
    /// <response code="404">Player with the specified ID was not found</response>
    [HttpPatch("{id}")]
    [ProducesResponseType(typeof(PlayerModel), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public async Task<PlayerModel> PatchPlayerAsync(
        [FromServices] ICommandHandler<UpdatePlayerCommand, Player> handler,
        long id,
        [FromBody] PatchPlayerModel patchModel,
        CancellationToken ct)
    {
        var command = _mapper.Map<UpdatePlayerCommand>(patchModel) with { Id = id };
        var patchedPlayer = await handler.HandleAsync(command, ct);
        return _mapper.Map<PlayerModel>(patchedPlayer);
    }

    /// <summary>
    /// Retrieve the list of transactions of the player with the specified ID
    /// </summary>
    /// <param name="id">ID of the player to retrieve transactions of</param>
    /// <param name="handler" />
    /// <param name="filter" />
    /// <param name="pageArgs" />
    /// <param name="ct" />
    /// <response code="404">Player with the specified ID was not found</response>
    [HttpGet("{id}/transactions")]
    [ProducesResponseType(typeof(PagedResult<TransactionModel>), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public async Task<PagedResult<TransactionModel>> ListPlayerTransactionsAsync(
        [FromServices] IQueryHandler<GetPlayerTransactionsQuery, PagedResult<Transaction>> handler,
        long id,
        [FromQuery] TransactionFilter filter,
        [FromQuery] PageArgs pageArgs,
        CancellationToken ct)
    {
        var query = new GetPlayerTransactionsQuery(
            pageArgs,
            id,
            filter.CreatedOn ?? RangeFilter<DateTime>.Empty,
            filter.Type,
            filter.SortDirection ?? SortDirection.Ascending);

        var transactions = await handler.HandleAsync(query, ct);
        return _mapper.Map<PagedResult<TransactionModel>>(transactions);
    }

    /// <summary />
    public PlayersController(IMapper mapper)
    {
        _mapper = mapper;
    }
}

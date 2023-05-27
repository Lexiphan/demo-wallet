using AutoMapper;
using Greentube.DemoWallet.Application.Players;
using Greentube.DemoWallet.Application.Transactions;
using Greentube.DemoWallet.Domain;

namespace Greentube.DemoWallet.Api.Models.V1;

internal class ApiV1MapperProfile : Profile
{
    public ApiV1MapperProfile()
    {
        CreateMap<Player, PlayerModel>();
        CreateMap<Transaction, TransactionModel>();
        CreateMap<CreatePlayerModel, AddPlayerCommand>();

        CreateMap<PatchPlayerModel, UpdatePlayerCommand>()
            .ForMember(d => d.Id, opts => opts.MapFrom(s => 0));

        CreateMap<PlayerAmountModel, TopUpCommand>();
        CreateMap<PlayerAmountModel, WinCommand>();
        CreateMap<PlayerAmountModel, BetCommand>();
    }
}

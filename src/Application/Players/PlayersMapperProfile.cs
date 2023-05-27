using AutoMapper;
using Greentube.DemoWallet.Domain;

namespace Greentube.DemoWallet.Application.Players;

internal class PlayersMapperProfile : Profile
{
    public PlayersMapperProfile()
    {
        CreateMap<AddPlayerCommand, Player>(MemberList.Source);

        CreateMap<UpdatePlayerCommand, Player>(MemberList.Source)
            .ForSourceMember(d => d.Id, opts => opts.DoNotValidate())
            .ForMember(d => d.Id, opts => opts.Ignore())
            .ForMember(d => d.Name, opts => opts.PreCondition(s => s.Name is not null))
            .ForMember(d => d.BalanceLimit, opts => opts.PreCondition(s => s.BalanceLimit is not null));
    }
}

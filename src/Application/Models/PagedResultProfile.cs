using AutoMapper;

namespace Greentube.DemoWallet.Application.Models;

internal class PagedResultProfile : Profile
{
    public PagedResultProfile()
    {
        CreateMap(typeof(PagedResult<>), typeof(PagedResult<>));
    }
}

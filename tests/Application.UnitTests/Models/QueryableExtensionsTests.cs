using Greentube.DemoWallet.Domain;
using Greentube.DemoWallet.Application.Models;

namespace Greentube.DemoWallet.Application.UnitTests.Models;

public class QueryableExtensionsTests
{
    private readonly Fixture _fixture = new();
    private readonly Player[] _entities;
    private readonly IQueryable<Player> _entitiesQueryable;

    public QueryableExtensionsTests()
    {
        _entities = _fixture.CreateMany<Player>(10).ToArray();
        _entitiesQueryable = _entities.AsQueryable();
    }

    [Theory]
    [InlineData(0, 100, false, 0, 10)]
    [InlineData(0, 10, false, 0, 10)]
    [InlineData(0, 9, true, 0, 9)]
    [InlineData(0, 3, true, 0, 3)]
    [InlineData(1, 3, true, 3, 3)]
    [InlineData(2, 3, true, 6, 3)]
    [InlineData(3, 3, false, 9, 1)]
    public void Paging_Works_TogetherWithPagedResultFactory(int page, int pageSize, bool expectedHasMore, int expectedOffset, int expectedCount)
    {
        var query = new PagedEntityQuery<Player>(new() { Page = page, PageSize = pageSize });
        var result = _entitiesQueryable.ApplyPaging(query).ToList().ToPagedResult(query);

        result.PageSize.Should().Be(query.PageArgs.PageSize);
        result.Page.Should().Be(query.PageArgs.Page);
        result.HasMore.Should().Be(expectedHasMore);
        result.Items.Should().Equal(_entities[expectedOffset..(expectedOffset + expectedCount)]);
    }

    [Fact]
    public void ApplyRange_Empty_ReturnsItself()
    {
        _entitiesQueryable.ApplyRange(RangeFilter<decimal>.Empty, x => x.BalanceLimit)
            .Should()
            .BeSameAs(_entitiesQueryable);
    }

    [Fact]
    public void ApplyRange_GreaterOrEqualToDefined_Works()
    {
        var min = _entities.Min(x => x.BalanceLimit);
        var max = _entities.Max(x => x.BalanceLimit);
        var someValue = _entities.First(x => x.BalanceLimit > min && x.BalanceLimit < max).BalanceLimit;

        var expectedEntities = _entities.Where(x => x.BalanceLimit >= someValue).ToArray();

        _entitiesQueryable.ApplyRange(new RangeFilter<decimal>(someValue, null), x => x.BalanceLimit).ToArray()
            .Should()
            .Contain(expectedEntities);
    }

    [Fact]
    public void ApplyRange_LessThanDefined_Works()
    {
        var min = _entities.Min(x => x.BalanceLimit);
        var max = _entities.Max(x => x.BalanceLimit);
        var someValue = _entities.First(x => x.BalanceLimit > min && x.BalanceLimit < max).BalanceLimit;

        var expectedEntities = _entities.Where(x => x.BalanceLimit < someValue).ToArray();

        _entitiesQueryable.ApplyRange(new RangeFilter<decimal>(null, someValue), x => x.BalanceLimit).ToArray()
            .Should()
            .Contain(expectedEntities);
    }

    [Fact]
    public void ApplyRange_BothDefined_Works()
    {
        var min = _entities.Min(x => x.BalanceLimit);
        var max = _entities.Max(x => x.BalanceLimit);
        var someValue1 = _entities.First(x => x.BalanceLimit > min && x.BalanceLimit < max).BalanceLimit;
        var someValue2 = _entities.Last(x => x.BalanceLimit > min && x.BalanceLimit < max).BalanceLimit;
        if (someValue1 > someValue2)
        {
            (someValue1, someValue2) = (someValue2, someValue1);
        }

        var expectedEntities = _entities.Where(x => x.BalanceLimit >= someValue1 && x.BalanceLimit < someValue2).ToArray();

        _entitiesQueryable.ApplyRange(new RangeFilter<decimal>(someValue1, someValue2), x => x.BalanceLimit).ToArray()
            .Should()
            .Contain(expectedEntities);
    }
}

using Greentube.DemoWallet.Api.Models.V1;
using Greentube.DemoWallet.Database.Contracts;
using Greentube.DemoWallet.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Greentube.DemoWallet.Api.Controllers.V1;

/// <summary>
/// Helps for testing
/// </summary>
[ApiController]
[Route("v{version:apiVersion}/[controller]")]
[ApiVersion("1")]
public class TestController : ControllerBase
{
    /// <summary>
    /// Adds some players optionally with transactions to the database
    /// </summary>
    [HttpPost("populateTestData")]
    public async Task PopulateTestDataAsync(
        [FromServices] IWalletDatabase walletDatabase,
        [FromBody] PopulateTestDataModel args,
        CancellationToken ct)
    {
        // Since this is a test controller which shouldn't go to production, I didn't apply CQRS pattern for its
        // realization for simplicity, the database is accessed right from the controller.

        await walletDatabase.TransactionAsync(
            async repository =>
            {
                for (var i = args.NumberOfPlayers; i > 0; i--)
                {
                    var now = DateTime.UtcNow -
                              Random.Shared.NextDouble() * TimeSpan.FromDays(335) -
                              TimeSpan.FromDays(30);

                    var player = new Player(
                        $"{RandomElement(Names)} {RandomElement(Surnames)}",
                        Random.Shared.Next(2) > 0 ? 0 : -Random.Shared.Next(51),
                        now - RandomTime);

                    for (var j = args.NumberOfTransactions; j > 0; now += (DateTime.UtcNow - now) / Math.Max(j, 1), j--)
                    {
                        var wantToBet = RandomAmount;

                        while (!player.Bet(wantToBet, now) && j > 0)
                        {
                            player.TopUp(Random.Shared.Next(100, 201), now += RandomTime);
                            j--;
                        }

                        if (Random.Shared.Next(2) > 0 && j > 0)
                        {
                            player.Win(wantToBet + RandomAmount, now += RandomTime);
                            j--;
                        }
                    }

                    await repository.AddPlayerAsync(player, ct);
                }
            },
            ct);
    }

    private static decimal RandomAmount => (decimal)Math.Round(Random.Shared.NextDouble() * 49.9 + 0.1, 2);

    private static TimeSpan RandomTime => Random.Shared.NextDouble() * TimeSpan.FromMinutes(15) + TimeSpan.FromMinutes(1);

    private static string RandomElement(string[] strings) => strings[Random.Shared.Next(0, strings.Length)];

    private static readonly string[] Names = (
            "Aaron,Adrian,Aidan,Albert,Alberta,Alberto,Alexia,Alexis,Alice,Alisa,Alison,Amanda,Anabelle,Angela,Angelique," +
            "Antonio,Arthur,Ashley,Beatrice,Benjamin,Bethany,Beverly,Blake,Blanca,Blanche,Brandi,Brian,Bruce,Camille," +
            "Cara,Carlos,Carolina,Cedric,Charlie,Christina,Christopher,Chrysta,Clarence,Clark,Claude,Claudia,Conner," +
            "Connor,Craig,Daisy,Dan,Danna,Dave,Deborah,Debra,Della,Desiree,Douglas,Eli,Ernest,Ethan,Eunice,Everett," +
            "Francesca,Fred,Gabriela,Gemma,George,Gerald,Gilbert,Glen,Gloria,Graciela,Greyson,Hanna,Hannah,Harmony," +
            "Harold,Harold,Harvey,Hector,Helena,Hope,Hunter,Isabel,Ivan,Jaime,Janessa,Janet,Janine,Jerry,Jessie,Jimmy," +
            "Joe,Joey,Johnny,Jorge,Joshua,Julian,Juliana,Julie,Katelynn,Kathie,Kathryn,Katrina,Kayla,Kayleigh,Keith," +
            "Kianna,Kimberly,Kingston,Lauren,Laverne,Leona,Lesley,Lewis,Liam,Loretta,Lucia,Lucille,Lucy,Luisa,Luka,Luna," +
            "Lynda,Maggie,Maribel,Marion,Mark,Marlin,Marshall,Martin,Melody,Mila,Miles,Milton,Miranda,Molly,Monique," +
            "Morris,Myra,Nathalie,Nathaniel,Neil,Nick,Nicole,Noel,Odessa,Olive,Orlando,Patricia,Paul,Paul,Perry,Peter," +
            "Philip,Phoebe,Rachelle,Ramon,Ricardo,Rick,Riley,Roberto,Robin,Robyn,Romeo,Ron,Rosa,Rose,Rosie,Sabrina," +
            "Samuel,Sandra,Sandy,Scott,Sebastian,Seth,Shane,Shawn,Silvia,Skyla,Sophia,Sophie,Stefan,Stella,Stephanie," +
            "Stuart,Ted,Teresa,Terry,Tommy,Tracey,Traci,Tristin,Troy,Tyra,Valentina,Vera,Vicki,Victor,Vivian,Wade," +
            "Walter,Whitney,Wiley"
            )
        .Split(',');

    private static readonly string[] Surnames = (
            "Adams,Allen,Alvarez,Anderson,Arnold,Bailey,Baker,Bell,Bennett,Black,Brown,Bryant,Burns,Butler,Campbell," +
            "Carter,Clark,Coleman,Cook,Cooper,Cox,Crawford,Daniel,Davis,Dudley,Ellis,Erickson,Eriksen,Evans,Fisher," +
            "Fisher,Fletcher,Flores,Ford,Foreman,Foster,Francis,Garcia,Gentry,Gibson,Gomez,Gonzalez,Gordon,Graham," +
            "Grant,Green,Griffin,Gross,Guzman,Hall,Hamilton,Harris,Hicks,Holland,Holmes,Howard,Howell,Hughes,Hunter," +
            "Jacobs,Jenkins,Jones,Jordan,Kennedy,Kimmel,Knight,Knowles,Lara,Leach,Lee,Lindsey,Little,Livingston,Long," +
            "Lopez,Lucas,Martin,Martinez,Mason,Matthews,Mcdonald,Meadows,Miles,Miller,Mitchell,Moody,Morgan,Mosby," +
            "Murphy,Murray,Nelson,Newton,Noble,Norman,Norris,Pace,Parks,Patel,Patterson,Perez,Perry,Peterson," +
            "Phillips,Potter,Powell,Ramirez,Reid,Reynolds,Rice,Richards,Richmond,Rivera,Roberts,Robinson,Rodriguez," +
            "Russell,Sanders,Saunders,Scott,Shaw,Shelton,Sheppard,Sherman,Simmons,Simon,Singh,Smith,Spencer,Stanley," +
            "Stephens,Stewart,Terry,Thomas,Trujillo,Tucker,Valentine,Wagner,Walker,Warburton,Ward,Warren,Watkins," +
            "Weasley,Wheeler,White,Williams,Wood,Woodward,Wright,Young"
        )
        .Split(',');
}

using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using UnitTest.Dk.Itu.Rlh.MasterProject;

static internal class FixtureHelper
{
    private static IFixture GetFixture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());
        fixture.Customizations.Add(new RegexSpecimenBuilder("paragrafNummer", @"^\d+( ([a-z]|[A-Z])(?!\w))?$"));
        return fixture;
    }
}
using Aidan.Common.Utils.Test;
using BrunelUni.IntelliFarm.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.Domain;

namespace BrunelUni.IntelliFarm.Tests.SceneAnalyser
{
    public class Given_A_RenderAnalyser : GivenWhenThen<IRenderAnalyser>
    {
        protected override void Given( )
        {
            SUT = new BlenderCyclesRenderAnalyser( );
        }

        public static PredictorFixtureDto [ ] WeyFixture => FixtureHelper.WeyFixture;
        public static object[ ] WeyOrderFixture1 => FixtureHelper.GetWeyOrderFixture1;
        public static object[ ] WeyOrderFixture2 => FixtureHelper.GetWeyOrderFixture2;
    }
}
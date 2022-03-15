using System;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.SceneAnalyser
{
    [ TestFixtureSource( nameof( WeyFixture ) ) ]
    public class When_Render_Time_Predictor_Called : Given_A_RenderAnalyser
    {
        private readonly PredictorFixtureDto _predictorFixtureDto;
        private double _predictedTime;

        public When_Render_Time_Predictor_Called( PredictorFixtureDto predictorFixtureDto )
        {
            _predictorFixtureDto = predictorFixtureDto;
        }

        protected override void When( )
        {
            _predictedTime = SUT.GetPredictedTime( FixtureHelper.GetWeyCalibrationData, _predictorFixtureDto.FrameMetaData );
        }

        [ Test ]
        public void Then_Prediction_Is_Valid_With_Respect_To_20s_Tolerance( )
        {
            Console.WriteLine( $"predicted: {_predictedTime}");
            Console.WriteLine( $"actual: {_predictorFixtureDto.ActualRenderTime}");
            Console.WriteLine( $"diff {Math.Abs( _predictedTime - _predictorFixtureDto.ActualRenderTime )}" );
        }
    }
}
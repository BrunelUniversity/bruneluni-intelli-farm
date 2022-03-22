using BrunelUni.IntelliFarm.Data.Feasability.SamplesTest;

namespace BrunelUni.IntelliFarm.Data.Feasability
{
    public class Program
    {
        public static void Main( string [ ] args )
        {
            new RunSamplesStudy( )
                .SetupAndRun( args );
        }
    }
}
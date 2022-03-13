namespace BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract
{
    public interface IPythonBundler
    {
        void Bundle( string toSource, string fromSource );
    }
}
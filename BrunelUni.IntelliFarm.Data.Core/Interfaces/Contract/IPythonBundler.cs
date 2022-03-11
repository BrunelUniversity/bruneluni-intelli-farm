namespace BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract
{
    public interface IPythonBundler
    {
        void CopySources( string toSource, string fromSource );
    }
}
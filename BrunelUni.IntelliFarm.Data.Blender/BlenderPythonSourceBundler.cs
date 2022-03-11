using System.IO;
using System.Linq;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.Data.Blender
{
    public class BlenderPythonSourceBundler : IPythonBundler
    {
        public void CopySources( string toSource, string fromSource )
        {
            Directory.CreateDirectory( $"{toSource}\\src" );
            var files = from f in Directory.EnumerateFiles( $"{fromSource}\\src" )
                where f.EndsWith( ".py" )
                select f;
            foreach( var file in files )
            {
                var fileName = Path.GetFileName( file );
                File.Copy( $"{fromSource}\\src\\{fileName}",
                    $"{toSource}\\src\\{fileName}",
                    true );
            }
        }
    }
}
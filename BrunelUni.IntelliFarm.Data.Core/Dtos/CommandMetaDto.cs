namespace BrunelUni.IntelliFarm.Data.Core.Dtos
{
    public class CommandMetaDto
    {
        public string Command { get; set; }
        public bool Render { get; set; }
        public RenderMetaDto RenderMetaDto { get; set; }
        public ScriptsRootDirectoryDto ScriptsRootDirectoryDto { get; set; }
    }
}
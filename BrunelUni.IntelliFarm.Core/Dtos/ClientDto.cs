using System;

namespace BrunelUni.IntelliFarm.Core.Dtos
{
    public class ClientDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public CallibrationDto CallibrationMeta { get; set; }
    }
}
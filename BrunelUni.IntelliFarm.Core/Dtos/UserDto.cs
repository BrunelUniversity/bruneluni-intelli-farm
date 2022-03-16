using System;
using System.ComponentModel;

namespace BrunelUni.IntelliFarm.Core.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }

        [ Description( "will be guid in db, but need to allow null" ) ]
        public string Client { get; set; }

        public string AuthId { get; set; }
    }
}
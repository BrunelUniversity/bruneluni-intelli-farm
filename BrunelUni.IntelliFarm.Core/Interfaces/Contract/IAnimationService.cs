using BrunelUni.IntelliFarm.Core.Dtos;

namespace BrunelUni.IntelliFarm.Core.Interfaces.Contract
{

    public interface IAnimationService
    {
        void RenderScene( SceneTinyType scene );
        CallibrationDto CollaborateScene( SceneTinyType scene );
    }
}
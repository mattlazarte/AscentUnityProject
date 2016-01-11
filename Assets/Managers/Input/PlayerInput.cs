
namespace Ascent.Managers.Input
{
    public class PlayerInput
    {
        public float MoveForward { get; set; }
        public float StrafeRight { get; set; }
        public float RollLeft { get; set; }
        public float StrafeUp { get; set; }
        public float YawDelta { get; set; }
        public float PitchDelta { get; set; }

        public bool SelectPlasmaGun { get; set; }
        public bool SelectLaser { get; set; }
        public bool SelectUnguidedMissiles { get; set; }
        public bool SelectGuidedMissiles { get; set; }

        public bool CiclePrimaryWeapon { get; set; }
        public bool CicleSecondaryWeapon { get; set; }

        public bool ShootPrimaryWeapon { get; set; }
        public bool ShootSecondaryWeapon { get; set; }

        public bool ToggleAutoLevel { get; set; }
        public bool ToggleHeadlights { get; set; }

        public bool Pause { get; set; } 
    }
}

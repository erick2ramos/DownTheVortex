using UnityEngine;
using System.Collections;

namespace BaseSystems.Managers
{
    public class VibrationManager : Manager
    {
        public void Vibrate()
        {
            if (DataPersistance.DataPersistanceManager.PlayerData.CanVibrate)
            {
                Handheld.Vibrate();
            }
        }
    }
}
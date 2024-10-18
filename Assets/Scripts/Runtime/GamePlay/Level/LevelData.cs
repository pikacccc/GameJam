using System.Collections.Generic;
using UnityEngine;

namespace Runtime.GamePlay.Level
{
    [CreateAssetMenu(fileName = "Level", menuName = "Level/LevelData", order = 1)]
    public class LevelData : ScriptableObject
    {
        public List<string> levelList;
    }
}
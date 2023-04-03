using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CCSystem
{
    /// <summary>
    /// Closed Caption Database is an asset that map given AudioClip to a list of lines & time used by the CCManager
    /// </summary>
    [CreateAssetMenu(fileName = "MusicDatabase", menuName = "ObjectSpawnEvent/Music Database")]
    public class Database : ScriptableObject
    {
        [System.Serializable]
        public class MusicDatas
        {
            public bool menuMusic;
            public AudioClip audioClip;
            public float Bpm;
            public float firstBpmDelay;
        }

        [System.Serializable]
        public class Timestamp
        {
            public float SpawnBeat;
            [Range(1, 3)] public int Lane;           
            public MovingElement.ElementType elementType;
        }


        public MusicDatas musicDatas = new MusicDatas();
        public Timestamp[] ObjectSpawns = new Timestamp[0];
    }
}
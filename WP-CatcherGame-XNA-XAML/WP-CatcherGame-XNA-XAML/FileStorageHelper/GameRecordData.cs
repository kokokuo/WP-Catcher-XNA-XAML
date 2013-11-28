using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization; 
using CatcherGame.GameObjects;

namespace CatcherGame.FileStorageHelper
{
    [DataContract]
    public class GameRecordData
    {
        [DataMember]   
        public int HistoryTopSavedNumber { get; set; }

        List<DropObjectsKeyEnum> caughtObjects;

        [DataMember]
        public List<DropObjectsKeyEnum> CaughtDropObjects { get; set; }

        [DataMember]
        public int CurrentSavePeopleNumber { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;
using System.IO;
using System.Diagnostics;
namespace CatcherGame.FileStorageHelper
{
    //Singleton Pattern : 檔案讀寫只能有一個類別存在 (由外面使用時,自己初始化自己,一旦初始後就不會再被初始化)
    public sealed class StorageHelperSingleton
    {   
        private static StorageHelperSingleton instance = null;
        private static readonly object padlock = new object();
        string fileName;
        IsolatedStorageFile recordFile;
        DataContractSerializer serializer;
        string recordTargetFolderName;
        private StorageHelperSingleton()
        {
            recordFile = IsolatedStorageFile.GetUserStoreForApplication();
            fileName = "catcher.record";
            recordTargetFolderName = "DataRecord";
            serializer = new DataContractSerializer(typeof(GameRecordData));
        }

        public static StorageHelperSingleton Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new StorageHelperSingleton();
                    }
                    return instance;
                }
            }
        }
        /// <summary>
        /// 載入檔案
        /// </summary>
        /// <returns></returns>
        public GameRecordData LoadGameRecordData(){
            GameRecordData loadData = null;
            string TargetFileName = String.Format("{0}\\{1}",
                                              recordTargetFolderName, fileName);
            try
            {
                if (recordFile.FileExists(TargetFileName))
                {
                    using (var sourceStream = recordFile.OpenFile(TargetFileName, FileMode.Open))
                    {
                        loadData = (GameRecordData)serializer.ReadObject(sourceStream);
                    }
                }
            }
            catch (Exception e) {
                Debug.WriteLine(e.Message);

                if (recordFile.FileExists(TargetFileName))
                {
                    using (var sourceStream = recordFile.OpenFile(TargetFileName, FileMode.Open))
                    {
                        loadData = (GameRecordData)serializer.ReadObject(sourceStream);
                    }
                }
            }
            return loadData; 
        }

        /// <summary>
        /// 以序列化方式存檔存入
        /// </summary>
        /// <param name="saveData"></param>
        public void SaveGameRecordData(GameRecordData saveData){
            string TargetFileName = String.Format("{0}\\{1}",
                                             recordTargetFolderName, fileName);
           
            if (!recordFile.DirectoryExists(recordTargetFolderName))
                recordFile.CreateDirectory(recordTargetFolderName);
            try
            {
                using (var targetFile = recordFile.CreateFile(TargetFileName))
                {
                    serializer.WriteObject(targetFile, saveData);
                } 
                //會有問題
                //using (var isoFileStream = new System.IO.IsolatedStorage.IsolatedStorageFileStream(TargetFileName, System.IO.FileMode.OpenOrCreate, recordFile))
                //{
                    //使用序列化方式寫入
                   // serializer.WriteObject(isoFileStream, saveData);
                //}
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            } 
        }
    }
}

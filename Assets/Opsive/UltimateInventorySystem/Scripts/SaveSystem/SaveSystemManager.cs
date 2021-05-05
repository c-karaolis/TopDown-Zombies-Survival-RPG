/// ---------------------------------------------
/// Ultimate Inventory System
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

//#define DEBUG_SYSTEM_SAVER

namespace Opsive.UltimateInventorySystem.SaveSystem
{
    using Opsive.Shared.Utility;
    using Opsive.UltimateInventorySystem.Core;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using EventHandler = Opsive.Shared.Events.EventHandler;

    /// <summary>
    /// The save system manager is used by saver components to load and save any serializable data.
    /// </summary>
    public class SaveSystemManager : MonoBehaviour
    {
        [Tooltip("Load the last save data automatically on start.")]
        [SerializeField] protected bool m_AutoLoadOnInitialize;
        [Tooltip("Load the last save data automatically on start.")]
        [SerializeField] protected bool m_AutoLoadOnSceneLoaded;
        [Tooltip("Save when a scene is unloaded.")]
        [SerializeField] protected bool m_AutoSaveOnSceneUnloaded;
        [Tooltip("Automatically save the game before the application quits.")]
        [SerializeField] protected bool m_AutoSaveOnApplicationQuit;
        [Tooltip("The save file name when saved on disk.")]
        [SerializeField] protected string m_SaveFileName = "SaveFile";
        [Tooltip("The maximum number of save files possible.")]
        [SerializeField] protected int m_MaxSaves = 5;
        [Tooltip("The Inventory System Manager Item Save used to save items that were created at runtime.")]
        [SerializeField] internal InventorySystemManagerItemSaver m_InventorySystemManagerItemSaver;
        [Tooltip("This will make a copy of the save file as Json, should only be used to debug save data.")]
        [SerializeField] protected bool m_DebugJsonCopy;

        protected SaveData m_SaveData;
        protected Dictionary<int, SaveData> m_Saves;
        protected List<SaverBase> m_Savers;
        protected SaveData[] m_SavesArray;

        #region Singleton and Static functions

#if UNITY_2019_3_OR_NEWER
        /// <summary>
        /// Reset the static variables for domain reloading.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void DomainReset()
        {
            s_Initialized = false;
            s_Instance = null;
        }
#endif

        private static SaveSystemManager s_Instance;
        public static SaveSystemManager Instance {
            get {
                if (!s_Initialized) {
                    s_Instance = new GameObject("SaveSystemManager").AddComponent<SaveSystemManager>();
                    s_Instance.Initialize(false);
                }
                return s_Instance;
            }
        }
        private static bool s_Initialized;

        /// <summary>
        /// Returns true if the InventorySystemManger was not Initialized.
        /// </summary>
        public static bool IsNull => s_Instance == null || s_Initialized == false;

        public static int MaxSaves => Instance.m_MaxSaves;

        public static IReadOnlyList<SaverBase> Savers => Instance.m_Savers;

        public static IReadOnlyDictionary<int, SaveData> Saves => Instance.m_Saves;

        public static InventorySystemManagerItemSaver InventorySystemManagerItemSaver =>
            Instance.m_InventorySystemManagerItemSaver;


        /// <summary>
        /// Called on Awake Initializes the Manager.
        /// </summary>
        protected virtual void Awake()
        {
            Initialize(false);
        }

        /// <summary>
        /// Initializes the Manager using the database if one is specified.
        /// </summary>
        public virtual void Initialize(bool force)
        {
            if (s_Initialized && !force) { return; }

            s_Instance = this;
            s_Initialized = true;
            SceneManager.sceneUnloaded -= SceneUnloadedWhileDisabled;

            if (m_InventorySystemManagerItemSaver == null) {
                m_InventorySystemManagerItemSaver = GetComponent<InventorySystemManagerItemSaver>();
            }

            m_SavesArray = new SaveData[m_MaxSaves];
            m_Savers = new List<SaverBase>();
            m_Saves = new Dictionary<int, SaveData>();
            GetSavesFromDisk(ref m_Saves);

            for (int i = 0; i < m_SavesArray.Length; i++) {
                if (m_Saves.ContainsKey(i)) {
                    m_SavesArray[i] = m_Saves[i];
                }
            }

            if (m_AutoLoadOnInitialize && m_Saves != null && m_Saves.ContainsKey(0)) {
                m_SaveData = new SaveData(m_Saves[0]);
            } else {
                m_SaveData = new SaveData();
            }
        }

        /// <summary>
        /// The object has been enabled.
        /// </summary>
        protected virtual void OnEnable()
        {
            SceneManager.sceneUnloaded += SceneUnloaded;
            SceneManager.sceneLoaded += SceneLoaded;
        }

        /// <summary>
        /// The object has been disabled.
        /// </summary>
        protected virtual void OnDisable()
        {
            SceneManager.sceneUnloaded += SceneUnloadedWhileDisabled;

            SceneManager.sceneUnloaded -= SceneUnloaded;
            SceneManager.sceneLoaded -= SceneLoaded;
        }

        /// <summary>
        /// Handle the scene being loaded.
        /// </summary>
        /// <param name="scene">The scene.</param>
        /// <param name="loadSceneMode">The loading mode.</param>
        protected virtual void SceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (m_AutoLoadOnSceneLoaded && m_Saves != null && m_Saves.ContainsKey(0)) {
                Load(0);
            }
        }

        /// <summary>
        /// Handle a scene being unloaded.
        /// </summary>
        /// <param name="scene">The scene being unloaded.</param>
        protected virtual void SceneUnloaded(Scene scene)
        {
            if (m_AutoSaveOnSceneUnloaded) {
                Save(0);
            }
        }

        #region Register Unregister Savers

        /// <summary>
        /// Register a saver component.
        /// </summary>
        /// <param name="saver">The saver to register.</param>
        public static void RegisterSaver(SaverBase saver)
        {
            Instance.RegisterSaverInternal(saver);
        }

        /// <summary>
        /// Unregister a saver.
        /// </summary>
        /// <param name="saver">The saver to unregister.</param>
        public static void UnregisterSaver(SaverBase saver)
        {
            Instance.UnregisterSaverInternal(saver);
        }

        #endregion

        #region Save & Load

        /// <summary>
        /// Add serialized data to the save data.
        /// </summary>
        /// <param name="fullKey">The serialized data key.</param>
        /// <param name="serializedSaveData">The serialized data.</param>
        public static void AddToSaveData(string fullKey, Serialization serializedSaveData)
        {
            Instance.AddToSaveDataInternal(fullKey, serializedSaveData);
        }

        /// <summary>
        /// Remove the serialized data from the save data.
        /// </summary>
        /// <param name="fullKey">The serialized data key.</param>
        public static void RemoveFromSaveData(string fullKey)
        {
            Instance.RemoveFromSaveDataInternal(fullKey);
        }

        /// <summary>
        /// Save the game at the file index.
        /// </summary>
        /// <param name="saveIndex">The save file index.</param>
        public static void Save(int saveIndex)
        {
            Instance.SaveInternal(saveIndex);
        }

        /// <summary>
        /// Load the game data from the save file index.
        /// </summary>
        /// <param name="saveIndex">The save file index.</param>
        public static void Load(int saveIndex)
        {
            Instance.LoadInternal(saveIndex);
        }

        #endregion

        #region Getters & Setters

        /// <summary>
        /// Try get the save data.
        /// </summary>
        /// <param name="fullKey">The serialized data key.</param>
        /// <param name="serializedData">The serialize data.</param>
        /// <returns>True if the save data exists.</returns>
        public static bool TryGetSaveData(string fullKey, out Serialization serializedData)
        {
            return Instance.TryGetSaverDataInternal(fullKey, out serializedData);
        }

        /// <summary>
        /// Get all the save data.
        /// </summary>
        /// <returns>The save datas.</returns>
        public static IReadOnlyList<SaveData> GetSaves()
        {
            return Instance.GetSavesInternal();
        }

        /// <summary>
        /// Get the current save data.
        /// </summary>
        /// <returns>The save data.</returns>
        public static SaveData GetCurrentSaveData()
        {
            return Instance.GetCurrentSaveDataInternal();
        }

        /// <summary>
        /// Set the current save data.
        /// </summary>
        /// <param name="newSaveData">The new save data.</param>
        public static void SetCurrentSaveData(SaveData newSaveData)
        {
            Instance.SetCurrentSaveDataInternal(newSaveData);
        }

        #endregion

        #region Delete

        /// <summary>
        /// Delete a save from the save file index.
        /// </summary>
        /// <param name="saveIndex">The save file index.</param>
        public static void DeleteSave(int saveIndex)
        {
            Instance.DeleteSaveInternal(saveIndex);
        }

        #endregion

        #region Singleton

        /// <summary>
        /// Destroys the object instance on the network.
        /// </summary>
        /// <param name="obj">The object to destroy.</param>
        public static void Destroy(GameObject obj)
        {
            if (s_Instance == null) {
                Debug.LogError("Error: Unable to destroy object - the Inventory System Manager doesn't exist.");
                return;
            }
            s_Instance.DestroyInternal(obj);
        }

        /// <summary>
        /// Reset the initialized variable when the scene is no longer loaded.
        /// </summary>
        /// <param name="scene">The scene that was unloaded.</param>
        protected virtual void SceneUnloadedWhileDisabled(Scene scene)
        {
            s_Instance = null;
            s_Initialized = false;

            SceneManager.sceneUnloaded -= SceneUnloadedWhileDisabled;
        }

        /// <summary>
        /// Do something when object is destroyed.
        /// </summary>
        protected virtual void OnDestroy()
        {
            DestroyInternal(gameObject);
        }

        #endregion
        #endregion

        /// <summary>
        /// Register the saver components
        /// </summary>
        /// <param name="saver">The saver.</param>
        protected virtual void RegisterSaverInternal(SaverBase saver)
        {
            for (int i = 0; i < m_Savers.Count; i++) {
                if (m_Savers[i].FullKey != saver.FullKey) { continue; }

                if (m_Savers[i] != saver) {
                    Debug.LogWarningFormat("Saver won't be registered because one with the same key is already registered");
                }

                return;
            }
            m_Savers.Add(saver);
        }

        /// <summary>
        /// Unregister saver components
        /// </summary>
        /// <param name="saver">The saver.</param>
        protected virtual void UnregisterSaverInternal(SaverBase saver)
        {
            m_Savers.Remove(saver);
        }

        /// <summary>
        /// Get the current save data.
        /// </summary>
        /// <returns>The save data.</returns>
        public virtual SaveData GetCurrentSaveDataInternal()
        {
            return m_SaveData;
        }

        /// <summary>
        /// Set the current save data.
        /// </summary>
        /// <param name="newSaveData">The new save data.</param>
        public virtual void SetCurrentSaveDataInternal(SaveData newSaveData)
        {
            if (newSaveData == null) { return; }
            m_SaveData = newSaveData;
        }

        /// <summary>
        /// Add save data.
        /// </summary>
        /// <param name="fullKey">The full key.</param>
        /// <param name="serializedSaveData">The serialized data.</param>
        protected virtual void AddToSaveDataInternal(string fullKey, Serialization serializedSaveData)
        {
            m_SaveData[fullKey] = serializedSaveData;
        }

        /// <summary>
        /// Add save data.
        /// </summary>
        /// <param name="fullKey">The full key.</param>
        /// <param name="serializedSaveData">The serialized data.</param>
        protected virtual void RemoveFromSaveDataInternal(string fullKey)
        {
            m_SaveData.Remove(fullKey);
        }

        /// <summary>
        /// Save the data to the index provided.
        /// </summary>
        /// <param name="saveIndex">The save index.</param>
        protected virtual void SaveInternal(int saveIndex)
        {
            EventHandler.ExecuteEvent<int>(EventNames.c_WillStartSaving_Index,saveIndex);
            
            OrderSaversByPriority(true);
            for (int i = 0; i < m_Savers.Count; i++) {
                m_Savers[i].Save();
            }

            SaveToDiskInternal(saveIndex);
            
            EventHandler.ExecuteEvent(EventNames.c_SavingComplete_Index,saveIndex);
        }

        /// <summary>
        /// Try get the save data.
        /// </summary>
        /// <param name="fullKey">The full key.</param>
        /// <param name="serializedData">The serialized data.</param>
        /// <returns>True if the save data exists.</returns>
        protected virtual bool TryGetSaverDataInternal(string fullKey, out Serialization serializedData)
        {
            if (m_SaveData == null) { Initialize(false); }
            return m_SaveData.TryGetValue(fullKey, out serializedData);
        }

        /// <summary>
        /// Load the saved data from the save index provided.
        /// </summary>
        /// <param name="saveIndex">The save index.</param>
        protected virtual void LoadInternal(int saveIndex)
        {
#if DEBUG_SYSTEM_SAVER
            Debug.Log("Load file at index "+saveIndex);
#endif
            if (m_Saves == null || !m_Saves.ContainsKey(saveIndex)) {
                Debug.LogError($"Cannot load save at index {saveIndex}.");
                return;
            }

            EventHandler.ExecuteEvent(EventNames.c_WillStartLoadingSave_Index,saveIndex);
            
            m_SaveData = new SaveData(m_Saves[saveIndex]);

            OrderSaversByPriority(false);
            for (int i = 0; i < m_Savers.Count; i++) {
                m_Savers[i].Load();
            }
            
            EventHandler.ExecuteEvent(EventNames.c_LoadingSaveComplete_Index,saveIndex);
        }

        /// <summary>
        /// Delete the save from the save index.
        /// </summary>
        /// <param name="saveIndex">The save index.</param>
        protected virtual void DeleteSaveInternal(int saveIndex)
        {
            DeleteFromDiskInternal(saveIndex);
        }

        /// <summary>
        /// Return the save folder path.
        /// </summary>
        [ContextMenu("PrintSaveFolderPath")]
        public void PrintSaveFolderPath()
        {
            Debug.Log(GetSaveFolderPath());
        }

        /// <summary>
        /// Return the save folder path.
        /// </summary>
        /// <returns>The save folder path.</returns>
        protected virtual string GetSaveFolderPath()
        {
            return Application.persistentDataPath;
        }

        /// <summary>
        /// Get the save file path.
        /// </summary>
        /// <param name="saveIndex">The save index.</param>
        /// <returns>The save file path.</returns>
        protected virtual string GetSaveFilePath(int saveIndex)
        {
            return string.Format("{0}/{1}_{2:000}.save",
                GetSaveFolderPath(), m_SaveFileName, saveIndex);
        }

        /// <summary>
        /// Save to disk.
        /// </summary>
        /// <param name="saveIndex">The save index.</param>
        protected virtual void SaveToDiskInternal(int saveIndex)
        {
#if DEBUG_SYSTEM_SAVER
            Debug.Log("Save file at index "+saveIndex);
#endif
            var saveFilePath = GetSaveFilePath(saveIndex);

#if DEBUG_SYSTEM_SAVER
            Debug.Log($"Save file at index {saveIndex} with path {saveFilePath}" +
                      $"\nThere are {m_SaveData.Count} Savers");
#endif

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(saveFilePath);

            m_SaveData.SetDateTime(DateTime.Now);
            var json = JsonUtility.ToJson(m_SaveData);

            //Save binary file
            bf.Serialize(file, json);
            file.Close();

            m_Saves[saveIndex] = new SaveData(m_SaveData);

            if (!m_DebugJsonCopy) { return; }

            var jsonFilePath = saveFilePath + ".json";
            Debug.Log("You are making a Debug Json Copy of the save file at the path: " + jsonFilePath);
            CreateDebugSaveFile(jsonFilePath, m_SaveData);
        }

        /// <summary>
        /// Create a Text Save File.
        /// </summary>
        /// <param name="filePath">The File Path.</param>
        /// <param name="value">The string to save.</param>
        private void CreateDebugSaveFile(string filePath, SaveData saveData)
        {
            // Delete the file if it exists.
            if (File.Exists(filePath)) { File.Delete(filePath); }

            var standardSaveDataJson = JsonUtility.ToJson(saveData, true);

            //Create the file.
            using (FileStream fs = File.Create(filePath)) {
                //Write the Entire save file
                WriteToFile(fs, "{\n\t\"StandardSaveData\": " + standardSaveDataJson + ",\n");

                WriteToFile(fs, "\t\"ReadableSaveData\": [\n");

                for (int i = 0; i < saveData.Count; i++) {

                    WriteToFile(fs, "{\n\t\t\"SaverKey\": \"" + saveData.SaveDataKeys[i] + "\",\n");

                    var jsonSaverData =
                        JsonUtility.ToJson(saveData.SerializedSaveData[i].DeserializeFields(MemberVisibility.All), true);
                    WriteToFile(fs, "\t\t\"SaverData\": " + jsonSaverData + "\n},\n");
                }

                WriteToFile(fs, "\t]\n}");
            }
        }

        /// <summary>
        /// Write to a file.
        /// </summary>
        /// <param name="fs">The file stream.</param>
        /// <param name="value">The string to write.</param>
        private static void WriteToFile(FileStream fs, string value)
        {
            var info = new UTF8Encoding(true).GetBytes(value);
            fs.Write(info, 0, info.Length);
        }

        /// <summary>
        /// Get the save data from the disk.
        /// </summary>
        /// <param name="saves">The saves dictionary.</param>
        protected virtual void GetSavesFromDisk(ref Dictionary<int, SaveData> saves)
        {
            for (int i = 0; i < m_MaxSaves; i++) {
                var saveFilePath = GetSaveFilePath(i);
                if (!File.Exists(saveFilePath)) { continue; }

                var saveData = new SaveData();

                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(saveFilePath, FileMode.Open);
                JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), saveData);
                file.Close();

                saves.Add(i, saveData);
            }
        }

        /// <summary>
        /// Delete the save from disk.
        /// </summary>
        /// <param name="saveIndex">The save index.</param>
        private void DeleteFromDiskInternal(int saveIndex)
        {
#if DEBUG_SYSTEM_SAVER
            Debug.Log("Delete file at index "+saveIndex);
#endif
            
            EventHandler.ExecuteEvent(EventNames.c_WillDeleteSave_Index,saveIndex);
            
            var saveFilePath = GetSaveFilePath(saveIndex);
            if (!File.Exists(saveFilePath)) { return; }

            File.Delete(saveFilePath);
            m_Saves.Remove(saveIndex);
            
            EventHandler.ExecuteEvent(EventNames.c_DeleteSaveComplete_Index,saveIndex);
        }

        /// <summary>
        /// Get the saves data.
        /// </summary>
        /// <returns>Returns all the saves.</returns>
        protected virtual IReadOnlyList<SaveData> GetSavesInternal()
        {
            for (int i = 0; i < m_SavesArray.Length; i++) {
                if (m_Saves.ContainsKey(i)) {
                    m_SavesArray[i] = m_Saves[i];
                } else { m_SavesArray[i] = null; }
            }

            return m_SavesArray;
        }

        /// <summary>
        /// Order saver components by priority.
        /// </summary>
        protected void OrderSaversByPriority(bool savePriority)
        {
            if (savePriority) {
                m_Savers.Sort((x, y) => x.SavePriority == y.SavePriority ? 0 : x.SavePriority > y.SavePriority ? 1 : -1);
            } else {
                m_Savers.Sort((x, y) => x.LoadPriority == y.LoadPriority ? 0 : x.LoadPriority > y.LoadPriority ? 1 : -1);
            }
        }

        /// <summary>
        /// Do something when object is destroyed.
        /// </summary>
        /// <param name="obj">The object being destroyed.</param>
        protected virtual void DestroyInternal(GameObject obj)
        {
            s_Initialized = false;
        }

        /// <summary>
        /// The game has ended. Determine if the game should be auto saved.
        /// </summary>
        protected virtual void OnApplicationQuit()
        {
            if (!m_AutoSaveOnApplicationQuit) { return; }

            OrderSaversByPriority(true);
            for (int i = 0; i < m_Savers.Count; i++) {
                if (m_Savers[i].SaveOnApplicationQuit) {
                    m_Savers[i].Save();
                }
            }

            SaveToDiskInternal(0);
        }
    }
}


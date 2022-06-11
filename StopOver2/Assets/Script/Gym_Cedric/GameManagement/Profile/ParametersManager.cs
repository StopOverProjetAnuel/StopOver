using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using UnityEngine.UI;

public class ParametersManager : MonoBehaviour
{
    [Header("Bus")]
    private Bus master;
    private Bus music;
    private Bus system;
    private Bus[] character;
    private Bus[] environnement;

    [Header("maxLevelBus")]
    private static float masterMaxLevel = 100f;
    private static float musicMaxLevel = 100f;
    private static float systemMaxLevel = 100f;
    private static float[] characterMaxLevel = {100f, 100f, 100f};
    private static float[] environnementMaxLevel = {100f, 100f};

    [Header("Requires")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider systemSlider;
    [SerializeField] private Slider characterSlider;
    [SerializeField] private Slider environnementSlider;
    private Profile currentProfile;



    private void Awake()
    {
        #region Get Bus
        master = RuntimeManager.GetBus("bus:/Master Bus");
        music = RuntimeManager.GetBus("bus:/Master Bus/Musique");
        system = RuntimeManager.GetBus("bus:/Master Bus/SFX_Game");
        character[0] = RuntimeManager.GetBus("bus:/Master Bus/SFX_Character_Boucle");
        character[1] = RuntimeManager.GetBus("bus:/Master Bus/SFX_Character_Feedback");
        character[2] = RuntimeManager.GetBus("bus:/Master Bus/SFX_Character_Sign");
        environnement[0] = RuntimeManager.GetBus("bus:/Master Bus/SFX_Environemment_Ambiance");
        environnement[1] = RuntimeManager.GetBus("bus:/Master Bus/SFX_Environemment_Feedback");
        #endregion

        CreateFolderAndProfile();
        LoadProfile();
    }

    private void CreateFolderAndProfile()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/Profile"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Profile");
        }


        if (!File.Exists(Application.persistentDataPath + "/Profile/Setting.fc"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/Profile/Setting.fc");
            Profile blank = new Profile();
            blank.masterVolume = 100f;
            blank.musicVolume = 100f;
            blank.systemVolume = 100f;
            blank.characterVolume = 100f;
            blank.environnementVolume = 100f;
            string json = JsonUtility.ToJson(blank);
            bf.Serialize(file, json);
            file.Close();
        }
    }

    private void LoadProfile()
    {
        if (!File.Exists(Application.persistentDataPath + "/Profile/Setting.fc")) return;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/Profile/Setting.fc", FileMode.Open);
        JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), currentProfile);
        file.Close();

        masterSlider.value = currentProfile.masterVolume;
        musicSlider.value = currentProfile.musicVolume;
        systemSlider.value = currentProfile.systemVolume;
        characterSlider.value = currentProfile.characterVolume;
        environnementSlider.value = currentProfile.environnementVolume;
    }

    public void SaveProfile()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/Profile/Setting.fc");
        string json = JsonUtility.ToJson(currentProfile);
        bf.Serialize(file, json);
        file.Close();
    }

    ////// VOLUME LEVEL \\\\\\

    public void ChangeMasterLevel(Slider slider) 
    {
        float currentVolumeLevel = slider.value;
        currentProfile.masterVolume = currentVolumeLevel;
        float ratioVolume = currentVolumeLevel / masterMaxLevel * 100;
        master.setVolume(ratioVolume);
    }

    public void ChangeMusicLevel(Slider slider) 
    {
        float currentVolumeLevel = slider.value;
        currentProfile.musicVolume = currentVolumeLevel;
        float ratioVolume = currentVolumeLevel / musicMaxLevel * 100;
        music.setVolume(ratioVolume);
    }

    public void ChangeSystemLevel(Slider slider) 
    {
        float currentVolumeLevel = slider.value;
        currentProfile.systemVolume = currentVolumeLevel;
        float ratioVolume = currentVolumeLevel / systemMaxLevel * 100;
        system.setVolume(ratioVolume);
    }

    public void ChangeCharacterLevel(Slider slider) 
    {
        float currentVolumeLevel = slider.value;
        currentProfile.characterVolume = currentVolumeLevel;
        for (int i = 0; i < character.Length; i++)
        {
            float ratioVolume = currentVolumeLevel / characterMaxLevel[i] * 100;
            character[i].setVolume(ratioVolume);
        }
    }

    public void ChangeEnvironnementLevel(Slider slider) 
    {
        float currentVolumeLevel = slider.value;
        currentProfile.environnementVolume = currentVolumeLevel;
        for (int i = 0; i < environnement.Length; i++)
        {
            float ratioVolume = currentVolumeLevel / environnementMaxLevel[i] * 100;
            environnement[i].setVolume(ratioVolume);
        }
    }
}
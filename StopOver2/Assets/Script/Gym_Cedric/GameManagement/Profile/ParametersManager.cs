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
    private Bus characterBoucle;
    private Bus characterFeedback;
    private Bus characterSign;
    private Bus environnementAmbiance;
    private Bus environnementFeedback;

    [Header("maxLevelBus")]
    private static float masterMaxLevel = 100f;
    private static float musicMaxLevel = 100f;
    private static float systemMaxLevel = 100f;
    private static float characterBoucleMaxLevel = 100f;
    private static float characterFeedbackMaxLevel = 100f;
    private static float characterSignMaxLevel = 100f;
    private static float environnementAmbianceMaxLevel = 100f;
    private static float environnementFeedbackMaxLevel = 100f;

    [Header("Requires")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider systemSlider;
    [SerializeField] private Slider characterSlider;
    [SerializeField] private Slider environnementSlider;
    private Profile currentProfile = new Profile();



    private void Awake()
    {
        #region Get Bus
        master = RuntimeManager.GetBus("bus:/");
        music = RuntimeManager.GetBus("bus:/Musique");
        system = RuntimeManager.GetBus("bus:/SFX_Game");
        characterBoucle = RuntimeManager.GetBus("bus:/SFX_Character_Boucle");
        characterFeedback = RuntimeManager.GetBus("bus:/SFX_Character_Feedback");
        characterSign = RuntimeManager.GetBus("bus:/SFX_Character_Sign");
        environnementAmbiance = RuntimeManager.GetBus("bus:/SFX_Environnement_Ambiance");
        environnementFeedback = RuntimeManager.GetBus("bus:/SFX_Environnement_Feedback");
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
            blank.masterVolume = 1f;
            blank.musicVolume = 1f;
            blank.systemVolume = 1f;
            blank.characterVolume = 1f;
            blank.environnementVolume = 1f;
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

        float ratioVolume = currentVolumeLevel / characterBoucleMaxLevel * 100;
        characterBoucle.setVolume(ratioVolume);

        ratioVolume = currentVolumeLevel / characterFeedbackMaxLevel * 100;
        characterFeedback.setVolume(ratioVolume);

        ratioVolume = currentVolumeLevel / characterSignMaxLevel * 100;
        characterSign.setVolume(ratioVolume);
    }

    public void ChangeEnvironnementLevel(Slider slider) 
    {
        float currentVolumeLevel = slider.value;
        currentProfile.environnementVolume = currentVolumeLevel;

        float ratioVolume = currentVolumeLevel / environnementAmbianceMaxLevel * 100;
        environnementAmbiance.setVolume(ratioVolume);

        ratioVolume = currentVolumeLevel / environnementFeedbackMaxLevel * 100;
        environnementFeedback.setVolume(ratioVolume);
    }
}
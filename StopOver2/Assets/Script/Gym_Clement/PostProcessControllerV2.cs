using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEditor;


public class PostProcessControllerV2 : MonoBehaviour
{
    #region variables public 
    [Space(10)]
    public Gradient progressBar; // aucun effet sur le code

    [Space(10)]
    public VolumeProfile Twilight;
    #region twilight settings
    private VisualEnvironment t_VisEnviro;
    private HDRISky t_HDRISky;
    private Fog t_Fog;
    private Exposure t_Exposure;
    private Tonemapping t_Tonemapping;
    private Bloom t_Bloom;
    private ColorAdjustments t_ColorAdjustments;
    private ChannelMixer t_ChannelMixer;
    private FilmGrain t_FilmGrain;
    private LiftGammaGain t_LiftGammaGain;
    private ShadowsMidtonesHighlights t_ShadowMidtonesHighlights;
    private SplitToning t_SplitTonning;
    private WhiteBalance t_WhiteBalance;
    #endregion
    //t_setting

    public VolumeProfile Night;
    #region Night settings
    private VisualEnvironment n_VisEnviro;
    private HDRISky n_HDRISky;
    private Fog n_Fog;
    private Exposure n_Exposure;
    private Tonemapping n_Tonemapping;
    private Bloom n_Bloom;
    private ColorAdjustments n_ColorAdjustments;
    private ChannelMixer n_ChannelMixer;
    private FilmGrain n_FilmGrain;
    private LiftGammaGain n_LiftGammaGain;
    private ShadowsMidtonesHighlights n_ShadowMidtonesHighlights;
    private SplitToning n_SplitTonning;
    private WhiteBalance n_WhiteBalance;
    #endregion
    //n_setting

    public VolumeProfile Dawn;
    #region dawn settings
    private VisualEnvironment d_VisEnviro;
    private HDRISky d_HDRISky;
    private Fog d_Fog;
    private Exposure d_Exposure;
    private Tonemapping d_Tonemapping;
    private Bloom d_Bloom;
    private ColorAdjustments d_ColorAdjustments;
    private ChannelMixer d_ChannelMixer;
    private FilmGrain d_FilmGrain;
    private LiftGammaGain d_LiftGammaGain;
    private ShadowsMidtonesHighlights d_ShadowMidtonesHighlights;
    private SplitToning d_SplitTonning;
    private WhiteBalance d_WhiteBalance;
    #endregion
    //d_setting

    [Space(10)]
    public float blendSpeed = 1; // vitesse du code pour alléger charge de travail

    [Space(10)]
    public float Timer = 240f; // temps du jeu

    [Space(10)]
    [Range(0,100)]
    public float twilightToNight; //début transition nuit en %
    [Range(0, 100)]
    public float nightToDawn;   //début transition aube en %
    [Range(0, 1)]
    public float TransitionNight;   //durée de la transition vers nuit
    [Range(0, 1)]
    public float TransitionDawn;    //durée de la transition vers aube

    #endregion

    #region variables private

    private Camera mCamera;                 // cherche main camera
    private Volume DNVolume;                // cherche component volume
    private VolumeProfile CurrentVolProfile;// cherche profile sur component

    private float progress;                 //progression timer entre 0 et 1
    
    //private Color[] AllColors = new Color[6] { Color.red, new Color (1,1,0,1), Color.green, Color.cyan, Color.blue, Color.magenta };        // couleurs de référence pas touche

    //private Gradient gradient;              // variable voir make gradient 

    #region Current Profile Settings
    private VisualEnvironment c_VisEnviro;
    private HDRISky c_HDRISky;
    private Fog c_Fog;
    private Exposure c_Exposure;
    private Tonemapping c_Tonemapping;
    private Bloom c_Bloom;
    private ColorAdjustments c_ColorAdjustments;
    private ChannelMixer c_ChannelMixer;
    private FilmGrain c_FilmGrain;
    private LiftGammaGain c_LiftGammaGain;
    private ShadowsMidtonesHighlights c_ShadowMidtonesHighlights;
    private SplitToning c_SplitTonning;
    private WhiteBalance c_WhiteBalance;
    #endregion
    //c_setting

    #region HDRI Sky Curves
    [HideInInspector]
    public AnimationCurve crvHDRISkyExposure;

    #endregion
    //crvHDRISkySetting

    #region Fog Curves
    [HideInInspector]
    public AnimationCurve crvFogBaseHeight;
    [HideInInspector]
    public AnimationCurve crvFogMaxHeight;
    [HideInInspector]
    public Gradient crvFogTint;
    [HideInInspector]
    public Gradient crvFogAlbedo;

    #endregion
    //crvFogSetting

    #region Exposure Curves
    [HideInInspector]
    public AnimationCurve crvExposureFixedExposure;
    [HideInInspector]
    public AnimationCurve crvExposureCompensation;
    #endregion
    //crvExposureSetting

    #region Bloom Curves
    [HideInInspector]
    public AnimationCurve crvBloomThreshold;
    [HideInInspector]
    public AnimationCurve crvBloomScatter;
    #endregion
    //crvBloomSetting

    #region Color Adjustment Curves
    [HideInInspector]
    public AnimationCurve crvColorAdjustmentPostExposure;
    [HideInInspector]
    public AnimationCurve crvColorAdjustmentContrast;
    [HideInInspector]
    public AnimationCurve crvColorAdjustmentSaturation;
    #endregion
    //crvColorAdjustmentSetting

    #region Channel Mixer
    [HideInInspector]
    public AnimationCurve crvChannelMixerRedR;
    [HideInInspector]
    public AnimationCurve crvChannelMixerRedG;
    [HideInInspector]
    public AnimationCurve crvChannelMixerRedB;
    [HideInInspector]
    public AnimationCurve crvChannelMixerGreenR;
    [HideInInspector]
    public AnimationCurve crvChannelMixerGreenG;
    [HideInInspector]
    public AnimationCurve crvChannelMixerGreenB;
    [HideInInspector]
    public AnimationCurve crvChannelMixerBlueR;
    [HideInInspector]
    public AnimationCurve crvChannelMixerBlueG;
    [HideInInspector]
    public AnimationCurve crvChannelMixerBlueB;

    #endregion
    //crvChannelMixerSetting

    #region Film Grain
    [HideInInspector]
    public AnimationCurve crvFilmGrainIntensity;
    [HideInInspector]
    public AnimationCurve crvFilmGrainResponse;

    #endregion
    //crvFilmGrainSetting

    #region Lift Gamma Gain
    [HideInInspector]
    public AnimationCurve crvLiftGammaGainLiftX;
    [HideInInspector]
    public AnimationCurve crvLiftGammaGainLiftY;
    [HideInInspector]
    public AnimationCurve crvLiftGammaGainLiftZ;
    [HideInInspector]
    public AnimationCurve crvLiftGammaGainLiftW;
    [HideInInspector]
    public AnimationCurve crvLiftGammaGainGammaX;
    [HideInInspector]
    public AnimationCurve crvLiftGammaGainGammaY;
    [HideInInspector]
    public AnimationCurve crvLiftGammaGainGammaZ;
    [HideInInspector]
    public AnimationCurve crvLiftGammaGainGammaW;
    [HideInInspector]
    public AnimationCurve crvLiftGammaGainGainX;
    [HideInInspector]
    public AnimationCurve crvLiftGammaGainGainY;
    [HideInInspector]
    public AnimationCurve crvLiftGammaGainGainZ;
    [HideInInspector]
    public AnimationCurve crvLiftGammaGainGainW;

    #endregion
    //crvLiftGammaGainSetting

    #region Shadow Midtones High
    [HideInInspector]
    public AnimationCurve crvShadowMidtonesHighShadowX;
    [HideInInspector]
    public AnimationCurve crvShadowMidtonesHighShadowY;
    [HideInInspector]
    public AnimationCurve crvShadowMidtonesHighShadowZ;
    [HideInInspector]
    public AnimationCurve crvShadowMidtonesHighShadowW;
    [HideInInspector]
    public AnimationCurve crvShadowMidtonesHighMidtonesX;
    [HideInInspector]
    public AnimationCurve crvShadowMidtonesHighMidtonesY;
    [HideInInspector]
    public AnimationCurve crvShadowMidtonesHighMidtonesZ;
    [HideInInspector]
    public AnimationCurve crvShadowMidtonesHighMidtonesW;
    [HideInInspector]
    public AnimationCurve crvShadowMidtonesHighHighX;
    [HideInInspector]
    public AnimationCurve crvShadowMidtonesHighHighY;
    [HideInInspector]
    public AnimationCurve crvShadowMidtonesHighHighZ;
    [HideInInspector]
    public AnimationCurve crvShadowMidtonesHighHighW;

    #endregion
    //crvShadowMidtoneHighSetting

    #region Split Toning
    [HideInInspector]
    public Gradient crvSplitToningShadow;
    [HideInInspector]
    public Gradient crvSplitToningHighlight;
    [HideInInspector]
    public AnimationCurve crvSplitToningBalance;

    #endregion
    //crvSplitToningSetting

    #region White balance
    [HideInInspector]
    public AnimationCurve crvWhiteBalanceTemperature;
    [HideInInspector]
    public AnimationCurve crvWhiteBalanceTint;

    #endregion
    //crvWhiteBalance

    #endregion


    public void Start()
    {
        initiate(); // set up des variables

        StartCoroutine(SetupCurentProfile()); // draw curves

        //StartCoroutine(StartDNCycle(UpdateSkybox,Timer)); // démare le timer et évolution du post process
    }

    private void initiate()
    {
        mCamera = Camera.main;

        DNVolume = mCamera.GetComponent<Volume>();

        CurrentVolProfile = DNVolume.profile;

        #region get settings Twillight
        Twilight.TryGet<VisualEnvironment>(out t_VisEnviro);
        Twilight.TryGet <HDRISky>(out t_HDRISky);
        Twilight.TryGet <Fog>(out t_Fog);
        Twilight.TryGet <Exposure>(out t_Exposure);
        Twilight.TryGet <Tonemapping> (out t_Tonemapping);
        Twilight.TryGet <Bloom>(out t_Bloom);
        Twilight.TryGet <ColorAdjustments> (out t_ColorAdjustments);
        Twilight.TryGet <ChannelMixer>(out t_ChannelMixer);
        Twilight.TryGet <FilmGrain>(out t_FilmGrain);
        Twilight.TryGet <LiftGammaGain>(out t_LiftGammaGain);
        Twilight.TryGet <ShadowsMidtonesHighlights>(out t_ShadowMidtonesHighlights);
        Twilight.TryGet <SplitToning>(out t_SplitTonning);
        Twilight.TryGet <WhiteBalance>(out t_WhiteBalance);
        #endregion
        //t_setting

        #region get settings Night
        Night.TryGet<VisualEnvironment>(out n_VisEnviro);
        Night.TryGet<HDRISky>(out n_HDRISky);
        Night.TryGet<Fog>(out n_Fog);
        Night.TryGet<Exposure>(out n_Exposure);
        Night.TryGet<Tonemapping>(out n_Tonemapping);
        Night.TryGet<Bloom>(out n_Bloom);
        Night.TryGet<ColorAdjustments>(out n_ColorAdjustments);
        Night.TryGet<ChannelMixer>(out n_ChannelMixer);
        Night.TryGet<FilmGrain>(out n_FilmGrain);
        Night.TryGet<LiftGammaGain>(out n_LiftGammaGain);
        Night.TryGet<ShadowsMidtonesHighlights>(out n_ShadowMidtonesHighlights);
        Night.TryGet<SplitToning>(out n_SplitTonning);
        Night.TryGet<WhiteBalance>(out n_WhiteBalance);
        #endregion
        //n_setting

        #region get settings Dawn
        Dawn.TryGet<VisualEnvironment>(out d_VisEnviro);
        Dawn.TryGet<HDRISky>(out d_HDRISky);
        Dawn.TryGet<Fog>(out d_Fog);
        Dawn.TryGet<Exposure>(out d_Exposure);
        Dawn.TryGet<Tonemapping>(out d_Tonemapping);
        Dawn.TryGet<Bloom>(out d_Bloom);
        Dawn.TryGet<ColorAdjustments>(out d_ColorAdjustments);
        Dawn.TryGet<ChannelMixer>(out d_ChannelMixer);
        Dawn.TryGet<FilmGrain>(out d_FilmGrain);
        Dawn.TryGet<LiftGammaGain>(out d_LiftGammaGain);
        Dawn.TryGet<ShadowsMidtonesHighlights>(out d_ShadowMidtonesHighlights);
        Dawn.TryGet<SplitToning>(out d_SplitTonning);
        Dawn.TryGet<WhiteBalance>(out d_WhiteBalance);
        #endregion
        //d_setting

        #region get settings Current
        CurrentVolProfile.TryGet<VisualEnvironment>(out c_VisEnviro);
        CurrentVolProfile.TryGet<HDRISky>(out c_HDRISky);
        CurrentVolProfile.TryGet<Fog>(out c_Fog);
        CurrentVolProfile.TryGet<Exposure>(out c_Exposure);
        CurrentVolProfile.TryGet<Tonemapping>(out c_Tonemapping);
        CurrentVolProfile.TryGet<Bloom>(out c_Bloom);
        CurrentVolProfile.TryGet<ColorAdjustments>(out c_ColorAdjustments);
        CurrentVolProfile.TryGet<ChannelMixer>(out c_ChannelMixer);
        CurrentVolProfile.TryGet<FilmGrain>(out c_FilmGrain);
        CurrentVolProfile.TryGet<LiftGammaGain>(out c_LiftGammaGain);
        CurrentVolProfile.TryGet<ShadowsMidtonesHighlights>(out c_ShadowMidtonesHighlights);
        CurrentVolProfile.TryGet<SplitToning>(out c_SplitTonning);
        CurrentVolProfile.TryGet<WhiteBalance>(out c_WhiteBalance);
        #endregion
        //c_setting
    }

    private IEnumerator StartDNCycle(float Refresh,float MaxTimer)
    {
        if (Timer > 0)
        {
            Timer -= Refresh;
            progress = 1 - (Timer / MaxTimer);
            ReadCurves();
            ShowProgress();
            yield return new WaitForSeconds(Refresh);
            StartCoroutine(StartDNCycle(Refresh,MaxTimer));
        }
    }

    private IEnumerator SetupCurentProfile()
    {
        MakeCurve(crvHDRISkyExposure, t_HDRISky.exposure.value, n_HDRISky.exposure.value, d_HDRISky.exposure.value);
            yield return new WaitForEndOfFrame();
        MakeCurve(crvFogBaseHeight,t_Fog.baseHeight.value,n_Fog.baseHeight.value,d_Fog.baseHeight.value);
        MakeCurve(crvFogMaxHeight,t_Fog.maximumHeight.value,n_Fog.maximumHeight.value,d_Fog.maximumHeight.value);
        MakeCurve(crvFogMaxHeight,t_Fog.maximumHeight.value,n_Fog.maximumHeight.value,d_Fog.maximumHeight.value);
        MakeGradient(crvFogTint, t_Fog.tint.value, n_Fog.tint.value, d_Fog.tint.value);
        MakeGradient(crvFogAlbedo, t_Fog.albedo.value, n_Fog.albedo.value, d_Fog.albedo.value);
            yield return new WaitForEndOfFrame();
        MakeCurve(crvExposureFixedExposure,t_Exposure.fixedExposure.value,n_Exposure.fixedExposure.value,d_Exposure.fixedExposure.value);
        MakeCurve(crvExposureCompensation,t_Exposure.compensation.value,n_Exposure.compensation.value,d_Exposure.compensation.value);
            yield return new WaitForEndOfFrame();
        MakeCurve(crvBloomThreshold, t_Bloom.threshold.value, n_Bloom.threshold.value, d_Bloom.threshold.value);
        MakeCurve(crvBloomScatter, t_Bloom.scatter.value, n_Bloom.scatter.value, d_Bloom.scatter.value);
            yield return new WaitForEndOfFrame();
        MakeCurve(crvColorAdjustmentPostExposure, t_ColorAdjustments.postExposure.value, n_ColorAdjustments.postExposure.value, d_ColorAdjustments.postExposure.value);
        MakeCurve(crvColorAdjustmentContrast, t_ColorAdjustments.contrast.value, n_ColorAdjustments.contrast.value, d_ColorAdjustments.contrast.value);
        MakeCurve(crvColorAdjustmentSaturation, t_ColorAdjustments.saturation.value, n_ColorAdjustments.saturation.value, d_ColorAdjustments.saturation.value);
            yield return new WaitForEndOfFrame();
        MakeCurve(crvChannelMixerRedR, t_ChannelMixer.redOutRedIn.value, n_ChannelMixer.redOutRedIn.value, d_ChannelMixer.redOutRedIn.value);
        MakeCurve(crvChannelMixerRedG, t_ChannelMixer.redOutGreenIn.value, n_ChannelMixer.redOutGreenIn.value, d_ChannelMixer.redOutGreenIn.value);
        MakeCurve(crvChannelMixerRedB, t_ChannelMixer.redOutBlueIn.value, n_ChannelMixer.redOutBlueIn.value, d_ChannelMixer.redOutBlueIn.value);
        MakeCurve(crvChannelMixerGreenR, t_ChannelMixer.greenOutRedIn.value, n_ChannelMixer.greenOutRedIn.value, d_ChannelMixer.greenOutRedIn.value);
        MakeCurve(crvChannelMixerGreenG, t_ChannelMixer.greenOutGreenIn.value, n_ChannelMixer.greenOutGreenIn.value, d_ChannelMixer.greenOutGreenIn.value);
        MakeCurve(crvChannelMixerGreenB, t_ChannelMixer.greenOutBlueIn.value, n_ChannelMixer.greenOutBlueIn.value, d_ChannelMixer.greenOutBlueIn.value);
        MakeCurve(crvChannelMixerBlueR, t_ChannelMixer.blueOutRedIn.value, n_ChannelMixer.blueOutRedIn.value, d_ChannelMixer.blueOutRedIn.value);
        MakeCurve(crvChannelMixerBlueG, t_ChannelMixer.blueOutGreenIn.value, n_ChannelMixer.blueOutGreenIn.value, d_ChannelMixer.blueOutGreenIn.value);
        MakeCurve(crvChannelMixerBlueB, t_ChannelMixer.blueOutBlueIn.value, n_ChannelMixer.blueOutBlueIn.value, d_ChannelMixer.blueOutBlueIn.value);
            yield return new WaitForEndOfFrame();
        MakeCurve(crvFilmGrainIntensity, t_FilmGrain.intensity.value, n_FilmGrain.intensity.value, d_FilmGrain.intensity.value);
        MakeCurve(crvFilmGrainResponse, t_FilmGrain.response.value, n_FilmGrain.response.value, d_FilmGrain.response.value);
            yield return new WaitForEndOfFrame();
        MakeCurve(crvLiftGammaGainLiftX, t_LiftGammaGain.lift.value.x, n_LiftGammaGain.lift.value.x, d_LiftGammaGain.lift.value.x);
        MakeCurve(crvLiftGammaGainLiftY, t_LiftGammaGain.lift.value.y, n_LiftGammaGain.lift.value.y, d_LiftGammaGain.lift.value.y);
        MakeCurve(crvLiftGammaGainLiftZ, t_LiftGammaGain.lift.value.z, n_LiftGammaGain.lift.value.z, d_LiftGammaGain.lift.value.z);
        MakeCurve(crvLiftGammaGainLiftW, t_LiftGammaGain.lift.value.w, n_LiftGammaGain.lift.value.w, d_LiftGammaGain.lift.value.w);
        MakeCurve(crvLiftGammaGainGammaX, t_LiftGammaGain.gamma.value.x, n_LiftGammaGain.gamma.value.x, d_LiftGammaGain.gamma.value.x);
        MakeCurve(crvLiftGammaGainGammaY, t_LiftGammaGain.gamma.value.y, n_LiftGammaGain.gamma.value.y, d_LiftGammaGain.gamma.value.y);
        MakeCurve(crvLiftGammaGainGammaZ, t_LiftGammaGain.gamma.value.z, n_LiftGammaGain.gamma.value.z, d_LiftGammaGain.gamma.value.z);
        MakeCurve(crvLiftGammaGainGammaW, t_LiftGammaGain.gamma.value.w, n_LiftGammaGain.gamma.value.w, d_LiftGammaGain.gamma.value.w);
        MakeCurve(crvLiftGammaGainGainX, t_LiftGammaGain.gain.value.x, n_LiftGammaGain.gain.value.x, d_LiftGammaGain.gain.value.x);
        MakeCurve(crvLiftGammaGainGainY, t_LiftGammaGain.gain.value.y, n_LiftGammaGain.gain.value.y, d_LiftGammaGain.gain.value.y);
        MakeCurve(crvLiftGammaGainGainZ, t_LiftGammaGain.gain.value.z, n_LiftGammaGain.gain.value.z, d_LiftGammaGain.gain.value.z);
        MakeCurve(crvLiftGammaGainGainW, t_LiftGammaGain.gain.value.w, n_LiftGammaGain.gain.value.w, d_LiftGammaGain.gain.value.w);
            yield return new WaitForEndOfFrame();
        MakeCurve(crvShadowMidtonesHighShadowX, t_ShadowMidtonesHighlights.shadows.value.x, n_ShadowMidtonesHighlights.shadows.value.x, d_ShadowMidtonesHighlights.shadows.value.x);
        MakeCurve(crvShadowMidtonesHighShadowY, t_ShadowMidtonesHighlights.shadows.value.y, n_ShadowMidtonesHighlights.shadows.value.y, d_ShadowMidtonesHighlights.shadows.value.y);
        MakeCurve(crvShadowMidtonesHighShadowZ, t_ShadowMidtonesHighlights.shadows.value.z, n_ShadowMidtonesHighlights.shadows.value.z, d_ShadowMidtonesHighlights.shadows.value.z);
        MakeCurve(crvShadowMidtonesHighShadowW, t_ShadowMidtonesHighlights.shadows.value.w, n_ShadowMidtonesHighlights.shadows.value.w, d_ShadowMidtonesHighlights.shadows.value.w);
        MakeCurve(crvShadowMidtonesHighMidtonesX, t_ShadowMidtonesHighlights.midtones.value.x, n_ShadowMidtonesHighlights.midtones.value.x, d_ShadowMidtonesHighlights.midtones.value.x);
        MakeCurve(crvShadowMidtonesHighMidtonesY, t_ShadowMidtonesHighlights.midtones.value.y, n_ShadowMidtonesHighlights.midtones.value.y, d_ShadowMidtonesHighlights.midtones.value.y);
        MakeCurve(crvShadowMidtonesHighMidtonesZ, t_ShadowMidtonesHighlights.midtones.value.z, n_ShadowMidtonesHighlights.midtones.value.z, d_ShadowMidtonesHighlights.midtones.value.z);
        MakeCurve(crvShadowMidtonesHighMidtonesW, t_ShadowMidtonesHighlights.midtones.value.w, n_ShadowMidtonesHighlights.midtones.value.w, d_ShadowMidtonesHighlights.midtones.value.w);
        MakeCurve(crvShadowMidtonesHighHighX, t_ShadowMidtonesHighlights.highlights.value.x, n_ShadowMidtonesHighlights.highlights.value.x, d_ShadowMidtonesHighlights.highlights.value.x);
        MakeCurve(crvShadowMidtonesHighHighY, t_ShadowMidtonesHighlights.highlights.value.y, n_ShadowMidtonesHighlights.highlights.value.y, d_ShadowMidtonesHighlights.highlights.value.y);
        MakeCurve(crvShadowMidtonesHighHighZ, t_ShadowMidtonesHighlights.highlights.value.z, n_ShadowMidtonesHighlights.highlights.value.z, d_ShadowMidtonesHighlights.highlights.value.z);
        MakeCurve(crvShadowMidtonesHighHighW, t_ShadowMidtonesHighlights.highlights.value.w, n_ShadowMidtonesHighlights.highlights.value.w, d_ShadowMidtonesHighlights.highlights.value.w);
        yield return new WaitForEndOfFrame();
        MakeGradient(crvSplitToningShadow, t_SplitTonning.shadows.value, n_SplitTonning.shadows.value, d_SplitTonning.shadows.value);
        MakeGradient(crvSplitToningHighlight, t_SplitTonning.highlights.value, n_SplitTonning.highlights.value, d_SplitTonning.highlights.value);
        MakeCurve(crvSplitToningBalance, t_SplitTonning.balance.value, n_SplitTonning.balance.value, d_SplitTonning.balance.value);
            yield return new WaitForEndOfFrame();
        MakeCurve(crvWhiteBalanceTemperature, t_WhiteBalance.temperature.value, n_WhiteBalance.temperature.value, d_WhiteBalance.temperature.value);
        MakeCurve(crvWhiteBalanceTint, t_WhiteBalance.tint.value, n_WhiteBalance.tint.value, d_WhiteBalance.tint.value);

        StartCoroutine(StartDNCycle(blendSpeed, Timer));
    }
    
    void ReadCurves()
    {
        c_HDRISky.exposure.value = crvHDRISkyExposure.Evaluate(progress);
        c_Fog.baseHeight.value = crvFogBaseHeight.Evaluate(progress);
        c_Fog.maximumHeight.value = crvFogMaxHeight.Evaluate(progress);
        c_Fog.tint.value = crvFogTint.Evaluate(progress);
        c_Fog.albedo.value = crvFogAlbedo.Evaluate(progress);
        c_Exposure.fixedExposure.value = crvExposureFixedExposure.Evaluate(progress);
        c_Exposure.compensation.value = crvExposureCompensation.Evaluate(progress);
        c_Bloom.threshold.value = crvBloomThreshold.Evaluate(progress);
        c_Bloom.scatter.value = crvBloomScatter.Evaluate(progress);
        c_ColorAdjustments.postExposure.value = crvColorAdjustmentPostExposure.Evaluate(progress);
        c_ColorAdjustments.contrast.value = crvColorAdjustmentContrast.Evaluate(progress);
        c_ColorAdjustments.saturation.value = crvColorAdjustmentSaturation.Evaluate(progress);
        c_ChannelMixer.redOutRedIn.value = crvChannelMixerRedR.Evaluate(progress);
        c_ChannelMixer.redOutGreenIn.value = crvChannelMixerRedG.Evaluate(progress);
        c_ChannelMixer.redOutBlueIn.value = crvChannelMixerRedB.Evaluate(progress);
        c_ChannelMixer.greenOutRedIn.value = crvChannelMixerGreenR.Evaluate(progress);
        c_ChannelMixer.greenOutGreenIn.value = crvChannelMixerGreenG.Evaluate(progress);
        c_ChannelMixer.greenOutBlueIn.value = crvChannelMixerGreenB.Evaluate(progress);
        c_ChannelMixer.blueOutRedIn.value = crvChannelMixerBlueR.Evaluate(progress);
        c_ChannelMixer.blueOutGreenIn.value = crvChannelMixerBlueG.Evaluate(progress);
        c_ChannelMixer.blueOutBlueIn.value = crvChannelMixerBlueB.Evaluate(progress);
        c_FilmGrain.intensity.value = crvFilmGrainIntensity.Evaluate(progress);
        c_FilmGrain.response.value = crvFilmGrainResponse.Evaluate(progress);
        c_LiftGammaGain.lift.value = new Vector4 (crvLiftGammaGainLiftX.Evaluate(progress), crvLiftGammaGainLiftY.Evaluate(progress), crvLiftGammaGainLiftZ.Evaluate(progress), crvLiftGammaGainLiftW.Evaluate(progress));
        c_LiftGammaGain.gamma.value = new Vector4 (crvLiftGammaGainGammaX.Evaluate(progress), crvLiftGammaGainGammaY.Evaluate(progress), crvLiftGammaGainGammaZ.Evaluate(progress), crvLiftGammaGainGammaW.Evaluate(progress));
        c_LiftGammaGain.gain.value = new Vector4 (crvLiftGammaGainGainX.Evaluate(progress), crvLiftGammaGainGainY.Evaluate(progress), crvLiftGammaGainGainZ.Evaluate(progress), crvLiftGammaGainGainW.Evaluate(progress));
        c_ShadowMidtonesHighlights.shadows.value = new Vector4 (crvShadowMidtonesHighShadowX.Evaluate(progress), crvShadowMidtonesHighShadowY.Evaluate(progress), crvShadowMidtonesHighShadowZ.Evaluate(progress), crvShadowMidtonesHighShadowW.Evaluate(progress));
        c_ShadowMidtonesHighlights.midtones.value = new Vector4 (crvShadowMidtonesHighMidtonesX.Evaluate(progress), crvShadowMidtonesHighMidtonesY.Evaluate(progress), crvShadowMidtonesHighMidtonesZ.Evaluate(progress), crvShadowMidtonesHighMidtonesW.Evaluate(progress));
        c_ShadowMidtonesHighlights.highlights.value = new Vector4 (crvShadowMidtonesHighHighX.Evaluate(progress), crvShadowMidtonesHighHighY.Evaluate(progress), crvShadowMidtonesHighHighZ.Evaluate(progress), crvShadowMidtonesHighHighW.Evaluate(progress));
        c_SplitTonning.shadows.value = crvSplitToningShadow.Evaluate(progress);
        c_SplitTonning.highlights.value = crvSplitToningHighlight.Evaluate(progress);
        c_SplitTonning.balance.value = crvSplitToningBalance.Evaluate(progress);
        c_WhiteBalance.temperature.value = crvWhiteBalanceTemperature.Evaluate(progress);
        c_WhiteBalance.tint.value = crvWhiteBalanceTemperature.Evaluate(progress);

        c_FilmGrain.type.value = (progress > twilightToNight/100 && progress < nightToDawn/100) ? n_FilmGrain.type.value : c_FilmGrain.type.value;
        c_FilmGrain.type.value = (progress > nightToDawn/100) ? d_FilmGrain.type.value : c_FilmGrain.type.value;
        
        //cubemap
    }

    void ShowProgress()
    {
        GradientColorKey[] kc = progressBar.colorKeys;        
        kc[0].time = progress;
        kc[1].time = progress + 0.001f;
        progressBar.SetKeys(kc, progressBar.alphaKeys);
    }

    void MakeGradient(Gradient g, Color c1, Color c2, Color c3)
    {
        GradientColorKey[] ck = new GradientColorKey[6];
        GradientAlphaKey[] ak = new GradientAlphaKey[1];
        ak[0].time = 0;
        ak[0].alpha = 1;

        ck[0].time = 0;
        ck[1].time = twilightToNight / 100;
        ck[2].time = twilightToNight / 100 + TransitionNight;
        ck[3].time = nightToDawn / 100;
        ck[4].time = nightToDawn / 100 + TransitionDawn;
        ck[5].time = 1;

        ck[0].color = c1;
        ck[1].color = c1;
        ck[2].color = c2;
        ck[3].color = c2;
        ck[4].color = c3;
        ck[5].color = c3;

        g.SetKeys(ck, ak);
    }
    void MakeCurve(AnimationCurve var , float a, float b,float c)
    {
        Keyframe[] k = new Keyframe[6];
        k[0].value = a;
        k[0].time = 0;
        k[1].value = a;
        k[1].time = twilightToNight / 100;
        k[2].value = b;
        k[2].time = twilightToNight / 100 + TransitionNight;
        k[3].value = b;
        k[3].time = nightToDawn / 100;
        k[4].value = c;
        k[4].time = nightToDawn / 100 + TransitionDawn;
        k[5].value = c;
        k[5].time = 1;
        var.keys = k;
    }
}

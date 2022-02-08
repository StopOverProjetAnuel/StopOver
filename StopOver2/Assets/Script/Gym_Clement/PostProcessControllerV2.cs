using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEditor;


public class PostProcessControllerV2 : MonoBehaviour
{
    #region variables public 

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
    public float UpdateSkybox; // vitesse du code pour alléger charge de travail

    [Space(10)]
    public float Timer; // temps du jeu

    [Space(10)]
    [Range(0,100)]
    public float twilightToNight; //début transition nuit en %
    [Range(0, 100)]
    public float nightToDawn;   //début transition aube en %
    [Range(0, 1)]
    public float TransitionNight;   //durée de la transition vers nuit
    [Range(0, 1)]
    public float TransitionDawn;    //durée de la transition vers aube

    public AnimationCurve transition;// feedback aucun effet sur le code 
    #endregion

    #region variables private

    private Camera mCamera;                 // cherche main camera
    private Volume DNVolume;                // cherche component volume
    private VolumeProfile CurrentVolProfile;// cherche profile sur component

    private float progress;                 //progressiou timer entre 0 et 1

    [SerializeField]
    private AnimationCurve RED;      // courbes de référence pas touche
    [SerializeField]
    private AnimationCurve GREEN;     //
    [SerializeField]
    private AnimationCurve BLUE;      //

    [SerializeField]
    private Color[] AllColors;        // couleurs de référence pas touche

    private Gradient gradient;        // variable voir make gradient 

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
    private AnimationCurve crvHDRISkyScrollspeed;
    private AnimationCurve crvHDRISkyExposure;
    private AnimationCurve crvHDRISkyRotation;

    #endregion
    //crvHDRISkySetting

    #region Fog Curves
    private AnimationCurve crvFogBaseHeight;
    private AnimationCurve crvFogMaxHeight;
    private Gradient crvFogTint;
    private Gradient crvFogAlbedo;

    #endregion
    //crvFogSetting

    #region Exposure Curves
    private AnimationCurve crvExposureFixedExposure;
    private AnimationCurve crvExposureCompensation;
    #endregion
    //crvExposureSetting

    #region Bloom Curves
    private AnimationCurve crvBloomThreshold;
    private AnimationCurve crvBloomScatter;
    #endregion
    //crvBloomSetting

    #region Color Adjustment Curves
    private AnimationCurve crvColorAdjustmentPostExposure;
    private AnimationCurve crvColorAdjustmentContrast;
    private AnimationCurve crvColorAdjustmentSaturation;
    #endregion
    //crvColorAdjustmentSetting

    #region Channel Mixer
    private AnimationCurve crvChannelMixerRedR;
    private AnimationCurve crvChannelMixerRedG;
    private AnimationCurve crvChannelMixerRedB;
    private AnimationCurve crvChannelMixerGreenR;
    private AnimationCurve crvChannelMixerGreenG;
    private AnimationCurve crvChannelMixerGreenB;
    private AnimationCurve crvChannelMixerBlueR;
    private AnimationCurve crvChannelMixerBlueG;
    private AnimationCurve crvChannelMixerBlueB;

    #endregion
    //crvChannelMixerSetting

    #region Film Grain
    private AnimationCurve crvFilmGrainIntensity;
    private AnimationCurve crvFilmGrainResponse;
    private Keyframe[] keyFilmGrainIntensity = new Keyframe[6];
    private Keyframe[] keyFilmGrainResponse = new Keyframe[6];
    #endregion
    //crvFilmGrainSetting

    #region Lift Gamma Gain
    private AnimationCurve crvLiftGammaGainLiftX; 
    private AnimationCurve crvLiftGammaGainLiftY; 
    private AnimationCurve crvLiftGammaGainLiftZ; 
    private AnimationCurve crvLiftGammaGainLiftW; 
    private AnimationCurve crvLiftGammaGainGammaX; 
    private AnimationCurve crvLiftGammaGainGammaY; 
    private AnimationCurve crvLiftGammaGainGammaZ; 
    private AnimationCurve crvLiftGammaGainGammaW; 
    private AnimationCurve crvLiftGammaGainGainX;
    private AnimationCurve crvLiftGammaGainGainY;
    private AnimationCurve crvLiftGammaGainGainZ;
    private AnimationCurve crvLiftGammaGainGainW;

    #endregion
    //crvLiftGammaGainSetting

    #region Shadow Midtones High
    private AnimationCurve crvShadowMidtonesHighShadowX;
    private AnimationCurve crvShadowMidtonesHighShadowY;
    private AnimationCurve crvShadowMidtonesHighShadowZ;
    private AnimationCurve crvShadowMidtonesHighShadowW;
    private AnimationCurve crvShadowMidtonesHighMidtonesX;
    private AnimationCurve crvShadowMidtonesHighMidtonesY;
    private AnimationCurve crvShadowMidtonesHighMidtonesZ;
    private AnimationCurve crvShadowMidtonesHighMidtonesW;
    private AnimationCurve crvShadowMidtonesHighHighX;
    private AnimationCurve crvShadowMidtonesHighHighY;
    private AnimationCurve crvShadowMidtonesHighHighZ;
    private AnimationCurve crvShadowMidtonesHighHighW;

    #endregion
    //crvShadowMidtoneHighSetting

    #region Split Toning
    private Gradient crvSplitToningShadow;
    private Gradient crvSplitToningHighlight;
    private AnimationCurve crvSplitToningBalance;

    #endregion
    //crvSplitToningSetting

    #region White balance
    private AnimationCurve crvWhiteBalanceTemperature;
    private AnimationCurve crvWhiteBalanceTint;
    #endregion
    //crvWhiteBalance

    

    #endregion


    public void Start()
    {
        initiate(); // set up des variables

        StartCoroutine(SetupCurentProfile()); // draw curves

        StartCoroutine(StartDNCycle(UpdateSkybox,Timer)); // démare le timer et évolution du post process

        ShowTransition(); //feedback

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
            yield return new WaitForSeconds(Refresh);
            StartCoroutine(StartDNCycle(Refresh,MaxTimer));
        }
    }

    private IEnumerator SetupCurentProfile()
    {
        MakeCurve(crvHDRISkyScrollspeed, t_HDRISky.scrollSpeed.value, n_HDRISky.scrollSpeed.value, d_HDRISky.scrollSpeed.value);
        MakeCurve(crvHDRISkyExposure, t_HDRISky.exposure.value, n_HDRISky.exposure.value, d_HDRISky.exposure.value);
        MakeCurve(crvHDRISkyRotation, t_HDRISky.rotation.value, n_HDRISky.rotation.value, d_HDRISky.rotation.value);
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
    }


    void ReadCurves()
    {
        c_HDRISky.scrollSpeed.value = crvHDRISkyScrollspeed.Evaluate(progress);
        c_HDRISky.exposure.value = crvHDRISkyExposure.Evaluate(progress);
        c_HDRISky.rotation.value = crvHDRISkyRotation.Evaluate(progress);
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
    }

    #region GradientMaker
    private IEnumerator MakeGradient(Gradient var, Color a, Color b, Color c)
    {
        float t = twilightToNight / 100;
        float d = nightToDawn / 100;

        yield return new WaitForEndOfFrame();
        SetSaturatedGradient(a, b);
        yield return new WaitForEndOfFrame();
        UpdateSaturation(a, b);
        yield return new WaitForEndOfFrame();
        UpdateLuminosity(a, b);
        yield return new WaitForEndOfFrame();
        ResizeGradient(t, TransitionNight);

        Gradient G1 = new Gradient();
        G1.SetKeys(gradient.colorKeys, gradient.alphaKeys);
        yield return new WaitForEndOfFrame();


        SetSaturatedGradient(b, c);
        yield return new WaitForEndOfFrame();
        UpdateSaturation(b, c);
        yield return new WaitForEndOfFrame();
        UpdateLuminosity(b, c);
        yield return new WaitForEndOfFrame();
        ResizeGradient(d, TransitionDawn);

        Gradient G2 = new Gradient();
        G2.SetKeys(gradient.colorKeys, gradient.alphaKeys);

        CombineGradient(G1, G2);
        var.SetKeys(gradient.colorKeys, gradient.alphaKeys);
        gradient = var;

    }
    Color RecognizeColor(Color c)
    {
        Vector4 V4 = c;
        V4 *= 1.5f;
        V4 = new Vector4(Mathf.Clamp(V4.x, 0, 1), Mathf.Clamp(V4.y, 0, 1), Mathf.Clamp(V4.z, 0, 1), Mathf.Clamp(V4.w, 0, 1));
        c = V4;

        if (c.maxColorComponent == c.r) // si le rouge qui est superieur
        {
            if (c.r == c.g) // si c'est jaune
            {
                return new Color(1, 1, 0, 1);
            }
            else if (c.r == c.b) // si c'est rose
            {
                return new Color(1, 0, 1, 1);
            }
            else // si c'est rouge
            {
                return new Color(1, 0, 0, 1);
            }
        }
        else if (c.maxColorComponent == c.g) // si c'est le vert qui est superieur
        {
            if (c.g == c.b) // si c'est cyan
            {
                return new Color(0, 1, 1, 1);
            }
            else // si c'est vert
            {
                return new Color(0, 1, 0, 1);
            }

        }
        else // si c'est le bleu qui est supperieur
        {
            return new Color(0, 0, 1, 1); //alors c'est bleu
        }
    }
    void SetSaturatedGradient(Color a, Color b)
    {
        Color A = RecognizeColor(a);
        Color B = RecognizeColor(b);
        int idxA;
        int idxB;
        if (A == AllColors[0])
        {
            idxA = 1;
        }
        else if (A == AllColors[1])
        {
            idxA = 2;
        }
        else if (A == AllColors[2])
        {
            idxA = 3;
        }
        else if (A == AllColors[3])
        {
            idxA = 4;
        }
        else if (A == AllColors[4])
        {
            idxA = 5;
        }
        else
        {
            idxA = 6;
        }

        if (B == AllColors[0])
        {
            idxB = 1;
        }
        else if (B == AllColors[1])
        {
            idxB = 2;
        }
        else if (B == AllColors[2])
        {
            idxB = 3;
        }
        else if (B == AllColors[3])
        {
            idxB = 4;
        }
        else if (B == AllColors[4])
        {
            idxB = 5;
        }
        else
        {
            idxB = 6;
        }

        int d = idxA - idxB;
        if (d == 1)
        {
            GradientColorKey[] colorKeys = new GradientColorKey[2];
            colorKeys[0].color = a;
            colorKeys[1].color = b;
            colorKeys[0].time = 0;
            colorKeys[1].time = 1;

            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[1];
            alphaKeys[0].alpha = 1;
            alphaKeys[0].time = 0;

            gradient.SetKeys(colorKeys, alphaKeys);
        }
        else if (d == 2)
        {
            GradientColorKey[] colorKeys = new GradientColorKey[3];
            colorKeys[0].color = b;
            colorKeys[1].color = AllColors[idxB];
            colorKeys[2].color = a;
            colorKeys[0].time = 0;
            colorKeys[1].time = 0.5f;
            colorKeys[2].time = 1;

            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[1];
            alphaKeys[0].alpha = 1;
            alphaKeys[0].time = 0;

            gradient.SetKeys(colorKeys, alphaKeys);

        }
        else if (d == 3)
        {
            GradientColorKey[] colorKeys = new GradientColorKey[4];
            colorKeys[0].color = a;
            colorKeys[1].color = AllColors[idxB + 1];
            colorKeys[2].color = AllColors[idxB];
            colorKeys[3].color = b;
            colorKeys[0].time = 0;
            colorKeys[1].time = 0.33f;
            colorKeys[2].time = 0.66f;
            colorKeys[3].time = 1;

            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[1];
            alphaKeys[0].alpha = 1;
            alphaKeys[0].time = 0;

            gradient.SetKeys(colorKeys, alphaKeys);
        }
        else if (d == 4)
        {
            GradientColorKey[] colorKeys = new GradientColorKey[3];
            colorKeys[0].color = a;
            colorKeys[1].color = AllColors[idxA];
            colorKeys[2].color = b;
            colorKeys[0].time = 0;
            colorKeys[1].time = 0.5f;
            colorKeys[2].time = 1;

            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[1];
            alphaKeys[0].alpha = 1;
            alphaKeys[0].time = 0;

            gradient.SetKeys(colorKeys, alphaKeys);
        }
        else if (d == 5)
        {
            GradientColorKey[] colorKeys = new GradientColorKey[2];
            colorKeys[0].color = a;
            colorKeys[1].color = b;
            colorKeys[0].time = 0;
            colorKeys[1].time = 1;

            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[1];
            alphaKeys[0].alpha = 1;
            alphaKeys[0].time = 0;

            gradient.SetKeys(colorKeys, alphaKeys);
        }
        else if (d == -1)
        {
            GradientColorKey[] colorKeys = new GradientColorKey[2];
            colorKeys[0].color = a;
            colorKeys[1].color = b;
            colorKeys[0].time = 0;
            colorKeys[1].time = 1;

            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[1];
            alphaKeys[0].alpha = 1;
            alphaKeys[0].time = 0;

            gradient.SetKeys(colorKeys, alphaKeys);
        }
        else if (d == -2)
        {
            GradientColorKey[] colorKeys = new GradientColorKey[3];
            colorKeys[0].color = a;
            colorKeys[1].color = AllColors[idxA];
            colorKeys[2].color = b;
            colorKeys[0].time = 0;
            colorKeys[1].time = 0.5f;
            colorKeys[2].time = 1;

            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[1];
            alphaKeys[0].alpha = 1;
            alphaKeys[0].time = 0;

            gradient.SetKeys(colorKeys, alphaKeys);
        }
        else if (d == -3)
        {
            GradientColorKey[] colorKeys = new GradientColorKey[4];
            colorKeys[0].color = a;
            colorKeys[1].color = AllColors[idxA];
            colorKeys[2].color = AllColors[idxA + 1];
            colorKeys[3].color = b;
            colorKeys[0].time = 0;
            colorKeys[1].time = 0.33f;
            colorKeys[2].time = 0.66f;
            colorKeys[3].time = 1;

            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[1];
            alphaKeys[0].alpha = 1;
            alphaKeys[0].time = 0;

            gradient.SetKeys(colorKeys, alphaKeys);
        }
        else if (d == -4)
        {
            GradientColorKey[] colorKeys = new GradientColorKey[3];
            colorKeys[0].color = a;
            colorKeys[1].color = AllColors[idxB];
            colorKeys[2].color = b;
            colorKeys[0].time = 0;
            colorKeys[1].time = 0.5f;
            colorKeys[2].time = 1;

            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[1];
            alphaKeys[0].alpha = 1;
            alphaKeys[0].time = 0;

            gradient.SetKeys(colorKeys, alphaKeys);
        }
        else if (d == -5)
        {
            GradientColorKey[] colorKeys = new GradientColorKey[2];
            colorKeys[0].color = a;
            colorKeys[1].color = b;
            colorKeys[0].time = 0;
            colorKeys[1].time = 1;

            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[1];
            alphaKeys[0].alpha = 1;
            alphaKeys[0].time = 0;

            gradient.SetKeys(colorKeys, alphaKeys);
        }
        else if (d == 0)
        {
            GradientColorKey[] colorKeys = new GradientColorKey[2];
            colorKeys[0].color = a;
            colorKeys[0].time = 0;
            colorKeys[1].color = b;
            colorKeys[1].time = 1;
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[1];
            alphaKeys[0].alpha = 1;
            alphaKeys[0].time = 0;

            gradient.SetKeys(colorKeys, alphaKeys);
        }
    }
    void UpdateSaturation(Color a, Color b)
    {
        GradientColorKey[] colorKeys = gradient.colorKeys;
        GradientAlphaKey[] alphaKeys = gradient.alphaKeys;
        float f = colorKeys.Length - 1;
        float minA = minvalue(a);
        float minB = minvalue(b);

        for (int i = 0; i <= f; i++)
        {
            float minVar = minvalue(colorKeys[i].color);

            if (colorKeys[i].color.r == minVar) //si rouge est le plus petit
            {
                colorKeys[i].color += new Color(Mathf.Lerp(minA, minB, i / f), 0, 0, 0);

                if (colorKeys[i].color.g == minVar)
                {
                    colorKeys[i].color += new Color(0, Mathf.Lerp(minA, minB, i / f), 0, 0);
                }
                else if (colorKeys[i].color.b == minVar)
                {
                    colorKeys[i].color += new Color(0, 0, Mathf.Lerp(minA, minB, i / f), 0);
                }
                gradient.SetKeys(colorKeys, alphaKeys);
            }
            else if (colorKeys[i].color.g == minVar) //si vert est le plus petit
            {
                colorKeys[i].color += new Color(0, Mathf.Lerp(minA, minB, i / f), 0, 0);

                if (colorKeys[i].color.r == minVar)
                {
                    colorKeys[i].color += new Color(Mathf.Lerp(minA, minB, i / f), 0, 0, 0);
                }
                else if (colorKeys[i].color.b == minVar)
                {
                    colorKeys[i].color += new Color(0, 0, Mathf.Lerp(minA, minB, i / f), 0);
                }
                gradient.SetKeys(colorKeys, alphaKeys);
            }
            else if (colorKeys[i].color.b == minVar) //si bleu est le plus petit
            {
                colorKeys[i].color += new Color(0, 0, Mathf.Lerp(minA, minB, i / f), 0);
                if (colorKeys[i].color.r == minVar)
                {
                    colorKeys[i].color += new Color(0, Mathf.Lerp(minA, minB, i / f), 0, 0);
                }
                else if (colorKeys[i].color.g == minVar)
                {
                    colorKeys[i].color += new Color(0, Mathf.Lerp(minA, minB, i / f), 0, 0);
                }
                gradient.SetKeys(colorKeys, alphaKeys);
            }
        }

    }
    void UpdateLuminosity(Color a, Color b)
    {
        GradientColorKey[] colorKeys = gradient.colorKeys;
        GradientAlphaKey[] alphaKeys = gradient.alphaKeys;
        float f = colorKeys.Length - 1;
        float maxA = a.maxColorComponent;
        float maxB = b.maxColorComponent;

        for (int i = 0; i <= f; i++)
        {
            float maxVar = colorKeys[i].color.maxColorComponent;
            if (colorKeys[i].color.r == maxVar)
            {
                colorKeys[i].color -= new Color(1 - Mathf.Lerp(maxA, maxB, i / f), 0, 0, 0);

                if (colorKeys[i].color.g == maxVar)
                {
                    colorKeys[i].color -= new Color(0, 1 - Mathf.Lerp(maxA, maxB, i / f), 0, 0);
                }
                else if (colorKeys[i].color.b == maxVar)
                {
                    colorKeys[i].color -= new Color(0, 0, 1 - Mathf.Lerp(maxA, maxB, i / f), 0);
                }
                gradient.SetKeys(colorKeys, alphaKeys);
            }
            else if (colorKeys[i].color.g == maxVar)
            {
                colorKeys[i].color -= new Color(0, 1 - Mathf.Lerp(maxA, maxB, i / f), 0, 0);

                if (colorKeys[i].color.r == maxVar)
                {
                    colorKeys[i].color -= new Color(1 - Mathf.Lerp(maxA, maxB, i / f), 0, 0, 0);
                }
                else if (colorKeys[i].color.b == maxVar)
                {
                    colorKeys[i].color -= new Color(0, 0, 1 - Mathf.Lerp(maxA, maxB, i / f), 0);
                }
                gradient.SetKeys(colorKeys, alphaKeys);
            }
            else if (colorKeys[i].color.b == maxVar)
            {
                colorKeys[i].color -= new Color(0, 0, 1 - Mathf.Lerp(maxA, maxB, i / f), 0);

                if (colorKeys[i].color.r == maxVar)
                {
                    colorKeys[i].color -= new Color(1 - Mathf.Lerp(maxA, maxB, i / f), 0, 0, 0);
                }
                else if (colorKeys[i].color.g == maxVar)
                {
                    colorKeys[i].color -= new Color(0, 1 - Mathf.Lerp(maxA, maxB, i / f), 0, 0);
                }
                gradient.SetKeys(colorKeys, alphaKeys);
            }

        }
    }
    void ResizeGradient(float tr, float ti)
    {
        GradientColorKey[] colorKeys = gradient.colorKeys;
        GradientAlphaKey[] alphaKeys = gradient.alphaKeys;
        float f = colorKeys.Length - 1;
        float e = tr + ti;

        for (int i = 0; i <= f; i++)
        {
            colorKeys[i].time = Mathf.Lerp(tr, e, i / f);
            gradient.SetKeys(colorKeys, alphaKeys);
        }
    }
    void CombineGradient(Gradient G1, Gradient G2)
    {
        GradientColorKey[] G1colorKeys = G1.colorKeys;
        GradientColorKey[] G2colorKeys = G2.colorKeys;
        GradientColorKey[] NEWColorKeys = new GradientColorKey[G1colorKeys.Length + G2colorKeys.Length];
        GradientAlphaKey[] alphaKeys = gradient.alphaKeys;
        int f1 = G1colorKeys.Length - 1;
        int f2 = G2colorKeys.Length - 1;

        for (int i = 0; i <= f1; i++)
        {
            NEWColorKeys[i].color = G1colorKeys[i].color;
            NEWColorKeys[i].time = G1colorKeys[i].time;
            gradient.SetKeys(NEWColorKeys, alphaKeys);
        }
        for (int i = f1 + 1; i <= f1 + f2 + 1; i++)
        {
            NEWColorKeys[i].color = G2colorKeys[i - f1 - 1].color;
            NEWColorKeys[i].time = G2colorKeys[i - f1 - 1].time;
            gradient.SetKeys(NEWColorKeys, alphaKeys);
        }
    }
    void ShowTransition()
    {
        Keyframe[] k = new Keyframe[6];
        k[0].value = 1;
        k[0].time = 0;
        k[1].value = 1;
        k[1].time = twilightToNight / 100;
        k[2].value = 0;
        k[2].time = twilightToNight / 100 + TransitionNight;
        k[3].value = 0;
        k[3].time = nightToDawn / 100;
        k[4].value = 1;
        k[4].time = nightToDawn / 100 + TransitionDawn;
        k[5].value = 1;
        k[5].time = 1;
        transition.keys = k;
    }
    float minvalue(Color input)
    {
        Vector4 a = input;
        Vector4 b = (a - new Vector4(1, 1, 1, 0));   // make négative color
        Color c = -b;                               // make new color
        return (1 - c.maxColorComponent);             // return min value
    }
    #endregion

    #region CurveMaker
    private void MakeCurve(AnimationCurve var , float a, float b,float c)
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
    #endregion
}

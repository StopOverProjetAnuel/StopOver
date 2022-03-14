using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEditor;
using Cinemachine;


[HelpURL("https://docs.google.com/presentation/d/1KgID6LoFfqxsCjdgWoTt3MLiCXOYd3v8bCV8YnJ4-HY/edit?usp=sharing")]
public class SpeedEffectCamera : MonoBehaviour
{
    //___________________________________________________//Variables\\____________________________________________________________________________________________________________________________________________________
    [Space()]
    [Range(0.0f, 1.0f)]                                  //la variable si dessous est un float variable entre 0 et 1 ET le float apparait sur l'inspector sous forme de slider
    public float speed;                                  //variable vitesse du joueur 

    //[HideInInspector]
    public AnimationCurve SpeedFOV;                      // (0 , 60) ; (0.3333 , 70) ; (0.6666 , 80) ; (1 , 100)
    //[HideInInspector]
    public AnimationCurve SpeedAmplitude;                // (0 , 0) ; (0.3333 , 0.5) ; (0.6666 , 2) ; (1 , 3)
    //[HideInInspector]
    public AnimationCurve SpeedFrequence;                // (0 , 0) ; (0.3333 , 0.5) ; (0.6666 , 2) ; (1 , 3)
    //[HideInInspector]
    public AnimationCurve SpeedOffsetY;                  // (0 , 3.6) ; (0.3333 , 3.6) ; (0.6666 , 3.2) ; (1 , 2.5)
    //[HideInInspector]
    public AnimationCurve SpeedOffsetZ;                  // (0 , -15) ; (0.3333 , -15) ; (0.6666 , -13) ; (1 , -11)


    private GameObject player;                           //le joueur
    private ParticleSystem SpeedEffectParticle;          //particle system de la cam�ra
    private Animator anim;                               //component
    private Volume volume;                               //component
    private ChromaticAberration chromaticAberration;     //settings demande: "using UnityEngine.Rendering" ET "using UnityEngine.Rendering.HighDefinition"
    private Vignette vignette;                           //settings demande: "using UnityEngine.Rendering" ET "using UnityEngine.Rendering.HighDefinition"
    private MotionBlur motionBlur;                       //settings demande: "using UnityEngine.Rendering" ET "using UnityEngine.Rendering.HighDefinition"

    private CinemachineVirtualCamera virtualCamera;      //!\\
    private CinemachineBasicMultiChannelPerlin CMNoise;  //!\\
    private CinemachineTransposer CMBody;                //!\\

    private Gradient gradient;                           //c'est un gradient il fonctionnne avec les deux valeurs si dessous
    private GradientAlphaKey[] AlphaKey;                 //les clefs alpha
    private GradientColorKey[] ColorKey;                 //les clefs couleurs

    private ParticleSystem CharaTrailParticle;           //le particule system fr�re de l'object
    private ParticleSystem CharaGroundTrail;             //le particule system neuveu de l'objet
    private ParticleSystem CharaTrailImpact;             //le particule system neuveu de l'objet

    private float FOVBonus;                              //!\\
    private float noiseAmplitudeBonus;                   //!\\
    private float noiseFrequencyBonus;                   //!\\
   


    private void Start()//__________________________________________________________//Start\\_________________________________________________________________________________________________________________________
    {
        initiate();                                                           //r�cup�re les component et les settings pour faires des variables exploitables
    }


    private void Update()//_________________________________________________________//Update\\________________________________________________________________________________________________________________________
    {
        UpdateVolume();                                                         //fait vairer le postprocess
        UpdateParticleSystem();                                                 //fait varier le particle system
        SpeedCamera();                                                          //!\\
        //TrailsIntensity();                                                    //!\\
        Debug.Log("speed : " + speed);
    }


    private void OnDrawGizmos()//___________________________________________________//OnDrawGizmos\\__________________________________________________________________________________________________________________
    {
        //Gizmos.color = Color.red;                                                                                                                       // trace en rouge
        //Gizmos.DrawRay(GameObject.FindGameObjectWithTag("Player").transform.localPosition + new Vector3(0.0f, -0.5f, 0.0f), Vector3.down * 5);          // trace un rayon rouge vers le bas de 5 unit� qui part du vaisseau voir  TrailsIntensity()
    }


    public void initiate()//________________________________________________________//Appel� dans start\\_____________________________________________________________________________________________________________

    {
        //GameObject parent = transform.parent.transform.GetChild(0).gameObject;
        anim = GetComponent<Animator>();                                                 //attrape le component "animator" sur l'objet
        volume = GetComponent<Volume>();                                                 //attrape le component "Volume" sur l'objet
        SpeedEffectParticle = Camera.main.GetComponent<ParticleSystem>();                                 //attrape le component "particle system" sur la main camera
        player = GameObject.FindGameObjectWithTag("Player");                             //attrape l'objet dans la scene avec le tag "Player" soit le joueur.

        volume.profile.TryGet<ChromaticAberration>(out chromaticAberration);             //attrape le setting "Chromatic Aberation" sur le componnent "Volume"
        volume.profile.TryGet<Vignette>(out vignette);                                   //attrape le setting "Vignette" sur le componnent "Volume"
        volume.profile.TryGet<MotionBlur>(out motionBlur);                               //attrape le setting "Motion Blur" sur le componnent "Volume"

        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();                               //!\\
        CMNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();      //!\\
        CMBody = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();                    //!\\

        gradient = new Gradient();
        UpdateGradient();                                                                //met � jour le gradient pour le particule system

        //CharaTrailParticle = parent.transform.Find("Chara's Trails").gameObject.GetComponent<ParticleSystem>();                 //attrape le particle system sur un fr�re appel� "Chara's Trails"
        //CharaTrailImpact = CharaTrailParticle.transform.Find("Chara's drop Impact").gameObject.GetComponent<ParticleSystem>();  //attrape le particle system d'un neuveu appel� "Chara's drop Impact"
        //CharaGroundTrail = CharaTrailParticle.transform.Find("Chara's Ground Trail").gameObject.GetComponent<ParticleSystem>(); //attrape le particle system d'un neuveu appel� "Chara's Ground Trail"

    }
    
    void UpdateVolume()//___________________________________________________________//appel� dans Update\\____________________________________________________________________________________________________________
    {
        chromaticAberration.intensity.value = speed;                  // le setting "chromatique aberation" fonctionne de mani�re dynamique en fonction de la variable "speed"
        vignette.intensity.value = 0.6f * speed;                      // le setting "vignette" fonctionne de mani�re dynamique en fonction de la variable "speed" ET au max la valeur est �gale � 0.6 
        motionBlur.intensity.value = 5.0f * speed;                    // le setting "motion blur" fonctionne de mani�re dynamique en fonction de la variable "speed" ET au max la valeur est �gale � 5 
    }
    void UpdateParticleSystem()//___________________________________________________// appel� dans Update\\___________________________________________________________________________________________________________
    {
        var main = SpeedEffectParticle.main;                                        // attrape le setting "Main" du component "particle system"
        main.startLifetime = 0.5f * Mathf.Sin(speed * Mathf.PI) + 0.05f;            // start life time = SLT =  0.5 x sin(speed x pi) +0.05   ; de cette mani�re speed = 0.5 <=> SLT = 0.5    ET   speed = 1 <=> SLT = 0.05                                   // attrape le setting "Main" du component "particle system"
        main.startSpeed = 30 * Mathf.Pow(speed, 2.6f);                              //                  start speed = SSP = 30 x "speed"^2,6   ; de cette mani�re speed = 0.5 <=> SSP = 5      ET   speed = 1 <=> SSP = 30


        var emission = SpeedEffectParticle.emission;                                // attrape le setting "�mission" du component "particle system"
        emission.rateOverTime = 300 * Mathf.Pow(speed, 1.6f);                       //                   ROL = 300 x "speed"^1,6      ; de cette mani�re speed = 0.5 <=> ROL = 100    ET   speed = 1 <=> ROL = 100

        UpdateGradient();                                                           // met � jour le gradient

        var color = SpeedEffectParticle.colorOverLifetime;                          // attrape le setting "color over lifetime" du component "particle system"
        color.color = gradient;                                                     // utilise le nouveau gradient

    }  
    public void UpdateGradient()//__________________________________________________//appel� dans Start ET UpdateParticleSystem\\_____________________________________________________________________________________
    {
        ColorKey = new GradientColorKey[2];                                             //le gradient � 1 cl�e couleur
        ColorKey[0].color = Color.white;                                                //la cl�e est blanche
        ColorKey[0].time = 0.0f;                                                        //la cl�e est au d�but du gradient
        ColorKey[1].color = Color.white;                                                //la cl�e est blanche
        ColorKey[1].time = 1.0f;                                                        //la cl�e est � la fin du gradient

        AlphaKey = new GradientAlphaKey[3];                                             //le gradient � 3 cl�es alpha
        AlphaKey[0].time = 0.0f;                                                        //la cl�e 1 est au d�but
        AlphaKey[0].alpha = 0.0f;                                                       //la cl�e 1 est transparente 
        AlphaKey[1].time = 0.5f;                                                        //la cl�e 2 est au milieu
        AlphaKey[1].alpha = 0.4f * Mathf.Pow(speed, 2.3f);                              //la cl�e 2 est visible en fonction de la vitesse; speed = 0.5 <=> alpha = 20  ET  speed = 1 <=> alpha = 100
        AlphaKey[2].time = 1.0f;                                                        //la cl�e 3 est � la fin
        AlphaKey[2].alpha = 0.0f;                                                       //la cl�e 3 est au transparente

        gradient.SetKeys(ColorKey,AlphaKey);                                            //met � jour le gradient pour le particule system
    }
    

    public void CameraCollision(float magnitude)//__________________________________//appel� chez le player\\_________________________________________________________________________________________________________
    {
        StartCoroutine(CSOnCollision(magnitude));                                   //d�mare un camera shake violent qui diminue avec le temps
    }
    public void CameraMediumHarvest()//_____________________________________________//appel� chez le player\\_________________________________________________________________________________________________________
    {
        StartCoroutine(CSOnMediumHarvest(1.0f));                                    //fait un recul sur la camera sur une r�colte moyenne (feedback)
    }
    public void CameraBigHarvest()//________________________________________________//appel� chez le player\\_________________________________________________________________________________________________________
    {
        StartCoroutine(CSOnBigHarvest(1.0f));                                       //fait un recul + camera shake sur une grosse r�colte (feedback)
    }
    public IEnumerator CSOnCollision(float Timer)//_________________________________//appel� dans CameraCollision\\___________________________________________________________________________________________________
    {

        float value = Mathf.Clamp((Timer - ((UnityEngine.Time.deltaTime) * 0.9f)),0.0f , 1.0f);      // diminue progressivement la valeur du shake

        if(value != 0.0f)                                                                 // si le shake n'est pas termin�
        {
            noiseAmplitudeBonus = 10 * value;                                             //!\\
            noiseFrequencyBonus = 10 * value;                                             //!\\
            yield return new WaitForEndOfFrame();                                         // attend une frame pour �viter le crash
            StartCoroutine(CSOnCollision(value));                                         // recomence la couroutine pour continuer le shake
        }
        else                                                                              // si le shake est fini
        {
            StopCoroutine(CSOnCollision(0.0f));                                 // arr�te la couroutine
        }
    }
    public IEnumerator CSOnMediumHarvest(float weight)//________________________//appel� dans CameraMediumHarvest\\___________________________________________________________________________________________________
    {
        float value = Mathf.Clamp((weight - Time.deltaTime), 0.0f, 1.0f);                // diminue progressivement la valeur du shake 
        if (value != 0.0f)                                                               // si le shake n'est pas termin�
        {
            FOVBonus = 20 * value;                                                       //!\\
            yield return new WaitForEndOfFrame();                                        // attend une frame pour �viter le crash
            StartCoroutine(CSOnMediumHarvest(value));                                    // recomence la couroutine pour continuer le shake

        }
        else                                                                             // si le shake est fini
        {
            StopCoroutine(CSOnMediumHarvest(0.0f));                                      // arr�te la couroutine
        }
    }
    public IEnumerator CSOnBigHarvest(float weight)//_____________________________//appel� dans CameraBigHarvest\\____________________________________________________________________________________________________
    {
        float value = Mathf.Clamp((weight - ((Time.deltaTime) * 2.0f)), 0.0f, 1.0f);    // diminue progressivement la valeur du shake 
       
        if (value != 0.0f)                                                              // si le shake n'est pas termin�
        {
            FOVBonus = 30 * value;
            noiseAmplitudeBonus = 0.5f * value;
            noiseFrequencyBonus = 2 * value;

            yield return new WaitForEndOfFrame();                                       // attend une frame pour �viter le crash
            StartCoroutine(CSOnBigHarvest(value));                                      // recomence la couroutine pour continuer le shake
        }
        else                                                                            // si le shake est fini
        {
            StopCoroutine(CSOnBigHarvest(0.0f));                                        // arr�te la couroutine
        }
    }

    public void TrailsIntensity()//_________________________________________________//appel� dans Update\\____________________________________________________________________________________________________________
    {
        RaycastHit hit;                                                                                                    // d�finit un impact du rayon appel� "hit"
        Ray ray = new Ray (player.transform.localPosition + new Vector3(0.0f, -0.5f, 0.0f), Vector3.down * 5);             // trace un trait de 5 unit�s vers le bas depuis le vaisseau
        var trailEmission = CharaTrailParticle.emission;                                                                   // enregistre les r�glages "emission" du particle system "CharaTrailEmission" sous le nom "trailEmission" 
        var groundEmission = CharaGroundTrail.emission;                                                                    // enregistre les r�glages "emission" du particle system "CharaGroundTrail" sous le nom "groundEmission"



        if (Physics.Raycast(ray, out hit) == true && hit.distance <= 5)                                                    // si le rayon touche quelque chose a 5 unit� (ou moins) de distance
        {
            CharaTrailParticle.Play();                                                                                     // active le particule system "CharaTrailParticle"
            CharaGroundTrail.Play();                                                                                       // active le particule system "CharaGroundTrail"
            CharaTrailParticle.transform.position = hit.point;                                                             // place le particule system "CharaTrailParticle" sur l'intessection du rayon avec le sol
            trailEmission.rateOverTime = (5/hit.distance) * speed * 4;                                                     // la cadence des particules est en fonction de la vitesse et de la distance avec le sol
            groundEmission.rateOverTime = (5/hit.distance) * speed * 4;                                                    // la cadence des particules est en fonction de la vitesse et de la distance avec le sol 

            CharaTrailParticle.transform.eulerAngles = new Vector3(-15, player.transform.eulerAngles.y + 180, 0);          // oriente le trail de particule vers l'arri�re du vaisseau
            
        }
        else                                                                                                               // si le rayon ne touche rien
        {
            CharaTrailParticle.Stop();                                                                                     // stoppe les trails de particules
            CharaGroundTrail.Stop();                                                                                       // stoppe les trails de particules
        }
        

    }

    void SpeedCamera()//____________________________________________________________//appel� dans update\\____________________________________________________________________________________________________________
    {
        virtualCamera.m_Lens.FieldOfView = (SpeedFOV.Evaluate(speed) * 10) + FOVBonus;
        CMNoise.m_AmplitudeGain = SpeedAmplitude.Evaluate(speed) + noiseAmplitudeBonus;
        CMNoise.m_FrequencyGain = SpeedFrequence.Evaluate(speed) + noiseFrequencyBonus;
        CMBody.m_FollowOffset.y = SpeedOffsetY.Evaluate(speed);
        CMBody.m_FollowOffset.z = SpeedOffsetZ.Evaluate(speed);

    }

}

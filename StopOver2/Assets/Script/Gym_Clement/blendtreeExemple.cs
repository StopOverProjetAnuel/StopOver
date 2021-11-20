using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEditor;



public class blendtreeExemple : MonoBehaviour
{
    //___________________________________________________//Variables__________________________________________________________________________________________________________________________

    [Range(0.0f, 1.0f)]                                  //la variable si dessous est un float variable entre 0 et 1 ET le float apparait sur l'inspector sous forme de slider
    public float speed;                                  //variable vitesse du joueur 

    private ParticleSystem pS;                           //particle system en enfant de la cam�ra est � r�f�rencer dans cette variable
    private Animator anim;                               //component
    private Volume volume;                               //component
    private ChromaticAberration chromaticAberration;     //settings demande: "using UnityEngine.Rendering" ET "using UnityEngine.Rendering.HighDefinition"
    private Vignette vignette;                           //settings demande: "using UnityEngine.Rendering" ET "using UnityEngine.Rendering.HighDefinition"
    private MotionBlur motionBlur;                       //settings demande: "using UnityEngine.Rendering" ET "using UnityEngine.Rendering.HighDefinition"
   
    private Gradient gradient;                           //c'est un gradient il fonctionnne avec les deux valeurs si dessous
    private GradientAlphaKey[] AlphaKey;                 //les clefs alpha
    private GradientColorKey[] ColorKey;                 //les clefs couleurs




    private void Start()//____________________________________________________//Start______________________________________________________________________________________________________________________________ 
    {
        initiate();                                                           //r�cup�re les component et les settings pour faires des variables exploitables
    }


    private void Update()//___________________________________________________//Update______________________________________________________________________________________________________________________________
    {
        UpdateBlendTree();                                                    //fait varier l'animator
        UpdateVolume();                                                       //fait vairer le postprocess
        UpdateParticleSystem();                                               //fait varier le particle system
    }

    public void initiate()//_____________________________________________________________//Appel� dans start____________________________________________________________________________________
    {
        anim = GetComponent<Animator>();                                                 //attrape le component "animator" sur l'objet
        volume = GetComponent<Volume>();                                                 //attrape le component "Volume" sur l'objet
        pS = Camera.main.GetComponent<ParticleSystem>();                                 //attrape le component "particle system" sur la main camera

        volume.profile.TryGet<ChromaticAberration>(out chromaticAberration);             //attrape le setting "Chromatic Aberation" sur le componnent "Volume"
        volume.profile.TryGet<Vignette>(out vignette);                                   //attrape le setting "Vignette" sur le componnent "Volume"
        volume.profile.TryGet<MotionBlur>(out motionBlur);                               //attrape le setting "Motion Blur" sur le componnent "Volume"

        gradient = new Gradient();
        UpdateGradient();                                                                //met � jour le gradient pour le particule system
    }


    void UpdateBlendTree()//__________________________________________//appel� dans Update____________________________________________________________________________________________________
    {
        anim.SetFloat("Speed", speed);                                // c'est la variable de l'animator qui agit sur le blendTree
    }
    void UpdateVolume()//_____________________________________________//appel� dans Update____________________________________________________________________________________________________
    {
        chromaticAberration.intensity.value = speed;                  // le setting "chromatique aberation" fonctionne de mani�re dynamique en fonction de la variable "speed"
        vignette.intensity.value = 0.6f * speed;                      // le setting "vignette" fonctionne de mani�re dynamique en fonction de la variable "speed" ET au max la valeur est �gale � 0.6 
        motionBlur.intensity.value = 5.0f * speed;                    // le setting "motion blur" fonctionne de mani�re dynamique en fonction de la variable "speed" ET au max la valeur est �gale � 5 
    }
    void UpdateParticleSystem()//___________________________________________________//appel� dans Update_______________________________________________________________________________________________________
    {
        var main = pS.main;                                                         // attrape le setting "Main" du component "particle system"
        main.startLifetime = 0.5f * Mathf.Sin(speed * Mathf.PI) + 0.05f;            // start life time = SLT =  0.5 x sin(speed x pi) +0.05   ; de cette mani�re speed = 0.5 <=> SLT = 0.5    ET   speed = 1 <=> SLT = 0.05                                   // attrape le setting "Main" du component "particle system"
        main.startSpeed = 30 * Mathf.Pow(speed, 2.6f);                              //     start speed = SSP = 30 x "speed"^2,6   ; de cette mani�re speed = 0.5 <=> SSP = 5      ET   speed = 1 <=> SSP = 30


        var emission = pS.emission;                                                 // attrape le setting "�mission" du component "particle system"
        emission.rateOverTime = 300 * Mathf.Pow(speed, 1.6f);                       //                   ROL = 300 x "speed"^1,6      ; de cette mani�re speed = 0.5 <=> ROL = 100    ET   speed = 1 <=> ROL = 100

        UpdateGradient();                                                           // met � jour le gradient

        var color = pS.colorOverLifetime;                                           // attrape le setting "color over lifetime" du component "particle system"
        color.color = gradient;                                                     // utilise le nouveau gradient

    }  
    public void UpdateGradient()//______________________________________________________//appel� dans Start ET UpdateParticleSystem____________________________________________________________________________________
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
}

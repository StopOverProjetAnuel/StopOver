using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEditor;



public class blendtreeExemple : MonoBehaviour
{
    //___________________________________________________//Variables\\____________________________________________________________________________________________________________________________________________________

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




    private void Start()//__________________________________________________________//Start\\_________________________________________________________________________________________________________________________
    {
        initiate();                                                           //r�cup�re les component et les settings pour faires des variables exploitables
    }


    private void Update()//_________________________________________________________//Update\\________________________________________________________________________________________________________________________
    {
        UpdateBlendTree();                                                    //fait varier l'animator
        UpdateVolume();                                                       //fait vairer le postprocess
        UpdateParticleSystem();                                               //fait varier le particle system
    }

    public void initiate()//________________________________________________________//Appel� dans start\\_____________________________________________________________________________________________________________
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


    void UpdateBlendTree()//________________________________________________________//appel� dans Update\\____________________________________________________________________________________________________________
    {
        anim.SetFloat("Speed", speed);                                // c'est la variable de l'animator qui agit sur le blendTree
    }
    void UpdateVolume()//___________________________________________________________//appel� dans Update\\____________________________________________________________________________________________________________
    {
        chromaticAberration.intensity.value = speed;                  // le setting "chromatique aberation" fonctionne de mani�re dynamique en fonction de la variable "speed"
        vignette.intensity.value = 0.6f * speed;                      // le setting "vignette" fonctionne de mani�re dynamique en fonction de la variable "speed" ET au max la valeur est �gale � 0.6 
        motionBlur.intensity.value = 5.0f * speed;                    // le setting "motion blur" fonctionne de mani�re dynamique en fonction de la variable "speed" ET au max la valeur est �gale � 5 
    }
    void UpdateParticleSystem()//___________________________________________________//appel� dans Update\\____________________________________________________________________________________________________________
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
    public IEnumerator CSOnCollision(float weight)//________________________________//appel� dans CameraCollision\\___________________________________________________________________________________________________
    {
        float value = Mathf.Clamp((weight - ((Time.deltaTime) * 0.9f)),0.0f , 1.0f);      // diminue progressivement la valeur du shake 
        anim.SetLayerWeight(1, value);                                                    // modifie l'influance du layer d'animation. les layers offrent des animations qui s'additionent avec les autres animatinons actives
        if(value != 0.0f)                                                                 // si le shake n'est pas termin�
        {
            yield return new WaitForEndOfFrame();                                         // attend une frame pour �viter le crash
            StartCoroutine(CSOnCollision(value));                                         // recomence la couroutine pour continuer le shake
        }
        else                                                                              // si le shake est fini
        {
            StopCoroutine(CSOnCollision(0.0f));                                           // arr�te la couroutine
        }
    }
    public IEnumerator CSOnMediumHarvest(float weight)//____________________________//appel� dans CameraMediumHarvest\\_______________________________________________________________________________________________
    {
        float value = Mathf.Clamp((weight - Time.deltaTime), 0.0f, 1.0f);                // diminue progressivement la valeur du shake 
        anim.SetLayerWeight(2, value);                                                   // modifie l'influance du layer d'animation. les layers offrent des animations qui s'additionent avec les autres animatinons actives
        if (value != 0.0f)                                                               // si le shake n'est pas termin�
        {
            yield return new WaitForEndOfFrame();                                        // attend une frame pour �viter le crash
            StartCoroutine(CSOnMediumHarvest(value));                                    // recomence la couroutine pour continuer le shake
        }
        else                                                                             // si le shake est fini
        {
            StopCoroutine(CSOnMediumHarvest(0.0f));                                      // arr�te la couroutine
        }
    }
    public IEnumerator CSOnBigHarvest(float weight)//_______________________________//appel� dans CameraBigHarvest\\__________________________________________________________________________________________________
    {
        float value = Mathf.Clamp((weight - ((Time.deltaTime) * 2.0f)), 0.0f, 1.0f);    // diminue progressivement la valeur du shake 
        anim.SetLayerWeight(3, value);                                                  // modifie l'influance du layer d'animation. les layers offrent des animations qui s'additionent avec les autres animatinons actives
        if (value != 0.0f)                                                              // si le shake n'est pas termin�
        {
            yield return new WaitForEndOfFrame();                                       // attend une frame pour �viter le crash
            StartCoroutine(CSOnBigHarvest(value));                                      // recomence la couroutine pour continuer le shake
        }
        else                                                                            // si le shake est fini
        {
            StopCoroutine(CSOnBigHarvest(0.0f));                                        // arr�te la couroutine
        }
    }




}

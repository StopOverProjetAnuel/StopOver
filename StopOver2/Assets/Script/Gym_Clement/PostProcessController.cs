using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEditor;


public class PostProcessController : MonoBehaviour
{
    //________________________________________________________________________________//Variables\\_______________________________________________________________________________________________



    //_________________________________________________________________________________//Public\\_________________________________________________________________________________________________
    [Header("Réglage De la durée de la partie en secondes")]
    [Header("______________________________________________________________________________________________________________________________________________________________________________")]
    [Space(10)]
    public float Timer = 600.0f;//____________________________________________________//c'est le Timer pour régler la durée de la partie en secondes il est arondis au centième au cas où il serait affiché à l'écran
    [Space()]
    [Header("(!!! Twilight To Night est toujours inferieur à Night To Dawn !!!)")]
    [Header("La Proportion crépuscule/Nuit/Aube en %")]
    [Header("______________________________________________________________________________________________________________________________________________________________________________")]
    [Range(0, 100)]
    public float twilightToNight = 33;//_____________________________________________//définit en % la durée du crépuscule par rapport à la variable Timer
    [Range(0, 100)]
    public float nightToDawn = 66;//_________________________________________________//définit en % la durée de la nuit par rapport à la variable Timer
    [Header("Ne peut être édité")]
    [Header("______________________________________________________________________________________________________________________________________________________________________________")]
    [Space(10)]
    [Range(0.0f, 1.0f)]
    public float WeightVolume = 0;//____________________________________________________// permet de montrer la progression du timer via un slider il est également utilisé pour rythmer l'influance des post process
    [HideInInspector]
    public AnimationCurve Vol1Weight;//______________________________________________// courbe d'animation utilisé pour définir l'influance du premier post process le long de la duré du Timer
    [HideInInspector]
    public AnimationCurve Vol2Weight;//______________________________________________// courbe d'animation utilisé pour définir l'influance du second post process le long de la duré du Timer
    [HideInInspector]
    public AnimationCurve Vol3Weight;//______________________________________________// courbe d'animation utilisé pour définir l'influance du troisème post process le long de la duré du Timer





    //________________________________________________________________________________//Private\\_________________________________________________________________________________________________

    private float CurentTimer = 0.0f;                                                // variable qui variera entre 0 et la fin du Timer

    private Volume[] allVol;                                              // array qui va lister les component PostProcessVolume sur le GameObject 
    private Volume Vol1;                                                  // premier component de type PostProcessVolume sur le GameObject
    private Volume Vol2;                                                  // deuxième component de type PostProcessVolume sur le GameObject
    private Volume Vol3;                                                  // troisième component de type PostProcessVolume sur le GameObject


    private Keyframe[] Vol1Keys = new Keyframe[4];                                   // array de 4 keyframes qui tracera la courbe PPV1Weight
    private Keyframe[] Vol2Keys = new Keyframe[5];                                   // array de 5 keyframes qui tracera la courbe PPV2Weight
    private Keyframe[] Vol3Keys = new Keyframe[4];                                   // array de 4 keyframes qui tracera la courbe PPV3Weight







    //______________________________________________________________________________//Fonctions\\____________________________________________________________________________________________________


    private void Start()//________________________________________________________//Start\\____________________________________________________________________________________________________
    {
        Initiate();                                                                 // récupère les component pour les exploiters sous formes de variables
    }


    public void Initiate()//____________________________________________________//initiate\\___________________________________________________________________________________________________
    {
        allVol = gameObject.GetComponents<Volume>();                     // définit un array qui liste tous les component PostProcessVolume sur le GameObject
        Vol1 = allVol[0];                                                           // le premier component de la liste sera associé à la valeur PPV1
        Vol2 = allVol[1];                                                           // le deuxième component de la liste sera associé à la valeur PPV2
        Vol3 = allVol[2];                                                           // le troisième component de la liste sera associé à la valeur PPV3

        // ralentis l'animator en fonction du timer
        SetCurves();                                                                // trace les courbes en fonction des valeurs twilightToNight et nightToDawn
        StartCoroutine(StartTimer(Timer));                                          // démare le timer avec la valeur Timer 
    }


    IEnumerator StartTimer(float MaxTimer)//______________________________//Appelé dans initiate\\_____________________________________________________________________________________________
    {
        CurentTimer += Time.deltaTime;                                              // ajoute le temps d'une frame a chaque fois que la coroutine se répète (varie en fonction des FPS).
        CurentTimer = Mathf.Clamp(CurentTimer, 0.0f, MaxTimer);                     // encadre la valeur CurentTimer entre 0 et le maximum.
        Timer = (Mathf.Ceil(CurentTimer * 1000)) / 1000;                            // modifie la valeur Timer visible dans l'inspector pour voir la progression il est arondis
        //                                                                             au centième au cas où la valeur doit être affiché à l'écran.

        WeightVolume = CurentTimer / MaxTimer;                                         // montre la progression du Timer sur un slider permet également de evaluer les courbes voir is dessous

        Vol1.weight = Vol1Weight.Evaluate(WeightVolume);                               // évaluation des courbes: grace au float entre 0 et 1 donné par la valeur WeightPPV un nouveau float 
        Vol2.weight = Vol2Weight.Evaluate(WeightVolume);//                                est donnée en sortie en fonction de la hauteur du point sur la courbe à l'instant donné par WeightPPV  
        Vol3.weight = Vol3Weight.Evaluate(WeightVolume);//                                cette valeur est ensuite utilisé pour définir l'influance du component PostProcessVolume
        //                                                                             il existe 3 courbes soit 1 courbes pour chaque component

        if (CurentTimer != MaxTimer)                                                // si le timer n'est pas fini à la fin de la fonction
        {
            yield return Time.deltaTime;                                            // répète la coroutine une frame plus tard
            StartCoroutine(StartTimer(MaxTimer));
        }

    }


    public void SetCurves()//_____________________________________________//Appelé dans initiate\\_____________________________________________________________________________________________
    {

        nightToDawn = Mathf.Clamp(nightToDawn, twilightToNight, 100);              // encadre la variable nightToDawn pour que les courbes se dessinent bien

        float n = twilightToNight / 100;                                            // twilightToNight est divisé par 100 pour avoir une variable compris entre 0 et 1 ainsi on convertis pourcentages en unités
        float d = nightToDawn / 100;                                                // nightToDawn est divisé par 100 pour avoir une variable compris entre 0 et 1 ainsi on convertis pourcentages en unités


                                                                                    // _____//Première Courbe\\_____
                                                                                    // la courbe aura la shape suivante:
                                                                                    //             ___
                                                                                    //                |
                                                                                    //                |________

        Vol1Keys[0] = new Keyframe { time = 0, value = 1 };                         // la première keyframe à pour coordonée (0,1)
        Vol1Keys[1] = new Keyframe { time = n, value = 1 };                         // la deuxime keyframe se situe en fonction du pourcentage donné avec TwilightToNight, elle est de valeur 1 pour former une ligne droite horiontale avec la première keyframe
        Vol1Keys[2] = new Keyframe { time = n + 0.2f, value = 0 };               // la keyframe suiante se situe 0.0001 plus loin et est de valeur 0 pour que la courbe fasse un angle droit et desactiver l'influance du PostProcessVolume(voir note si après pour savoir pourquoi)
        Vol1Keys[3] = new Keyframe { time = 1, value = 0 };                         // la dernière keyframe de coordonée (1,0) fait une ligne horizontale avec la troisième keyframe

                                                                                    // ____//Deuxième Courbe\\____
                                                                                    //la courbe aura la shape suivante:    
                                                                                    //               _____
                                                                                    //              /     |
                                                                                    //             /      |___

        Vol2Keys[0] = new Keyframe { time = 0, value = 0 };                         //la première keyframe à pour coordonée (0,0)
        Vol2Keys[1] = new Keyframe { time = n, value = 1 };                         //la deuxime keyframe se situe en fonction du pourcentage donné avec twilightToNight, elle est de valeur 1 pour former une ligne montante avec la première keyframe
        Vol2Keys[2] = new Keyframe { time = d, value = 1 };                         //la troisième keyframe se situe en fonction du pourcentage donné avec nightToDawn, elle est de valeur 1 pour former une ligne horizontale avec la deuxième keyframe
        Vol2Keys[3] = new Keyframe { time = d + 0.2f, value = 0 };               //la keyframe suiante se situe 0.0001 plus loin et est de valeur 0 pour que la courbe fasse un angle droit et desactiver l'influance du PostProcessVolume(voir note si après pour savoir pourquoi)
        Vol2Keys[4] = new Keyframe { time = 1, value = 0 };                         //la dernière keyframe de coordonée (1,0) fait une ligne horizontale avec la quatrième keyframe


                                                                                    // _____//Troisième Courbe\\_____
                                                                                    // la courbe aura la shape suivante:
                                                                                    //                  ____
                                                                                    //                 /
                                                                                    //         _______/

        Vol3Keys[0] = new Keyframe { time = 0, value = 0 };                         // la première keyframe à pour coordonée (0,0)
        Vol3Keys[1] = new Keyframe { time = n, value = 0 };                         // la deuxime keyframe se situe en fonction du pourcentage donné avec TwilightToNight, elle est de valeur 0 pour former une ligne droite horiontale avec la première keyframe
        Vol3Keys[2] = new Keyframe { time = d, value = 1 };                         // la troisième keyframe se situe en fonction du pourcentage donné avec nightToDawn, elle est de valeur 1 pour former une ligne ascendante avec la deuxième keyframe
        Vol3Keys[3] = new Keyframe { time = 1, value = 1 };                         // la dernière keyframe de coordonée (1,1) fait une ligne horizontale avec la troisième keyframe 

        Vol1Weight.keys = Vol1Keys;                                                 // traçage de courbe avec les keyframes de l'array PPV1Keys
        Vol2Weight.keys = Vol2Keys;                                                 // traçage de courbe avec les keyframes de l'array PPV2Keys
        Vol3Weight.keys = Vol3Keys;                                                 // traçage de courbe avec les keyframes de l'array PPV3Keys


                                                                                    // Note: certains des PostProcessVolume sont desactivé aux valeurs n + 0.0001 et d + 0.0001
                                                                                    //       le but de cette manoeuvre est de garder 2 PostProcessVolume actif UNIQUEMENT lors de la transition entre les deux profils
                                                                                    //       le ProcessProfile de la nuit peut peut être se dispenser de certains options du PostProcessProfile du Crépuscule
                                                                                    //       par conséquent le surplus d'options doient être désactivé  


    }



}

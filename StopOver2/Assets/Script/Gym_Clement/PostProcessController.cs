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
    [Header("R�glage De la dur�e de la partie en secondes")]
    [Header("______________________________________________________________________________________________________________________________________________________________________________")]
    [Space(10)]
    public float Timer = 600.0f;//____________________________________________________//c'est le Timer pour r�gler la dur�e de la partie en secondes il est arondis au centi�me au cas o� il serait affich� � l'�cran
    [Space()]
    [Header("(!!! Twilight To Night est toujours inferieur � Night To Dawn !!!)")]
    [Header("La Proportion cr�puscule/Nuit/Aube en %")]
    [Header("______________________________________________________________________________________________________________________________________________________________________________")]
    [Range(0, 100)]
    public float twilightToNight = 33;//_____________________________________________//d�finit en % la dur�e du cr�puscule par rapport � la variable Timer
    [Range(0, 100)]
    public float nightToDawn = 66;//_________________________________________________//d�finit en % la dur�e de la nuit par rapport � la variable Timer
    [Header("Ne peut �tre �dit�")]
    [Header("______________________________________________________________________________________________________________________________________________________________________________")]
    [Space(10)]
    [Range(0.0f, 1.0f)]
    public float WeightVolume = 0;//____________________________________________________// permet de montrer la progression du timer via un slider il est �galement utilis� pour rythmer l'influance des post process
    [HideInInspector]
    public AnimationCurve Vol1Weight;//______________________________________________// courbe d'animation utilis� pour d�finir l'influance du premier post process le long de la dur� du Timer
    [HideInInspector]
    public AnimationCurve Vol2Weight;//______________________________________________// courbe d'animation utilis� pour d�finir l'influance du second post process le long de la dur� du Timer
    [HideInInspector]
    public AnimationCurve Vol3Weight;//______________________________________________// courbe d'animation utilis� pour d�finir l'influance du trois�me post process le long de la dur� du Timer





    //________________________________________________________________________________//Private\\_________________________________________________________________________________________________

    private float CurentTimer = 0.0f;                                                // variable qui variera entre 0 et la fin du Timer

    private Volume[] allVol;                                              // array qui va lister les component PostProcessVolume sur le GameObject 
    private Volume Vol1;                                                  // premier component de type PostProcessVolume sur le GameObject
    private Volume Vol2;                                                  // deuxi�me component de type PostProcessVolume sur le GameObject
    private Volume Vol3;                                                  // troisi�me component de type PostProcessVolume sur le GameObject


    private Keyframe[] Vol1Keys = new Keyframe[4];                                   // array de 4 keyframes qui tracera la courbe PPV1Weight
    private Keyframe[] Vol2Keys = new Keyframe[5];                                   // array de 5 keyframes qui tracera la courbe PPV2Weight
    private Keyframe[] Vol3Keys = new Keyframe[4];                                   // array de 4 keyframes qui tracera la courbe PPV3Weight







    //______________________________________________________________________________//Fonctions\\____________________________________________________________________________________________________


    private void Start()//________________________________________________________//Start\\____________________________________________________________________________________________________
    {
        Initiate();                                                                 // r�cup�re les component pour les exploiters sous formes de variables
    }


    public void Initiate()//____________________________________________________//initiate\\___________________________________________________________________________________________________
    {
        allVol = gameObject.GetComponents<Volume>();                     // d�finit un array qui liste tous les component PostProcessVolume sur le GameObject
        Vol1 = allVol[0];                                                           // le premier component de la liste sera associ� � la valeur PPV1
        Vol2 = allVol[1];                                                           // le deuxi�me component de la liste sera associ� � la valeur PPV2
        Vol3 = allVol[2];                                                           // le troisi�me component de la liste sera associ� � la valeur PPV3

        // ralentis l'animator en fonction du timer
        SetCurves();                                                                // trace les courbes en fonction des valeurs twilightToNight et nightToDawn
        StartCoroutine(StartTimer(Timer));                                          // d�mare le timer avec la valeur Timer 
    }


    IEnumerator StartTimer(float MaxTimer)//______________________________//Appel� dans initiate\\_____________________________________________________________________________________________
    {
        CurentTimer += Time.deltaTime;                                              // ajoute le temps d'une frame a chaque fois que la coroutine se r�p�te (varie en fonction des FPS).
        CurentTimer = Mathf.Clamp(CurentTimer, 0.0f, MaxTimer);                     // encadre la valeur CurentTimer entre 0 et le maximum.
        Timer = (Mathf.Ceil(CurentTimer * 1000)) / 1000;                            // modifie la valeur Timer visible dans l'inspector pour voir la progression il est arondis
        //                                                                             au centi�me au cas o� la valeur doit �tre affich� � l'�cran.

        WeightVolume = CurentTimer / MaxTimer;                                         // montre la progression du Timer sur un slider permet �galement de evaluer les courbes voir is dessous

        Vol1.weight = Vol1Weight.Evaluate(WeightVolume);                               // �valuation des courbes: grace au float entre 0 et 1 donn� par la valeur WeightPPV un nouveau float 
        Vol2.weight = Vol2Weight.Evaluate(WeightVolume);//                                est donn�e en sortie en fonction de la hauteur du point sur la courbe � l'instant donn� par WeightPPV  
        Vol3.weight = Vol3Weight.Evaluate(WeightVolume);//                                cette valeur est ensuite utilis� pour d�finir l'influance du component PostProcessVolume
        //                                                                             il existe 3 courbes soit 1 courbes pour chaque component

        if (CurentTimer != MaxTimer)                                                // si le timer n'est pas fini � la fin de la fonction
        {
            yield return Time.deltaTime;                                            // r�p�te la coroutine une frame plus tard
            StartCoroutine(StartTimer(MaxTimer));
        }

    }


    public void SetCurves()//_____________________________________________//Appel� dans initiate\\_____________________________________________________________________________________________
    {

        nightToDawn = Mathf.Clamp(nightToDawn, twilightToNight, 100);              // encadre la variable nightToDawn pour que les courbes se dessinent bien

        float n = twilightToNight / 100;                                            // twilightToNight est divis� par 100 pour avoir une variable compris entre 0 et 1 ainsi on convertis pourcentages en unit�s
        float d = nightToDawn / 100;                                                // nightToDawn est divis� par 100 pour avoir une variable compris entre 0 et 1 ainsi on convertis pourcentages en unit�s


                                                                                    // _____//Premi�re Courbe\\_____
                                                                                    // la courbe aura la shape suivante:
                                                                                    //             ___
                                                                                    //                |
                                                                                    //                |________

        Vol1Keys[0] = new Keyframe { time = 0, value = 1 };                         // la premi�re keyframe � pour coordon�e (0,1)
        Vol1Keys[1] = new Keyframe { time = n, value = 1 };                         // la deuxime keyframe se situe en fonction du pourcentage donn� avec TwilightToNight, elle est de valeur 1 pour former une ligne droite horiontale avec la premi�re keyframe
        Vol1Keys[2] = new Keyframe { time = n + 0.2f, value = 0 };               // la keyframe suiante se situe 0.0001 plus loin et est de valeur 0 pour que la courbe fasse un angle droit et desactiver l'influance du PostProcessVolume(voir note si apr�s pour savoir pourquoi)
        Vol1Keys[3] = new Keyframe { time = 1, value = 0 };                         // la derni�re keyframe de coordon�e (1,0) fait une ligne horizontale avec la troisi�me keyframe

                                                                                    // ____//Deuxi�me Courbe\\____
                                                                                    //la courbe aura la shape suivante:    
                                                                                    //               _____
                                                                                    //              /     |
                                                                                    //             /      |___

        Vol2Keys[0] = new Keyframe { time = 0, value = 0 };                         //la premi�re keyframe � pour coordon�e (0,0)
        Vol2Keys[1] = new Keyframe { time = n, value = 1 };                         //la deuxime keyframe se situe en fonction du pourcentage donn� avec twilightToNight, elle est de valeur 1 pour former une ligne montante avec la premi�re keyframe
        Vol2Keys[2] = new Keyframe { time = d, value = 1 };                         //la troisi�me keyframe se situe en fonction du pourcentage donn� avec nightToDawn, elle est de valeur 1 pour former une ligne horizontale avec la deuxi�me keyframe
        Vol2Keys[3] = new Keyframe { time = d + 0.2f, value = 0 };               //la keyframe suiante se situe 0.0001 plus loin et est de valeur 0 pour que la courbe fasse un angle droit et desactiver l'influance du PostProcessVolume(voir note si apr�s pour savoir pourquoi)
        Vol2Keys[4] = new Keyframe { time = 1, value = 0 };                         //la derni�re keyframe de coordon�e (1,0) fait une ligne horizontale avec la quatri�me keyframe


                                                                                    // _____//Troisi�me Courbe\\_____
                                                                                    // la courbe aura la shape suivante:
                                                                                    //                  ____
                                                                                    //                 /
                                                                                    //         _______/

        Vol3Keys[0] = new Keyframe { time = 0, value = 0 };                         // la premi�re keyframe � pour coordon�e (0,0)
        Vol3Keys[1] = new Keyframe { time = n, value = 0 };                         // la deuxime keyframe se situe en fonction du pourcentage donn� avec TwilightToNight, elle est de valeur 0 pour former une ligne droite horiontale avec la premi�re keyframe
        Vol3Keys[2] = new Keyframe { time = d, value = 1 };                         // la troisi�me keyframe se situe en fonction du pourcentage donn� avec nightToDawn, elle est de valeur 1 pour former une ligne ascendante avec la deuxi�me keyframe
        Vol3Keys[3] = new Keyframe { time = 1, value = 1 };                         // la derni�re keyframe de coordon�e (1,1) fait une ligne horizontale avec la troisi�me keyframe 

        Vol1Weight.keys = Vol1Keys;                                                 // tra�age de courbe avec les keyframes de l'array PPV1Keys
        Vol2Weight.keys = Vol2Keys;                                                 // tra�age de courbe avec les keyframes de l'array PPV2Keys
        Vol3Weight.keys = Vol3Keys;                                                 // tra�age de courbe avec les keyframes de l'array PPV3Keys


                                                                                    // Note: certains des PostProcessVolume sont desactiv� aux valeurs n + 0.0001 et d + 0.0001
                                                                                    //       le but de cette manoeuvre est de garder 2 PostProcessVolume actif UNIQUEMENT lors de la transition entre les deux profils
                                                                                    //       le ProcessProfile de la nuit peut peut �tre se dispenser de certains options du PostProcessProfile du Cr�puscule
                                                                                    //       par cons�quent le surplus d'options doient �tre d�sactiv�  


    }



}

using UnityEngine;
using UnityEngine.VFX;

public class C_CharacterFX : MonoBehaviour
{
    C_CharacterBoost _CharacterBoost;

    public Material matBoost;
    public Material matSurchauffe;
    public VisualEffect flammeSurchauffe;
    private float currentTimeSurchauffe;

    private float t1;


    public void InitiateFXValue(C_CharacterBoost cB)
    {
        _CharacterBoost = cB;
    }

    public void ActiveBoost()
    {
        matBoost.SetFloat("_Top_Flame_Gradient__Stop_flame_", 0);
       
        matBoost.SetFloat("_BoostFlame", 1);
    }

    public void DesactiveBoost()
    {
        matBoost.SetFloat("_BoostFlame", 0.63f);
    }

    public void DesactiveBosstSurchauffe()
    {
        flammeSurchauffe.Play();
        matBoost.SetFloat("_BoostFlame", 0.63f);
        matBoost.SetFloat("_Top_Flame_Gradient__Stop_flame_", 1);
    }

    public void SurchauffeBoost()
    {
        currentTimeSurchauffe = Mathf.Lerp(0, 1, _CharacterBoost.t2);
        matSurchauffe.SetFloat("Color_Size", currentTimeSurchauffe);
        matSurchauffe.SetFloat("_ColorDensity", currentTimeSurchauffe);
        matBoost.SetFloat("_Flame_ColorGradient", currentTimeSurchauffe);
    }

    public void SurchauffeBoostDecres(bool isNotActif) 
    {

        if(t1 >= 1)
        {
            t1 = 0;
        }
        else
        {
            t1 -= Time.deltaTime / _CharacterBoost.currentColdownBoost;
        }
        currentTimeSurchauffe = Mathf.Lerp(1, 0, t1);
        matSurchauffe.SetFloat("Color_Size", currentTimeSurchauffe);
        matSurchauffe.SetFloat("_ColorDensity", currentTimeSurchauffe);
        matBoost.SetFloat("_Flame_ColorGradient", currentTimeSurchauffe);
    }
}

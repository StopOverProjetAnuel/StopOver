using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CollidedPlayer : MonoBehaviour
{
    [Header("Require Information")]
    private RessourceManager ressourceManager;
    [SerializeField]private FMOD_SoundCaller _SoundCaller;

    private NudgeBars nudgeBars;

    [Header("Parameters")]
    public GameObject roche;
    public bool isDropOnDestroy = false;
    public static GameObject droppedItem;
    public float resourceGive;
    public int resistanceLevel;
    public bool isDestroyWithRequirement = false;
    public float ifNotRequireSpeedMultiplier = 0.75f;

    [Header("VFX Parameters")]
    [SerializeField] private string ID = "";
    [SerializeField] private bool useVarScale = false;
    [SerializeField] private Vector3 vfxScale = Vector3.one;

    [Space(10)]

    [SerializeField] private bool showDebug = false;

    private void Awake()
    {
        ressourceManager = FindObjectOfType<RessourceManager>();
        FMOD_SoundCaller soundCall;
        _SoundCaller = (TryGetComponent<FMOD_SoundCaller>(out soundCall) && !_SoundCaller) ? _SoundCaller = soundCall : null;
        nudgeBars = FindObjectOfType<NudgeBars>();
    }

    #region OLD
    /**private void OnCollisionEnter(Collision col)
    {
        Collider other = col.collider;
        if (other.CompareTag(playerWeaponTagName) && nudgeBars.nbState >= resistanceLevel + 1)
        {
            if (ressourceManager.currentRessource != ressourceManager.maxRessource)
            {
                ressourceManager.TriggerRessourceCount(resourceGive);
            }
            else
            {
                GameObject item = Instantiate(droppedItem, this.transform.position, Quaternion.identity);
                item.GetComponent<PickUp>().ressourceGive = resourceGive;
            }
            Transform.Destroy(gameObject);
        }
    }*/
    #endregion

    public void TriggerCollisionPlayer(Rigidbody rb)
    {
        if (nudgeBars.nbState >= resistanceLevel + 1)
        {
            GettingDestroy();
        }
        else if (!isDestroyWithRequirement || nudgeBars.nbState >= resistanceLevel)
        {
            Vector3 storedVelocity = rb.velocity;
            storedVelocity.x *= ifNotRequireSpeedMultiplier;
            storedVelocity.z *= ifNotRequireSpeedMultiplier;
            rb.velocity = storedVelocity;
            GettingDestroy();
        }

        #region Debug
        if (showDebug)
        {
            Debug.Log(gameObject.name + " have been triggered");
        }
        #endregion
    }

    private void GettingDestroy()
    {
        if (ressourceManager.currentRessource != ressourceManager.maxRessource)
        {
            ressourceManager.TriggerRessourceCount(resourceGive);
        }
        else if (isDropOnDestroy)
        {
            GameObject item = Instantiate(droppedItem, this.transform.position, Quaternion.identity);
            item.GetComponent<PickUp>().ressourceGive = resourceGive;
        }

        InstantiateVFX();
        if(_SoundCaller) _SoundCaller.SoundStart();

        roche.SetActive(false);

        #region Debug
        if (showDebug)
        {
            Debug.Log(gameObject.name + " task executed");
        }
        #endregion
    }

    private void InstantiateVFX()
    {
        Transform g = ObjectPool.Instance.GetFromPool(ID, transform).transform;
        g.localScale = (useVarScale) ? vfxScale : transform.localScale;
        Transform player = FindObjectOfType<C_CharacterManager>().gameObject.transform;
        Vector3 newRotationVector = player.rotation.eulerAngles;
        g.rotation = Quaternion.Euler(newRotationVector);
    }

    private void OnDisable()
    {
        if (_SoundCaller) _SoundCaller.SoundStop();
    }
}
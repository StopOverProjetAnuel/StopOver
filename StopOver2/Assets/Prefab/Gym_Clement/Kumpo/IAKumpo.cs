using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class IAKumpo : MonoBehaviour
{
    #region variables
    public enum IAKStates { Alpha, Follower, Escape}
    
    [Space(10)][Header("Reminder : Check Nav Mesh Agent for Speed")]

    [Tooltip("Comportement Actuel")]public IAKStates state;

    [Tooltip("aléatoire permet d'être chef de meute")][Space(20)] [SerializeField] private int Leadership = 0;

    [Tooltip("le chef de meute qui dirige le groupe")] [Space(10)] [SerializeField] private IAKumpo Alpha;

    [Tooltip("la distance maximale où l'alpha peut potentiellment aller")] [Space(10)] [SerializeField] private float roamingRange = 100; 

    [Tooltip("vitesse pour échapper au joueur")] [Space(10)] [SerializeField] private float escapeSpeed = 30;

    [Tooltip("vitesse de marche normale")] [Space(10)] [SerializeField] private float normalSpeed = 3.5f;

    [Tooltip("détecte ses copains pour les éviter")] [Space(10)] [SerializeField] private float radiusPal = 20;

    [Tooltip("si trouve un autre koumpo il découvre si il est le nouveau chef")] [Space(10)] [SerializeField] private float radiusAlpha = 50;

    [Tooltip("détecte le joueur pour le fuir")] [Space(10)] [SerializeField] private float radiusPlayer = 50; // à revoir mieux vaux que ce soit le joueur qui les fasse fuir

    [Tooltip("vitesse animation")] [Space(10)] [SerializeField] private float AnimSpeed = 1.5f;
    
    private GameObject player;
    private IAKumpo him;
    private NavMeshAgent nav;
    private Transform AlphaPos;
    private Vector3 Destination;
    private Collider[] pals;
    private Collider[] Kumpos;
    private Animator anim;
    private float currentSpeed;
    private NavMeshPath path;
    private CollidedPlayer _CollidedPlayer;
    #endregion

    #region initiate
    private void Awake()
    {
        initiate();
    } 
    void initiate()
    {
        him = GetComponent<IAKumpo>();
        Alpha = him;
        Leadership = Random.Range(0, 999);
        player = GameObject.FindGameObjectWithTag("Player");
        nav = GetComponent<NavMeshAgent>();
        AlphaPos = transform;
        anim = GetComponent<Animator>();
        path = new NavMeshPath();
    } 
    #endregion

    #region States
    private void Update()
    {
        Check_State();

        currentSpeed = nav.velocity.magnitude / nav.speed;
        anim.SetFloat("Speed", currentSpeed);
        anim.SetFloat("SpeedAnim", 1 + (currentSpeed * AnimSpeed));

    } // check V
    void Check_State()
    {
        // active the right behavior per frame
        switch (state)
        {
            case IAKStates.Alpha:

                ElectAlpha();
                Escape_Player();
                Alpha_GoAway();
                Debug.Log(Destination);

            break;

            case IAKStates.Follower:

                ElectAlpha();
                Distance_Pal();
                Escape_Player();
                Follow_Alpha();

            break;

            case IAKStates.Escape:

                Escape_Player();
                Distance_Pal();

            break;
        }     
    } // check V
    #endregion

    #region Fonctions
    void Alpha_GoAway()
    {
        nav.SetDestination(Destination);
        path = nav.path;

        if (Vector3.Distance(transform.position,Destination) < 5 || !nav.CalculatePath(Destination,path))
        {
            Vector2 r = Random.insideUnitCircle * roamingRange;
            Destination = new Vector3(transform.position.x + r.x, transform.position.y, transform.position.z + r.y);
        }
    } 
    void Follow_Alpha()
    {
        nav.SetDestination(Destination);
    } 


    void Escape_Player()
    {
        if(Vector3.Distance(transform.position, player.transform.position) < radiusPlayer)
        {
            nav.SetDestination((transform.position) + (transform.position - player.transform.position));
            state = IAKStates.Escape;
            nav.speed = escapeSpeed;
        }
        else if (state == IAKStates.Escape)
        {
            nav.speed = normalSpeed;
            state = IAKStates.Alpha;
        }
    }  // activé par le joueur
    void Distance_Pal()
    {
        pals = Physics.OverlapSphere(transform.position,radiusPal,1<<7);

        float nearestDist = radiusPal;
        int nearestIndex = 0;

        for (int i = 0; i < pals.Length ; i++)
        {
            float f = Vector3.Distance(transform.position, pals[i].transform.position);

            if (f < nearestDist && f != 0)
            {
                nearestDist = f;
                nearestIndex = i;
            }
        }

        Vector3 LeavePal = (nearestIndex != 0) ? ((transform.position) + (transform.position - pals[nearestIndex].transform.position)) : Vector3.one;

        Destination = Vector3.Lerp(LeavePal, AlphaPos.position, nearestDist / radiusPal);


    }  // occupe une place prédéfinie dans le groupe
    void ElectAlpha()
    {
        Kumpos = Physics.OverlapSphere(transform.position, radiusAlpha, 1<<7);

        foreach(Collider c in Kumpos)
        {
            
            IAKumpo k = c.GetComponent<IAKumpo>();
            Alpha = (k.Leadership >= Alpha.Leadership && Alpha.Leadership >= Leadership) ? k : Alpha;
        }

        Alpha = (Kumpos == null) ? him : Alpha;
        Leadership += (Alpha.Leadership == Leadership && Alpha != him) ? Random.Range(-10, 10) : 0; // il réélection la frame suivante si égalité
        state = (Alpha == him) ? IAKStates.Alpha : IAKStates.Follower;
        AlphaPos = Alpha.transform;


    }  // + de conditions
    // quand alpha meurs
    // au début
    // fin échapatoire
    // quand alpha trouve un autre koumpo (sonar)
    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusPlayer);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radiusAlpha);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radiusPal);
    }

}

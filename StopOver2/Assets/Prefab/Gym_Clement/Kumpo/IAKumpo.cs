using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class IAKumpo : MonoBehaviour
{
    #region variables
    public enum IAKStates { Alpha, Follower, Escape}
    
    [Space(10)][Header("Reminder : Check Nav Mesh Agent for Speed")]

    public IAKStates state;

    [Space(20)] [SerializeField] private int Leadership = 0; //aléatoire permet d'être chef de meute______voir ElectAlpha()

    [Space(10)] [SerializeField] private IAKumpo Alpha; // le chef de meute qui dirige le groupe_____voir FollowAlpha()

    [Space(10)] [SerializeField] private float roamingRange = 100; // la distance maximale où l'alpha peut potentiellment aller____voir Alpha_GoAway()

    [Space(10)] [SerializeField] private float escapeSpeed = 30; // vitesse pour échapper au joueur_____voir Escape_Player()

    [Space(10)] [SerializeField] private float normalSpeed = 3.5f; // vitesse de marche normale_____voir Escape_Player()

    [Space(10)] [SerializeField] private float radiusPal = 20; // détecte ses copains pour les éviter____voir Distance_Pal()

    [Space(10)] [SerializeField] private float radiusAlpha = 50; // si trouve un autre koumpo il découvre si il est le nouveau chef_____voir ElectAlpha
    
    [Space(10)] [SerializeField] private float radiusPlayer = 50; // détecte le joueur pour le fuir_____voir Escape_Player()
    
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
        anim.SetFloat("SpeedAnim", 1 + (currentSpeed * 1.5f));

    } // check V
    void Check_State()
    {
        // active the right behavior per frame
        if (state == IAKStates.Alpha)
        {
            ElectAlpha();
            Escape_Player();
            Alpha_GoAway();
            Debug.Log(Destination);

        }
        else if (state == IAKStates.Follower)
        {
            ElectAlpha();
            Distance_Pal();
            Escape_Player();
            Follow_Alpha();
        }
        else if (state == IAKStates.Escape)
        {
            Escape_Player();
            Distance_Pal();
        }

        
        
    } // check V
    #endregion

    // /!\ Layers
    #region Fonctions
    void Alpha_GoAway()
    {
        nav.SetDestination(Destination);
        path = nav.path;
        Debug.Log("KOUMPO" + nav.CalculatePath(Destination,path));
        //Debug.Log("KOUMPO" + Destination);

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
    }  
    void Distance_Pal()
    {
        pals = Physics.OverlapSphere(transform.position,radiusPal,1<<7); // /!\ Layer

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

        Vector3 LeavePal = ((transform.position) + (transform.position - pals[nearestIndex].transform.position));

        Destination = Vector3.Lerp(LeavePal, AlphaPos.position, nearestDist / radiusPal);


    }  // /!\ Layer
    void ElectAlpha()
    {
        Kumpos = Physics.OverlapSphere(transform.position, radiusAlpha, 1<<7);// /!\ Layer

        foreach(Collider c in Kumpos)
        {
            
            IAKumpo k = c.GetComponent<IAKumpo>();
            Alpha = (k.Leadership >= Alpha.Leadership && Alpha.Leadership >= Leadership) ? k : Alpha;
        }

        Alpha = (Kumpos == null) ? him : Alpha;
        Leadership += (Alpha.Leadership == Leadership && Alpha != him) ? Random.Range(-10, 10) : 0; // il réélection la frame suivante si égalité
        state = (Alpha == him) ? IAKStates.Alpha : IAKStates.Follower;
        AlphaPos = Alpha.transform;


    } // /!\ Layer
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    private Collider2D drag;
    public LayerMask layer;
    [SerializeField]
    private bool clicked;
    private Touch touch;

    public LineRenderer lineFront;
    public LineRenderer lineBack;

    private Ray leftCatapultRay;
    private CircleCollider2D passaroCol;
    private Vector2 catapultToBird;
    private Vector3 pointL;

    private SpringJoint2D spring;
    private Vector2 prevVel;
    [SerializeField]
    private Rigidbody2D passaroRB;   

    //limite elastico

    private Transform catapult;
    private Ray rayToMT;

    //rastro

    private TrailRenderer rastro;

    public Rigidbody2D catapultRB;
    public bool estouPronto = false;

    public AudioSource audioPassaro;    

    private void Awake()
    {
        drag = GetComponent<Collider2D>();
        passaroCol = GetComponent<CircleCollider2D>();
        passaroRB = GetComponent<Rigidbody2D>();
        drag = GetComponent<Collider2D>();

        leftCatapultRay = new Ray(lineFront.transform.position, Vector3.zero);
        spring = GetComponent<SpringJoint2D>();
        passaroRB = GetComponent<Rigidbody2D>();

        catapult = spring.connectedBody.transform;
        rayToMT = new Ray(catapult.position, Vector3.zero);
    }

    void Start()
    {
        SetupLine();
        
    }    
    void Update()
    {
        LineUpdate();
        SpringEffect();
        prevVel = passaroRB.velocity;

        if (clicked)
        {            
            Dragging();
        }

        if (clicked == false && passaroRB.isKinematic == false)
        {
            MataPassaro();
        }
    }

    void SetupLine()
    {
        lineFront.SetPosition(0, lineFront.transform.position);
        lineBack.SetPosition(0, lineBack.transform.position);
    }

    void LineUpdate()
    {
        catapultToBird = transform.position - lineFront.transform.position;
        leftCatapultRay.direction = catapultToBird;

        //retorna comprimento do vetor
        pointL = leftCatapultRay.GetPoint(catapultToBird.magnitude + (passaroCol.radius/7));

        lineFront.SetPosition(1, pointL);
        lineBack.SetPosition(1, pointL);
    }

    //passaro "larga" estilingue
    void SpringEffect()
    {
        if(spring != null)
        {
            if (passaroRB.isKinematic == false)
            {
                if(prevVel.sqrMagnitude > passaroRB.velocity.sqrMagnitude)
                {
                    lineFront.enabled = false;
                    lineBack.enabled = false;
                    Destroy(spring);
                    passaroRB.velocity = prevVel;

                }
            }
            else if (passaroRB.isKinematic == true)
            {
                lineFront.enabled = true;
                lineBack.enabled = true;
            }
        }
    }

    void MataPassaro()
    {
        if (passaroRB.velocity.magnitude == 0)
        {
            StartCoroutine(TempoMorte());
            StartCoroutine(TempoNasc());
        }
    }

    IEnumerator TempoMorte()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
        estouPronto = false;
    }

    IEnumerator TempoNasc()
    {
        yield return new WaitForSeconds(5);
        //Instantiate(this, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        
    }

    //mouse
    void Dragging()
    {
        if (passaroRB.isKinematic)
        {
            Vector3 mouseWP = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWP.z = 0f;
            
            catapultToBird = mouseWP - catapult.position;
            if (catapultToBird.magnitude > 3f)
            {
                rayToMT.direction = catapultToBird;
                mouseWP = rayToMT.GetPoint(3f);
            }

            transform.position = mouseWP;
        }
    }

    void OnMouseDown()
    {       
        clicked = true;        
        estouPronto = true;
    }

    void OnMouseUp()
    {
        if (estouPronto)
        {
            passaroRB.isKinematic = false;
            clicked = false;            
            audioPassaro.Play();
        }
    }
}    


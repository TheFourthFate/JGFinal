using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public State characterState;

    public Animator characterAnimator;

    public int pHP = 100;

    public int pDamage = 25;
    
    public GameObject greenPointLight;

    public GameObject explosionParticle;

    public bool pActive = false;

    public bool pRunning = false;

    public static bool pAttacking = false;

    private NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {

    }

    void Update()
    {
        ShowCharacterPointLight();

        PState();

        PMove();

        PDie();
    }

    public void ShowCharacterPointLight()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    pActive = !pActive;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pActive = false;
        }

        if (pActive == true)
        {
            greenPointLight.SetActive(true);
        }

        else
        {
            greenPointLight.SetActive(false);
        }
    }

    public enum State
    {
        Idle,
        Attack,
    }

    private void PState()
    {
        switch (characterState)
        {
            case State.Idle:

                //

                break;

            case State.Attack:

                StartCoroutine(PlayerAttack());

                break;
        }
    }

    public void PMove()
    {
        characterAnimator.SetBool("Run", pRunning);

        if (pActive == true)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                StopAllCoroutines();

                characterState = State.Idle;

                characterAnimator.SetBool("Slash", false);

                pAttacking = false;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    agent.destination = hit.point;
                }
            }

            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                pRunning = false;
            }

            else
            {
                pRunning = true;
            }
        }

        else
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                pRunning = false;
            }

            else
            {
                pRunning = true;
            }
        }
    }

    public IEnumerator PlayerAttack()
    {
        characterAnimator.SetBool("Slash", true);

        yield return new WaitForSeconds(1f);

        pAttacking = true;

        yield return new WaitForSeconds(0.5f);

        characterAnimator.SetBool("Slash", false);

        pAttacking = false;

        //StartCoroutine(CharacterAttack());
    }

    private void PDie()
    {
        if (pHP <= 0)
        {
            Destroy(this.gameObject);

            Instantiate(explosionParticle, transform.position + (transform.up * 0.2f), Quaternion.identity);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bomb")
        {
            characterState = State.Attack;
        }

        if (other.gameObject.tag == "Bullet")
        {
            pHP -= 10;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Bomb")
        {
            characterState = State.Idle;
        }
    }


}

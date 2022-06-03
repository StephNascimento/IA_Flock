using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    //Pegando o outro c�digo e a uma vari�vel para a velocidade
    public FlockingManager myManager;
    float speed;

    private void Start()
    {
        //Escolhendo a velocidade do peixe entre o m�nimo e o m�ximo, que est� no FlockingManager
        speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
    }

    private void Update()
    {
        //Pegando um m�todo para aplicar regras
        ApplyRules();
        //Faz a movimenta��o do peixe
        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    void ApplyRules()
    {
        //Um array que puxa outro array
        GameObject[] gos;
        gos = myManager.allFish;

        //Vari�veis para definir o centro, velocidade, dist�ncia e o tamanho do grupo
        Vector3 vCentre = Vector3.zero;
        Vector3 vAvoid = Vector3.zero;
        float gSpeed = 0.01f;
        float nDistance;
        int groupSize = 0;
        
        foreach(GameObject go in gos)
        {
            //Se go for diferente desse gameobject
            if(go != this.gameObject)
            {
                nDistance = Vector3.Distance(go.transform.position, this.transform.position);

                //Dist�ncia for menor que a dist�ncia da vizinhan�a
                if(nDistance <= myManager.neighbourDistance)
                {
                    vCentre += go.transform.position;
                    groupSize++;

                    //Dist�ncia menor que 1 faz dispesar
                    if(nDistance < 1.0f)
                    {
                        vAvoid = vAvoid + (this.transform.position - go.transform.position);
                    }

                    //Pegando outro flock e setando a velocidade
                    Flock anotherFlock  = go.GetComponent<Flock>();
                    gSpeed = gSpeed + anotherFlock.speed;
                }
            }
        }

        if(groupSize > 0)
        {
            //Divindido o centro e a velocidade pelo tamanho do grupo
            vCentre = vCentre / groupSize;
            speed = gSpeed / groupSize;

            speed = Mathf.Clamp(speed, myManager.minSpeed, myManager.maxSpeed);

            Vector3 direction = (vCentre + vAvoid) - transform.position;

            //A dire��o for diferente de zero ele define a rota��o do peixe
            if(direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), myManager.rotationSpeed * Time.deltaTime);
            }
        }
    }
}

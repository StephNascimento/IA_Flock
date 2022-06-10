using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    //Pegando o outro código e a uma variável para a velocidade
    public FlockingManager myManager;
    float speed;
    bool turning = false;

    private void Start()
    {
        //Escolhendo a velocidade do peixe entre o mínimo e o máximo, que está no FlockingManager
        speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
    }

    private void Update()
    {
        //Limitando a posição do FlockManager e o limites da natação
        Bounds b = new Bounds(myManager.transform.position, myManager.swinLimits * 2);
        RaycastHit hit = new RaycastHit();
        Vector3 direction = myManager.transform.position - transform.position;

        //Se diferente de bounds com contains ele tornará o turning verdadeiro
        //E ajustará a direção para a mesma do manager menos a posição dele
        if (!b.Contains(transform.position))
        {
            turning = true;
            direction = myManager.transform.position - transform.position;
        }

        //Criando a uma linha de detecção para refletir a direção
        else if(Physics.Raycast(transform.position, this.transform.forward * 50, out hit))
        {
            turning = true;
            direction = Vector3.Reflect(this.transform.forward, hit.normal);
        }

        //Tornando o turning para falso
        else
        {
            turning = false;
        }

        //Se o turning for verdadeiro, irá setar a rotação do peixe
        if (turning)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), myManager.rotationSpeed * Time.deltaTime);
        }

        else
        {
            //Alterando a velocidade aleatoriamente se for menor que 10
            if(Random.Range(0, 100) < 10)
            {
                speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
            }

            //Se menor que 20 vai aplicar o método ApplyRules
            if(Random.Range(0, 100) < 20)
            {
                ApplyRules();
            }

            transform.Translate(0,0,Time.deltaTime * speed);
        }

        //Pegando um método para aplicar regras
        ApplyRules();
        //Faz a movimentação do peixe
        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    void ApplyRules()
    {
        //Um array que puxa outro array
        GameObject[] gos;
        gos = myManager.allFish;

        //Variáveis para definir o centro, velocidade, distância e o tamanho do grupo
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

                //Distância for menor que a distância da vizinhança
                if(nDistance <= myManager.neighbourDistance)
                {
                    vCentre += go.transform.position;
                    groupSize++;

                    //Distância menor que 1 faz dispesar
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
            //Calculando o goalPos menos a posição do peixe
            vCentre = vCentre / groupSize + (myManager.goalPos - this.transform.position);
            speed = gSpeed / groupSize;

            speed = Mathf.Clamp(speed, myManager.minSpeed, myManager.maxSpeed);

            Vector3 direction = (vCentre + vAvoid) - transform.position;

            //A direção for diferente de zero ele define a rotação do peixe
            if(direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), myManager.rotationSpeed * Time.deltaTime);
            }
        }
    }
}

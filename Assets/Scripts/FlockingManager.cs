using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingManager : MonoBehaviour
{
    //Pegando o prefab do peixe e definindo a quantidade e dist�ncia entre eles
    public GameObject fishPrefab;
    public int numFish = 20;
    public GameObject[] allFish;
    public Vector3 swinLimits = new Vector3(5, 5, 5);

    //COnfigura��es para escolher a velocidade m�nima e m�xima entre os peixes
    [Header("Configura��es do Cardume")]
    [Range(0.0f, 5.0f)]
    public float minSpeed;
    [Range(0.0f,5.0f)]
    public float maxSpeed;

    //Dist�ncia entre os vizinhos e velocidade da rota��o
    [Range(1.0f, 50.0f)]
    public float neighbourDistance;
    [Range(1.0f, 5.0f)]
    public float rotationSpeed;

    private void Start()
    {
        allFish = new GameObject[numFish];

        for(int i = 0; i < numFish; i++)
        {
            //Definindo a dire��o em que os prefab ir�o se movimentar
            Vector3 pos =  this.transform.position + new Vector3(Random.Range(-swinLimits.x,swinLimits.x), 
                Random.Range(-swinLimits.y, swinLimits.y), Random.Range(-swinLimits.z,swinLimits.z));

            //Inst�nciando os outros peixes na cena
            allFish[i] = (GameObject)Instantiate(fishPrefab, pos, Quaternion.identity);
            //Setando o c�digo Flock no manager
            allFish[i].GetComponent<Flock>().myManager = this;
        }
    }
}

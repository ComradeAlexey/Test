using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quadcopter : MonoBehaviour {
    public Rigidbody[] rigidbodies;//компоненты физики
    public QuadRotor[] quadRotors;//классы роторов
    void Start () {
        for (int i = 0; i < 4; i++)
        {//присваиваем в цикле значения переменным ротора
            quadRotors[i].motor = rigidbodies[i];//задаём движок
            quadRotors[i].quadcopter = this;//задаём ссылку на класс коптера
            quadRotors[i].transformQuad = this.transform;//задаём текущий трансформ
        }
       
	}
}
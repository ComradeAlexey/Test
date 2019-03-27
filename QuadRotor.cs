using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuadRotor : MonoBehaviour {
    public Rigidbody motor;//компонент физики, по факту - мотор
    public Quadcopter quadcopter;//класс коптера
    public Transform transformQuad;//трансформ коптера
    public float speed;//скорость мотора
    public float pitch;//тангаж, если больше 0 то передние двигатели гасим, задние увеличиваем, иначе наоборот.
    public float roll;//крен
    public float yaw;//рысканье
    public Vector2 directionMotor;//направление двигателя, х - лево(-1) право(1), у - перед(1) зад(-1)
    public Vector3 vectorRotate;//ограничитель поворота двигателя
    private void Update()
    {
        //ограничение мотора во время поворота
        if ((transformQuad.rotation.eulerAngles.x > vectorRotate.x - 1 & transformQuad.rotation.eulerAngles.x < vectorRotate.x + 1) || (transformQuad.rotation.eulerAngles.x < (360 - (vectorRotate.x - 1)) & transformQuad.rotation.eulerAngles.x > (360 - (vectorRotate.x + 1))))
            pitch = 0;
        if ((transformQuad.rotation.eulerAngles.z > vectorRotate.z - 1 & transformQuad.rotation.eulerAngles.z < vectorRotate.z + 1) || (transformQuad.rotation.eulerAngles.z < (360 - (vectorRotate.z - 1)) & transformQuad.rotation.eulerAngles.z > (360 - (vectorRotate.z + 1))))
            roll = 0;
        if ((transformQuad.rotation.eulerAngles.y > vectorRotate.y - 1 & transformQuad.rotation.eulerAngles.y < vectorRotate.y + 1) || (transformQuad.rotation.eulerAngles.y < (360 - (vectorRotate.y - 1)) & transformQuad.rotation.eulerAngles.y > (360 - (vectorRotate.y + 1))))
            yaw = 0;
    }
    // Update is called once per frame
    void FixedUpdate () {
        //если тангаж больше 0 то происходит тангаж(логично вроде бы)
        if (pitch != 0 & roll == 0 & yaw == 0)
            motor.velocity = new Vector3(0, directionMotor.y  * pitch , 0);//задаём силу тяги с помощью направления потора по y и умножаем это дело на переменную pitch
        else if (pitch == 0 & roll != 0 & yaw == 0)//тоже что и выше
            motor.velocity = new Vector3(0, directionMotor.x * roll, 0);//тоже самое что и выше, только вместо у - х, а вместо pitch - roll
        else if(pitch != 0 & roll != 0 || yaw != 0)
        {//если происходит одновременно и тангаж, и крен, или рысканье, то выполняем то что тут
            float pitchSpeed = directionMotor.y * pitch;//определяем скорость тангажа, тоже что и выше, просто в отдельную переменную записываем
            float rollSpeed = directionMotor.x * roll;//смотри выше
            motor.velocity = new Vector3(0, pitchSpeed + rollSpeed/* + speed*/, 0);//складываем всё воедино, скорость тангажа и крена + увеличиваем скорость мотора на speed
            
            motor.angularVelocity = new Vector3(0, yaw * 100, 0);//задаём силу рысканья мотора
            
        }
        else if (pitch == 0 & roll == 0 & yaw != 0)
        {
            //если только рыскаем то задаём вращение двигателю и вертикальную скорость двигателю
            motor.velocity = new Vector3(0, speed, 0);
            motor.AddRelativeTorque(0, 10000, 0, ForceMode.VelocityChange);
        }
        else//если ничего из вышеназванного не происходит, просто летим вверх
            motor.velocity = new Vector3(0, speed, 0);

        this.GetComponent<Rigidbody>().velocity += this.transform.up/4;
    }
}

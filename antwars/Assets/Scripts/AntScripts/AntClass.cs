using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class AntClass : MonoBehaviourPunCallbacks
{
    public string type;
    public List<string> types;
    public bool selected = false;
    public bool secondClick = false;
    public int _playerClan;
    public float hp;
    float maxHp;
    public int damageRadius = 4;
    public Vector3 futurePosition;
    public Vector3 pastPosition;
    public int attackTimes;
    public int combatMod;
    public MeshRenderer mesh;
    public Slider HealthBarValue;
    [PunRPC]
    public void AssignType(int playerClan)
    {
        _playerClan = playerClan;
        type = types[playerClan];
        maxHp = hp;
        HealthBarValue.maxValue = maxHp;
        HealthBarValue.value = maxHp;
    }
    public void OnMouseDown()
    {
        selected = true;
    }
    [PunRPC]
    public void TakeDamage()
    {
        hp -= 5;
        HealthBarValue.value = hp;
    }
    public void Move(Vector3 targetSquare, int playerClan, float maxDistance, float moveSpeed)
    {
        if (selected == true)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (_playerClan == playerClan)
                {
                    if (secondClick)
                    {
                        if ((targetSquare - transform.position).magnitude > maxDistance)
                        {
                            Vector3 moveDirection = (targetSquare - transform.position).normalized;
                            targetSquare = transform.position + moveDirection * maxDistance;
                        }
                        float time = (targetSquare-transform.position).magnitude/ moveSpeed/5;
                        //selected = false;
                        //secondClick = false;
                        futurePosition = targetSquare;
                        GameManager.instance.AntCheck(this);
                        StartCoroutine(MoveDuration(transform.position, targetSquare, time));
                        transform.LookAt(futurePosition);
                        GameManager.instance.damageTime = time;
                        pastPosition = futurePosition;
                    }
                    else
                    {
                        secondClick = true;
                    }
                }

            }
        }
    }
    IEnumerator MoveDuration(Vector3 beginPos, Vector3 endPos, float time)
    {
        for (float t = 0; t < 1; t += Time.deltaTime / time)
        {
            transform.position = Vector3.Lerp(beginPos, endPos, t);
            yield return null;
        }
    }
}

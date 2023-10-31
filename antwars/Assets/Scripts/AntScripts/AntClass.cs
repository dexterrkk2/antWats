using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class AntClass : MonoBehaviourPunCallbacks
{
    public string type;
    public List<string> types;
    public int damage;
    public bool selected = false;
    public bool secondClick = false;
    public int _playerClan;
    public int hp;
    public int damageRadius = 4;
    public Vector3 futurePosition;
    public int futureDamage;
    public int combatMod;
    public MeshRenderer mesh;
    [PunRPC]
    public void AssignType(int playerClan)
    {
        _playerClan = playerClan;
        type = types[playerClan];
    }
    public void OnMouseDown()
    {
        selected = true;
    }
    public void AssignDamage(int damage)
    {
        futureDamage = damage;
    }
    [PunRPC]
    public void TakeDamage()
    {
        hp -= futureDamage;
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
                        StartCoroutine(MoveDuration(transform.position, targetSquare, time));
                        selected = false;
                        secondClick = false;
                        futurePosition = targetSquare;
                        transform.LookAt(futurePosition);
                        GameManager.instance.damageTime = time;
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

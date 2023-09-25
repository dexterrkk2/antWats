using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class AntClass : MonoBehaviour
{
    public string type;
    public List<string> types;
    public int damage;
    public bool selected = false;
    public bool secondClick = false;
    public int _playerClan;
    public int hp;
    public void AssignType(int playerClan)
    {
        _playerClan = playerClan;
        type = types[playerClan];
    }
    public void OnMouseDown()
    {
        selected = true;
    }
    public void TakeDamage(int damage)
    {
        hp -= damage;
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
                            targetSquare.x = Mathf.Clamp(targetSquare.x, transform.position.x - maxDistance, transform.position.x + maxDistance);
                            targetSquare.z = Mathf.Clamp(targetSquare.z, transform.position.x - maxDistance, transform.position.z + maxDistance);
                        }
                        float time = (targetSquare.magnitude/ moveSpeed/10);
                        Debug.Log(time);
                        StartCoroutine(MoveDuration(transform.position, targetSquare, time));
                        selected = false;
                        secondClick = false;
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

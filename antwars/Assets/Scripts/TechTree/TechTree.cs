using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TechTree : MonoBehaviour
{
    public PlayerController player;
    public List<Skill> skillTree;
    public List<GameObject> buttons;
    public List<TextMeshProUGUI> descriptions;
    public void Start()
    {
        CreateTree();
    }
    public void CreateTree()
    {
        skillTree[0].previousPaid = true;
        float offset = 500;
        for (int i = 0; i < skillTree.Count; i++)
        {
            int right = (i + 1) * 2;
            int left = ((i + 1) * 2) - 1;
            skillTree[i].player = player;
            Vector3 position = new Vector3(offset, 500, 0);
            if(i == 0)
            {
                skillTree[i].position = position;
            }
            if (skillTree.Count > right)
            {
                skillTree[i].right = skillTree[right];
            }
            if (skillTree.Count-1 > left)
            {
                skillTree[i].left = skillTree[left];
            }
            CreateButton(skillTree[i], buttons[i], descriptions[i], i);
        }
    }
    public void CreateButton(Skill skill, GameObject ability, TextMeshProUGUI description, int i)
    {
        Button button = ability.GetComponentInChildren<Button>();
        button.onClick.AddListener(() => skill.ButtonClick());
        skill.colorChange = ability.GetComponent<ColorChange>();
        skill.button = button;
        if (skill.left != null)
        {
            int left = ((i + 1) * 2)-1;
        }
        if (skill.right != null)
        {
            int right = (i + 1) * 2;
        }
        button.name = skill.name;
        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
        {
            buttonText.text = skill.name + " " + skill.cost;
        }
        button.transform.SetParent(transform);
        description.text = skill.description;
    }
}

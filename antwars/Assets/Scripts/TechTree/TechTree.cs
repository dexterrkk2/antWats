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
            skillTree[i].player = player;
            Vector3 position = new Vector3(offset, 500, 0);
            if(i == 0)
            {
                skillTree[i].position = position;
            }
            if (skillTree.Count > ((i + 1) * 2))
            {
                skillTree[i].right = skillTree[(i + 1) * 2];
            }
            if (skillTree.Count-1 > ((i + 1) * 2)-1)
            {
                skillTree[i].left = skillTree[((i + 1) * 2) - 1];
            }
            CreateButton(skillTree[i], buttons[i], descriptions[i]);
        }
    }
    public void CreateButton(Skill skill, GameObject ability, TextMeshProUGUI description)
    {
        Button button = ability.GetComponentInChildren<Button>();
        button.onClick.AddListener(() => skill.ButtonClick());
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

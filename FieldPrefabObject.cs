using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class FieldPrefabObject //generovani prazdnych bilych policek
{
    private int _row;
    private int _column;
    private GameObject _instance;

    public FieldPrefabObject(GameObject instance, int row, int column)
    {
        _instance = instance;
        Row = row;
        Column = column;
    }

    public bool IsChangeAble = true;

    public void ChangeColor(Color color)
    {
        _instance.GetComponent<Image>().color = color;
    }

    public void ChangeColorToGreen()
    {
        ChangeColor(Color.green);
    }

    public void ChangeColorToRed()
    {
        ChangeColor(Color.red);
    }

    public bool TryGetTextByName(string name, out Text text)
    {
        text = null;
        Text[] texts = _instance.GetComponentsInChildren<Text>();
        foreach (var currentText in texts)
        {
            if (currentText.name.Equals(name))
            {
                text = currentText;
                return true;
            }
        }
        return false;
    }

    public int Row { get => _row; set => _row = value; }
    public int Column { get => _column; set => _column = value; }

    public void SetHowerMode()
    {
        ChangeColor(new Color(0.01f, 0.43f, 0.08f));
    }

    public void UnsetHowerMode()
    {
        ChangeColor(Color.white);
    }

    public int Number;

    public void SetNumber(int number)
    {
        if (TryGetTextByName("Value", out Text text))
        {
            Number = number;
            text.text = number.ToString();
            for (int i = 1; i < 10; i++)
            {
                if (TryGetTextByName($"Number_{i}", out Text textNumber))
                {
                    textNumber.text = "";
                }
            }
        }
    }

    public void SetSmallNumber(int number)
    {
        if (TryGetTextByName($"Number_{number}", out Text text))
        {
            text.text = number.ToString();
            if (TryGetTextByName($"Value", out Text textValue))
            {
                textValue.text = "";
            }
        }
    }
}

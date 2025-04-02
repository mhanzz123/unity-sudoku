using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class SudokuObject
{
    public int[,] Values = new int[9,9];

    public void GetGroupIndex(int group, out int startRow, out int startColumn) //Vytvori skupinky 3x3 a jejich index
    {
        startRow = 0;
        startColumn = 0;
        switch (group)
        {
            case 1:
                startRow = 0;
                startColumn = 0;
                break;

            case 2:
                startRow = 0;
                startColumn = 3;
                break;

            case 3:
                startRow = 0;
                startColumn = 6;
                break;

            case 4:
                startRow = 3;
                startColumn = 0;
                break;

            case 5:
                startRow = 3;
                startColumn = 3;
                break;

            case 6:
                startRow = 3;
                startColumn = 6;
                break;

            case 7:
                startRow = 6;
                startColumn = 0;
                break;

            case 8:
                startRow = 6;
                startColumn = 3;
                break;

            case 9:
                startRow = 6;
                startColumn = 6;
                break;
        }
    }

    public bool IsPossibleNumberInPosition(int number, int row, int col) //Zjisti jestli je mozne na urcitou pozici vlozit cislo
    {
        if(IsPossibleNumberInRow(number, row) && IsPossibleNumberInCol(number, col))
        {
            if(IsPossibleNumberInGroup(number, GetGroup(row, col)))
            {
                return true;
            }
        }
        return false;
    }
    
    private int GetGroup(int row, int col) //vytvari skupinky podle radku a sloupcu
    {
        if (row < 3) 
        {
            if(col < 3) { return 1; }
            if (col < 6) { return 2; }
            else { return 3; }
        }
        if (row < 6)
        {
            if (col < 3) { return 4; }
            if (col < 6) { return 5; }
            else { return 6; }
        }
        else
        {
            if (col < 3) { return 7; }
            if (col < 6) { return 8; }
            else { return 9; }
        }
    }

    private bool IsPossibleNumberInRow(int number,int row) //Zjisti jestli muze vlozit cislo do radku(podle pravidel sudoku)
    {
        for(int i = 0; i < 9; i++)
        {
            if (Values[row, i] == number)
            {
                return false;
            }
        }
        return true;
    }


    private bool IsPossibleNumberInCol(int number, int col) //Zjisti jestli muze vlozit cislo do sloupce(podle pravidel sudoku)
    {
        for (int i = 0; i < 9; i++)
        {
            if (Values[i, col] == number)
            {
                return false;
            }
        }
        return true;
    }

    private bool IsPossibleNumberInGroup(int number, int group) //Zjisti jestli muzeme dle platnych pravidel sudoku vlozit cisla do skupin 3x3
    {
        GetGroupIndex(group, out int startRow, out int startColumn);
        for(int row = startRow; row < startRow + 3 ; row++)
        {
            for( int col = startColumn; col < startColumn + 3 ; col++)
            {
                if(Values[row, col] == number)
                {
                    return false;
                }
              
            }
        }
        return true;

    } 

}

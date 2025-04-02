using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SudokuGenerator
{
    public static void CreateSudokuObject(out SudokuObject finalObject, out SudokuObject gameObject)
    {
        _finalSudokuObject = null;
        SudokuObject sudokuObject = new SudokuObject();
        CreateRandomGroups(sudokuObject);

        if (TryToSolve(sudokuObject))
        {
            sudokuObject = _finalSudokuObject;
        }
        else
        {
            throw new System.Exception("N?co Se Pokazilo");
        }
        finalObject = sudokuObject;
        gameObject = RemoveRandomNumbers(sudokuObject);


    }


    private static SudokuObject RemoveRandomNumbers(SudokuObject sudokuObject)
    {
        SudokuObject newSudokuObject = new SudokuObject();
        newSudokuObject.Values = (int[,])sudokuObject.Values.Clone();
        List<Tuple<int, int>> values = GetValues();
        int EndValueIndex = 10;
        if (GameSettings.EazyMiddleHard_Number == 1) { EndValueIndex = 71; }
        if (GameSettings.EazyMiddleHard_Number == 2) { EndValueIndex = 61; }
        bool isFinish = false;
        while (!isFinish)
        {
            int index = Random.Range(0, values.Count);
            var searchedIndex = values[index];


            SudokuObject nextSudokuObject = new SudokuObject();
            nextSudokuObject.Values = (int[,])newSudokuObject.Values.Clone();
            nextSudokuObject.Values[searchedIndex.Item1, searchedIndex.Item2] = 0;

            if (TryToSolve(nextSudokuObject, true))
            {
                newSudokuObject = nextSudokuObject;
            }
            values.RemoveAt(index);

            if (values.Count < EndValueIndex)
            {
                isFinish = true;
            }
        }
        return newSudokuObject;

    }

    private static List<Tuple<int, int>> GetValues()
    {
        List<Tuple<int, int>> values = new List<Tuple<int, int>>();
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                values.Add(new Tuple<int, int>(i, j));
            }
        }
        return values;
    }

    private static SudokuObject _finalSudokuObject;


    private static bool TryToSolve(SudokuObject sudokuObject, bool OnlyOne = false)
    {
        //Najít polí?ka které m?žou být vypln?ny
        if (HasEmptyFieldsToFill(sudokuObject, out int row, out int column, OnlyOne))
        {
            List<int> possibleValues = GetPossibleValues(sudokuObject, row, column);
            foreach (var possibleValue in possibleValues)
            {
                SudokuObject nextSudokuObject = new SudokuObject();
                nextSudokuObject.Values = (int[,])sudokuObject.Values.Clone();
                nextSudokuObject.Values[row, column] = possibleValue;
                if (TryToSolve(nextSudokuObject, OnlyOne))
                {
                    return true;
                }
            }
        }

        //Má prázdné polícka?
        if (HasEmptyFields(sudokuObject))
        {
            return false;
        }
        _finalSudokuObject = sudokuObject;
        return true;


        
    }


    private static bool HasEmptyFields(SudokuObject sudokuObject)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (sudokuObject.Values[i, j] == 0)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private static List<int> GetPossibleValues(SudokuObject sudokuObject, int row, int col)
    {
        List<int> possibleValues = new List<int>();
        for (int value = 1; value < 10; value++)
        {
            if (sudokuObject.IsPossibleNumberInPosition(value, row, col))
            {
                possibleValues.Add(value);
            }
        }
        return possibleValues;
    }


    private static bool HasEmptyFieldsToFill(SudokuObject sudokuObject, out int row, out int col, bool OnlyOne = false)
    {
        row = 0;
        col = 0;
        int amountOfPossibleValues = 10;
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (sudokuObject.Values[i, j] == 0)
                {
                    int currentAmount = GetPossibleAmountOfValues(sudokuObject, i, j);
                    if (currentAmount != 0)
                    {
                        if (currentAmount < amountOfPossibleValues)
                        {
                            amountOfPossibleValues = currentAmount;
                            row = i;
                            col = j;
                        }
                    }

                }
            }
        }
        if (OnlyOne)
        {
            if (amountOfPossibleValues == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        if (amountOfPossibleValues == 10)
        {
            return false;
        }
        return true;
    }

    private static int GetPossibleAmountOfValues(SudokuObject sudokuObject, int row, int col)
    {

        int amount = 0;
        for (int value = 1; value < 10; value++)
        {
            if (sudokuObject.IsPossibleNumberInPosition(value, row, col))
            {
                amount++;
            }
        }
        return amount;
    }


    public static void CreateRandomGroups(SudokuObject sudokuObject)
    {
        List<int> values = new List<int>() { 0, 1, 2 };

        int index = Random.Range(0, values.Count);
        InsertRandomGroup(sudokuObject, 1 + values[index]);
        values.RemoveAt(index);

        index = Random.Range(0, values.Count);
        InsertRandomGroup(sudokuObject, 4 + values[index]);
        values.RemoveAt(index);

        index = Random.Range(0, values.Count);
        InsertRandomGroup(sudokuObject, 7 + values[index]);
    }

    public static void InsertRandomGroup(SudokuObject sudokuObject, int group)
    {
        sudokuObject.GetGroupIndex(group, out int startRow, out int startColumn);
        List<int> values = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        for (int row = startRow; row < startRow + 3; row++)
        {
            for (int col = startColumn; col < startColumn + 3; col++)
            {
                int index = Random.Range(0, values.Count);
                sudokuObject.Values[row, col] = values[index];
                values.RemoveAt(index);

            }
        }

    }

}

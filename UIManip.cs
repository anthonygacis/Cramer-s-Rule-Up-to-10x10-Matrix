using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManip : MonoBehaviour {

    public Dropdown dpMatrix;
    public Button btnGenerate;
    public Button btnSolve;
    public Text textHolder;
    

    int row;
    int column;

    public InputField cellInput;
    InputField clone;
    double[,] origMatrix;
    double[] constantMatrix;

    void Start()
    {
        btnGenerate.onClick.AddListener(() => GenerateMatrix(btnGenerate.GetComponentInChildren<Text>().text));
        btnSolve.onClick.AddListener(() => GenerateMatrix(btnSolve.GetComponentInChildren<Text>().text));
    }
    void GenerateMatrix(string btnText) {
        
        if (btnText == "Generate") {

            foreach (InputField inf in this.GetComponentsInChildren<InputField>())
            {
                DestroyObject(inf.gameObject);
            }

            row = Convert.ToInt32(dpMatrix.captionText.text);
            column = row + 1;
            gameObject.GetComponent<GridLayoutGroup>().constraintCount = column;

            for (int i = 0; i < row * column; i++)
            {
                clone = Instantiate(cellInput, this.transform, false);
                clone.placeholder.GetComponent<Text>().text = i.ToString();
            }

            //Debug.Log(this.GetComponentsInChildren<InputField>().Length);

            
        }else if(btnText == "Solve"){
            origMatrix = new double[row, row];
            constantMatrix = new double[row];
            int r = 0, c = 0, counter = 1;

            foreach (InputField inf in this.GetComponentsInChildren<InputField>())
            {
                if (counter % column == 0)
                {
                    constantMatrix[r] = Convert.ToDouble(inf.text);
                    r++;
                    c = 0;
                }
                else
                {
                    origMatrix[r, c] = Convert.ToDouble(inf.text);
                    c++;
                }
                counter++;
                Debug.Log(inf.text);
            }
            Debug.Log(printArray(origMatrix));


            Main_Script ms = new Main_Script();
            double[,] tmpMatrix = new double[origMatrix.GetLength(0), origMatrix.GetLength(1)];
            double mainDeterminant = ms.compute(origMatrix);
            double[] solve = new double[constantMatrix.GetLength(0)];

            for (int i = 0; i < solve.GetLength(0); i++)
            {
                Array.Copy(swapMatrixConstant(origMatrix, i, constantMatrix), 0, tmpMatrix, 0, tmpMatrix.Length);
                solve[i] = ms.compute(tmpMatrix);
                Debug.Log(solve[i]);
            }

            string output = "Main Determinant: " + mainDeterminant + "\n";
            for (int i = 0; i < tmpMatrix.GetLength(0); i++)
            {
                output += "x" + (i + 1) + " = (" + solve[i] + ")/(" + mainDeterminant + ") = " + Math.Round(solve[i] / mainDeterminant, 2) + "\n";
            }

            string appendOutput = "";

            foreach (string tIn in ms.returnArrayList()) {
                appendOutput += tIn;
            }
            textHolder.GetComponent<Text>().text = output + "\n" + appendOutput;

        }
    }

    string printArray(double[,] tmpArray)
    {
        string temp = "";
        for (int i = 0; i < tmpArray.GetLength(0); i++)
        {
            for (int j = 0; j < tmpArray.GetLength(1); j++)
            {
                temp = temp + tmpArray[i, j] + " ";
            }
            temp = temp + "\n";
        }
        Debug.Log(temp);
        return "";
    }

    public double[,] swapMatrixConstant(double[,] theMatrix, int column, double[] constants)
    {
        double[,] tmpMatrix = new double[theMatrix.GetLength(0), theMatrix.GetLength(1)];
        for (int i = 0; i < theMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < theMatrix.GetLength(1); j++)
            {
                if (j == column)
                {
                    tmpMatrix[i, j] = constants[i];
                }
                else
                {
                    tmpMatrix[i, j] = theMatrix[i, j];
                }
            }
        }

        return tmpMatrix;
    }
}

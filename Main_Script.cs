using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Main_Script {

    public Text textHolder;
    bool globalSwap = false;

    public GameObject inputHolder;

    ArrayList outputMatrix = new ArrayList();

    void Start() {

        //double[,] temp = { { 10, 3, 21, 12, 3 }, { 5, 2, 32, 10, 3 }, { 2, 5, 2, 7, 4 }, { 4, 16, 9, 0, 2 }, { 5, 3, 5, 6, 2 } };
        //double[] constant = { 4, 15, 21, 55, 18 };

        //double[,] tmpMatrix = new double[temp.GetLength(0), temp.GetLength(1)];

        //double mainDeterminant = compute(temp);
        //double[] solve = new double[constant.GetLength(0)];

        //for (int i = 0; i < solve.GetLength(0); i++)
        //{
        //    Array.Copy(swapMatrixConstant(temp, i, constant), 0, tmpMatrix, 0, tmpMatrix.Length);
        //    solve[i] = compute(tmpMatrix);
        //    Debug.Log(solve[i]);
        //}

        //string output = "Main Determinant: " + mainDeterminant + "\n";
        //for (int i = 0; i < tmpMatrix.GetLength(0); i++)
        //{
        //    output += "x" + (i + 1) + " = (" + solve[i] + ")/(" + mainDeterminant + ") = " + Math.Round(solve[i] / mainDeterminant, 2) + "\n";
        //}

        //textHolder.GetComponent<Text>().text = output;
    }

    public double[,] swapMatrixConstant(double[,] theMatrix, int column, double[] constants) {
        double[,] tmpMatrix = new double[theMatrix.GetLength(0), theMatrix.GetLength(1)];
        for (int i = 0; i < theMatrix.GetLength(0); i++) {
            for (int j = 0; j < theMatrix.GetLength(1); j++) {
                if (j == column)
                {
                    tmpMatrix[i, j] = constants[i];
                }
                else {
                    tmpMatrix[i, j] = theMatrix[i, j];
                }
            }
        }

        return tmpMatrix;
    }

    string printArray(double[,] tmpArray)
    {
        string temp = "";
        for (int i = 0; i < tmpArray.GetLength(0); i++)
        {
            for (int j = 0; j < tmpArray.GetLength(1); j++)
            {
                temp = temp + "[" + tmpArray[i, j] + "]" + " ";
            }
            temp = temp + "\n";
        }
        return temp;
    }

    public double compute(double[,] matrix)
    {
        int multiplierSign = 1;
        double[,] holdMatrix = new double[matrix.GetLength(0), matrix.GetLength(0)];
        double[,] tempMatrix = new double[holdMatrix.GetLength(0),holdMatrix.GetLength(1)];
        double[,] restoreMatrix = new double[matrix.GetLength(0), matrix.GetLength(1)];
        int len = matrix.Length;
        Array.Copy(matrix, 0, holdMatrix, 0, len);

        Debug.Log("=========Original Matrix=========");
        printArray(holdMatrix);
        outputMatrix.Add("=========Original Matrix=========" + "\n" + printArray(holdMatrix));
        Debug.Log("=========Original Matrix=========");
        for (int i = 0; i < matrix.GetLength(0); i++) {
            Array.Copy(holdMatrix, 0, restoreMatrix, 0, holdMatrix.Length);

            if (holdMatrix[i, i] != 0)
            {
                //divide the row
                double tmpR = holdMatrix[i, i];
                for (int d = i; d < matrix.GetLength(0); d++)
                {
                    //holdMatrix[i, d] = 0.0;
                    holdMatrix[i, d] = holdMatrix[i, d] / tmpR;
                }
            }
            else {
                Debug.Log("=========Before Swap=========");
                printArray(holdMatrix);
                Debug.Log("=========Before Swap=========");
                Array.Copy(zeroSwap(holdMatrix, i), 0, holdMatrix, 0, holdMatrix.Length);
                Array.Copy(holdMatrix, 0, restoreMatrix, 0, holdMatrix.Length);
                Debug.Log("=========After Swap=========");
                printArray(holdMatrix);
                outputMatrix.Add("=========Swap [0] to [" + i + "]=========" + "\n" + printArray(holdMatrix));
                Debug.Log("=========After Swap=========");
                if (globalSwap) {
                    multiplierSign = multiplierSign * -1;
                    double tmpR = holdMatrix[i, i];
                    for (int d = i; d < matrix.GetLength(0); d++)
                    {
                        //holdMatrix[i, d] = 0.0;
                        holdMatrix[i, d] = holdMatrix[i, d] / tmpR;
                    }
                }
                Array.Copy(holdMatrix, 0, tempMatrix, 0, tempMatrix.Length);
            }

            Debug.Log("=========After Divide=========");
            printArray(holdMatrix);
            outputMatrix.Add("=========Divide " + restoreMatrix[i,i] + " to this row=========" + "\n" + printArray(holdMatrix));
            Debug.Log("=========After Divide=========");

            for (int m = i + 1; m < matrix.GetLength(0); m++) {
                double tM = holdMatrix[m, i];
                for (int ii = i; ii < matrix.GetLength(0); ii++)
                {
                    holdMatrix[m, ii] = holdMatrix[m, ii] - (holdMatrix[i, ii] * tM);
                }
                Debug.Log("=========Multiply and Subtract=========");
                printArray(holdMatrix);
                outputMatrix.Add("=========Multiply " + tM + " to first row and Subtract this row to first row=========" + "\n" + printArray(holdMatrix));
                Debug.Log("=========Multiply and Subtract=========");
            }

            //restore
            for (int r = i; r < matrix.GetLength(0); r++)
            {
                holdMatrix[i, r] = restoreMatrix[i, r];
            }
            
            Debug.Log("=========Restored Matrix=========");
            printArray(holdMatrix);
            outputMatrix.Add("=========Restore the first row to its original value=========" + "\n" + printArray(holdMatrix));
            Debug.Log("=========Restored Matrix=========");
        }

        //multiply the diagonal
        double determinant=1;
        for (int a = 0; a < matrix.GetLength(0); a++) { 
            determinant = determinant * holdMatrix[a,a];
        }
        determinant = determinant * multiplierSign;
        return Math.Round(determinant,2);
    }

    public double[,] zeroSwap(double[,] theMatrix, int currentRow) {
        double[,] tempMatrix = new double[theMatrix.GetLength(0), theMatrix.GetLength(1)];
        Array.Copy(theMatrix, 0, tempMatrix, 0, theMatrix.Length);
        bool flag = false;
        int foundRow = -1;
        //check nearby value which is greater than 0
        for (int r = currentRow+1; r < tempMatrix.GetLength(0); r++) {
            if (tempMatrix[r,currentRow] != 0) {
                flag = true;
                foundRow = r;
                break;
            }
        }
        double temp;
        if (flag) { 
            //perform swap
            for (int c = 0; c < theMatrix.GetLength(1); c++) {
                temp = tempMatrix[currentRow, c];
                tempMatrix[currentRow, c] = tempMatrix[foundRow, c];
                tempMatrix[foundRow, c] = temp;
            }
        }
        globalSwap = flag;
        return tempMatrix;
    }

    public ArrayList returnArrayList() {
        return outputMatrix;
    }
}

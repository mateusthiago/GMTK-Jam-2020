using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Primos : MonoBehaviour
{    
    public Text label;
    public InputField inputText;
    public int input;
    
    List<int> listPrimos = new List<int>();

    public void Calcula()
    {
        listPrimos.Clear();
        input = int.Parse(inputText.text);
        if (input <= 1) label.text = "BOTA NUMERO MAIOR QUE 1, PORRA";
        else
        {
            listPrimos.Add(2);
            for (int i = 3; i <= input; i++)
            {
                bool isPrimo = true;

                for (int j = 0; j < listPrimos.Count; j++)
                {
                    if (i % listPrimos[j] == 0)
                    {
                        isPrimo = false;
                        break;
                    }
                }

                if (isPrimo) listPrimos.Add(i);
            }
        }

        if (listPrimos.Count > 0)
        {
            StringBuilder str = new StringBuilder("TOMA SEUS PRIMO\n\n");
            foreach (int primo in listPrimos)
            {
                str.Append(primo + "    ");
            }
            label.text = str.ToString();
        }

    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using KModkit;


public class FiveLetterWords : MonoBehaviour
{

    public KMAudio Audio;
    public KMBombInfo Bomb;
    public KMBombModule Module;

    public AudioClip[] SFX;

    public KMSelectable[] Selectables;
    public GameObject[] Disabler;

    public TextMesh[] WordDex;
    public TextAsset FiverData;
    private bool Playable = false;

    private int[] TheValues = { 0, 0, 0 };

    // Logging
    static int moduleIdCounter = 1;
    int moduleId;
    private bool ModuleSolved;

    void Awake()
    {
        moduleId = moduleIdCounter++;
        Selectables[0].OnInteract += delegate () { Number0(); return false; };
        Selectables[1].OnInteract += delegate () { Number1(); return false; };
        Selectables[2].OnInteract += delegate () { Number2(); return false; };
    }

    void Start()
    {
        Module.OnActivate += BombAnswer;
    }

    void BombAnswer()
    {
        Playable = true;
        string[] TheAnswer = JsonConvert.DeserializeObject<string[]>(FiverData.text).Shuffle();
        int Stent = 0;
        for (int i = 0; i < 3; i++)
        {
            WordDex[Stent].text = TheAnswer[Stent];
            int Gnomon = 0;
            for (int a = 0; a < 5; a++)
            {
                if (Bomb.GetIndicators().Count() > 3 || Bomb.GetPortCount() > 3 || Bomb.GetBatteryCount() > 3 || Bomb.GetPortPlates().Count() > 3)
                {
                    if (TheAnswer[Stent][Gnomon].ToString() == "E" || TheAnswer[Stent][Gnomon].ToString() == "S" || TheAnswer[Stent][Gnomon].ToString() == "X")
                    {
                        TheValues[Stent] = TheValues[Stent] + 1;
                    }

                    else if (TheAnswer[Stent][Gnomon].ToString() == "A" || TheAnswer[Stent][Gnomon].ToString() == "K" || TheAnswer[Stent][Gnomon].ToString() == "N")
                    {
                        TheValues[Stent] = TheValues[Stent] + 2;
                    }

                    else if (TheAnswer[Stent][Gnomon].ToString() == "C" || TheAnswer[Stent][Gnomon].ToString() == "H" || TheAnswer[Stent][Gnomon].ToString() == "V" || TheAnswer[Stent][Gnomon].ToString() == "Z")
                    {
                        TheValues[Stent] = TheValues[Stent] + 3;
                    }

                    else if (TheAnswer[Stent][Gnomon].ToString() == "T" || TheAnswer[Stent][Gnomon].ToString() == "Y")
                    {
                        TheValues[Stent] = TheValues[Stent] + 4;
                    }

                    else if (TheAnswer[Stent][Gnomon].ToString() == "F" || TheAnswer[Stent][Gnomon].ToString() == "P" || TheAnswer[Stent][Gnomon].ToString() == "R" || TheAnswer[Stent][Gnomon].ToString() == "W")
                    {
                        TheValues[Stent] = TheValues[Stent] + 5;
                    }

                    else if (TheAnswer[Stent][Gnomon].ToString() == "B" || TheAnswer[Stent][Gnomon].ToString() == "G" || TheAnswer[Stent][Gnomon].ToString() == "L")
                    {
                        TheValues[Stent] = TheValues[Stent] + 6;
                    }

                    else if (TheAnswer[Stent][Gnomon].ToString() == "I")
                    {
                        TheValues[Stent] = TheValues[Stent] + 7;
                    }

                    else if (TheAnswer[Stent][Gnomon].ToString() == "D" || TheAnswer[Stent][Gnomon].ToString() == "J" || TheAnswer[Stent][Gnomon].ToString() == "M")
                    {
                        TheValues[Stent] = TheValues[Stent] + 8;
                    }

                    else
                    {
                        TheValues[Stent] = TheValues[Stent] + 9;
                    }

                }

                else
                {
                    if (TheAnswer[Stent][Gnomon].ToString() == "F" || TheAnswer[Stent][Gnomon].ToString() == "K" || TheAnswer[Stent][Gnomon].ToString() == "Y")
                    {
                        TheValues[Stent] = TheValues[Stent] + 1;
                    }

                    else if (TheAnswer[Stent][Gnomon].ToString() == "N" || TheAnswer[Stent][Gnomon].ToString() == "W" || TheAnswer[Stent][Gnomon].ToString() == "X" || TheAnswer[Stent][Gnomon].ToString() == "Y")
                    {
                        TheValues[Stent] = TheValues[Stent] + 2;
                    }

                    else if (TheAnswer[Stent][Gnomon].ToString() == "H" || TheAnswer[Stent][Gnomon].ToString() == "L" || TheAnswer[Stent][Gnomon].ToString() == "Q")
                    {
                        TheValues[Stent] = TheValues[Stent] + 3;
                    }

                    else if (TheAnswer[Stent][Gnomon].ToString() == "A" || TheAnswer[Stent][Gnomon].ToString() == "C")
                    {
                        TheValues[Stent] = TheValues[Stent] + 4;
                    }

                    else if (TheAnswer[Stent][Gnomon].ToString() == "G" || TheAnswer[Stent][Gnomon].ToString() == "S" || TheAnswer[Stent][Gnomon].ToString() == "U")
                    {
                        TheValues[Stent] = TheValues[Stent] + 5;
                    }

                    else if (TheAnswer[Stent][Gnomon].ToString() == "D" || TheAnswer[Stent][Gnomon].ToString() == "I" || TheAnswer[Stent][Gnomon].ToString() == "V")
                    {
                        TheValues[Stent] = TheValues[Stent] + 6;
                    }

                    else if (TheAnswer[Stent][Gnomon].ToString() == "J" || TheAnswer[Stent][Gnomon].ToString() == "M" || TheAnswer[Stent][Gnomon].ToString() == "O")
                    {
                        TheValues[Stent] = TheValues[Stent] + 7;
                    }

                    else if (TheAnswer[Stent][Gnomon].ToString() == "B" || TheAnswer[Stent][Gnomon].ToString() == "P" || TheAnswer[Stent][Gnomon].ToString() == "R")
                    {
                        TheValues[Stent] = TheValues[Stent] + 8;
                    }

                    else
                    {
                        TheValues[Stent] = TheValues[Stent] + 9;
                    }
                }
                Gnomon++;
            }
            Stent++;
        }

        if ((TheValues[0] == TheValues[1]) || (TheValues[0] == TheValues[2]) || (TheValues[1] == TheValues[2]))
        {
            TheValues[0] = 0; TheValues[1] = 0; TheValues[2] = 0;
            BombAnswer();
        }

        else
        {
            Debug.LogFormat("[Five Letter Words #{0}] The word scores are: {1}", moduleId, string.Join(", ", TheValues.Select(x => x.ToString()).ToArray()));
        }
    }

    void Number0()
    {
        Audio.PlaySoundAtTransform(SFX[0].name, transform);
        if ((TheValues[0] > TheValues[1]) && (TheValues[0] > TheValues[2]))
        {
            if (TheValues[1] < TheValues[2])
            {
                if (((int)Bomb.GetTime()) % 60 == TheValues[1])
                {
                    StartCoroutine(RouletteCheck());
                }

                else
                {
                    StartCoroutine(RouletteWrong());
                }
            }

            else if (TheValues[2] < TheValues[1])
            {
                if (((int)Bomb.GetTime()) % 60 == TheValues[2])
                {
                    StartCoroutine(RouletteCheck());
                }

                else
                {
                    StartCoroutine(RouletteWrong());
                }
            }
        }

        else
        {
            StartCoroutine(RouletteWrong());
        }
    }

    void Number1()
    {
        Audio.PlaySoundAtTransform(SFX[0].name, transform);
        if ((TheValues[1] > TheValues[0]) && (TheValues[1] > TheValues[2]))
        {
            if (TheValues[0] < TheValues[2])
            {
                if (((int)Bomb.GetTime()) % 60 == TheValues[0])
                {
                    StartCoroutine(RouletteCheck());
                }

                else
                {
                    StartCoroutine(RouletteWrong());
                }
            }

            else if (TheValues[2] < TheValues[0])
            {
                if (((int)Bomb.GetTime()) % 60 == TheValues[2])
                {
                    StartCoroutine(RouletteCheck());
                }

                else
                {
                    StartCoroutine(RouletteWrong());
                }
            }
        }

        else
        {
            StartCoroutine(RouletteWrong());
        }
    }

    void Number2()
    {
        Audio.PlaySoundAtTransform(SFX[0].name, transform);
        if ((TheValues[2] > TheValues[0]) && (TheValues[2] > TheValues[1]))
        {
            if (TheValues[0] < TheValues[1])
            {
                if (((int)Bomb.GetTime()) % 60 == TheValues[0])
                {
                    StartCoroutine(RouletteCheck());
                }

                else
                {
                    StartCoroutine(RouletteWrong());
                }
            }

            else if (TheValues[1] < TheValues[0])
            {
                if (((int)Bomb.GetTime()) % 60 == TheValues[1])
                {
                    StartCoroutine(RouletteCheck());
                }

                else
                {
                    StartCoroutine(RouletteWrong());
                }
            }
        }

        else
        {
            StartCoroutine(RouletteWrong());
        }
    }

    IEnumerator RouletteCheck()
    {
        int Hell = 0;
        for (int q = 0; q < 3; q++)
        {
            Disabler[Hell].SetActive(false);
            Hell++;
        }

        string[] TheAnswer = JsonConvert.DeserializeObject<string[]>(FiverData.text).Shuffle();
        for (int d = 0; d < 100; d++)
        {
            int Coal = 0;
            for (int f = 0; f < 3; f++)
            {
                int Celve = UnityEngine.Random.Range(0, TheAnswer.Length);
                WordDex[Coal].text = TheAnswer[Celve];
                Coal++;
            }
            yield return new WaitForSecondsRealtime(0.01f);
        }

        int Chill = 0;
        for (int m = 0; m < 3; m++)
        {
            WordDex[Chill].text = "YES!";
            WordDex[Chill].color = Color.green;
            Chill++;
        }
        Module.HandlePass();
        Audio.PlaySoundAtTransform(SFX[2].name, transform);
    }

    IEnumerator RouletteWrong()
    {
        int Hell = 0;
        for (int q = 0; q < 3; q++)
        {
            Disabler[Hell].SetActive(false);
            Hell++;
        }

        string[] TheAnswer = JsonConvert.DeserializeObject<string[]>(FiverData.text).Shuffle();
        for (int d = 0; d < 100; d++)
        {
            int Coal = 0;
            for (int f = 0; f < 3; f++)
            {
                int Celve = UnityEngine.Random.Range(0, TheAnswer.Length);
                WordDex[Coal].text = TheAnswer[Celve];
                Coal++;
            }
            yield return new WaitForSecondsRealtime(0.01f);
        }

        int Chill = 0;
        for (int m = 0; m < 3; m++)
        {
            WordDex[Chill].text = "NO!";
            WordDex[Chill].color = Color.red;
            Chill++;
        }

        Audio.PlaySoundAtTransform(SFX[1].name, transform);
        yield return new WaitForSecondsRealtime(0.5f);

        Module.HandleStrike();

        int Chili = 0;
        for (int m = 0; m < 3; m++)
        {
            WordDex[Chili].color = Color.white;
            Chili++;
        }

        string[] TheAnswerGenerates = JsonConvert.DeserializeObject<string[]>(FiverData.text).Shuffle();
        for (int d = 0; d < 50; d++)
        {
            int Cola = 0;
            for (int f = 0; f < 3; f++)
            {
                int Celvo = UnityEngine.Random.Range(0, TheAnswerGenerates.Length);
                WordDex[Cola].text = TheAnswerGenerates[Celvo];
                Cola++;
            }
            yield return new WaitForSecondsRealtime(0.01f);
        }

        int Heaven = 0;
        for (int q = 0; q < 3; q++)
        {
            Disabler[Heaven].SetActive(true);
            Heaven++;
        }

        TheValues[0] = 0; TheValues[1] = 0; TheValues[2] = 0;
        BombAnswer();
    }
}

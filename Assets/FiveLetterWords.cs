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
    public KMRuleSeedable RuleSeed;

    public AudioClip[] SFX;

    public KMSelectable[] Selectables;
    public GameObject[] Disabler;

    public TextMesh[] WordDex;
    public TextAsset FiverData;

    private int[] TheValues = { 0, 0, 0 };
    private string[] TheNames = { "", "", "" };
    private bool StrikeIncoming = false;
    private bool SolveIncoming = false;

    string[] RegularAlphabet = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
    int[] ConditionMet = { 2, 6, 3, 8, 1, 5, 6, 3, 7, 8, 2, 6, 8, 2, 9, 5, 9, 5, 1, 4, 9, 3, 5, 1, 4, 3 };
    int[] ConditionNotMet = { 4, 8, 4, 6, 9, 1, 5, 3, 6, 7, 1, 3, 7, 2, 7, 8, 3, 8, 5, 9, 5, 6, 2, 2, 1, 2 };

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
        var RuleSeedRNG = RuleSeed.GetRNG();
        Debug.LogFormat("[Five Letter Words #{0}] Ruleseed Number: {1}", moduleId, RuleSeedRNG.Seed.ToString());
        if (RuleSeedRNG.Seed != 1)
        {
            int[] MustHave = Enumerable.Range(0, 10).ToArray();
            int[] OtherCopies = { 0, 0, 0, 1, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4, 4, 5, 5, 5, 6, 6, 6, 7, 7, 7, 8, 8, 8, 9, 9, 9 };
            int[] RandomPlacement = Enumerable.Range(0, 26).ToArray();
            for (int y = 0; y < 2; y++)
            {
                RuleSeedRNG.ShuffleFisherYates(MustHave);
                RuleSeedRNG.ShuffleFisherYates(OtherCopies);
                RuleSeedRNG.ShuffleFisherYates(RandomPlacement);
                if (y == 0)
                {
                    for (int x = 0; x < ConditionMet.Length; x++)
                    {
                        if (x < 10)
                        {
                            ConditionMet[RandomPlacement[x]] = MustHave[x];
                        }

                        else
                        {
                            ConditionMet[RandomPlacement[x]] = OtherCopies[x - 10];
                        }
                    }
                }

                else
                {
                    for (int x = 0; x < ConditionNotMet.Length; x++)
                    {
                        if (x < 10)
                        {
                            ConditionNotMet[RandomPlacement[x]] = MustHave[x];
                        }

                        else
                        {
                            ConditionNotMet[RandomPlacement[x]] = OtherCopies[x - 10];
                        }
                    }
                }
            }  
        }
        BombAnswer();
    }

    void BombAnswer()
    {
        string[] TheAnswer = JsonConvert.DeserializeObject<string[]>(FiverData.text).Shuffle();
        do
		{
            TheAnswer.Shuffle();
            TheValues = new int[] { 0, 0, 0 };
			for (int i = 0; i < 3; i++)
			{
                TheNames[i] = TheAnswer[i];
				WordDex[i].text = TheAnswer[i];
				for (int j = 0; j < 5; j++)
				{
					if (Bomb.GetIndicators().Count() > 3 || Bomb.GetPortCount() > 3 || Bomb.GetBatteryCount() > 3 || Bomb.GetPortPlates().Count() > 3)
					{
                        TheValues[i] = TheValues[i] + ConditionMet[Array.IndexOf(RegularAlphabet, TheAnswer[i][j].ToString())];
					}

					else
					{
                        TheValues[i] = TheValues[i] + ConditionNotMet[Array.IndexOf(RegularAlphabet, TheAnswer[i][j].ToString())];

                    }
				}
			}
		}
		while ((TheValues[0] == TheValues[1]) || (TheValues[0] == TheValues[2]) || (TheValues[1] == TheValues[2]));

		Debug.LogFormat("[Five Letter Words #{0}] The words are: {1}", moduleId, string.Join(", ", TheNames.Select(x => x.ToString()).ToArray()));
		Debug.LogFormat("[Five Letter Words #{0}] The word scores are: {1}", moduleId, string.Join(", ", TheValues.Select(x => x.ToString()).ToArray()));
    }

    void Number0()
    {
        if (!Disabler[0].activeSelf) return;
        Debug.LogFormat("[Five Letter Words #{0}] You pressed {1} when the last digits of the bomb were {2}", moduleId, TheNames[0], (((int)Bomb.GetTime()) % 60).ToString());
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
        if (!Disabler[0].activeSelf) return;
        Debug.LogFormat("[Five Letter Words #{0}] You pressed {1} when the last digits of the bomb were {2}", moduleId, TheNames[1], (((int)Bomb.GetTime()) % 60).ToString());
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
        if (!Disabler[0].activeSelf) return;
        Debug.LogFormat("[Five Letter Words #{0}] You pressed {1} when the last digits of the bomb were {2}", moduleId, TheNames[2], (((int)Bomb.GetTime()) % 60).ToString());
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
        SolveIncoming = true;
        foreach (GameObject d in Disabler)
            d.SetActive(false);

        string[] TheAnswer = JsonConvert.DeserializeObject<string[]>(FiverData.text).Shuffle();
        for (int i = 0; i < 100; i++)
        {
            foreach (TextMesh t in WordDex)
                t.text = TheAnswer.PickRandom();
            yield return new WaitForSecondsRealtime(0.01f);
        }

        foreach (TextMesh t in WordDex)
        {
            t.text = "YES!";
            t.color = Color.green;
        }
        Module.HandlePass();
        ModuleSolved = true;
        SolveIncoming = false;
        Audio.PlaySoundAtTransform(SFX[2].name, transform);
        Debug.LogFormat("[Five Letter Words #{0}] Correct! Module solved.", moduleId);
    }

    IEnumerator RouletteWrong()
    {
        StrikeIncoming = true;
        foreach (GameObject d in Disabler)
            d.SetActive(false);

        string[] TheAnswer = JsonConvert.DeserializeObject<string[]>(FiverData.text).Shuffle();
        for (int i = 0; i < 100; i++)
        {
            foreach (TextMesh t in WordDex)
                t.text = TheAnswer.PickRandom();
            yield return new WaitForSecondsRealtime(0.01f);
        }

        foreach (TextMesh t in WordDex)
        {
            t.text = "NO!";
            t.color = Color.red;
        }

        Audio.PlaySoundAtTransform(SFX[1].name, transform);
        yield return new WaitForSecondsRealtime(0.5f);

        Module.HandleStrike();
        StrikeIncoming = false;
        Debug.LogFormat("[Five Letter Words #{0}] Incorrect! Strike! Module resetting...", moduleId);

        foreach (TextMesh t in WordDex)
        {
            t.color = Color.white;
        }

        string[] TheAnswerGenerates = JsonConvert.DeserializeObject<string[]>(FiverData.text).Shuffle();
        for (int i = 0; i < 50; i++)
        {
            foreach (TextMesh t in WordDex)
                t.text = TheAnswerGenerates.PickRandom();
            yield return new WaitForSecondsRealtime(0.01f);
        }

        foreach (GameObject d in Disabler)
            d.SetActive(true);

        TheValues[0] = 0; TheValues[1] = 0; TheValues[2] = 0;
        BombAnswer();
    }

    // Twitch Plays
    #pragma warning disable 414
    private readonly string TwitchHelpMessage = "!{0} press <top/middle/bottom> <#> [Presses the button in that position when the last two digits of the timer are #.]";
    #pragma warning restore 414

    IEnumerator ProcessTwitchCommand(string input)
    {
        var cmd = input.ToLowerInvariant().Split(' ').ToArray();
        if (cmd.Length != 3)
            yield break;
        var numbers = Enumerable.Range(0, 60).Select(x => x.ToString()).ToArray();
        var positions = new string[] { "top", "middle", "bottom" };
        if (cmd[0] != "press" || !positions.Contains(cmd[1]) || !numbers.Contains(cmd[2]))
            yield break;
        while ((((int)Bomb.GetTime()) % 60) == Array.IndexOf(numbers, cmd[2]))
            yield return "trycancel The command to perform the action was cancelled due to a cancel request.";
        while ((((int)Bomb.GetTime()) % 60) != Array.IndexOf(numbers, cmd[2]))
            yield return "trycancel The command to perform the action was cancelled due to a cancel request.";
        yield return null;
        yield return "solve";
        yield return "strike";
        Selectables[Array.IndexOf(positions, cmd[1])].OnInteract();
    }

    IEnumerator TwitchHandleForcedSolve()
    {
        if (StrikeIncoming)
        {
            StopAllCoroutines();
            foreach (TextMesh t in WordDex)
            {
                t.text = "YES!";
                t.color = Color.green;
            }
            Module.HandlePass();
            ModuleSolved = true;
            Audio.PlaySoundAtTransform(SFX[2].name, transform);
            yield break;
        }
        if (!SolveIncoming)
        {
            while (!Disabler[0].activeSelf) yield return true;
            var mx = Array.IndexOf(TheValues, TheValues.Max());
            var mn = TheValues.Min();
            while ((((int)Bomb.GetTime()) % 60) != mn)
                yield return true;
            Selectables[mx].OnInteract();
        }
        while (!ModuleSolved)
            yield return true;
    }
}

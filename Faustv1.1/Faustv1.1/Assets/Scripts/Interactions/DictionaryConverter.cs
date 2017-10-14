using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DictionaryConverter : MonoBehaviour
{
    public Dialogue dialogue;
    public Dictionary<string, string> sentences_Siebel = new Dictionary<string, string>();
    public Dictionary<string, string> sentences_Frosch = new Dictionary<string, string>();
    public Dictionary<string, string> responses_Siebel = new Dictionary<string, string>();
    public Dictionary<string, string> responses_Frosch = new Dictionary<string, string>();
    //public Dictionary<string, string>[] dialogueArchives;

    void Awake()
    {
        Dialogue siebelDialogue = JsonUtility.FromJson<Dialogue>(File.ReadAllText(Application.streamingAssetsPath + "/SiebelDialogue.json"));
        string[] dSTags_Siebel = siebelDialogue.tags_NPC;
        string[] dRTags_Siebel = siebelDialogue.tags_Usr;
        string[] dSentences_Siebel = siebelDialogue.Sentences;
        string[] dResponses_Siebel = siebelDialogue.Responses;

        Dialogue froschDialogue = JsonUtility.FromJson<Dialogue>(File.ReadAllText(Application.streamingAssetsPath + "/FroschDialogue.json"));
        string[] dSTags_Frosch = froschDialogue.tags_NPC;
        string[] dRTags_Frosch = froschDialogue.tags_Usr;
        string[] dSentences_Frosch = froschDialogue.Sentences;
        string[] dResponses_Frosch = froschDialogue.Responses;

        for (int i = 0; i < dSTags_Siebel.Length; i++)
        {
            string indexedKey = dSTags_Siebel[i];
            string indexedElem = dSentences_Siebel[i];
            sentences_Siebel.Add(indexedKey, indexedElem);
        }
        for (int i = 0; i < dRTags_Siebel.Length; i++)
        {
            string indexedKey = dRTags_Siebel[i];
            string indexedElem = dResponses_Siebel[i];
            responses_Siebel.Add(indexedKey, indexedElem);
        }
        for (int i = 0; i < dSTags_Frosch.Length; i++)
        {
            string indexedKey = dSTags_Frosch[i];
            string indexedElem = dSentences_Frosch[i];
            sentences_Frosch.Add(indexedKey, indexedElem);
        }
        for (int i = 0; i < dRTags_Frosch.Length; i++)
        {
            string indexedKey = dRTags_Frosch[i];
            string indexedElem = dResponses_Frosch[i];
            responses_Frosch.Add(indexedKey, indexedElem);
        }

    }
}


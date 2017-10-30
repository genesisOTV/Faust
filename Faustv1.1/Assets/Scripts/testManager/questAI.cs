using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class questAI : MonoBehaviour, IDialogue {
	public IqBase AI_qLoader;
	public bool qCompleted;

    string jsonString;
    string path;
    public string fileName;

    public Dialogue ReadDialogue()
    {
        path = Application.streamingAssetsPath + "/" + fileName + ".json";
        jsonString = File.ReadAllText(path);
        Dialogue dialogue = JsonUtility.FromJson<Dialogue>(jsonString);
        return dialogue;
    }

	void OnTriggerStay2D(Collider2D Player){
		if (Player.CompareTag ("User")) {
			if (Input.GetKeyDown (KeyCode.E)) {
				if (!qCompleted) {
					//Debug.Log ("E");
					if (AI_qLoader == null) {
						Debug.Log ("Empty");
					}
					AI_qLoader.qPack_AI (gameObject.tag);
				}
			}
		}
	}

    void OnTriggerStay(Collider Player)
    {
        if (Player.CompareTag("User"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!qCompleted)
                {
                    //Debug.Log ("E");
                    if (AI_qLoader == null)
                    {
                        Debug.Log("Empty");
                    }
                    AI_qLoader.qPack_AI(gameObject.tag);
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class limbo_buttonPaths : MonoBehaviour {
	public string Cont1;
	public string Cont2;

	//BUTTON PATHS FOR SIEBEL (LEVEL1, Q1):

	public void Siebelq1p1(int selection){
		Cont1 = "Holder";
		Cont2 = "Holder";
		int Choice = selection;
		if (selection == 1) {
			//Text to "Greet"
			Cont1 = "A local man I presume! Good day to you too.";
		} else if (selection == 2) {
			//Text to "Belittle"
			Cont1 = "What's it to you, a low-life patron?";
		}
		Messenger<string, string, int>.Broadcast ("Siebelq1Rtrn", Cont1, Cont2, Choice);
	}

	public void Sibelq1p2(int selection){
		Cont1 = "Holder";
		Cont2 = "Holder";
		int Choice = selection;
		if (selection == 1) {
			//Text to "Respond"
			Cont1 = "I'm a professor - academics is my vocation. However that's all can say.";
		} else if(selection == 2){
			//Text to "Change Topic"
			Cont1 = "I came not to discuss my day job! Now tell me, where might I be able to find some alcohol?";
		}
		Messenger<string, string, int>.Broadcast ("Siebelq1Rtrn", Cont1, Cont2, Choice);
	}

	public void Siebelq1p3(int selection){
		Cont1 = "Holder";
		Cont2 = "Holder";
		int Choice = selection;
		if (selection == 1) {
			Cont1 = "My trip has been a delightful experience, Leipzig's proven quite the destination.";
		} else if(selection == 2){
			Cont1 = "The details of my trip are unimportant, I am simply concerned with tonight's festivities.";
		}
		Messenger<string, string, int>.Broadcast ("Siebelq1Rtrn", Cont1, Cont2, Choice);
	}

	public void Sibelq1p4(int selection){
		Cont1 = "Holder";
		Cont2 = "Holder";
		int Choice = selection;
		if (selection == 1) {
			Cont1 = "Indeed, but I seek guidance, which beverage might you recommend?";
		} else if (selection == 2) {
			Cont1 = "Indeed, but dry wines don't interest me, I yearn for what's sweet.";
		}
		Messenger<string, string, int>.Broadcast ("Siebelq1Rtrn", Cont1, Cont2, Choice);
	}

	public void Siebelq1p5(int selection){
		Cont1 = "Holder";
		Cont2 = "Holder";
		int Choice = selection;
		if (selection == 1) {
			Cont1 = "My apologies, I did not mean to insult you my good fellow.";
		} else if (selection == 2) {
			Cont2 = "Sorry, here, let me fill your cup! Which wine might strike your fancy?";
		}
		Messenger<string, string, int>.Broadcast ("Siebelq1Rtrn", Cont1, Cont2, Choice);
	}

	public void Siebelq1p6(int selection){
		Cont1 = "Holder";
		Cont2 = "Holder";
		int Choice = selection;
		if (selection == 1) {
			Cont1 = "When it comes to wines I'm indifferent, what do you recommend?";
		} else if (selection == 2) {
			Cont1 = "Dry wines don't interest me, I'll take anything that's sweet.";
		}
		Messenger<string, string, int>.Broadcast ("Siebelq1Rtrn", Cont1, Cont2, Choice);
	}

	public void SiebelGoodbye(){
		Cont2 = "Holder";
		int Choice = 0;
		Cont1 = "Alright then, goodbye my friend!";
		Messenger<string, string, int>.Broadcast ("SiebelGoodbye", Cont1, Cont2, Choice);
	}


	//BUTTON PATHS FOR FROSCH (LEVEL1, Q2):

	public void Froschq2p1(int selection){
		Cont1 = "Holder";
		Cont2 = "Holder";
		int Choice = selection;
		if (selection == 1) {
			Cont1 = "Hello there! Frosch I presume. I was told you might be able to help me.";
			Cont2 = "I need to order a drink.";
		} else if (selection == 2) {
			Cont1 = "Good evening my friend.";
		}

		Messenger<string, string, int>.Broadcast ("Froschq2Rtrn", Cont1, Cont2, Choice);
	}

	public void Froschq2p2(int selection){
		Cont1 = "Holder";
		Cont2 = "Holder";
		int Choice = selection;
		if (selection == 1) {
			Cont1 = "Uhm, I suppose it could do no harm.";
		} else if (selection == 2) {
			Cont1 = "I'm afraid I couldn't sing. Sorry my friend, but not tonight.";
		}

		Messenger<string, string, int>.Broadcast ("Froschq2Rtrn", Cont1, Cont2, Choice);
	}

	public void Froschq2p3(int selection){
		Cont1 = "Holder";
		Cont2 = "Holder";
		int Choice = selection;
		if (selection == 1) {
			Cont1 = "Nothing, I just wanted to meet my fellow patrons.";
			Cont2 = "Speaking of which, do you come here often?";
		} else if (selection == 2) {
			Cont1 = "I was told to meet you if I wanted some wine.";
		}

		Messenger<string, string, int>.Broadcast ("Froschq2Rtrn", Cont1, Cont2, Choice);
	}

	public void Froschq2p4(int selection){
		Cont1 = "Holder";
		Cont2 = "Holder";
		int Choice = selection;
		if (selection == 1) {
			Cont1 = "Nothing, I just wanted to meet my fellow patrons.";
		} else if (selection == 2) {
			Cont1 = "I was told to meet you if I wanted some wine.";
		}

		Messenger<string, string, int>.Broadcast ("Froschq2Rtrn", Cont1, Cont2, Choice);
	}

	public void Froschq2p5(int selection){
		Cont1 = "Holder";
		Cont2 = "Holder";
		int Choice = selection;
		if (selection == 1) {
			Cont1 = "Nothing, I just wanted to meet my fellow patrons.";
		} else if (selection == 2) {
			Cont1 = "I was told to meet you if I wanted some wine.";
		}

		Messenger<string, string, int>.Broadcast ("Froschq2Rtrn", Cont1, Cont2, Choice);
	}


	public void Froschq2p6(int selection){
		Cont1 = "Holder";
		Cont2 = "Holder";
		int Choice = selection;
		if (selection == 1) {
			Cont1 = "So you're a native, I assume.";
			Cont2 = "What is life in Leipzig like?";
		} else if (selection == 2) {
			Cont1 = "Speaking of which, might you know where I can find a drink?";
		}

		Messenger<string, string, int>.Broadcast ("Froschq2Rtrn", Cont1, Cont2, Choice);
	}

	public void Froschq2p7(int selection){
		Cont1 = "Holder";
		Cont2 = "Holder";
		int Choice = selection;
		if (selection == 1) {
			Cont1 = "Siebel instructed me to ask for this tavern's finest sweet wine.";
		} else if (selection == 2) {
			Cont1 = "I am searching for this tavern's finest sweet wine.";
		}

		Messenger<string, string, int>.Broadcast ("Froschq2Rtrn", Cont1, Cont2, Choice);
	}
}

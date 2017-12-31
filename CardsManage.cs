using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TeenPatti
{
public class CardsManage : MonoBehaviour {


	public List<Sprite> UICards;
	public Sprite BackCards;
	public List<Cards> Deck;
	public List<Cards> PlayDeck;
	int[] cardval = new int[]{2,3,4,5,6,7,8,9,10,11,12,13,35};
	//                        0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10,11,12
	//						  13,14,15,16,17,18,19,20,21,22,23,24,25
	//                        26,27,28,29,30,31,32,33,34,35,36,37,38
	//                        39,40,41,42,43,44,45,46,47,48,49,50,51
	
	public List<Player> UIPlayers;
	public List<UIVIEW> UIview;
	[ContextMenu("SetDeck")]
	public void SetDeck()
	{
		//Jack=2,
		//Spades=3,
		//Heart=4,
		//Diamond=5

		int cardcounter=0;
		for (int i = 0; i < cardval.Length; i++) 
		{
			Cards C = new Cards ();
			C.cardvalue = cardval [i];
			C.cardtype = CardType.Diamond;
			C.cardindex = cardcounter;
			cardcounter++;
			Deck.Add (C);
		}
		for (int i = 0; i < cardval.Length; i++) 
		{
			Cards C = new Cards ();
			C.cardvalue = cardval [i];
			C.cardtype = CardType.Heart;
			C.cardindex = cardcounter;
			cardcounter++;
			Deck.Add (C);
		}
		for (int i = 0; i < cardval.Length; i++) 
		{
			Cards C = new Cards ();
			C.cardvalue = cardval [i];
			C.cardtype = CardType.Spades;
			C.cardindex = cardcounter;
			cardcounter++;
			Deck.Add (C);
		}
		for (int i = 0; i < cardval.Length; i++) 
		{
			Cards C = new Cards ();
			C.cardvalue = cardval [i];
			C.cardtype = CardType.Club;
			C.cardindex = cardcounter;
			cardcounter++;
			Deck.Add (C);
		}


	}
	public static CardsManage Instance;

	public List<Cards> TempDeck;
	// Use this for initialization
	void Start () 
	{
		Instance = this;
	
	}




	public void SuffleCards()
	{
		TempDeck.Clear ();
		for (int i = 0; i < 52; i++) {
			TempDeck.Add (Deck[i]);
		}
		TempDeck.Shuffle ();
		int cnt = 0;
		for (int i = 0; i < UIPlayers.Count; i++)
		{
			UIPlayers [i].Cards.Clear ();
			if (UIPlayers [i].PlayerStatus)
			{
				for (int j = 0; j < 3; j++) 
				{
					UIPlayers [i].Cards.Add (TempDeck [cnt]);
					UIview [i].VisibalCards [j].sprite = UICards [UIPlayers [i].Cards [j].cardindex];
					cnt++;
				}
			}
		}
		List<int> ids = new List<int> ();
		Debug.Log (ids.Count);
		int id= 0;
		List<Player> UIPlayersX = new List<Player> ();
		UIPlayersX = UIPlayers;



		ids = CheckCards (UIPlayersX);
		for (int i = 0; i < UIPlayers.Count; i++) 
		{
			UIview [i].WinStatus.text="";
			for (int j = 0; j < ids.Count; j++) 
			{				
				if (ids[j] == UIPlayers [i].PlayerID) 
				{
					UIview [i].WinStatus.text = "Win, ";
				}
			}
			if (UIPlayers [i].PlayerStatus) 
			{
				UIview [i].Userspec.text = UIPlayers [i].MySpec.ToString ();
			}
			else 
			{
				UIview [i].Userspec.text = "";
			}
			string S = UIview [i].WinStatus.text;
			S+=UIPlayers[i].Cards[0].cardindex.ToString()+", "+UIPlayers[i].Cards[1].cardindex.ToString()+", "+UIPlayers[i].Cards[2].cardindex.ToString(); 
			UIview [i].WinStatus.text=S;
		}
		SortByOrder ();
	}

	public void SortByOrder()
	{
		for (int i = 0; i < UIPlayers.Count; i++)
		{			
			if (UIPlayers [i].PlayerStatus)
			{
				for (int j = 0; j < 3; j++) 
				{				
					UIview [i].VisibalCards [j].sprite = UICards [UIPlayers [i].Cards [j].cardindex];
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public List<Player> tempplay;

		// Check User Spesifcation he has trail, high card, sequnce, pure sequnce, color , pair

	public List<int> CheckCards(List<Player> UIPlayerX)
	{
		int noofPlayers=UIPlayerX.Count;
		List<int> winners= new List<int>();
		tempplay.Clear ();
		for (int i = 0; i < noofPlayers; i++)
		{
			if (UIPlayerX [i].PlayerStatus&&UIPlayerX[i].Mode!=PlayerMode.Pack) 
			{
				tempplay.Add (UIPlayerX [i]);
			}

		}
		for (int i = 0; i < tempplay.Count; i++)
		{
			
			 CheckSpecification (tempplay[i]);
		}
		tempplay.Sort((x, y) => y.MySpec.CompareTo(x.MySpec));
		List<Player> tempplay2= new List<Player>();
		List<Player> tempplay3= new List<Player>();
		List<Player> tempplay4= new List<Player>();
		List<Player> tempplay5= new List<Player>();
		if (tempplay [0].MySpec == tempplay [1].MySpec) 
		{

			if (tempplay [0].MySpec == UserSpecificarion.HighCard) 
			{
				// First card check
				tempplay.Sort (((x, y) => y.Cards[0].cardvalue.CompareTo (x.Cards[0].cardvalue)));
				tempplay2.Add (tempplay [0]);
				for (int i = 1; i < tempplay.Count; i++) 
				{			
					if (tempplay [0].Cards[0].cardvalue == tempplay [i].Cards[0].cardvalue) 
					{
						tempplay2.Add (tempplay [i]);
					}
				}

				if (tempplay2.Count > 1) 
				{
					// Second card check
					tempplay2.Sort (((x, y) => y.Cards[1].cardvalue.CompareTo (x.Cards[1].cardvalue)));
					tempplay3.Clear ();
					tempplay3.Add (tempplay2 [0]);
					for (int i = 1; i < tempplay2.Count; i++) 
					{			
						if (tempplay3 [0].Cards[1].cardvalue == tempplay2 [i].Cards[1].cardvalue) 
						{
							tempplay3.Add (tempplay2 [i]);
						}
					}
					if (tempplay3.Count > 1) 
					{
						// Third card check
						tempplay3.Sort (((x, y) => y.Cards[2].cardvalue.CompareTo (x.Cards[2].cardvalue)));
						tempplay4.Clear ();
						tempplay4.Add (tempplay3 [0]);
						for (int i = 1; i < tempplay3.Count; i++) 
						{			
							if (tempplay4 [0].Cards[2].cardvalue == tempplay3 [i].Cards[2].cardvalue) 
							{
								tempplay4.Add (tempplay3 [i]);
							}
						}

						// missing some for high
						tempplay = tempplay4;
						for (int i = 0; i < tempplay.Count; i++)
						{
							
							winners.Add (tempplay [i].PlayerID);
						}
						//return tempplay [0].PlayerID;
						return winners;
					}
					else 
					{
						tempplay = tempplay3;
						winners.Add (tempplay [0].PlayerID);
						//return tempplay [0].PlayerID;
						return winners;
					}
				}
				else 
				{
					tempplay = tempplay2;
					winners.Add (tempplay [0].PlayerID);
					//return tempplay [0].PlayerID;
					return winners;
				}

				//return 0;
			}
			else if (tempplay [0].MySpec == UserSpecificarion.Color) // Color Check
			{
				tempplay2.Add (tempplay[0]);
				for (int i = 1; i < tempplay.Count; i++) 
				{
					if (tempplay [0].MySpec == tempplay [i].MySpec) 
					{
						tempplay2.Add (tempplay[i]);
					}
				}

				// First card check color
				tempplay2.Sort (((x, y) => y.Cards[0].cardvalue.CompareTo (x.Cards[0].cardvalue)));
				tempplay3.Add (tempplay2[0]);
				for (int i = 1; i < tempplay2.Count; i++) 
				{			
					if (tempplay3 [0].Cards[0].cardvalue == tempplay2 [i].Cards[0].cardvalue) 
					{
						tempplay3.Add (tempplay2 [i]);
					}
				}
				if (tempplay3.Count > 1) 
				{
					// Second card check color

					tempplay3.Sort (((x, y) => y.Cards[1].cardvalue.CompareTo (x.Cards[1].cardvalue)));
					tempplay4.Add (tempplay3[0]);
					for (int i = 1; i < tempplay3.Count; i++) 
					{			
						if (tempplay4 [0].Cards[1].cardvalue == tempplay3 [i].Cards[1].cardvalue) 
						{
							tempplay4.Add (tempplay3 [i]);
						}
					}
					if (tempplay4.Count > 1)
					{

						// Third card check color

						tempplay4.Sort (((x, y) => y.Cards[2].cardvalue.CompareTo (x.Cards[2].cardvalue)));
						tempplay5.Add (tempplay4[0]);
						for (int i = 1; i < tempplay4.Count; i++) 
						{			
							if (tempplay5 [0].Cards[2].cardvalue == tempplay3 [i].Cards[2].cardvalue) 
							{
								tempplay5.Add (tempplay4 [i]);
							}
						}
						// missing some color
						tempplay = tempplay5;
						for (int i = 0; i < tempplay.Count; i++)
						{

							winners.Add (tempplay [i].PlayerID);
						}
						//return tempplay [0].PlayerID;
						return winners;
					}
					else 
					{
						tempplay = tempplay4;
						winners.Add (tempplay [0].PlayerID);
						//return tempplay [0].PlayerID;
						return winners;
					}

				}
				else 
				{
					tempplay = tempplay3;
					winners.Add (tempplay [0].PlayerID);
					//return tempplay [0].PlayerID;
					return winners;
				}

			
			}
			else
			{
				tempplay2.Add (tempplay [0]);
				for (int i = 1; i < tempplay.Count; i++) 
				{			
					if (tempplay [0].MySpec == tempplay [i].MySpec) 
					{
						tempplay2.Add (tempplay [i]);
					}
				}
				tempplay2.Sort (((x, y) => y.CardTotal.CompareTo (x.CardTotal)));
				tempplay = tempplay2;
				winners.Add (tempplay [0].PlayerID);
				//return tempplay [0].PlayerID;
				return winners;
			}


		}
		else 
		{
			winners.Add (tempplay [0].PlayerID);
			//return tempplay [0].PlayerID;
			return winners;
		}

		//return 0;
		return winners;
	}

	public void CheckSpecification(Player Player)
	{
		if (Player.Cards [0].cardvalue == Player.Cards [1].cardvalue && Player.Cards [1].cardvalue==Player.Cards [2].cardvalue) 
		{
		//	return UserSpecificarion.Trail;	
			Player.MySpec=UserSpecificarion.Trail;
			Player.CardTotal = Player.Cards [0].cardvalue * 3;
			return;
		}
		Player.Cards.Sort(((x, y) => y.cardvalue.CompareTo(x.cardvalue)));


		//check for Pure Sequence
		if (Player.Cards [0].cardvalue == 35) 
		{
			if (Player.Cards [1].cardvalue == 3 && Player.Cards [2].cardvalue == 2) 
			{
				if (CheckForColor (Player.Cards)) 
				{
					Player.MySpec = UserSpecificarion.PureSequence;
					Player.CardTotal = Player.Cards [0].cardvalue +Player.Cards [1].cardvalue +Player.Cards [2].cardvalue;
					Player.CardTotal += (int)Player.Cards [0].cardtype;
					return;
				}
			}
			else if (Player.Cards [1].cardvalue == 13 && Player.Cards [2].cardvalue == 12) 
			{
				if (CheckForColor (Player.Cards)) 
				{
					Player.MySpec = UserSpecificarion.PureSequence;
					Player.CardTotal = Player.Cards [0].cardvalue + Player.Cards [1].cardvalue + Player.Cards [2].cardvalue;
					Player.CardTotal += (int)Player.Cards [0].cardtype;
					return;
				}
			}
		}
		else 
		{
			if (Player.Cards [0].cardvalue-1 ==Player.Cards [1].cardvalue && Player.Cards [1].cardvalue-1 ==Player.Cards [2].cardvalue) 
			{
				if (CheckForColor (Player.Cards)) 
				{
					Player.MySpec = UserSpecificarion.PureSequence;
					Player.CardTotal = Player.Cards [0].cardvalue + Player.Cards [1].cardvalue + Player.Cards [2].cardvalue;
					Player.CardTotal += (int)Player.Cards [0].cardtype;
					return;
				}
			}
		}


		//check for Sequence
		if (Player.Cards [0].cardvalue == 35) 
		{
			if (Player.Cards [1].cardvalue == 3 && Player.Cards [2].cardvalue == 2) 
			{
				Player.MySpec = UserSpecificarion.Sequence;
				Player.CardTotal = Player.Cards [0].cardvalue +Player.Cards [1].cardvalue +Player.Cards [2].cardvalue;
				Player.ColorTotal=(int)Player.Cards [0].cardtype;
				return;
			}
			else if (Player.Cards [1].cardvalue == 13 && Player.Cards [2].cardvalue == 12) 
			{
				Player.MySpec = UserSpecificarion.Sequence;
				Player.CardTotal = Player.Cards [0].cardvalue +Player.Cards [1].cardvalue +Player.Cards [2].cardvalue;
				Player.ColorTotal=(int)Player.Cards [0].cardtype;
				return;
			}
		}
		else 
		{
			if (Player.Cards [0].cardvalue-1 ==Player.Cards [1].cardvalue && Player.Cards [1].cardvalue-1 ==Player.Cards [2].cardvalue) 
			{
				Player.MySpec = UserSpecificarion.Sequence;
				Player.CardTotal = Player.Cards [0].cardvalue +Player.Cards [1].cardvalue +Player.Cards [2].cardvalue;
				Player.ColorTotal=(int)Player.Cards [0].cardtype;
				return;
			}
		}


		//check for color
		if (Player.Cards [0].cardtype == Player.Cards [1].cardtype && Player.Cards [1].cardtype==Player.Cards [2].cardtype) 
		{
			Player.MySpec=UserSpecificarion.Color;
			Player.CardTotal = Player.Cards [0].cardvalue +Player.Cards [1].cardvalue +Player.Cards [2].cardvalue;
			Player.CardTotal += (int)Player.Cards [0].cardtype;
			return;
		}

		//check for pair

		if (Player.Cards [0].cardvalue == Player.Cards [1].cardvalue || Player.Cards [1].cardvalue==Player.Cards [2].cardvalue) 
		{
			Player.MySpec=UserSpecificarion.Pair;
			if (Player.Cards [0].cardvalue == Player.Cards [1].cardvalue) 
			{
				Player.CardTotal = Player.Cards [0].cardvalue + Player.Cards [1].cardvalue;
			}
			else 
			{
				Player.CardTotal = Player.Cards [1].cardvalue + Player.Cards [2].cardvalue;
			}
			return;
		}

		//check for highcard

		Player.CardTotal = Player.Cards [0].cardvalue +Player.Cards [1].cardvalue +Player.Cards [2].cardvalue;
		Player.MySpec=UserSpecificarion.HighCard;
		return;

	}

	public bool CheckForColor(List<Cards> card)
	{
		if (card [0].cardtype == card [1].cardtype &&card [1].cardtype == card [2].cardtype) 
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}

[System.Serializable]
public class UIVIEW
{
	public List<Image> VisibalCards;
	public Text WinStatus;
	public Text Userspec;
}
public static class ListExtensions {
	/// <summary>
	/// Shuffles the element order of the specified list.
	/// </summary>
	public static void Shuffle<T>(this IList<T> ts) {
		var count = ts.Count;
		var last = count - 1;
		for (var i = 0; i < last; ++i) 
		{
			var r = UnityEngine.Random.Range(i, count);
			var tmp = ts[i];
			ts[i] = ts[r];
			ts[r] = tmp;
		}
	}
}
[System.Serializable]
public class Cards  //Card Object
{
	public int cardindex;
	public int cardvalue;
	public CardType cardtype;
}
public enum CardType  // Card Type
{
	Spades=1,
	Heart=2,
	Diamond=3,
	Club=4,
}
public enum UserSpecificarion // User have Cards Of Type
{
	Trail=6,
	PureSequence=5,
	Sequence=4,
	Color=3,
	Pair=2,
	HighCard=1
}
[System.Serializable]
public enum PlayerState
{
	None,
	Wait,
	Active,
	Deactive,
	Removed
}
[System.Serializable]
public enum PlayerMode
{
	Blind,
	Seen,
	Chal,
	Pack
}
[System.Serializable]
public class Player
{
	public int PlayerID;
	public string PlayerName;
	public bool PlayerStatus;
	public PlayerState State;
	public PlayerMode Mode;
	public bool CardSeen;
	public Player()
	{
		PlayerID = 0;
		PlayerName = "";
		PlayerStatus = false;
	}
	public List<Cards> Cards= new List<Cards>();
	public UserSpecificarion MySpec;
	public int CardTotal;
	public int ColorTotal;
}
}
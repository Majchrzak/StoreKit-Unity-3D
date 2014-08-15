using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Store;

public class Example : MonoBehaviour, IStoreDelegate
{
	/// <summary>
	/// Fullgame identifier.
	/// </summary>
	private const string m_FullgameUnlockID = "com.game.fullgame";

	/// <summary>
	/// Setup store.
	/// </summary>
	private void Awake()
	{
		StoreKit.Instance.Delegate = this;
	}

	/// <summary>
	/// Draw GUI.
	/// </summary>
	private void OnGUI()
	{
		IStore store = StoreKit.Instance;

		// In App Purchases can been disabled in iOS settings.
		if (!store.IsAvailable)
			return;

		Rect position = new Rect(Screen.width * 0.5f - 90f, 100f, 180f, 45f);

		if (GUI.Button(position, "Request data"))
		{
			store.Request(new [] { m_FullgameUnlockID });
		}

		position.y += 60f;

		if (GUI.Button(position, "Unlock game"))
		{
			store.Purchase(m_FullgameUnlockID);
		}

		position.y += 60f;

		if (GUI.Button(position, "Restore"))
		{
			store.Restore();
		}
	}

	/// <summary>
	/// Handles the request success event.
	/// </summary>
	/// <param name="identifier">product identifier.</param>
	public void OnStoreRequestSuccess(IEnumerable<Product> products)
	{
	}
	
	/// <summary>
	/// Handles the request failed event.
	/// </summary>
	public void OnStoreRequestFailed(string error)
	{
	}
	
	/// <summary>
	/// Handles the transaction success event.
	/// </summary>
	/// <param name="identifier">product identifier.</param>
	public void OnStoreTransactionSuccess(string identifier)
	{
		switch (identifier)
		{
		case m_FullgameUnlockID:
			// Save fullgame value in player prefs.
			break;
		}
	}
	
	/// <summary>
	/// Handles the transaction failed event.
	/// </summary>
	/// <param name="identifier">product identifier.</param>
	public void OnStoreTransactionFailed(string identifier)
	{
	}
	
	/// <summary>
	/// Handles the transaction restore event.
	/// </summary>
	/// <param name="identifier">product identifier.</param>
	public void OnStoreTransactionRestore(string identifier)
	{
	}
}

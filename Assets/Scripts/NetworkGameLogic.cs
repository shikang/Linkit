﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class NetworkGameLogic : Photon.PunBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	string GetGemsCSV( List<Gem> gems )
	{
		string csv = "";
		foreach ( Gem gem in gems )
		{
			PhotonView p = gem.GetComponent<PhotonView>();
			csv += p.instantiationId + ",";
		}
		//csv.TrimEnd( ","[0] );
		csv = csv.Remove( csv.Length - 1 );

		return csv;
	}

	string[] GetGemsId( string idsCSV )
	{
		return idsCSV.Split(","[0]); ;
	}

	// Player 2 to Player 1
	public void LinkNetworkGem( Gem gem, bool link )
	{
		PhotonView p = gem.GetComponent<PhotonView>();
		photonView.RPC( "LinkNetworkGem_RPC", PhotonTargets.Others, p.instantiationId, link );
	}

	[PunRPC]
	public void LinkNetworkGem_RPC( int id, bool link )
	{
		GameObject.Find( "GemSpawner" ).GetComponent<GemSpawner>().LinkNetworkGem( id, link );
	}

	// Player 2 to Player 1
	public void UnlinkNetworkGems( List<Gem> gems )
	{
		string csv = GetGemsCSV( gems );

		photonView.RPC( "UnlinkNetworkGems_RPC", PhotonTargets.Others, csv );
	}

	[PunRPC]
	public void UnlinkNetworkGems_RPC( string idsCSV )
	{
		string[] ids = GetGemsId( idsCSV );
		GemSpawner spawner = GameObject.Find( "GemSpawner" ).GetComponent<GemSpawner>();

		foreach ( string id in ids )
		{
			spawner.LinkNetworkGem( Convert.ToInt32( id ), false );
		}
	}

	// Player 2 to Player 1
	public void DestroyNetworkGems( List<Gem> gems )
	{
		string csv = GetGemsCSV( gems );

		photonView.RPC( "DestroyNetworkGems_RPC", PhotonTargets.Others, csv );
	}

	[PunRPC]
	public void DestroyNetworkGems_RPC( string idsCSV )
	{
		string[] ids = GetGemsId( idsCSV );
		GemSpawner spawner = GameObject.Find( "GemSpawner" ).GetComponent<GemSpawner>();

		foreach ( string id in ids )
		{
			spawner.DestroyNetworkGem( Convert.ToInt32( id ), ids.Length );
		}
	}

	// Player 1 to Player 2
	public void InformGemsDestroyed( List<Gem> gems )
	{
		string csv = GetGemsCSV( gems );

		photonView.RPC("InformGemsDestroyed_RPC", PhotonTargets.Others, csv );
	}

	[PunRPC]
	public void InformGemsDestroyed_RPC( string idsCSV )
	{
		string[] ids = GetGemsId( idsCSV );
		GemSpawner spawner = GameObject.Find( "GemSpawner" ).GetComponent<GemSpawner>();

		foreach ( string id in ids )
		{
			spawner.NetworkAddGainPointsEffect( Convert.ToInt32( id ), ids.Length );
		}
	}

	// Both ways
	public void CreateRepel( Gem gem )
	{
		PhotonView p = gem.GetComponent<PhotonView>();
		photonView.RPC( "CreateRepel_RPC", PhotonTargets.Others, p.instantiationId );
	}

	[PunRPC]
	public void CreateRepel_RPC( int id )
	{
		GameObject.Find("GemSpawner").GetComponent<GemSpawner>().CreateNetworkRepel( id );
	}
}
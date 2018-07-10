﻿using System.Net;
using SpeedDate.Configuration;
using SpeedDate.Server;
using SpeedDate.ServerPlugins.Authentication;
using SpeedDate.ServerPlugins.Lobbies;
using UnityEngine;

public class Server : MonoBehaviour
{
	public static Server Instance;
	
	private readonly SpeedDateServer _server = new SpeedDateServer();
	
	public int Port = 60125;
	
	public bool EnableGuestLogin = true;

	public string GuestPrefix = "Guest-";

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			Destroy(this.gameObject);
		}
	}

	// Use this for initialization
	void Start ()
	{		
		_server.Started += () =>
		{
			Debug.Log("Server started");
		};
		_server.Start(new DefaultConfigProvider(new NetworkConfig(IPAddress.Any, Port), PluginsConfig.DefaultServerPlugins, new IConfig[]
		{
			new AuthConfig
			{
				GuestPrefix = GuestPrefix,
				EnableGuestLogin = EnableGuestLogin,
				UsernameMaxChars = 10,
				UsernameMinChars =  2
			},
		}));
		_server.GetPlugin<LobbiesPlugin>().AddFactory(new LobbyFactoryAnonymous("Deathmatch", _server.GetPlugin<LobbiesPlugin>(), DemoLobbyFactories.Deathmatch));
		_server.GetPlugin<LobbiesPlugin>().AddFactory(new LobbyFactoryAnonymous("2 vs 2 vs 4", _server.GetPlugin<LobbiesPlugin>(), DemoLobbyFactories.TwoVsTwoVsFour));
		_server.GetPlugin<LobbiesPlugin>().AddFactory(new LobbyFactoryAnonymous("3 vs 3 auto", _server.GetPlugin<LobbiesPlugin>(), DemoLobbyFactories.ThreeVsThreeQueue));
	}
}

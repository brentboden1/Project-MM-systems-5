﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18051
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LobbyService.Web
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="MonopolyGameDatabase")]
	public partial class DataClasses1DataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertLobby(Lobby instance);
    partial void UpdateLobby(Lobby instance);
    partial void DeleteLobby(Lobby instance);
    partial void InsertPlayerLobby(PlayerLobby instance);
    partial void UpdatePlayerLobby(PlayerLobby instance);
    partial void DeletePlayerLobby(PlayerLobby instance);
    partial void InsertPlayer(Player instance);
    partial void UpdatePlayer(Player instance);
    partial void DeletePlayer(Player instance);
    #endregion
		
		public DataClasses1DataContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["MonopolyGameDatabaseConnectionString"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public DataClasses1DataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataClasses1DataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataClasses1DataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataClasses1DataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Lobby> Lobbies
		{
			get
			{
				return this.GetTable<Lobby>();
			}
		}
		
		public System.Data.Linq.Table<PlayerLobby> PlayerLobbies
		{
			get
			{
				return this.GetTable<PlayerLobby>();
			}
		}
		
		public System.Data.Linq.Table<Player> Players
		{
			get
			{
				return this.GetTable<Player>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Lobby")]
	public partial class Lobby : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _LobbyId;
		
		private string _LobbyName;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnLobbyIdChanging(int value);
    partial void OnLobbyIdChanged();
    partial void OnLobbyNameChanging(string value);
    partial void OnLobbyNameChanged();
    #endregion
		
		public Lobby()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LobbyId", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int LobbyId
		{
			get
			{
				return this._LobbyId;
			}
			set
			{
				if ((this._LobbyId != value))
				{
					this.OnLobbyIdChanging(value);
					this.SendPropertyChanging();
					this._LobbyId = value;
					this.SendPropertyChanged("LobbyId");
					this.OnLobbyIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LobbyName", DbType="NChar(10)")]
		public string LobbyName
		{
			get
			{
				return this._LobbyName;
			}
			set
			{
				if ((this._LobbyName != value))
				{
					this.OnLobbyNameChanging(value);
					this.SendPropertyChanging();
					this._LobbyName = value;
					this.SendPropertyChanged("LobbyName");
					this.OnLobbyNameChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.PlayerLobby")]
	public partial class PlayerLobby : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _PlayerId;
		
		private int _LobbyId;
		
		private System.Nullable<int> _HostPlayer;
		
		private System.Nullable<bool> _IsWaitingForPlayers;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnPlayerIdChanging(int value);
    partial void OnPlayerIdChanged();
    partial void OnLobbyIdChanging(int value);
    partial void OnLobbyIdChanged();
    partial void OnHostPlayerChanging(System.Nullable<int> value);
    partial void OnHostPlayerChanged();
    partial void OnIsWaitingForPlayersChanging(System.Nullable<bool> value);
    partial void OnIsWaitingForPlayersChanged();
    #endregion
		
		public PlayerLobby()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PlayerId", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int PlayerId
		{
			get
			{
				return this._PlayerId;
			}
			set
			{
				if ((this._PlayerId != value))
				{
					this.OnPlayerIdChanging(value);
					this.SendPropertyChanging();
					this._PlayerId = value;
					this.SendPropertyChanged("PlayerId");
					this.OnPlayerIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LobbyId", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int LobbyId
		{
			get
			{
				return this._LobbyId;
			}
			set
			{
				if ((this._LobbyId != value))
				{
					this.OnLobbyIdChanging(value);
					this.SendPropertyChanging();
					this._LobbyId = value;
					this.SendPropertyChanged("LobbyId");
					this.OnLobbyIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_HostPlayer", DbType="Int")]
		public System.Nullable<int> HostPlayer
		{
			get
			{
				return this._HostPlayer;
			}
			set
			{
				if ((this._HostPlayer != value))
				{
					this.OnHostPlayerChanging(value);
					this.SendPropertyChanging();
					this._HostPlayer = value;
					this.SendPropertyChanged("HostPlayer");
					this.OnHostPlayerChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsWaitingForPlayers", DbType="Bit")]
		public System.Nullable<bool> IsWaitingForPlayers
		{
			get
			{
				return this._IsWaitingForPlayers;
			}
			set
			{
				if ((this._IsWaitingForPlayers != value))
				{
					this.OnIsWaitingForPlayersChanging(value);
					this.SendPropertyChanging();
					this._IsWaitingForPlayers = value;
					this.SendPropertyChanged("IsWaitingForPlayers");
					this.OnIsWaitingForPlayersChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Player")]
	public partial class Player : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _PlayerId;
		
		private string _PlayerName;
		
		private bool _AlreadExist;
		
		private System.Nullable<int> _DiceOne;
		
		private System.Nullable<int> _DiceTwo;
		
		private System.Nullable<bool> _OnceDouble;
		
		private System.Nullable<bool> _TwiceDouble;
		
		private System.Nullable<bool> _Jail;
		
		private System.Nullable<int> _DiceEyes;
		
		private System.Nullable<bool> _IsDiceRolling;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnPlayerIdChanging(int value);
    partial void OnPlayerIdChanged();
    partial void OnPlayerNameChanging(string value);
    partial void OnPlayerNameChanged();
    partial void OnAlreadExistChanging(bool value);
    partial void OnAlreadExistChanged();
    partial void OnDiceOneChanging(System.Nullable<int> value);
    partial void OnDiceOneChanged();
    partial void OnDiceTwoChanging(System.Nullable<int> value);
    partial void OnDiceTwoChanged();
    partial void OnOnceDoubleChanging(System.Nullable<bool> value);
    partial void OnOnceDoubleChanged();
    partial void OnTwiceDoubleChanging(System.Nullable<bool> value);
    partial void OnTwiceDoubleChanged();
    partial void OnJailChanging(System.Nullable<bool> value);
    partial void OnJailChanged();
    partial void OnDiceEyesChanging(System.Nullable<int> value);
    partial void OnDiceEyesChanged();
    partial void OnIsDiceRollingChanging(System.Nullable<bool> value);
    partial void OnIsDiceRollingChanged();
    #endregion
		
		public Player()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PlayerId", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int PlayerId
		{
			get
			{
				return this._PlayerId;
			}
			set
			{
				if ((this._PlayerId != value))
				{
					this.OnPlayerIdChanging(value);
					this.SendPropertyChanging();
					this._PlayerId = value;
					this.SendPropertyChanged("PlayerId");
					this.OnPlayerIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PlayerName", DbType="NChar(10)")]
		public string PlayerName
		{
			get
			{
				return this._PlayerName;
			}
			set
			{
				if ((this._PlayerName != value))
				{
					this.OnPlayerNameChanging(value);
					this.SendPropertyChanging();
					this._PlayerName = value;
					this.SendPropertyChanged("PlayerName");
					this.OnPlayerNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AlreadExist", DbType="Bit NOT NULL")]
		public bool AlreadExist
		{
			get
			{
				return this._AlreadExist;
			}
			set
			{
				if ((this._AlreadExist != value))
				{
					this.OnAlreadExistChanging(value);
					this.SendPropertyChanging();
					this._AlreadExist = value;
					this.SendPropertyChanged("AlreadExist");
					this.OnAlreadExistChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DiceOne", DbType="Int")]
		public System.Nullable<int> DiceOne
		{
			get
			{
				return this._DiceOne;
			}
			set
			{
				if ((this._DiceOne != value))
				{
					this.OnDiceOneChanging(value);
					this.SendPropertyChanging();
					this._DiceOne = value;
					this.SendPropertyChanged("DiceOne");
					this.OnDiceOneChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DiceTwo", DbType="Int")]
		public System.Nullable<int> DiceTwo
		{
			get
			{
				return this._DiceTwo;
			}
			set
			{
				if ((this._DiceTwo != value))
				{
					this.OnDiceTwoChanging(value);
					this.SendPropertyChanging();
					this._DiceTwo = value;
					this.SendPropertyChanged("DiceTwo");
					this.OnDiceTwoChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_OnceDouble", DbType="Bit")]
		public System.Nullable<bool> OnceDouble
		{
			get
			{
				return this._OnceDouble;
			}
			set
			{
				if ((this._OnceDouble != value))
				{
					this.OnOnceDoubleChanging(value);
					this.SendPropertyChanging();
					this._OnceDouble = value;
					this.SendPropertyChanged("OnceDouble");
					this.OnOnceDoubleChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TwiceDouble", DbType="Bit")]
		public System.Nullable<bool> TwiceDouble
		{
			get
			{
				return this._TwiceDouble;
			}
			set
			{
				if ((this._TwiceDouble != value))
				{
					this.OnTwiceDoubleChanging(value);
					this.SendPropertyChanging();
					this._TwiceDouble = value;
					this.SendPropertyChanged("TwiceDouble");
					this.OnTwiceDoubleChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Jail", DbType="Bit")]
		public System.Nullable<bool> Jail
		{
			get
			{
				return this._Jail;
			}
			set
			{
				if ((this._Jail != value))
				{
					this.OnJailChanging(value);
					this.SendPropertyChanging();
					this._Jail = value;
					this.SendPropertyChanged("Jail");
					this.OnJailChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DiceEyes", DbType="Int")]
		public System.Nullable<int> DiceEyes
		{
			get
			{
				return this._DiceEyes;
			}
			set
			{
				if ((this._DiceEyes != value))
				{
					this.OnDiceEyesChanging(value);
					this.SendPropertyChanging();
					this._DiceEyes = value;
					this.SendPropertyChanged("DiceEyes");
					this.OnDiceEyesChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsDiceRolling", DbType="Bit")]
		public System.Nullable<bool> IsDiceRolling
		{
			get
			{
				return this._IsDiceRolling;
			}
			set
			{
				if ((this._IsDiceRolling != value))
				{
					this.OnIsDiceRollingChanging(value);
					this.SendPropertyChanging();
					this._IsDiceRolling = value;
					this.SendPropertyChanged("IsDiceRolling");
					this.OnIsDiceRollingChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
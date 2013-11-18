﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18051
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WpfApplication1.ServiceReference1 {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Player", Namespace="http://schemas.datacontract.org/2004/07/LobbyService.Web.DTO")]
    [System.SerializableAttribute()]
    public partial class Player : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int PlayerIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PlayerNameField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int PlayerId {
            get {
                return this.PlayerIdField;
            }
            set {
                if ((this.PlayerIdField.Equals(value) != true)) {
                    this.PlayerIdField = value;
                    this.RaisePropertyChanged("PlayerId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PlayerName {
            get {
                return this.PlayerNameField;
            }
            set {
                if ((object.ReferenceEquals(this.PlayerNameField, value) != true)) {
                    this.PlayerNameField = value;
                    this.RaisePropertyChanged("PlayerName");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="PlayerLobby", Namespace="http://schemas.datacontract.org/2004/07/LobbyService.Web.DTO")]
    [System.SerializableAttribute()]
    public partial class PlayerLobby : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private WpfApplication1.ServiceReference1.Player HostPlayerField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool IsAwaitingForPlayersField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private WpfApplication1.ServiceReference1.Lobby LobbyIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private WpfApplication1.ServiceReference1.Player[] PlayerField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public WpfApplication1.ServiceReference1.Player HostPlayer {
            get {
                return this.HostPlayerField;
            }
            set {
                if ((object.ReferenceEquals(this.HostPlayerField, value) != true)) {
                    this.HostPlayerField = value;
                    this.RaisePropertyChanged("HostPlayer");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsAwaitingForPlayers {
            get {
                return this.IsAwaitingForPlayersField;
            }
            set {
                if ((this.IsAwaitingForPlayersField.Equals(value) != true)) {
                    this.IsAwaitingForPlayersField = value;
                    this.RaisePropertyChanged("IsAwaitingForPlayers");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public WpfApplication1.ServiceReference1.Lobby LobbyId {
            get {
                return this.LobbyIdField;
            }
            set {
                if ((object.ReferenceEquals(this.LobbyIdField, value) != true)) {
                    this.LobbyIdField = value;
                    this.RaisePropertyChanged("LobbyId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public WpfApplication1.ServiceReference1.Player[] Player {
            get {
                return this.PlayerField;
            }
            set {
                if ((object.ReferenceEquals(this.PlayerField, value) != true)) {
                    this.PlayerField = value;
                    this.RaisePropertyChanged("Player");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Lobby", Namespace="http://schemas.datacontract.org/2004/07/LobbyService.Web.DTO")]
    [System.SerializableAttribute()]
    public partial class Lobby : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int LobbyIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int LobbyNameField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int LobbyId {
            get {
                return this.LobbyIdField;
            }
            set {
                if ((this.LobbyIdField.Equals(value) != true)) {
                    this.LobbyIdField = value;
                    this.RaisePropertyChanged("LobbyId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int LobbyName {
            get {
                return this.LobbyNameField;
            }
            set {
                if ((this.LobbyNameField.Equals(value) != true)) {
                    this.LobbyNameField = value;
                    this.RaisePropertyChanged("LobbyName");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference1.IService1")]
    public interface IService1 {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/DoWork", ReplyAction="http://tempuri.org/IService1/DoWorkResponse")]
        void DoWork();
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IService1/DoWork", ReplyAction="http://tempuri.org/IService1/DoWorkResponse")]
        System.IAsyncResult BeginDoWork(System.AsyncCallback callback, object asyncState);
        
        void EndDoWork(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/CreateLobby", ReplyAction="http://tempuri.org/IService1/CreateLobbyResponse")]
        void CreateLobby(WpfApplication1.ServiceReference1.Player host, int lobby);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IService1/CreateLobby", ReplyAction="http://tempuri.org/IService1/CreateLobbyResponse")]
        System.IAsyncResult BeginCreateLobby(WpfApplication1.ServiceReference1.Player host, int lobby, System.AsyncCallback callback, object asyncState);
        
        void EndCreateLobby(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/GatAvailablePlayLobbies", ReplyAction="http://tempuri.org/IService1/GatAvailablePlayLobbiesResponse")]
        WpfApplication1.ServiceReference1.PlayerLobby[] GatAvailablePlayLobbies();
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IService1/GatAvailablePlayLobbies", ReplyAction="http://tempuri.org/IService1/GatAvailablePlayLobbiesResponse")]
        System.IAsyncResult BeginGatAvailablePlayLobbies(System.AsyncCallback callback, object asyncState);
        
        WpfApplication1.ServiceReference1.PlayerLobby[] EndGatAvailablePlayLobbies(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/AddPlayer", ReplyAction="http://tempuri.org/IService1/AddPlayerResponse")]
        WpfApplication1.ServiceReference1.Player AddPlayer(WpfApplication1.ServiceReference1.Player pl);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IService1/AddPlayer", ReplyAction="http://tempuri.org/IService1/AddPlayerResponse")]
        System.IAsyncResult BeginAddPlayer(WpfApplication1.ServiceReference1.Player pl, System.AsyncCallback callback, object asyncState);
        
        WpfApplication1.ServiceReference1.Player EndAddPlayer(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IService1Channel : WpfApplication1.ServiceReference1.IService1, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GatAvailablePlayLobbiesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public GatAvailablePlayLobbiesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public WpfApplication1.ServiceReference1.PlayerLobby[] Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((WpfApplication1.ServiceReference1.PlayerLobby[])(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class AddPlayerCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public AddPlayerCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public WpfApplication1.ServiceReference1.Player Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((WpfApplication1.ServiceReference1.Player)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class Service1Client : System.ServiceModel.ClientBase<WpfApplication1.ServiceReference1.IService1>, WpfApplication1.ServiceReference1.IService1 {
        
        private BeginOperationDelegate onBeginDoWorkDelegate;
        
        private EndOperationDelegate onEndDoWorkDelegate;
        
        private System.Threading.SendOrPostCallback onDoWorkCompletedDelegate;
        
        private BeginOperationDelegate onBeginCreateLobbyDelegate;
        
        private EndOperationDelegate onEndCreateLobbyDelegate;
        
        private System.Threading.SendOrPostCallback onCreateLobbyCompletedDelegate;
        
        private BeginOperationDelegate onBeginGatAvailablePlayLobbiesDelegate;
        
        private EndOperationDelegate onEndGatAvailablePlayLobbiesDelegate;
        
        private System.Threading.SendOrPostCallback onGatAvailablePlayLobbiesCompletedDelegate;
        
        private BeginOperationDelegate onBeginAddPlayerDelegate;
        
        private EndOperationDelegate onEndAddPlayerDelegate;
        
        private System.Threading.SendOrPostCallback onAddPlayerCompletedDelegate;
        
        public Service1Client() {
        }
        
        public Service1Client(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public Service1Client(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public Service1Client(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public Service1Client(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> DoWorkCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CreateLobbyCompleted;
        
        public event System.EventHandler<GatAvailablePlayLobbiesCompletedEventArgs> GatAvailablePlayLobbiesCompleted;
        
        public event System.EventHandler<AddPlayerCompletedEventArgs> AddPlayerCompleted;
        
        public void DoWork() {
            base.Channel.DoWork();
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public System.IAsyncResult BeginDoWork(System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginDoWork(callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public void EndDoWork(System.IAsyncResult result) {
            base.Channel.EndDoWork(result);
        }
        
        private System.IAsyncResult OnBeginDoWork(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return this.BeginDoWork(callback, asyncState);
        }
        
        private object[] OnEndDoWork(System.IAsyncResult result) {
            this.EndDoWork(result);
            return null;
        }
        
        private void OnDoWorkCompleted(object state) {
            if ((this.DoWorkCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.DoWorkCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void DoWorkAsync() {
            this.DoWorkAsync(null);
        }
        
        public void DoWorkAsync(object userState) {
            if ((this.onBeginDoWorkDelegate == null)) {
                this.onBeginDoWorkDelegate = new BeginOperationDelegate(this.OnBeginDoWork);
            }
            if ((this.onEndDoWorkDelegate == null)) {
                this.onEndDoWorkDelegate = new EndOperationDelegate(this.OnEndDoWork);
            }
            if ((this.onDoWorkCompletedDelegate == null)) {
                this.onDoWorkCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnDoWorkCompleted);
            }
            base.InvokeAsync(this.onBeginDoWorkDelegate, null, this.onEndDoWorkDelegate, this.onDoWorkCompletedDelegate, userState);
        }
        
        public void CreateLobby(WpfApplication1.ServiceReference1.Player host, int lobby) {
            base.Channel.CreateLobby(host, lobby);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public System.IAsyncResult BeginCreateLobby(WpfApplication1.ServiceReference1.Player host, int lobby, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginCreateLobby(host, lobby, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public void EndCreateLobby(System.IAsyncResult result) {
            base.Channel.EndCreateLobby(result);
        }
        
        private System.IAsyncResult OnBeginCreateLobby(object[] inValues, System.AsyncCallback callback, object asyncState) {
            WpfApplication1.ServiceReference1.Player host = ((WpfApplication1.ServiceReference1.Player)(inValues[0]));
            int lobby = ((int)(inValues[1]));
            return this.BeginCreateLobby(host, lobby, callback, asyncState);
        }
        
        private object[] OnEndCreateLobby(System.IAsyncResult result) {
            this.EndCreateLobby(result);
            return null;
        }
        
        private void OnCreateLobbyCompleted(object state) {
            if ((this.CreateLobbyCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.CreateLobbyCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void CreateLobbyAsync(WpfApplication1.ServiceReference1.Player host, int lobby) {
            this.CreateLobbyAsync(host, lobby, null);
        }
        
        public void CreateLobbyAsync(WpfApplication1.ServiceReference1.Player host, int lobby, object userState) {
            if ((this.onBeginCreateLobbyDelegate == null)) {
                this.onBeginCreateLobbyDelegate = new BeginOperationDelegate(this.OnBeginCreateLobby);
            }
            if ((this.onEndCreateLobbyDelegate == null)) {
                this.onEndCreateLobbyDelegate = new EndOperationDelegate(this.OnEndCreateLobby);
            }
            if ((this.onCreateLobbyCompletedDelegate == null)) {
                this.onCreateLobbyCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnCreateLobbyCompleted);
            }
            base.InvokeAsync(this.onBeginCreateLobbyDelegate, new object[] {
                        host,
                        lobby}, this.onEndCreateLobbyDelegate, this.onCreateLobbyCompletedDelegate, userState);
        }
        
        public WpfApplication1.ServiceReference1.PlayerLobby[] GatAvailablePlayLobbies() {
            return base.Channel.GatAvailablePlayLobbies();
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public System.IAsyncResult BeginGatAvailablePlayLobbies(System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginGatAvailablePlayLobbies(callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public WpfApplication1.ServiceReference1.PlayerLobby[] EndGatAvailablePlayLobbies(System.IAsyncResult result) {
            return base.Channel.EndGatAvailablePlayLobbies(result);
        }
        
        private System.IAsyncResult OnBeginGatAvailablePlayLobbies(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return this.BeginGatAvailablePlayLobbies(callback, asyncState);
        }
        
        private object[] OnEndGatAvailablePlayLobbies(System.IAsyncResult result) {
            WpfApplication1.ServiceReference1.PlayerLobby[] retVal = this.EndGatAvailablePlayLobbies(result);
            return new object[] {
                    retVal};
        }
        
        private void OnGatAvailablePlayLobbiesCompleted(object state) {
            if ((this.GatAvailablePlayLobbiesCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.GatAvailablePlayLobbiesCompleted(this, new GatAvailablePlayLobbiesCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void GatAvailablePlayLobbiesAsync() {
            this.GatAvailablePlayLobbiesAsync(null);
        }
        
        public void GatAvailablePlayLobbiesAsync(object userState) {
            if ((this.onBeginGatAvailablePlayLobbiesDelegate == null)) {
                this.onBeginGatAvailablePlayLobbiesDelegate = new BeginOperationDelegate(this.OnBeginGatAvailablePlayLobbies);
            }
            if ((this.onEndGatAvailablePlayLobbiesDelegate == null)) {
                this.onEndGatAvailablePlayLobbiesDelegate = new EndOperationDelegate(this.OnEndGatAvailablePlayLobbies);
            }
            if ((this.onGatAvailablePlayLobbiesCompletedDelegate == null)) {
                this.onGatAvailablePlayLobbiesCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGatAvailablePlayLobbiesCompleted);
            }
            base.InvokeAsync(this.onBeginGatAvailablePlayLobbiesDelegate, null, this.onEndGatAvailablePlayLobbiesDelegate, this.onGatAvailablePlayLobbiesCompletedDelegate, userState);
        }
        
        public WpfApplication1.ServiceReference1.Player AddPlayer(WpfApplication1.ServiceReference1.Player pl) {
            return base.Channel.AddPlayer(pl);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public System.IAsyncResult BeginAddPlayer(WpfApplication1.ServiceReference1.Player pl, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginAddPlayer(pl, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public WpfApplication1.ServiceReference1.Player EndAddPlayer(System.IAsyncResult result) {
            return base.Channel.EndAddPlayer(result);
        }
        
        private System.IAsyncResult OnBeginAddPlayer(object[] inValues, System.AsyncCallback callback, object asyncState) {
            WpfApplication1.ServiceReference1.Player pl = ((WpfApplication1.ServiceReference1.Player)(inValues[0]));
            return this.BeginAddPlayer(pl, callback, asyncState);
        }
        
        private object[] OnEndAddPlayer(System.IAsyncResult result) {
            WpfApplication1.ServiceReference1.Player retVal = this.EndAddPlayer(result);
            return new object[] {
                    retVal};
        }
        
        private void OnAddPlayerCompleted(object state) {
            if ((this.AddPlayerCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.AddPlayerCompleted(this, new AddPlayerCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void AddPlayerAsync(WpfApplication1.ServiceReference1.Player pl) {
            this.AddPlayerAsync(pl, null);
        }
        
        public void AddPlayerAsync(WpfApplication1.ServiceReference1.Player pl, object userState) {
            if ((this.onBeginAddPlayerDelegate == null)) {
                this.onBeginAddPlayerDelegate = new BeginOperationDelegate(this.OnBeginAddPlayer);
            }
            if ((this.onEndAddPlayerDelegate == null)) {
                this.onEndAddPlayerDelegate = new EndOperationDelegate(this.OnEndAddPlayer);
            }
            if ((this.onAddPlayerCompletedDelegate == null)) {
                this.onAddPlayerCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnAddPlayerCompleted);
            }
            base.InvokeAsync(this.onBeginAddPlayerDelegate, new object[] {
                        pl}, this.onEndAddPlayerDelegate, this.onAddPlayerCompletedDelegate, userState);
        }
    }
}
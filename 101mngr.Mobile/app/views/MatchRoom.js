import React from 'react';
import { StyleSheet, View, AsyncStorage, ScrollView, RefreshControl, ActivityIndicator } from 'react-native';
import { Text, Card, ListItem, Button } from 'react-native-elements'
import { Environment } from '../Environment';
import { HubConnectionBuilder } from '@aspnet/signalr';

export class MatchRoom extends React.Component {
    static navigationOptions = {
      title: 'Match Room',
    };

    constructor(props) {
        super(props);
        this.state = {
            id: 0,
            name: "Match Room",
            createdAt: null,
            playerList: [],
            playerId: null,
            refreshing: false,
            players: [],
            virtualPlayers: [],
            ownerId: "",
            ownerName: "",
            isOwner: false,
            matchStarted: false,
            loading: true
        };

        this.connection = new HubConnectionBuilder().withUrl(`${Environment.API_URI}/rooms`).build();
    }

    componentDidMount = async () => {
        let matchId = this.props.navigation.getParam('matchId');
        
        this.setState({id:matchId});
                
        await this.connection.start();
        var data = await this.connection.invoke("GetMatchRoom",matchId);
        var owner = data.players.find(item => item.id == data.ownerPlayerId);        
        this.setState({
            virtualPlayers:data.virtualPlayers,
            players:data.players,
            ownerId:owner.id,
            ownerName: owner.name,
            matchStarted: data.matchStarted
        });

        this.connection.on("PlayerJoinedRoom", this.onPlayerJoinedRoom);
            
        this.connection.on("PlayerLeftRoom", this.onPlayerLeftRoom);

        this.connection.on("MatchStarted", this.onMatchStarted);

        let token = await AsyncStorage.getItem('token');
        if (token != null) {
            let profileResponse = await fetch(`${Environment.API_URI}/api/account/profile`, {
                method: 'GET',
                headers: {
                    Authorization: token
                }});
            let profileJson = await profileResponse.json();
            this.setState({isOwner: profileJson.id == this.state.ownerId,playerId:profileJson.id});
        }

        this.setState({loading: false});
    }

    componentWillUnmount = async () => {
        await this.connection.stop();
    }
    
    _onRefresh = async () => {
        this.setState({refreshing: true});
        //await this._refreshMatchInfo();
        this.setState({refreshing: false});
    }

    _refreshMatchInfo = async () => {
        try {
            let matchId = this.props.navigation.getParam('matchId');
            let matchResponse = await fetch(`${Environment.API_URI}/api/match/${matchId}`);
            let matchJson = await matchResponse.json();
            this.setState({
              id: matchJson.id,
              name: matchJson.name,
              createdAt: matchJson.createdAt,
              playerList: matchJson.players
            });  
            let token = await AsyncStorage.getItem('token');
            let profileResponse = await fetch(`${Environment.API_URI}/api/account/profile`, {
                method: 'GET',
                headers: {
                    Authorization: token
                }});
            let profileJson = await profileResponse.json();
            this.setState({
                accountId: profileJson.id
              });
        } catch (error) {
            console.error(error);
        }
    }

    inPlayerList = () => {
        return this.state.playerList.some(x=>x.id === this.state.accountId);
    }

    onPlayerJoinedRoom = (matchRoomId, playerId, playerName) => {
        var players = this.state.players.concat({id: playerId, name:playerName});
        this.setState({players:players});
        console.log(`player ${playerId} ${playerName} joined room ${matchRoomId}`);
    }

    onPlayerLeftRoom = (matchRoomId, playerId) => {
        var players = this.state.players.filter(item => item.id !== playerId);
        this.setState({players:players});
        console.log(`player ${playerId} left room ${matchRoomId}`);
    }

    onMatchStarted = (matchRoomId) => {
        if (this.state.id == matchRoomId) {            
            this.setState({matchStarted: true});
        }
    }
 
    playMatch = async () => {
        try {
            let token = await AsyncStorage.getItem('token');
            let response = await fetch(`${Environment.API_URI}/api/match/${this.state.id}/start`, {
                method: 'PUT',
                headers: {
                    Authorization: token
                }});
            this.props.navigation.navigate('MatchInfo', {matchId: this.state.id});

        } catch (error) { 
            console.error(error);
        }
    }

    showMatch = async () => {
        try {
            this.props.navigation.navigate('MatchInfo', {matchId: this.state.id});

        } catch (error) { 
            console.error(error);
        }
    }

    joinMatch = async () => {
        try {
            let token = await AsyncStorage.getItem('token');
            let profileResponse = await fetch(`${Environment.API_URI}/api/account/profile`, {
                method: 'GET',
                headers: {
                    Authorization: token
                }});
            let profileJson = await profileResponse.json();

            var matchRoomId = this.state.id;
            var playerId = profileJson.id;
            var playerName = `${profileJson.firstName} ${profileJson.lastName}`.trim();
            this.connection.invoke("JoinRoom", matchRoomId, playerId, playerName);
            
        } catch (error) {
           console.error(error);
        }
    }

    leaveMatch = async () => {
        try {
            let token = await AsyncStorage.getItem('token');
            let profileResponse = await fetch(`${Environment.API_URI}/api/account/profile`, {
                method: 'GET',
                headers: {
                    Authorization: token
                }});
            let profileJson = await profileResponse.json();

            var matchRoomId = this.state.id;
            var playerId = profileJson.id;
            this.connection.invoke("LeaveRoom", matchRoomId, playerId);

            this.props.navigation.navigate('MatchList');
        } catch (error) {
            console.error(error);
        }
    }

    canJoin = () => {
        if (this.state.playerId == null) {
            return false;
        }
        var player = this.state.players.find(x => x.id == this.state.playerId);
        return player == null;
    }

    canLeave = () => {
        if (this.state.playerId == null) {
            return false;
        }
        var player = this.state.players.find(x => x.id == this.state.playerId);
        return player != null;
    }

    render () {
        const { navigate } = this.props.navigation;

        return (
            
            <View style={{flex:1}}>
                {this.state.loading && 
                    <View style={styles.loadingContainer}>
                        <ActivityIndicator size="large" color="#0000ff" />
                    </View>
                }
                {!this.state.loading && 
            
                    <ScrollView>
                        <Card title='Match details' containerStyle={{paddingHorizontal: 0}} >
                            <ListItem title={this.state.name} subtitle='Name' containerStyle={{paddingTop: 0}} />
                            <ListItem title={this.state.ownerName} subtitle='Owner' containerStyle={{paddingTop: 0}} />
                        </Card>
                        <Card title='Players' containerStyle={{paddingHorizontal: 0}} >
                        {
                            this.state.virtualPlayers.map((u, i) => {
                                return (
                                    <ListItem
                                        key={this.state.players[i] != null ? this.state.players[i].id : u.id}
                                        title={this.state.players[i] != null ? this.state.players[i].name : u.name}
                                        containerStyle={{paddingVertical:0}} />);
                                })
                        }
                        </Card>

                        <Button title="Join"  onPress={this.joinMatch} underlayColor='#31e981' containerStyle={{margin:10}} disabled={!this.canJoin() || this.state.matchStarted} />
                        <Button title="Leave"  onPress={this.leaveMatch} underlayColor='#31e981' containerStyle={{margin:10}} disabled={!this.canLeave() || this.state.matchStarted} />                
                        <Button title="Match" onPress={this.showMatch} underlayColor='#31e981' containerStyle={{margin:10}} disabled={!this.state.matchStarted}  />                
                        <Button title="Start" onPress={this.playMatch} underlayColor='#31e981' containerStyle={{margin:10}} disabled={!this.state.isOwner || this.state.matchStarted}  />
                    </ScrollView>
                }
                </View>
        );
    }
}

const styles = StyleSheet.create({
    container: {
        flex: 1,
        alignItems:'center',
        paddingBottom: '10%',
        paddingTop: '10%',
    },
    selectedPlayer: {
        flex:1,
        backgroundColor: '#008000'
    },
    heading: {
        fontSize: 16,
        margin:10
    },
    inputs: {
        flex:1,
        width: '80%',
        padding: 10
    },
    buttons:{
        marginTop:15,
        fontSize:16
    },
    labels: {
        paddingBottom: 10
    },
    alternativeLayoutButtonContainer: {
      flexDirection: 'row',
      justifyContent: 'space-between',
      alignItems:'center',
      width: '75%'
    },
    loadingContainer: {
        flex: 1,
        justifyContent: 'center',
        alignItems: 'center'
      }
});
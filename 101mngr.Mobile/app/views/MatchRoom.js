import React from 'react';
import { StyleSheet, View, AsyncStorage, ScrollView, RefreshControl, FlatList } from 'react-native';
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
            accountId: '',
            refreshing: false,
            players: [],
            virtualPlayers: [],
            ownerId: "",
            ownerName: ""
        };

        this.connection = new HubConnectionBuilder().withUrl(`${Environment.API_URI}/rooms`).build();     
    }

    componentDidMount = async () => {
        let matchId = this.props.navigation.getParam('matchId');
        
        this.setState({id:matchId});
                
        this.connection.start().then(() => {            

            this.connection.invoke("GetMatchRoom",matchId).then((data) => {
                var owner = data.players.find(item => item.id == data.ownerPlayerId);
                console.log(owner);
                this.setState({
                    virtualPlayers:data.virtualPlayers,
                    players:data.players,
                    ownerId:owner.id,
                    ownerName: owner.name
                });
            }).catch(function (err) {
                return console.error(err.toString());
            });  
        });

        this.connection.on("PlayerJoinedRoom", this.onPlayerJoinedRoom);
            
        this.connection.on("PlayerLeftRoom", this.onPlayerLeftRoom);
            
        console.log(this.state);
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

    render () {
        const { navigate } = this.props.navigation;

        return (
            
            <ScrollView refreshControl={
                <RefreshControl
                  refreshing={this.state.refreshing}
                  onRefresh={this._onRefresh}
                />
              }>
                <Card title='Match details' containerStyle={{paddingHorizontal: 0}} >
                    <ListItem title={this.state.name} subtitle='Name' containerStyle={{paddingTop: 0}} />
                    <ListItem title={this.state.ownerName} subtitle='Owner' containerStyle={{paddingTop: 0}} />
                </Card>
                {/* <ListItem key={this.state.name} title={this.state.name} subtitle='Name' containerStyle={{margin:0}} bottomDivider />
                <ListItem key={this.state.createdAt} title={this.state.createdAt} subtitle='Created at' containerStyle={{margin:0}} bottomDivider /> */}
                <Card title='Players' containerStyle={{paddingHorizontal: 0}} >
                {
                    this.state.virtualPlayers.map((u, i) => {
                        return (
                            <ListItem
                                key={this.state.players[i] != null ? this.state.players[i].id : u.id}
                                title={this.state.players[i] != null ? this.state.players[i].name : u.name}
                                containerStyle={{paddingVertical:0}} />);
                        })
                    // this.state.playerList.map((u, i) => {
                    // return (
                    //     <ListItem
                    //         key={u.player != null ? u.player.id : u.virtualPlayer.id}
                    //         title={u.player != null ? u.player.name : u.virtualPlayer.name}
                    //         containerStyle={{paddingVertical:0}} />);
                    // })
                }
                </Card>

                <Button title="Join"  onPress={this.joinMatch} underlayColor='#31e981' containerStyle={{margin:10}}  />
                <Button title="Leave"  onPress={this.leaveMatch} underlayColor='#31e981' containerStyle={{margin:10}}  />
                <Button title="Start" onPress={this.playMatch} underlayColor='#31e981' containerStyle={{margin:10}}  />
            </ScrollView>
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
    }
});
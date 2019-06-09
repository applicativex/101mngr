import React from 'react';
import { StyleSheet, Text, View, ScrollView, RefreshControl, TouchableHighlight, FlatList } from 'react-native';
import { ListItem, Button } from 'react-native-elements'
import { Environment } from '../Environment'
import { HubConnectionBuilder } from '@aspnet/signalr'

export class MatchRoomList extends React.Component {
    static navigationOptions = {
      title: 'Match Rooms',
    };

    constructor(props) {
        super(props);
        this.state = {
            isInvited: false,
            loggedUser: false,
            matchList: [],
            refreshing: false,
        };

        this.connection = new HubConnectionBuilder().withUrl(`${Environment.API_URI}/rooms`).build();
    }
    
    _onRefresh = async () => {
        this.setState({refreshing: true});
        this.setState({refreshing: false});
    }
    
    componentDidMount = async () => {
                
        this.connection.start().then(() => {            

            this.connection.invoke("GetMatchRooms").then((data) => {
                var rooms = data.map(x => x.matchId);
                this.setState({
                    matchList:rooms
                });
            }).catch(function (err) {
                return console.error(err.toString());
            });
        });

        this.connection.on("MatchRoomAdded", this.onMatchRoomAdded);
            
        this.connection.on("MatchRoomRemoved", this.onMatchRoomRemoved);
    }

    componentWillUnmount = async () => {
        await this.connection.stop();
    }

    onMatchRoomAdded = (matchRoomId, playerId) => {
        var rooms = this.state.matchList.concat(matchRoomId);
        this.setState({matchList:rooms});
        console.log(`added room ${matchRoomId}`);
    }

    onMatchRoomRemoved = (matchRoomId) => {
        var rooms = this.state.matchList.filter(item => item.id !== matchRoomId);
        this.setState({matchList:rooms});
        console.log(`removed room ${matchRoomId}`);
    }

    _refreshMatchList = async () => {
        try {
            let response = await fetch(`${Environment.API_URI}/api/match`);
            let responseJson = await response.json(); 

            console.log(responseJson);
            this.setState({
              matchList: responseJson,
            }, function(){
    
            });
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
                
                <FlatList   data={this.state.matchList}
                            keyExtractor={(item, index) => item}
                            renderItem={({item}) =>
                            <MatchItem
                                navigate={navigate}
                                id={item}
                                name={item} />
                            }    
                        />
            </ScrollView>
        );
    }
}

export class MatchItem extends React.Component {
    onPress = () => {
        //Alert.alert(this.props.name);
        this.props.navigate('MatchRoom', {matchId: this.props.id} );
    }

    render(){
        return(
            <ListItem title={this.props.name} onPress={this.onPress} bottomDivider />
        );
    }
}

export class PlayerList extends React.Component {
    onPress = () => {
        this.props.navigate('PlayerPickRT', {players: this.props.playersList} );
    }

    render(){
        return(
            
            <ScrollView refreshControl={
                <RefreshControl
                  refreshing={this.state.refreshing}
                  onRefresh={this._onRefresh}
                />
              }>
                <TouchableHighlight onPress={this.onPress} underlayColor='#31e981'>
                    <Text style={styles.buttons}>Pick Players</Text>
                </TouchableHighlight>
                <FlatList style={{flex:1, margin: 10, justifyContent: 'space-between',width:'100%'}}                            
                            data={this.props.playersList}
                            renderItem={({item})=>
                                <Text style={{width:'100%'}}>{item.userName}</Text>
                            }
                        />
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
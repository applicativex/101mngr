import React from 'react';
import { StyleSheet, Text, View, ScrollView, RefreshControl, TouchableHighlight, FlatList } from 'react-native';
import {PlayersData} from '../data/Players.js';
import { ListItem, Button } from 'react-native-elements'
import { Environment } from '../Environment'
import { HubConnectionBuilder } from '@aspnet/signalr'

export class MatchList extends React.Component {
    static navigationOptions = {
      title: 'Current Matches',
    };

    constructor(props) {
        super(props);
        this.state = {
            isInvited: false,
            loggedUser: false,
            matchList: [],
            refreshing: false,
        };

        this.connection = new HubConnectionBuilder().withUrl(`${Environment.API_URI}/matches`).build();
    }
    
    _onRefresh = async () => {
        this.setState({refreshing: true});
        //await this._refreshMatchList();
        this.setState({refreshing: false});
      }
    

    componentDidMount = async () => {
        // await this._refreshMatchList();
                
        this.connection.start().then(() => {            

            this.connection.invoke("GetCurrentMatches").then((data) => {
                this.setState({
                    matchList:data
                });
            }).catch(function (err) {
                return console.error(err.toString());
            });

            this.subscription = this.connection.stream("GetCurrentMatchesStream").subscribe({
              close: false,
              next: this.onMatchUpdate,
              error: function (err) {
                  console.log(err);
              }
            });  
        });
    }

    componentWillUnmount = async () => {
        this.subscription.dispose();
        await this.connection.stop();
    }
    
    onMatchUpdate = (match) => {
        var added = 1;
        var removed = 4;
        var list = [];
        if (match.matchListEventType == added) {
            list = this.state.matchList.concat(match);    
        } else if (match.matchListEventType == removed) {
            list = this.state.matchList.filter(item => item.id !== match.id);
        } else {
            list = this.state.matchList.map(item => item.id !== match.id ? item : match);
        }
        this.setState({matchList:list});
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

    complexName (item) {
        var suffix = item.matchPeriod == 2 ?
                        'HT' : item.matchPeriod == 4 ?
                                'FT' : '';
        return `${item.name} ${item.minute}' ${suffix}`.trim();
    }

    invitePlayers = () =>{
        this.setState({
            isInvited: true,
            playersList: Array.from(PlayersData.players)
        });
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
                            keyExtractor={(item, index) => item.id}
                            renderItem={({item}) =>
                            <MatchItem
                                navigate={navigate}
                                id={item.id}
                                name={this.complexName(item)} />
                            }    
                        />

            </ScrollView>
        );
    }
}

export class MatchItem extends React.Component {
    onPress = () => {
        //Alert.alert(this.props.name);
        this.props.navigate('MatchInfo', {matchId: this.props.id} );
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
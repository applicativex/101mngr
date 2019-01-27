import React from 'react';
import { StyleSheet, Text, View, ScrollView, RefreshControl, AsyncStorage, Alert, TouchableHighlight, FlatList, TouchableWithoutFeedback, Button } from 'react-native';
import {PlayersData} from '../data/Players.js';
import { PlayerListItem } from '../sections/PlayerListItem.js';

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
    }
    
    _onRefresh = async () => {
        this.setState({refreshing: true});
        await this._refreshMatchList();
        this.setState({refreshing: false});
      }
    

    componentDidMount = async () => {
        await this._refreshMatchList();
    }

    _refreshMatchList = async () => {
        try {
            let response = await fetch('http://35.228.60.109/api/match');
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

    invitePlayers = () =>{
        this.setState({
            isInvited: true,
            playersList: Array.from(PlayersData.players)
        });
    }

    newMatch = () => {
        this.props.navigation.navigate('NewMatch');
    }

    render () {
        const { navigate } = this.props.navigation;

        return (
            
            <ScrollView contentContainerStyle={styles.container} refreshControl={
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
                                name={item.name} />
                            }    
                        />

                <Button title="New Match" onPress={this.newMatch} underlayColor='#31e981'  />
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
            <TouchableWithoutFeedback onPress={this.onPress}>
                <View style={{paddingTop:20,alignItems:'center'}}>
                    <Text>
                        {this.props.name}
                    </Text>
                </View>
            </TouchableWithoutFeedback>
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
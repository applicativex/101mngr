import React from 'react';
import { StyleSheet, Text, View, Image, AsyncStorage, Alert, TouchableHighlight, FlatList } from 'react-native';
import {PlayersData} from '../data/Players.js';
import { PlayerListItem } from '../sections/PlayerListItem.js';

export class NewMatch extends React.Component {
    staticnavigationOptions = {
        header: null
    };

    constructor(props) {
        super(props);
        this.state = {
            isInvited: false,
            loggedUser: false,
            matchList: []
        };
    }

    componentDidMount(){
        return fetch('http://192.168.0.101:80/api/match')
          .then((response) => response.json())
          .then((responseJson) => {

            console.log(responseJson);
            this.setState({
              matchList: responseJson,
            }, function(){
    
            });
    
          })
          .catch((error) =>{
            console.error(error);
          });
    }

    newMatch(){
        fetch('http://192.168.0.101:80/api/match', {
            method: 'POST',
            headers: {
                Accept: 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                firstParam: 'yourValue',
                secondParam: 'yourOtherValue',
            }),
        });
    }

    invitePlayers = () =>{
        this.setState({
            isInvited: true,
            playersList: Array.from(PlayersData.players)
        });
    }

    newMatch = () => {
        this.props.navigation.navigate('CreateMatchRT');
    }

    render () {
        const { navigate } = this.props.navigation;

        return (
            <View style={styles.container}>
                <TouchableHighlight onPress={this.newMatch} underlayColor='#31e981'>
                    <Text style={styles.buttons}>New Match</Text>
                </TouchableHighlight>

                <Text style={styles.heading}>Current Matches</Text>

                <FlatList style={{flex:1, margin: 10}}
                            data={this.state.matchList}
                            renderItem={({item})=>
                                <Text>{item.name}</Text>
                            }
                        />
                
                <TouchableHighlight onPress={this.newMatch} underlayColor='#31e981'>
                    <Text style={styles.buttons}>New Match</Text>
                </TouchableHighlight>
            </View>
        );
    }
}

export class PlayerList extends React.Component {
    onPress = () => {
        this.props.navigate('PlayerPickRT', {players: this.props.playersList} );
    }

    render(){
        return(
            <View>
                <TouchableHighlight onPress={this.onPress} underlayColor='#31e981'>
                    <Text style={styles.buttons}>Pick Players</Text>
                </TouchableHighlight>
                <FlatList style={{flex:1, margin: 10}}
                            data={this.props.playersList}
                            renderItem={({item})=>
                                <Text>{item.userName}</Text>
                            }
                        />
            </View>
        );
    }
}

const styles = StyleSheet.create({
    container: {
        flex: 1,
        alignItems: 'center'
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
    }
});
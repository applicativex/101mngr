import React from 'react';
import { StyleSheet, Text, View, Image, AsyncStorage, Alert, TouchableHighlight, FlatList } from 'react-native';
import {PlayersData} from '../data/Players.js';
import { PlayerListItem } from '../sections/PlayerListItem.js';

export class PlayerPick extends React.Component {
    staticnavigationOptions = {
        header: null
    };

    constructor(props) {
        super(props);
        this.state = {
            isPicked: false,
            loggedUser: false
        };
    }

    invitePlayers = () =>{
        this.setState({
            isInvited: true,
            playersList: Array.from(PlayersData.players)
        });
    }

    randomPick = (players) => { 
        var team1 = [];
        var team2 = [];
        for(i = 0; i < 22; i++){
            if(i < 11) {
                team1.push(players[i]);
            } 
            else {
                team2.push(players[i]);
            }
        }
        this.setState({isPicked:true, team1:team1, team2:team2});
    }

    startMatch = () => {
        this.props.navigation.navigate('StartMatchRT');
    }

    render () {
        const { navigate } = this.props.navigation;
        let players = this.props.navigation.getParam('players');

        return (
            <View style={styles.container}>

                {!this.state.isPicked && (
                    <View>    
                        <TouchableHighlight navigate={navigate} onPress={()=>this.randomPick(players)} underlayColor='#31e981'>
                            <Text style={styles.buttons}>Random Pick</Text>
                        </TouchableHighlight>
                        <FlatList style={{flex:1, margin: 10}}
                                    data={players}
                                    renderItem={({item})=>
                                    <Text>{item.userName}</Text>
                                    }
                                />
                    </View>
                )}

                {this.state.isPicked && (
                    <View>
                        <TouchableHighlight navigate={navigate} onPress={this.startMatch} underlayColor='#31e981'>
                            <Text style={styles.buttons}>Start Match</Text>
                        </TouchableHighlight>

                        <Text style={styles.buttons}>Team 1</Text>
                        <FlatList style={{flex:1, margin: 10}}
                                    data={this.state.team1}
                                    renderItem={({item})=>
                                    <Text>{item.userName}</Text>
                                    }
                                />

                        <Text style={styles.buttons}>Team 2</Text>
                        <FlatList style={{flex:1, margin: 10}}
                                    data={this.state.team2}
                                    renderItem={({item})=>
                                    <Text>{item.userName}</Text>
                                    }
                                />
                    </View>
                )}
            </View>
        )
    }
}

const styles = StyleSheet.create({
    container: {
        flex: 1,
        alignItems: 'center'
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
    }
});
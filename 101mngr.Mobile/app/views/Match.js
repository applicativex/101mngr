import React from 'react';
import { StyleSheet, Text, View, Image, AsyncStorage, Alert, TouchableHighlight, FlatList } from 'react-native';
import {PlayersData} from '../data/Players.js';
import { PlayerListItem } from '../sections/PlayerListItem.js';

export class Match extends React.Component {
    staticnavigationOptions = {
        header: null
    };

    constructor(props) {
        super(props);
        this.state = {
            isInvited: false,
            loggedUser: false
        };
    }

    // componentDidMount(){
    //     return fetch('http://192.168.0.101:80/api/match')
    //       .then((response) => response.json())
    //       .then((responseJson) => {
    

    //         console.log(responseJson);
    //         // this.setState({
    //         //   isLoading: false,
    //         //   dataSource: responseJson.movies,
    //         // }, function(){
    
    //         // });
    
    //       })
    //       .catch((error) =>{
    //         console.error(error);
    //       });
    // }

    invitePlayers = () =>{
        this.setState({
            isInvited: true,
            playersList: Array.from(PlayersData.players)
        });
    }

    pickPlayers = () => {
        this.props.navigate('PlayerPickRT', {players: this.state.playersList} );
    }

    render () {
        const { navigate } = this.props.navigation;

        return (
            <View style={styles.container}>
                <Text style={styles.heading}>Start Match</Text>

                {!this.state.isInvited && (
                    <TouchableHighlight onPress={this.invitePlayers} underlayColor='#31e981'>
                        <Text style={styles.buttons}>Invite Players</Text>
                    </TouchableHighlight>
                )}
                
                {this.state.isInvited && (
                    <PlayerList navigate={navigate} playersList={this.state.playersList} />
                )}
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
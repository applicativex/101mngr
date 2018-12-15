import React from 'react';
import { StyleSheet, Text, View, Image, AsyncStorage, Alert, TouchableHighlight, FlatList } from 'react-native';
import {PlayersData} from '../data/Players.js';
import { PlayerListItem } from '../sections/PlayerListItem.js';

export class StartMatch extends React.Component {
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

    endMatch = () => {
        this.props.navigation.navigate('EndMatchRT', {winner: ((Math.floor(Math.random() * 10) % 2) + 1)});
    }

    render () {
        const { navigate } = this.props.navigation;

        return (
            <View style={styles.container}>
                <TouchableHighlight navigate={navigate} onPress={this.endMatch} underlayColor='#31e981'>
                    <Text style={styles.buttons}>End Match</Text>
                </TouchableHighlight>
            </View>
        );
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
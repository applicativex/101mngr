import React from 'react';
import { StyleSheet, Text, View, Image, AsyncStorage, Alert, TouchableHighlight, FlatList } from 'react-native';
import {PlayersData} from '../data/Players.js';
import { PlayerListItem } from '../sections/PlayerListItem.js';

export class EndMatch extends React.Component {
    staticnavigationOptions = {
        header: null
    };

    constructor(props) {
        super(props);
    }

    render () {
        const { navigate } = this.props.navigation;
        let winner = this.props.navigation.getParam('winner');

        return (
            <View style={styles.container}>
                <Text style={styles.heading}>Winner Team {winner}</Text>
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
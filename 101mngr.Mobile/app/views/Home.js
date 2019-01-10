import React from 'react';
import { StyleSheet, Text, View } from 'react-native';
import { Menu } from '../sections/Menu.js';
import { Header } from '../sections/Header.js';

export class Home extends React.Component {
    static navigationOptions = {
      title: 'Home',
    };

    render () {
        const { navigate } = this.props.navigation;

        return (
            <View style={styles.container}>
            </View>
        );
    }
}

const styles = StyleSheet.create({
    container: {
        flex: 1,
        alignItems: 'center',
        paddingBottom: '45%',
        paddingTop: '10%'
    },
    heading: {
        fontSize: 16,
        flex: 1
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
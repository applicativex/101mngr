import React from 'react';
import { StyleSheet, Text, View } from 'react-native';
import { Menu } from '../sections/Menu.js';
import { Header } from '../sections/Header.js';

export class Home extends React.Component {
    static navigationOptions = {
        header: null
    };

    render () {
        const { navigate } = this.props.navigation;

        return (
            <View style={styles.container}>
                <Header navigate={navigate} message = 'Press to Login' />
                <Menu navigate = {navigate} />
            </View>
        );
    }
}

const styles = StyleSheet.create({
    container: {
        flex: 1
    }
});
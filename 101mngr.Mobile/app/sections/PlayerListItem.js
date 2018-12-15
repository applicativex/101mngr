import React from 'react';
import { StyleSheet, Text, View, TouchableHighlight, Alert } from 'react-native';

export class PlayerListItem extends React.Component {
    static navigationOptions = {
        header: null
    };

    constructor(props) {
        super(props);
        this.state = {
            selected: false,
            correct: false
        }
    }

    choosePlayer = (userName) => {
        this.setState({selected:!this.state.selected});
        console.log(this.state.selected);
    }

    render() {
        return (
            <View style={styles.container}>
                {!this.state.selected && (
                    <TouchableHighlight onPress={()=>this.choosePlayer(this.props.player.userName)} underlayColor='#31e981'>
                        <Text>{this.props.player.userName}</Text>
                    </TouchableHighlight>
                )}
                {this.state.selected && (
                    <TouchableHighlight style={styles.selectedPlayer} onPress={()=>this.choosePlayer(this.props.player.userName)} underlayColor='#31e981'>
                        <Text>{this.props.player.userName}</Text>
                    </TouchableHighlight>
                )}
            </View>
        );
    }
}

const styles = StyleSheet.create({
    container: {
        flex: 1,
        paddingTop: 20
    },
    correctContainer: {
        flex: 1,
        paddingTop: 20,
        backgroundColor: '#008000'
    },
    wrongContainer: {
        flex: 2,
        paddingTop: 20,
        backgroundColor: '#ff0000'
    },
    selectedPlayer: {
        flex:1,
        backgroundColor: '#008000'
    },
    answerText: {
        flex:2,
        padding:15,
        fontSize: 20,
        textAlign: 'center'
    }
});
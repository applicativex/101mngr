import React from 'react';
import {
    StyleSheet,
    Text,
    View,
    TextInput,
    TouchableHighlight,
    Alert,
    AsyncStorage
} from 'react-native';

export class CreateMatch extends React.Component {

    static navigationOptions = {
        header: null
    }

    constructor(props) {
        super(props);
        this.state = {
            name: ''
        }
    }

    cancel = () => {
        console.log('match create cancel');
        this.props.navigation.navigate('NewMatchRT');
    }

    createMatch = () => {
        
        fetch('http://192.168.0.101:80/api/match/new', {
            method: 'POST',
            headers: {
                Accept: 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                playerId: 1,
                matchName: this.state.name,
            }),
        })
        .then((response) => response.json())
        .then((responseJson) => {

          console.log(responseJson.id);  
          this.props.navigation.navigate('MatchInfoRT', {matchId: responseJson.id});
        })
        .catch((error) =>{
          console.error(error);
        });
        console.log("Match created");
    }

    render(){
        return(
            <View style={styles.container}>
                <Text style={styles.heading}>Create Match</Text>

                <TextInput
                    style={styles.inputs}
                    onChangeText={(text) => this.setState({name: text})}
                    value={this.state.name}
                />
                <Text style={styles.label}>Match name</Text>

                <TouchableHighlight onPress={this.createMatch} underlayColor='#31e981'>
                    <Text style={styles.buttons}>Create</Text>
                </TouchableHighlight>

                <TouchableHighlight onPress={this.cancel} underlayColor='#31e981'>
                    <Text style={styles.buttons}>Cancel</Text>
                </TouchableHighlight>

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
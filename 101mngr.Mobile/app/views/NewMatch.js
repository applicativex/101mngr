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
import { Input, Button } from 'react-native-elements'
import { Environment } from '../Environment'

export class NewMatch extends React.Component {

    static navigationOptions = {
        title: 'New Match',
      };

    constructor(props) {
        super(props);
        this.state = {
            name: ''
        }
    }

    cancel = () => {
        console.log('match create cancel');
        this.props.navigation.navigate('MatchList');
    }

    createMatch = async () => {
        try {
            let token = await AsyncStorage.getItem('token');
            var response = await fetch(`${Environment.API_URI}/api/match/new`, {
                method: 'POST',
                headers: {
                    Accept: 'application/json',
                    'Content-Type': 'application/json',
                    Authorization: token
                },
                body: JSON.stringify({
                    playerId: 1,
                    matchName: this.state.name,
                }),
            });
            let responseJson = await response.json();
            console.log(responseJson.id);
            this.props.navigation.navigate('MatchList');
        } catch (error) {
            console.error(error);
        }
    }

    render() {
        return (
            <View style={styles.container}>
                <Input
                    containerStyle={{ margin: 10, marginTop: 100, width:'75%' }}
                    placeholder='Match name'
                    onChangeText={(text) => this.setState({ name: text })}
                    value={this.state.name}
                    />
                
                <Button title="Create" onPress={this.createMatch} underlayColor='#31e981' containerStyle={{margin:10, marginTop: 20, width:'75%'}}  />

            </View>
        );
    }
}

const styles = StyleSheet.create({
    container: {
        flex: 1,
        alignItems:'center',
        margin: '10%',
        paddingBottom: '10%',
        paddingTop: '10%',

    },
    label:{
        width:'100%'
    },
    input:{
        width:'100%',
        margin:'15%'
    },
    heading: {
        fontSize: 16
    },
    alternativeLayoutButtonContainer: {
      flexDirection: 'row',
      justifyContent: 'space-between',
      alignItems:'center',
      width: '50%'
    }
});
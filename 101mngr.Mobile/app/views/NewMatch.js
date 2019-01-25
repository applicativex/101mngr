import React from 'react';
import {
    StyleSheet,
    Text,
    View,
    TextInput,
    TouchableHighlight,
    Alert,
    AsyncStorage,
    Button
} from 'react-native';

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
            var response = await fetch('http://35.228.60.109/api/match/new', {
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

                <TextInput
                    style={styles.input}
                    onChangeText={(text) => this.setState({ name: text })}
                    value={this.state.name}
                />

                <View style={styles.alternativeLayoutButtonContainer}>
                    <Button style={{margin:'20%'}} title="Create" onPress={this.createMatch} underlayColor='#31e981'  />

                    <Button style={{margin:'20%'}} title="Cancel" onPress={this.cancel} />
                </View>

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
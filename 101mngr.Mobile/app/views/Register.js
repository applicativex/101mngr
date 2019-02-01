import React from 'react';
import {
    StyleSheet,
    Text,
    View,
    TextInput,
    TouchableHighlight,
    Alert,
    AsyncStorage,
} from 'react-native';
import { Input, Button } from 'react-native-elements'

export class Register extends React.Component {

    static navigationOptions = {
        title: 'Register',
      };

    constructor(props) {
        super(props);
        this.state = {
            username: '',
            passwrd: '',
            passwrdConfirm: '',
            email: ''
        }
    }

    _registerAsync = async () => {
        if (!this.state.username){
            Alert.alert('Please enter a username');
        } else if(this.state.passwrd !== this.state.passwrdConfirm){
            Alert.alert('Passwords do not match');
        }else {
        
            try {
                var response = await fetch('http://35.228.60.109/api/account/register', {
                    method: 'POST',
                    headers: {
                        Accept: 'application/json',
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify({
                        email: this.state.email,
                        userName: this.state.username,
                        password: this.state.passwrd
                    }),
                });
                var responseJson = await response.json();
                console.log(responseJson.id);  
                this.props.navigation.navigate('SignIn');
            } catch (error) {
                console.error(error);
            }
        }
    }

    render () {
        return (
            <View style={styles.container}>
                
                <Input
                    containerStyle={{ margin: 10, marginTop: 50, width:'75%' }}
                    placeholder='User name'
                    onChangeText={(text) => this.setState({ username: text })}
                    value={this.state.username}
                    />
                
                <Input
                    containerStyle={{ margin: 10, width:'75%' }}
                    placeholder='Email'
                    onChangeText={(text) => this.setState({ email: text })}
                    value={this.state.email}
                    />
                
                <Input
                    containerStyle={{ margin: 10, width:'75%' }}
                    placeholder='Password'
                    onChangeText={(text) => this.setState({ passwrd: text })}
                    value={this.state.passwrd}
                    secureTextEntry={true}
                    />
                
                <Input
                    containerStyle={{ margin: 10, width:'75%' }}
                    placeholder='Confirm Password'
                    onChangeText={(text) => this.setState({ passwrdConfirm: text })}
                    value={this.state.passwrdConfirm}
                    secureTextEntry={true}
                    />

                <Button title="Sign up" onPress={this._registerAsync} containerStyle={{margin:10, marginTop: 20, width:'75%'}} />

            </View>
        );
    }
}

const styles = StyleSheet.create({
    container: {
        flex: 1,
        alignItems: 'center',
        paddingBottom: '35%',
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
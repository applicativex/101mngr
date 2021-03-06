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
import { Environment } from '../Environment'

export class Login extends React.Component {

    static navigationOptions = {
        title: 'Login',
      };

    constructor(props) {
        super(props);
        this.state = {
            username: '',
            passwrd: ''
        }
    }

    cancelLogin = () => {
        Alert.alert('Login cancelled');
        this.props.navigation.navigate('HomeRT');
    }

    loginUser = () => {
        if(!this.state.username){
            Alert.alert('Please enter a username');
        }
        else if(!this.state.passwrd) {
            Alert.alert('Please enter a passwrd');
        }
        else{

            return fetch(`${Environment.API_URI}/api/account/login`, {
                                        method: 'POST',
                                        headers: {
                                            Accept: 'application/json',
                                            'Content-Type': 'application/json',
                                        },
                                        body: JSON.stringify({
                                            email: this.state.username,
                                            password: this.state.passwrd
                                        }),
                                    })
                                    .then((response) => response.json())
                                    .then((responseJson) => {
                                        console.log('lll '+responseJson.token);
                                        AsyncStorage.setItem('token', responseJson.token, (err,result)=>{
                                            Alert.alert(`${this.state.username} logged in`);
                                            this.props.navigation.navigate('HomeRT');
                                        })
                                        console.log(responseJson.token);  
                                        this.props.navigation.navigate('HomeRT');
                                    })
                                    .catch((error) =>{
                                        console.error(error);
                                    });

            // AsyncStorage.getItem('userLoggedIn', (err,result)=>{

            //     if(result !=='none'){
            //         Alert.alert('Someone already logged on');
            //         this.props.navigation.navigate('HomeRT');
            //     }
            //     else {
            //         AsyncStorage.getItem(this.state.username,(err,result)=>{
            //             if(result!==null){
            //                 if(result!==this.state.passwrd){
            //                     Alert.alert('Password incorrect');
            //                 }
            //                 else{
                                

            //                     AsyncStorage.setItem('userLoggedIn', this.state.username, (err,result)=>{
            //                         Alert.alert(`${this.state.username} logged in`);
            //                         this.props.navigation.navigate('HomeRT');
            //                     })
            //                 }
            //             }
            //             else {
            //                 Alert.alert(`No account for ${this.state.username}`);
            //             }
            //         })
            //     }
            // })
        }
    }

    render(){
        return(
            <View style={styles.container}>

                <TextInput
                    style={styles.inputs}
                    onChangeText={(text) => this.setState({username: text})}
                    value={this.state.username}
                />
                <Text style={styles.label}>Enter Username</Text>

                <TextInput
                    style={styles.inputs}
                    onChangeText={(text) => this.setState({passwrd: text})}
                    value={this.state.passwrd}
                    secureTextEntry={true}
                />
                <Text style={styles.label}>Enter Password</Text>

                <TouchableHighlight onPress={this.loginUser} underlayColor='#31e981'>
                    <Text style={styles.buttons}>Login</Text>
                </TouchableHighlight>

                <TouchableHighlight onPress={this.cancelLogin} underlayColor='#31e981'>
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
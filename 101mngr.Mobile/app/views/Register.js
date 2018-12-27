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

export class Register extends React.Component {

    static navigationOptions = {
        header: null
    }

    constructor(props) {
        super(props);
        this.state = {
            username: '',
            passwrd: '',
            passwrdConfirm: '',
            email: ''
        }
    }

    cancelRegister = () => {
        Alert.alert('Registration cancelled');
        this.props.navigation.navigate('HomeRT');
    }

    registerAccount = () =>{
        if (!this.state.username){
            Alert.alert('Please enter a username');
        } else if(this.state.passwrd !== this.state.passwrdConfirm){
            Alert.alert('Passwords do not match');
        }else {
        
            return fetch('http://35.228.60.109/api/account/register', {
                method: 'POST',
                headers: {
                    Accept: 'application/json',
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    email: this.state.email,
                    userName: this.state.username,
                    password: this.state.passwrd,
                    countryCode: "UA"
                }),
            })
            .then((response) => response.json())
            .then((responseJson) => {
    
              console.log(responseJson.id);  
              this.props.navigation.navigate('HomeRT');
            })
            .catch((error) =>{
              console.error(error);
            });
            // AsyncStorage.getItem(this.state.username, (err,result)=>{
            //     if (result!==null){
            //         Alert.alert(`${this.state.username} account exists`)
            //     }
            //     else{
            //         AsyncStorage.setItem(this.state.username, this.state.passwrd, (err,result=>{
            //             Alert.alert(`${this.state.username} account created`);
            //             this.props.navigation.navigate('HomeRT');
            //         }))
            //     }
            // });
        }
    }

    render () {
        return (
            <View style={styles.container}>
                <Text style={styles.heading}>Register Account</Text>

                <TextInput
                    style={styles.inputs}
                    onChangeText={(text) => this.setState({username: text})}
                    value={this.state.username}
                />
                <Text style={styles.label}>Enter Username</Text>

                <TextInput
                    style={styles.inputs}
                    onChangeText={(text) => this.setState({email: text})}
                    value={this.state.email}
                />
                <Text style={styles.label}>Enter Email</Text>

                <TextInput
                    style={styles.inputs}
                    onChangeText={(text) => this.setState({passwrd: text})}
                    value={this.state.passwrd}
                    secureTextEntry={true}
                />
                <Text style={styles.label}>Enter Password</Text>

                <TextInput
                    style={styles.inputs}
                    onChangeText={(text) => this.setState({passwrdConfirm: text})}
                    value={this.state.passwrdConfirm}
                    secureTextEntry={true}
                />
                <Text style={styles.label}>Confirm Password</Text>

                <TouchableHighlight onPress={this.registerAccount} underlayColor='#31e981'>
                    <Text style={styles.buttons}>Register</Text>
                </TouchableHighlight>

                <TouchableHighlight onPress={this.cancelRegister} underlayColor='#31e981'>
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
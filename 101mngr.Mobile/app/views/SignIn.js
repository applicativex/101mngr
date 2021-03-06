import React from 'react';
import {
    StyleSheet,
    View,
    AsyncStorage,
    TextInput
} from 'react-native';
import { Input, Button } from 'react-native-elements'
import { Environment } from '../Environment'

export class SignIn extends React.Component {
    static navigationOptions = {
      title: 'Please sign in',
    };

    constructor(props) {
        super(props);
        this.state = {
            username: '',
            passwrd: ''
        }
    }
      
    _showRegister = () => {
      this.props.navigation.navigate('Register');
    };
  
    _signInAsync = async () => {
      try {
        let response = await fetch(
            `${Environment.API_URI}/api/account/login`, {
                                            method: 'POST',
                                            headers: {
                                                Accept: 'application/json',
                                                'Content-Type': 'application/json',
                                            },
                                            body: JSON.stringify({
                                                email: this.state.username,
                                                password: this.state.passwrd
                                            }),
                                        }
          );
          let responseJson = await response.json(); 
          console.log(responseJson);
    
          await AsyncStorage.setItem('token', responseJson.token);
          console.log(responseJson);
          this.props.navigation.navigate('App');
      } catch (error) {
        console.error(error);          
      }
    };
  
    render() {
      return (
        <View style={styles.container}>
  
            <Input
                    containerStyle={{ margin: 10, marginTop: 100, width:'75%' }}
                    placeholder='User name'
                    onChangeText={(text) => this.setState({ username: text })}
                    value={this.state.username}
                    />

            <Input
                    containerStyle={{ margin: 10, width:'75%' }}
                    placeholder='Password'
                    onChangeText={(text) => this.setState({ passwrd: text })}
                    value={this.state.passwrd}
                    secureTextEntry={true}
                    />

          <Button title="Sign in" onPress={this._signInAsync} containerStyle={{margin:10, marginTop: 20, width:'75%'}} />
          <Button title="Sign up" onPress={this._showRegister} containerStyle={{margin:10, width:'75%'}} />
        </View>
      );
    }
  } 
  
  const styles = StyleSheet.create({
    container: {
      flex: 1,
      alignItems: 'center'
    },
    inputs: {
        width: '50%',
        margin: 10
    },
  });
import React from 'react';
import {
    StyleSheet,
    View,
    AsyncStorage,
    TextInput,
    Button
} from 'react-native';

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
            'http://35.228.60.109/api/account/login', {
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
          this.props.navigation.navigate('App');
      } catch (error) {
        console.error(error);          
      }
    };
  
    render() {
      return (
        <View style={styles.container}>

            <TextInput
                style={styles.inputs}
                onChangeText={(text) => this.setState({username: text})}
                value={this.state.username}
            />

            <TextInput
                style={styles.inputs}
                onChangeText={(text) => this.setState({passwrd: text})}
                value={this.state.passwrd}
                secureTextEntry={true}
            />

          <Button title="Sign in" onPress={this._signInAsync} />
          <Button title="Sign up" onPress={this._showRegister} />
        </View>
      );
    }
  }
  
  const styles = StyleSheet.create({
    container: {
      flex: 1,
      alignItems: 'center',
      justifyContent: 'center'
    },
    inputs: {
        width: '50%',
        margin: 10
    },
  });
import React from 'react';
import { StyleSheet, View, AsyncStorage } from 'react-native';
import { Button } from 'react-native-elements'
import { Environment } from '../Environment'

export class Home extends React.Component {
    static navigationOptions = {
      title: 'Home',
    };

    componentDidMount() {
      console.log(Environment.API_URI);
    }  
    
    render() {
      return (
        <View style={styles.container}>
          <Button title="Current matches" onPress={this._showCurrentMatches} containerStyle={{margin:10, marginTop: 20, width:'75%'}} />
          <Button title="Profile" onPress={this._showProfile} containerStyle={{margin:10, width:'75%'}} />
          <Button title="Leaderboard" onPress={this._showLeaderboard} containerStyle={{margin:10, width:'75%'}} />
          <Button title="Leagues" onPress={this._showLeagues} containerStyle={{margin:10, width:'75%'}} />
          <Button title="Training" onPress={this._showTraining} containerStyle={{margin:10, width:'75%'}} />
          <Button title="Sign out" onPress={this._signOutAsync} containerStyle={{margin:10, width:'75%'}} />
        </View>
      );
    }
   
    _showCurrentMatches = () => {
      this.props.navigation.navigate('MatchList');
    };
  
    _showLeaderboard = () => {
      this.props.navigation.navigate('Leaderboard');
    };
  
    _showLeagues = () => {
      this.props.navigation.navigate('Countries');
    };  
  
    _showProfile = () => {
      this.props.navigation.navigate('Profile');
    };

    _showTraining = () => {
      this.props.navigation.navigate('Training');
    };
  
    _signOutAsync = async () => {
      await AsyncStorage.clear();
      this.props.navigation.navigate('Auth');
    };
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
import React from 'react';
import { StyleSheet, Text, View, Button, AsyncStorage } from 'react-native';

export class Home extends React.Component {
    static navigationOptions = {
      title: 'Home',
    };

    render() {
      return (
        <View style={styles.container}>
          <Button title="Current matches" onPress={this._showCurrentMatches} />
          <Button title="Profile" onPress={this._showProfile} />
          <Button title="Leaderboard" onPress={this._showLeaderboard} />
          <Button title="Leagues" onPress={this._showLeagues} />
          <Button title="Sign out" onPress={this._signOutAsync} />
        </View>
      );
    }
  
    _showCurrentMatches = () => {
      this.props.navigation.navigate('MatchList');
    };
  
    _showLeaderboard = () => {
      //this.props.navigation.navigate('MatchHistory');
    };
  
    _showLeagues = () => {
      this.props.navigation.navigate('Countries');
    };
  
    _showProfile = () => {
      this.props.navigation.navigate('Profile');
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
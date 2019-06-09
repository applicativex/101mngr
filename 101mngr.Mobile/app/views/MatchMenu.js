import React from 'react';
import { StyleSheet, View, AsyncStorage } from 'react-native';
import { Button } from 'react-native-elements'
import { Environment } from '../Environment'

export class MatchMenu extends React.Component {
    static navigationOptions = {
      title: 'Matches',
    };

    componentDidMount() {
      console.log(Environment.API_URI);
    }  
    
    render() {
      return (
        <View style={styles.container}>
          <Button title="Current matches" onPress={this._showCurrentMatches} containerStyle={{margin:10, marginTop: 20, width:'75%'}} />
          <Button title="Finished matches" onPress={this._showFinishedMatches} containerStyle={{margin:10, width:'75%'}} />
          <Button title="Match rooms" onPress={this._showMatchRooms} containerStyle={{margin:10, width:'75%'}} />
          <Button title="New match" onPress={this._newMatch} containerStyle={{margin:10, width:'75%'}} />
        </View>
      );
    }
   
    _showCurrentMatches = () => {
      this.props.navigation.navigate('MatchList');
    };
  
    _showFinishedMatches = () => {
      this.props.navigation.navigate('FinishedMatches');
    };
  
    _showMatchRooms = () => {
      this.props.navigation.navigate('MatchRoomList');
    };  

    _newMatch = async () => {
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
                  playerId: 1
              }),
          });
          let responseJson = await response.json();
          this.props.navigation.navigate('MatchRoom', {matchId:responseJson.id});
      } catch (error) {
          console.error(error);
      }
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
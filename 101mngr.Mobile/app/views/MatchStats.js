import React from 'react';
import { View, Text, Button, StyleSheet } from 'react-native';

export class MatchStats extends React.Component {
    static navigationOptions = {
      title: 'Match Statistics',
    };

    constructor(props) {
        super(props);
        this.state = {
            id: 0,
            name: ""
        };
    }

    componentDidMount = async () => {
        try {
            let matchId = this.props.navigation.getParam('matchId');
            let matchResponse = await fetch(`http://35.228.60.109/api/match/${matchId}`);
            let matchJson = await matchResponse.json();
            this.setState({
              id: matchJson.id,
              name: matchJson.name
            });
            
        } catch (error) {
            console.error(error);
        }
    }
  
    _showHome = () => {
      this.props.navigation.navigate('Home');
    };

    render () {
        return (
            <View>
                <Text style={styles.heading}>Id: {this.state.id}</Text>
                <Text style={styles.heading}>Name: {this.state.name}</Text>
                        
                <Button title="Home" onPress={this._showHome} />

            </View>
        );
    }
}

const styles = StyleSheet.create({
    container: {
        flex: 1,
        alignItems:'center',
        paddingBottom: '10%',
        paddingTop: '10%',
    },
    selectedPlayer: {
        flex:1,
        backgroundColor: '#008000'
    },
    heading: {
        fontSize: 16,
        margin:10
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
    },
    alternativeLayoutButtonContainer: {
      flexDirection: 'row',
      justifyContent: 'space-between',
      alignItems:'center',
      width: '75%'
    }
});
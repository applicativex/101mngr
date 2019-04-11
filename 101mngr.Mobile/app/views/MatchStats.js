import React from 'react';
import { View, StyleSheet } from 'react-native';
import { Text, Card, ListItem, Button } from 'react-native-elements'
import { Environment } from '../Environment';

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
            let matchResponse = await fetch(`${Environment.API_URI}/api/match/${matchId}`);
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
                <Card title='Match details'>
                    <ListItem key={this.state.id.toString()} title={this.state.id.toString()} subtitle='Id' />
                    <ListItem key={this.state.name}  title={this.state.name} subtitle='Name' />
                </Card>

                <Button title="Home" onPress={this._showHome} containerStyle={{margin:10}} />

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
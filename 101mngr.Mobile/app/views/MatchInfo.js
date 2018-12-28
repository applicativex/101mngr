import React from 'react';
import { StyleSheet, Text, View, Button, AsyncStorage, Alert, TouchableHighlight, FlatList } from 'react-native';

export class MatchInfo extends React.Component {
    staticnavigationOptions = {
        header: null
    };

    constructor(props) {
        super(props);
        this.state = {
            id: 0,
            name: "",
            createdAt: null,
            playerList: []
        };
    }

    componentDidMount() {
        let matchId = this.props.navigation.getParam('matchId');
        return fetch(`http://35.228.60.109/api/match/${matchId}`)
          .then((response) => response.json())
          .then((responseJson) => {

            console.log(responseJson);
            this.setState({
              id: responseJson.id,
              name: responseJson.name,
              createdAt: responseJson.createdAt,
              playerList: responseJson.players
            }, function(){
    
            });
    
          })
          .catch((error) =>{
            console.error(error);
          });
    }

    playMatch = () => {
        return AsyncStorage.getItem('token', (err, result) => {
            if (result !== null) {
                return fetch(`http://35.228.60.109/api/match/${this.state.id}/start`, {
                    method: 'PUT',
                    headers: {
                        Authorization: result
                    }})
                    .then((response) => {
                        this.props.navigation.navigate('MatchHistoryRT');
                    })
                    .catch((error) => {
                        console.error(error);
                    })
            }
            else {
                Alert.alert(`Please login`);
            }
        });
    }

    render () {
        const { navigate } = this.props.navigation;

        return (
            <View style={styles.container}>

                <Text style={styles.heading}>Id: {this.state.id}</Text>
                <Text style={styles.heading}>Name: {this.state.name}</Text>
                <Text style={styles.heading}>Created At: {this.state.createdAt}</Text>
                <Text style={styles.heading}>Players:</Text>
                
                <FlatList style={{flex:1, margin: 10}}
                                    data={this.state.playerList}
                                    renderItem={({item})=>
                                    <Text>Player: {item.userName}</Text>
                                    } />

                
                <Button title="Start Match" onPress={this.playMatch} underlayColor='#31e981'  />
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
    }
});
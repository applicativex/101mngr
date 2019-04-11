import React from 'react';
import { StyleSheet, Text, View, Image, AsyncStorage, Alert, TouchableHighlight, FlatList } from 'react-native';
import { ListItem } from 'react-native-elements'
import { Environment } from '../Environment'

export class MatchHistory extends React.Component {
    static navigationOptions = {
      title: 'Match History',
    };

    constructor(props) {
        super(props);
        this.state = {
            matchList: []
        };
    }

    componentDidMount() {
        return AsyncStorage.getItem('token', (err, result) => {
            if (result !== null) {
                return fetch(`${Environment.API_URI}/api/account/match-history`, {
                    method: 'GET',
                    headers: {
                        Accept: 'application/json',
                        'Content-Type': 'application/json',
                        Authorization: result
                    }})
                    .then((response) => {
                        console.log(response);
                        return response.json();})
                    .then((responseJson) => {
                        console.log(responseJson);
                        this.setState({
                          matchList: responseJson
                        }, function(){
                
                        });
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
            <View>
                <FlatList data={this.state.matchList}
                                    keyExtractor={(item, index) => item.id}
                                    renderItem={({item})=>
                                        <ListItem title={item.name} subtitle={item.date} bottomDivider />
                                    } />
            </View>
        );
    }
}

const styles = StyleSheet.create({
    container: {
        flex: 1,
        alignItems: 'center'
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
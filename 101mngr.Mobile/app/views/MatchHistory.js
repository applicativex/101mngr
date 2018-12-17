import React from 'react';
import { StyleSheet, Text, View, Image, AsyncStorage, Alert, TouchableHighlight, FlatList } from 'react-native';

export class MatchHistory extends React.Component {
    staticnavigationOptions = {
        header: null
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
                return fetch('http://192.168.0.102:80/api/account/match-history', {
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
            <View style={styles.container}>
                <Text style={styles.heading}>Match history</Text>
                <FlatList style={{flex:1, margin: 10}}
                                    data={this.state.matchList}
                                    renderItem={({item})=>
                                    <Text>Id: {item.id} - Name: {item.name} - Date: {item.date}</Text>
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
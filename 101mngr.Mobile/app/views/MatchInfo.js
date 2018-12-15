import React from 'react';
import { StyleSheet, Text, View, Image, AsyncStorage, Alert, TouchableHighlight, FlatList } from 'react-native';

export class MatchInfo extends React.Component {
    staticnavigationOptions = {
        header: null
    };

    constructor(props) {
        super(props);
        this.state = {
            id: 0,
            name: "",
            createdAt: null
        };
    }

    componentDidMount() {
        let matchId = this.props.navigation.getParam('matchId');
        return fetch(`http://192.168.0.101:80/api/match/${matchId}`)
          .then((response) => response.json())
          .then((responseJson) => {

            console.log(responseJson);
            this.setState({
              id: responseJson.id,
              name: responseJson.name,
              createdAt: responseJson.createdAt
            }, function(){
    
            });
    
          })
          .catch((error) =>{
            console.error(error);
          });
    }

    render () {
        const { navigate } = this.props.navigation;

        return (
            <View style={styles.container}>
                <Text style={styles.heading}>Id: {this.state.id}</Text>
                <Text style={styles.heading}>Name: {this.state.name}</Text>
                <Text style={styles.heading}>Created At: {this.state.createdAt}</Text>
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
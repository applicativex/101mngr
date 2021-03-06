import React from 'react';
import { View, TouchableWithoutFeedback, Text, FlatList, StyleSheet } from 'react-native'
import { ListItem } from 'react-native-elements'
import { Environment } from '../Environment'

export class Leagues extends React.Component {
    static navigationOptions = {
        title: 'Leagues'
    }

    constructor (props) {
        super(props);
        this.state = {
            leagues: []
        };
    }

    componentDidMount = async () => {
        try {
            let countryId = this.props.navigation.getParam('countryId');
            let response = await fetch(`${Environment.API_URI}/api/leagues?countryId=${countryId}`);
            let responseJson = await response.json();
            this.setState({
                leagues: responseJson,
            }, function(){
    
            });
        } catch (error) {
            console.error(error);
        }
    }

    render () {
        const { navigate } = this.props.navigation;

        return (
            <View>
                
                <FlatList 
                            data={this.state.leagues}
                            keyExtractor={(item, index) => item.id}
                            renderItem={({item}) =>
                            <LeagueItem
                                navigate={navigate}
                                id={item.id}
                                name={item.name} />
                            }    
                        />
            </View>
        );
    }
}

export class LeagueItem extends React.Component {
    onPress = () => {
        this.props.navigate('Seasons', {leagueId: this.props.id} );
    }

    render(){
        return(
            <View>
               <ListItem title={this.props.name} onPress={this.onPress} leftAvatar={{ source: { uri: 'http://schah.at/img/public/premier-league-logo-vector-380x380.jpg' } }} bottomDivider />
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
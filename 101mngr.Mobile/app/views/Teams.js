import React from 'react';
import { View, TouchableWithoutFeedback, Text, FlatList, StyleSheet } from 'react-native'
import { ListItem } from 'react-native-elements'
import { Environment } from '../Environment'

export class Teams extends React.Component {
    static navigationOptions = {
        title: 'Teams'
    }

    constructor (props) {
        super(props);
        this.state = {
            teams: []
        };
    }

    componentDidMount = async () => {
        try {
            let leagueId = this.props.navigation.getParam('leagueId');
            let seasonId = this.props.navigation.getParam('seasonId');
            let response = await fetch(`${Environment.API_URI}/api/leagues/${leagueId}/seasons/${seasonId}/teams`);
            let responseJson = await response.json();
            var i;
            for (i = 0; i < responseJson.length; i++) { 
                if (responseJson[i].id == 1) {
                    responseJson[i].avatar_url = 'http://t2.gstatic.com/images?q=tbn:ANd9GcTGHmVADITLo0K1zMlJzAfMDYEX11Dq3L6-QdgMRBxvNuQSxmAq'
                } else if (responseJson[i].id == 2) {
                    responseJson[i].avatar_url = 'http://toplogos.ru/images/logo-manchester-united.jpg'
                }
            }
            this.setState({
                teams: responseJson,
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
                            data={this.state.teams}
                            keyExtractor={(item, index) => item.id}
                            renderItem={({item}) =>
                            <TeamItem
                                navigate={navigate}
                                id={item.id}
                                avatar_url={item.avatar_url}
                                name={item.name} />
                            }    
                        />
            </View>
        );
    }
}

export class TeamItem extends React.Component {
    onPress = () => {
        // this.props.navigate('Players', {teamId: this.props.id} );
    }

    render(){
        return(
            <View>
               <ListItem title={this.props.name} onPress={this.onPress} leftAvatar={{ source: { uri: this.props.avatar_url } }} bottomDivider />
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
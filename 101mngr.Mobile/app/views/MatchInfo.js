import React from 'react';
import { StyleSheet, View, AsyncStorage, ScrollView, RefreshControl, FlatList } from 'react-native';
import { Text, Card, ListItem, Button } from 'react-native-elements'
import { Environment } from '../Environment';

export class MatchInfo extends React.Component {
    static navigationOptions = {
      title: 'Match Info',
    };

    constructor(props) {
        super(props);
        this.state = {
            id: 0,
            name: "",
            createdAt: null,
            playerList: [],
            accountId: '',
            refreshing: false
        };
    }

    componentDidMount = async () => {
        await this._refreshMatchInfo();
    }
    
    _onRefresh = async () => {
        this.setState({refreshing: true});
        await this._refreshMatchInfo();
        this.setState({refreshing: false});
    }


    _refreshMatchInfo = async () => {
        try {
            let matchId = this.props.navigation.getParam('matchId');
            let matchResponse = await fetch(`${Environment.API_URI}/api/match/${matchId}`);
            let matchJson = await matchResponse.json();
            this.setState({
              id: matchJson.id,
              name: matchJson.name,
              createdAt: matchJson.createdAt,
              playerList: matchJson.players
            });  
            let token = await AsyncStorage.getItem('token');
            let profileResponse = await fetch(`${Environment.API_URI}/api/account/profile`, {
                method: 'GET',
                headers: {
                    Authorization: token
                }});
            let profileJson = await profileResponse.json();
            this.setState({
                accountId: profileJson.id
              });
        } catch (error) {
            console.error(error);
        }
    }

    inPlayerList = () => {
        return this.state.playerList.some(x=>x.id === this.state.accountId);
    }
 
    playMatch = async () => {
        try {
            let token = await AsyncStorage.getItem('token');
            let response = await fetch(`${Environment.API_URI}/api/match/${this.state.id}/start`, {
                method: 'PUT',
                headers: {
                    Authorization: token
                }});
            this.props.navigation.navigate('MatchStats', {matchId: this.state.id});

        } catch (error) {
            console.error(error);
        }
    }

    joinMatch = async () => {
        try {
            let token = await AsyncStorage.getItem('token');
            let response = await fetch(`${Environment.API_URI}/api/match/${this.state.id}/join`, {
                method: 'PUT',
                headers: {
                    Authorization: token
                }});
            let profileResponse = await fetch(`${Environment.API_URI}/api/account/profile`, {
                method: 'GET',
                headers: {
                    Authorization: token
                }});
            let profileJson = await profileResponse.json();
            this.setState(state => ({
                playerList: [...state.playerList, { id: profileJson.id, userName: `${profileJson.firstName} ${profileJson.lastName}`}]
              }));
            console.log('Success join');
            
        } catch (error) {
            console.error(error);
        }
    }

    leaveMatch = async () => {
        try {
            let token = await AsyncStorage.getItem('token');
            let response = await fetch(`${Environment.API_URI}/api/match/${this.state.id}/leave`, {
                method: 'PUT',
                headers: {
                    Authorization: token
                }});
            this.props.navigation.navigate('MatchList');
        } catch (error) {
            console.error(error);
        }
    }

    render () {
        const { navigate } = this.props.navigation;

        return (
            
            <ScrollView refreshControl={
                <RefreshControl
                  refreshing={this.state.refreshing}
                  onRefresh={this._onRefresh}
                />
              }>
                <Card title='Match details' containerStyle={{paddingHorizontal: 0}} >
                    <ListItem title={this.state.name} subtitle='Name' containerStyle={{paddingTop: 0}} />
                    <ListItem title={this.state.createdAt} subtitle='Created at' containerStyle={{paddingTop: 0}} />
                </Card>
                {/* <ListItem key={this.state.name} title={this.state.name} subtitle='Name' containerStyle={{margin:0}} bottomDivider />
                <ListItem key={this.state.createdAt} title={this.state.createdAt} subtitle='Created at' containerStyle={{margin:0}} bottomDivider /> */}
                <Card title='Players' containerStyle={{paddingHorizontal: 0}} >
                {
                    this.state.playerList.map((u, i) => {
                    return (
                        <ListItem
                            key={u.id}
                            title={u.userName}
                            containerStyle={{paddingVertical:0}}
                            />
                            );
                    })
                }
                </Card>

                <Button title={!this.inPlayerList() ? "Join" : "Leave"} onPress={ !this.inPlayerList() ? this.joinMatch : this.leaveMatch} underlayColor='#31e981' containerStyle={{margin:10}}  />
                <Button title="Start" onPress={this.playMatch} underlayColor='#31e981' containerStyle={{margin:10}}  />
            </ScrollView>
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
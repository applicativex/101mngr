import React from 'react';
import { StyleSheet, View, AsyncStorage, ScrollView, RefreshControl, FlatList } from 'react-native';
import { Text, Card, ListItem, Button } from 'react-native-elements'
import { Environment } from '../Environment';
import { HubConnectionBuilder } from '@aspnet/signalr';

export class MatchInfo extends React.Component {
    static navigationOptions = {
      title: 'Match Info',
    };

    constructor(props) {
        super(props);
        let matchId = this.props.navigation.getParam('matchId');
        this.state = {
            id: matchId,
            name: "some match",
            minute: 0,
            matchPeriod: 0,
            createdAt: null,
            playerList: [],
            accountId: '',
            refreshing: false
        };
        console.log(matchId);
        this.connection = new HubConnectionBuilder().withUrl(`${Environment.API_URI}/matches`).build();
    }

    componentDidMount = async () => {
        // await this._refreshMatchInfo();

        this.connection.start().then(() => {           

            this.connection.invoke("GetMatch",this.state.id).then((data) => {
                this.setState({
                    name:data.name,
                    minute:data.minute,
                    matchPeriod:data.matchPeriod});
            });  
            
            this.subscription = this.connection.stream("GetMatchStream",this.state.id).subscribe({
                close: false,
                next: (match) => {
                    this.setState({minute:match.minute,matchPeriod:match.matchPeriod});
                },
                error: function (err) {
                    console.log(err);
                }
            });
        });
    }

    componentWillUnmount = async () => {
        this.subscription.dispose();
        await this.connection.stop();
    }
    
    _onRefresh = async () => {
        this.setState({refreshing: true});
        // await this._refreshMatchInfo();
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
 
    showMatchList = async () => {
        this.props.navigation.navigate('MatchList');
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
                    <ListItem title={this.state.matchPeriod.toString()} subtitle='Period' containerStyle={{paddingTop: 0}} />
                    <ListItem title={this.state.minute.toString()} subtitle='Minute' containerStyle={{paddingTop: 0}} />
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

                <Button title="Match list" onPress={this.showMatchList} underlayColor='#31e981' containerStyle={{margin:10}}  />
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
import React from 'react';
import { StyleSheet, View, AsyncStorage, ScrollView, ActivityIndicator } from 'react-native';
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
            loading: true,
            name: "some match",
            minute: 0,
            matchPeriod: 0,
            matchEvents: [],
            createdAt: null,
            playerList: [],
            accountId: '',
            refreshing: false,
            homeTeam: null,
            awayTeam: null,
            home: 0,
            away: 0
        };
        console.log(matchId);
        this.connection = new HubConnectionBuilder().withUrl(`${Environment.API_URI}/matches`).build();
    }

    componentDidMount = async () => {
        // await this._refreshMatchInfo();

        this.connection.start().then(() => {           

            this.connection.invoke("GetMatch",this.state.id).then((data) => {
                console.log(data.matchEvents);
                this.setState({
                    name:data.name,
                    minute:data.minute,
                    matchPeriod:data.matchPeriod,
                    matchEvents:data.matchEvents,
                    homeTeam:data.homeTeam,
                    awayTeam:data.awayTeam,
                    home: data.goals.home,
                    away: data.goals.away,
                    loading:false
                });
            });  
            
            this.subscription = this.connection.stream("GetMatchStream",this.state.id).subscribe({
                close: false,
                next: (match) => {
                    var matchEvents = match.matchEventType != 5 || match.matchPeriod == 2 || match.matchPeriod == 4
                    ? Array.of(match).concat(this.state.matchEvents)
                    : this.state.matchEvents;
                    var home = match.matchEventType == 1 && match.home != null && match.home.value ? this.state.home + 1 : this.state.home;
                    var away = match.matchEventType == 1 && match.home != null && !match.home.value ? this.state.away + 1 : this.state.away;
                    this.setState({minute:match.minute,matchPeriod:match.matchPeriod,matchEvents:matchEvents,home:home,away:away});
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

    showMatchRoom = () => {
        this.props.navigation.navigate('MatchRoom',{matchId:this.state.id});
    }

    formatMatchEvent = (matchEvent) => {
        switch(matchEvent.matchEventType){
            case 1:
                var player = matchEvent.home 
                ? this.state.homeTeam.players.find(x=>x.id == matchEvent.playerId)
                : this.state.awayTeam.players.find(x=>x.id == matchEvent.playerId);
                return {
                    id: matchEvent.id,
                    title: `${matchEvent.minute}'`,
                    subtitle: `${player.name}`,
                    imageSource: 'https://i.imgur.com/T3jqCOz.png'
                };
            case 2:                
                    var player = matchEvent.home 
                    ? this.state.homeTeam.players.find(x=>x.id == matchEvent.playerId)
                    : this.state.awayTeam.players.find(x=>x.id == matchEvent.playerId);
                    return {
                        id: matchEvent.id,
                        title: `${matchEvent.minute}'`,
                        subtitle: `${player.name}`,
                        imageSource: 'https://i.imgur.com/6NieE6C.png'
                    };                        
            
            case 3:   
                var player = matchEvent.home 
                ? this.state.homeTeam.players.find(x=>x.id == matchEvent.playerId)
                : this.state.awayTeam.players.find(x=>x.id == matchEvent.playerId);
                return {
                    id: matchEvent.id,
                    title: `${matchEvent.minute}'`,
                    subtitle: `${player.name}`,
                    imageSource: 'https://i.imgur.com/4sZgxsK.png'
                };

            case 5:
                return {
                    id: matchEvent.id,
                    title: `${matchEvent.minute}'`,
                    subtitle: `${matchEvent.matchPeriod == 2 ? 'HT':'FT'}`,
                    imageSource: 'https://i.imgur.com/j377s0z.png'
                };

            default:
                return "";
        }

    }

    matchHeader = () => {
        if(this.state.homeTeam == null || this.state.awayTeam == null) {
            return "";
        }
        return `${this.state.homeTeam.name} - ${this.state.awayTeam.name}`;
    }

    matchScoreTitle = () => {
        var suffix = this.state.matchPeriod == 1?
                        'First Time' : this.state.matchPeriod == 2 ?
                                'HT' : this.state.matchPeriod == 4 ?
                                        'FT' : this.state.matchPeriod == 3 ?
                                            'Second Time' : '';
        return `${this.state.minute}' ${suffix}`
    }

    renderLoading() {
        if (this.state.loading) {
          return (
            <ActivityIndicator size="large"  color="black" style={{
                position:'absolute', left:0, right:0, bottom:0, top:0 }}/>        
          )
        } else {
          return null
        }
    }

    render () {
        const { navigate } = this.props.navigation;

        return (
            
            <View style={{flex:1}}>
                {this.state.loading && 
                    <View style={styles.loadingContainer}>
                        <ActivityIndicator size="large" color="#0000ff" />
                    </View>
                }
                {!this.state.loading && 
                    <ScrollView>
                        <Card title='Match' containerStyle={{paddingHorizontal: 0}} >
                            <ListItem title={this.matchHeader()} containerStyle={{paddingTop: 0}} />
                            <ListItem title={`${this.state.home} - ${this.state.away}`} subtitle={this.matchScoreTitle()} containerStyle={{paddingTop: 0}} />
                        </Card>

                        <View>                   
                            <Card title='Match events' containerStyle={{paddingHorizontal: 0}} >
                            {
                                this.state.matchEvents.map((u, i) => this.formatMatchEvent(u)).map(x => {
                                    return (
                                        <ListItem
                                            key={x.id}
                                            title={x.title}
                                            subtitle={x.subtitle}
                                            leftAvatar={{ source: {uri: x.imageSource} }} 
                                            bottomDivider />
                                            );
                                    }) 
                            }
                            </Card>
                            
                        </View>

                        <Button title="Match room" onPress={this.showMatchRoom} underlayColor='#31e981' containerStyle={{margin:10}}  />
                        <Button title="Match list" onPress={this.showMatchList} underlayColor='#31e981' containerStyle={{margin:10}}  />
                    </ScrollView>    
                }
                
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
    
//   container: {
//     flex: 1,
//     marginTop: 40,
//     marginHorizontal: 10,
//     justifyContent: 'center',
//   },
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
    },
    loadingContainer: {
        flex: 1,
        justifyContent: 'center',
        alignItems: 'center'
      }
});
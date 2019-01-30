import React from 'react';
import { StyleSheet, Text, View, Button, AsyncStorage, Alert, ScrollView, RefreshControl, FlatList } from 'react-native';

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
            let matchResponse = await fetch(`http://35.228.60.109/api/match/${matchId}`);
            let matchJson = await matchResponse.json();
            this.setState({
              id: matchJson.id,
              name: matchJson.name,
              createdAt: matchJson.createdAt,
              playerList: matchJson.players
            });  
            let token = await AsyncStorage.getItem('token');
            let profileResponse = await fetch('http://35.228.60.109/api/account/profile', {
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
            let response = await fetch(`http://35.228.60.109/api/match/${this.state.id}/start`, {
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
            let response = await fetch(`http://35.228.60.109/api/match/${this.state.id}/join`, {
                method: 'PUT',
                headers: {
                    Authorization: token
                }});
            let profileResponse = await fetch('http://35.228.60.109/api/account/profile', {
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
            let response = await fetch(`http://35.228.60.109/api/match/${this.state.id}/leave`, {
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
            
            <ScrollView contentContainerStyle={styles.container} refreshControl={
                <RefreshControl
                  refreshing={this.state.refreshing}
                  onRefresh={this._onRefresh}
                />
              }>

                <Text style={styles.heading}>Id: {this.state.id}</Text>
                <Text style={styles.heading}>Name: {this.state.name}</Text>
                <Text style={styles.heading}>Created At: {this.state.createdAt}</Text>
                <Text style={styles.heading}>Players:</Text>
                
                <FlatList style={{flex:1, margin: 10}}
                                    data={this.state.playerList}
                                    keyExtractor={(item, index) => item.id.toString()}
                                    renderItem={({item})=>
                                    <Text>Player: {item.userName}</Text>
                                    } />

                
                <View style={styles.alternativeLayoutButtonContainer}>
                
                    <Button style={{margin:'30%'}} title="Start" onPress={this.playMatch} underlayColor='#31e981'  />
                    <Button style={{margin:'30%'}} title={!this.inPlayerList() ? "Join" : "Leave"} onPress={ !this.inPlayerList() ? this.joinMatch : this.leaveMatch} underlayColor='#31e981'  />
                </View>
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
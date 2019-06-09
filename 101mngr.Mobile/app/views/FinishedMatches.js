import React from 'react';
import { StyleSheet, Text, View, ScrollView, RefreshControl, FlatList } from 'react-native';
import { ListItem, Button } from 'react-native-elements'
import { Environment } from '../Environment'

export class FinishedMatches extends React.Component {
    static navigationOptions = {
      title: 'Finished Matches',
    };

    constructor(props) {
        super(props);
        this.state = {
            isInvited: false,
            loggedUser: false,
            matchList: [],
            refreshing: false,
        };
    }
    
    _onRefresh = async () => {
        this.setState({refreshing: true});
        await this._refreshMatchList();
        this.setState({refreshing: false});
      }
    

    componentDidMount = async () => {
        await this._refreshMatchList();;
    }

    componentWillUnmount = async () => {
    }

    _refreshMatchList = async () => {
        try {
            let response = await fetch(`${Environment.API_URI}/api/match?finished=true`);
            let responseJson = await response.json(); 

            this.setState({
              matchList: responseJson,
            }, function(){
    
            });
        } catch (error) {
            console.error(error);
        }
    }

    complexName (item) {
        var suffix = item.matchPeriod == 2 ?
                        'HT' : item.matchPeriod == 4 ?
                                'FT' : '';
        return `${item.name} ${item.minute}' ${suffix}`.trim();
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
                
                <FlatList   data={this.state.matchList}
                            keyExtractor={(item, index) => item.id}
                            renderItem={({item}) =>
                            <MatchItem
                                navigate={navigate}
                                id={item.id}
                                name={this.complexName(item)} />
                            }    
                        />
            </ScrollView>
        );
    }
}

export class MatchItem extends React.Component {
    onPress = () => {
        this.props.navigate('MatchInfo', {matchId: this.props.id} );
    }

    render(){
        return(
            <ListItem title={this.props.name} onPress={this.onPress} bottomDivider />
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
    },
    alternativeLayoutButtonContainer: {
      flexDirection: 'row',
      justifyContent: 'space-between',
      alignItems:'center',
      width: '75%'
    }
});
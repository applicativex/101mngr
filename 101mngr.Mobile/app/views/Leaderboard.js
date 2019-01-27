import React from 'react';
import { ScrollView, RefreshControl, FlatList, Text, View, StyleSheet } from 'react-native';

export class Leaderboard extends React.Component {

    static navigationOptions = {
        title: 'Leaderboard',
      };

    constructor(props) {
        super(props);
        this.state = {
            refreshing: false,
            players: [
                {id:'1',name:'Jack Daniels'},
                {id:'2',name:'Johny Walker'},
                {id:'3',name:'Tulamour Deuw'}
            ]
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
                
                <FlatList   data={this.state.players}
                            keyExtractor={(item, index) => item.id}
                            renderItem={({item}) =>
                            <LeaderboardItem
                                navigate={navigate}
                                id={item.id}
                                name={item.name} />
                            }    
                        />
            </ScrollView>
        );
    }
}

export class LeaderboardItem extends React.Component {
    render(){
        return(
            <View style={{paddingTop:20,alignItems:'center'}}>
                <Text>
                    {this.props.name}
                </Text>
            </View>
        );
    }
}

const styles = StyleSheet.create({
    container: {
        flex: 1,
        alignItems: 'center',
        paddingBottom: '45%',
        paddingTop: '10%'
    },
    heading: {
        fontSize: 16,
        flex: 1
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
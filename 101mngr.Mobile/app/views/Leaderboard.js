import React from 'react';
import { ScrollView, RefreshControl, FlatList, Text, View, StyleSheet } from 'react-native';
import { ListItem } from 'react-native-elements'

export class Leaderboard extends React.Component {

    static navigationOptions = {
        title: 'Leaderboard',
      };

    constructor(props) {
        super(props);
        this.state = {
            refreshing: false,
            players: [
                {id:'1',userName:'baltazar',fullName:'Jack Daniels', level:1},
                {id:'2',userName:'dude',fullName:'Johny Walker', level:1},
                {id:'3',userName:'maduro',fullName:'Tulamour Deuw', level:1}
            ]
        }
    }

    keyExtractor = (item, index) => item.id

    renderItem = ({ item }) => (
        <ListItem
            title={item.userName}
            badge={{ value: item.level, textStyle: { color: 'white' }, containerStyle: { marginTop: -20 } }}
            subtitle={item.fullName}
            // leftAvatar={{ source: { uri: item.avatar_url } }}
        />
    )

    render () {
        const { navigate } = this.props.navigation;

        return (
            
            <ScrollView refreshControl={
                <RefreshControl
                  refreshing={this.state.refreshing}
                  onRefresh={this._onRefresh}
                />
              }>
              
              <FlatList
                    keyExtractor={this.keyExtractor}
                    data={this.state.players}
                    renderItem={this.renderItem} />
                
            </ScrollView>
        );
    }
}

export class LeaderboardItem extends React.Component {
    render(){
        return(
            <View style={{paddingTop:20,alignItems:'center'}}>
                <ListItem
                    title={this.props.name}
                    subtitle={this.props.name}
                />
            </View>
        );
    }
}

styles = StyleSheet.create({
    subtitleView: {
      flexDirection: 'row',
      paddingLeft: 10,
      paddingTop: 5
    },
    ratingImage: {
      height: 19.21,
      width: 100
    },
    ratingText: {
      paddingLeft: 10,
      color: 'grey'
    }
  })
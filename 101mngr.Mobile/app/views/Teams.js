import React from 'react';
import { View, TouchableWithoutFeedback, Text, FlatList, StyleSheet } from 'react-native'

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
            let response = await fetch(`http://35.228.60.109/api/leagues/${leagueId}/seasons/${seasonId}/teams`);
            let responseJson = await response.json();
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
            <View style={styles.container}>
                
                <FlatList 
                            data={this.state.teams}
                            keyExtractor={(item, index) => item.id}
                            renderItem={({item}) =>
                            <TeamItem
                                navigate={navigate}
                                id={item.id}
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
            <TouchableWithoutFeedback onPress={this.onPress}>
                <View style={{paddingTop:20,alignItems:'center'}}>
                    <Text>
                        {this.props.name}
                    </Text>
                </View>
            </TouchableWithoutFeedback>
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
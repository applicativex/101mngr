import React from 'react';
import { View, TouchableWithoutFeedback, Text, FlatList, StyleSheet } from 'react-native'

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
            let response = await fetch(`http://35.228.60.109/api/leagues?countryId=${countryId}`);
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
            <View style={styles.container}>
                
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
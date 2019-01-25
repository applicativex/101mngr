import React from 'react';
import { View, TouchableWithoutFeedback, Text, FlatList } from 'react-native'

export class Players extends React.Component {
    static navigationOptions = {
        title: 'Players'
    }

    constructor (props) {
        super(props);
        this.state = {
            players: []
        };
    }

    render () {
        const { navigate } = this.props.navigation;

        return (
            <View style={styles.container}>
                
                <FlatList 
                            data={this.state.leagues}
                            keyExtractor={(item, index) => item.id}
                            renderItem={({item}) =>
                            <PlayerItem
                                navigate={navigate}
                                id={item.id}
                                name={item.name} />
                            }    
                        />
            </View>
        );
    }
}

export class PlayerItem extends React.Component {
    onPress = () => {
        //this.props.navigate('Players', {teamId: this.props.id} );
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
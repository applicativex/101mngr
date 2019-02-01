import React from 'react';
import { View, TouchableWithoutFeedback, Text, FlatList, StyleSheet } from 'react-native'
import { ListItem } from 'react-native-elements'

export class Countries extends React.Component {
    static navigationOptions = {
      title: 'Countries',
    };

    constructor(props) {
        super(props);
        this.state = {
            countries: []
        };
    }

    componentDidMount = async () => {
        try {
            let response = await fetch('http://35.228.60.109/api/leagues/countries');
            let responseJson = await response.json();
            this.setState({
                countries: responseJson,
            }, function(){
    
            });
        } catch (error) {
            console.error(error);
        }
    }
    
    render () {
        const { navigate } = this.props.navigation;

        return (
            <View>
              
              <FlatList data={this.state.countries}
                        keyExtractor={(item, index) => item.id}
                        renderItem={({item}) =>
                            <CountryItem
                                navigate={navigate}
                                id={item.id}
                                name={item.name} />
                            }    
                        />
            </View>
        );
    }

}

export class CountryItem extends React.Component {
    onPress = () => {
        console.log(this.props.id);
        this.props.navigate('Leagues', {countryId: this.props.id} );
    }

    render(){
        return(
            <View>
               <ListItem title={this.props.name} onPress={this.onPress} leftAvatar={{ source: { uri: 'https://www.crwflags.com/fotw/images/g/gb-eng.gif' } }} bottomDivider />
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
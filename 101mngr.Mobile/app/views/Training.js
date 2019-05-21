import React from 'react';
import { View, Text, ScrollView, StyleSheet, AsyncStorage } from 'react-native'
import { Card, ListItem, Button } from 'react-native-elements'
import { Environment } from '../Environment'

export class Training extends React.Component {
    static navigationOptions = {
        title: 'Training'
    }

    constructor (props) {
        super(props);
        this.state = {
            passing: 0,
            endurance: 0,
            dribbling: 0,
            coverage: 0
        };
    }

    componentDidMount = async () => {
        try {
            let token = await AsyncStorage.getItem('token');
            let response = await fetch(`${Environment.API_URI}/api/training`, {
                method: 'GET',
                headers: {
                    Accept: 'application/json',
                    'Content-Type': 'application/json',
                    Authorization: token
                }});
            let responseJson = await response.json();
            console.log(responseJson);
            this.setState({
                passing: responseJson.passingDelta,
                endurance: responseJson.enduranceDelta,
                dribbling: responseJson.dribblingDelta,
                coverage: responseJson.coverageDelta
            }, function(){
            });
        } catch (error) {
            console.error(error);
        }
    }
  
    _trainPassing = async () => {
        let token = await AsyncStorage.getItem('token');
        let response = await fetch(`${Environment.API_URI}/api/training/passing`, {
            method: 'PUT',
            headers: {
                Accept: 'application/json',
                'Content-Type': 'application/json',
                Authorization: token
            }
        });
        let responseJson = await response.json();
        this.setState({
            passing: responseJson.passingDelta
        }, function(){
        });
    };
  
    _trainEndurance = async () => {
        let token = await AsyncStorage.getItem('token');
        let response = await fetch(`${Environment.API_URI}/api/training/endurance`, {
            method: 'PUT',
            headers: {
                Accept: 'application/json',
                'Content-Type': 'application/json',
                Authorization: token
            }
        });
        let responseJson = await response.json();
        this.setState({
            endurance: responseJson.enduranceDelta
        }, function(){
        });
    }; 
  
    _trainDribbling = async () => {
        let token = await AsyncStorage.getItem('token');
        let response = await fetch(`${Environment.API_URI}/api/training/dribbling`, {
            method: 'PUT',
            headers: {
                Accept: 'application/json',
                'Content-Type': 'application/json',
                Authorization: token
            }
        });
        let responseJson = await response.json();
        this.setState({
            dribbling: responseJson.dribblingDelta
        }, function(){
        });
    };
  
    _trainCoverage = async () => {
        let token = await AsyncStorage.getItem('token');
        let response = await fetch(`${Environment.API_URI}/api/training/coverage`, {
            method: 'PUT',
            headers: {
                Accept: 'application/json',
                'Content-Type': 'application/json',
                Authorization: token
            }
        });
        let responseJson = await response.json();
        this.setState({
            coverage: responseJson.coverageDelta
        }, function(){
        });
    };

    _finishTraining = async () => {
        let token = await AsyncStorage.getItem('token');
        let response = await fetch(`${Environment.API_URI}/api/training/finish`, {
            method: 'PUT',
            headers: {
                Accept: 'application/json',
                'Content-Type': 'application/json',
                Authorization: token
            }
        });
        this.props.navigation.navigate('Home');
    }

    render () {
        const { navigate } = this.props.navigation;

        return (
            <ScrollView>
                <Card title='Current Training'>
                    <ListItem title={this.state.passing.toString()} subtitle='Passing' onPress={this._trainPassing}  />
                    <ListItem title={this.state.endurance.toString()} subtitle='Endurance' onPress={this._trainEndurance}  />
                    <ListItem title={this.state.dribbling.toString()} subtitle='Dribbling' onPress={this._trainDribbling} />
                    <ListItem title={this.state.coverage.toString()} subtitle='Coverage' onPress={this._trainCoverage} />
                </Card>    

                <Button title="Finish Training" onPress={this._finishTraining} containerStyle={{margin:10, marginTop: 20}} />                
                
            </ScrollView>
        );
    }
}

const styles = StyleSheet.create({
    container: {
        flex: 1,
        alignItems: 'center'
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
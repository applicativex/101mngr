import React from 'react';
import {
    StyleSheet,
    View,
    ScrollView,
    TextInput,
    TouchableHighlight,
    Picker,
    AsyncStorage    
} from 'react-native';
import { Text, Card, ListItem, Button } from 'react-native-elements'

export class Profile extends React.Component {

    static navigationOptions = {
        title: 'Profile',
      };

    constructor(props) {
        super(props);
        this.state = {
            firstName: '',
            lastName: '',
            dateOfBirth: '',
            countryCode: '',
            weight: '',
            height: '',
            playerType: 0
        }
    }

    componentDidMount = async () => {
        try {
            let token = await AsyncStorage.getItem('token');
            let response = await fetch('http://35.228.60.109/api/account/profile', {
                method: 'GET',
                headers: {
                    Accept: 'application/json',
                    'Content-Type': 'application/json',
                    Authorization: token
                }});
            let responseJson = await response.json();
            console.log(responseJson);
            this.setState({
                firstName: responseJson.firstName,
                lastName: responseJson.lastName,
                dateOfBirth: responseJson.dateOfBirth,
                countryCode: responseJson.countryCode,
                weight: responseJson.weight,
                height: responseJson.height,
                playerType: responseJson.playerType
            }, function(){

            });
        } catch (error) {
            console.error(error);
        }
    }

    saveProfile = async () =>{
        try {
            
            let token = await AsyncStorage.getItem('token');
            let response = await fetch('http://35.228.60.109/api/account/profile', {
                method: 'PUT',
                headers: {
                    Accept: 'application/json',
                    'Content-Type': 'application/json',
                    Authorization: token
                },
                body: JSON.stringify({
                    firstName: this.state.firstName,
                    lastName: this.state.lastName,
                    dateOfBirth: this.state.dateOfBirth,
                    countryCode: this.state.countryCode,
                    weight: this.state.weight,
                    height: this.state.height,
                    playerType: this.state.playerType
                }),
            });
            this.setState({
                isEditing: false
              }, function(){
      
              });
        } catch (error) {
            console.error(error);
        };
    }

    editProfile = () =>{
        this.setState({
            isEditing: true
          }, function(){
  
          });
    }
  
    _showMatchHistory = () => {
      this.props.navigation.navigate('MatchHistory');
    };
  
    _randomMatch = async () => {
        let token = await AsyncStorage.getItem('token');
        let response = await fetch('http://35.228.60.109/api/match/random', {
            method: 'POST',
            headers: {
                Accept: 'application/json',
                'Content-Type': 'application/json',
                Authorization: token
            }
        });
        let responseJson = await response.json();
        console.log(responseJson.id);
        this.props.navigation.navigate('MatchInfo', {matchId: responseJson.id});
    };

    render () {
        if(this.state.isEditing) {
            return (
            <View style={styles.container}>
                <TextInput
                    style={styles.inputs}
                    onChangeText={(text) => this.setState({firstName: text})}
                    value={this.state.firstName}
                />
                <Text style={styles.label}>First Name</Text>

                <TextInput
                    style={styles.inputs}
                    onChangeText={(text) => this.setState({lastName: text})}
                    value={this.state.lastName}
                />
                <Text style={styles.label}>Last Name</Text>

                {/* <TextInput
                    style={styles.inputs}
                    onChangeText={(text) => this.setState({dateOfBirth: text})}
                    value={this.state.dateOfBirth}
                /> */}
                <Text style={styles.label}>Date Of Birth</Text>

                <TextInput
                    style={styles.inputs}
                    onChangeText={(text) => this.setState({countryCode: text})}
                    value={this.state.countryCode}
                />
                <Text style={styles.label}>Country</Text>

                <Picker
                    selectedValue={this.state.playerType}
                    style={{ height: 50, width: '50%' }}
                    prompt="Player Type"
                    onValueChange={(itemValue, itemIndex) => this.setState({playerType: itemValue})}>
                    <Picker.Item label="Goalkeeper" value="1" />
                    <Picker.Item label="Defender" value="2" />
                    <Picker.Item label="Midfielder" value="3" />
                    <Picker.Item label="Forward" value="4" />
        
                </Picker>
                <Text style={styles.label}>Player Type</Text>

                {/* <TextInput
                    style={styles.inputs}
                    onChangeText={(text) => this.setState({height: text})}
                    value={this.state.height}
                />
                <Text style={styles.label}>Height</Text>

                <TextInput
                    style={styles.inputs}
                    onChangeText={(text) => this.setState({weight: text})}
                    value={this.state.weight}
                />
                <Text style={styles.label}>Weight</Text> */}

                <TouchableHighlight onPress={this.saveProfile} underlayColor='#31e981'>
                    <Text style={styles.buttons}>Save Profile</Text>
                </TouchableHighlight>

            </View>
            )
        }
        return (
            <ScrollView>
                <Card title='Account info'>
                    <ListItem title={this.state.firstName} subtitle='First name' />
                    <ListItem title={this.state.lastName} subtitle='Last name' />
                    <ListItem title={this.state.dateOfBirth} subtitle='Date of birth' />
                    <ListItem title={this.state.countryCode} subtitle='Country' />
                </Card>

                <Card title='Player info'>
                    <ListItem title={this.state.playerType.toString()} subtitle='Player type' />
                    <ListItem title={this.state.height.toString()} subtitle='Height' />
                    <ListItem title={this.state.weight.toString()} subtitle='Weight' />
                </Card>

                <Button title="Edit" onPress={this.editProfile} containerStyle={{margin:10, marginTop: 20}} />
                <Button title="Match history" onPress={this._showMatchHistory} containerStyle={{margin:10}} />
                <Button title="Random match" onPress={this._randomMatch} containerStyle={{margin:10}} />

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
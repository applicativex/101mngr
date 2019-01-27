import React from 'react';
import {
    StyleSheet,
    Text,
    View,
    TextInput,
    TouchableHighlight,
    Picker,
    AsyncStorage,
    Button
} from 'react-native';

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
        this.props.navigation.navigate('MatchHistory');
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
            <View style={styles.container}>

                <Text style={styles.heading}>First Name: {this.state.firstName}</Text>
                <Text style={styles.heading}>Last Name: {this.state.lastName}</Text>
                <Text style={styles.heading}>Date Of Birth: {this.state.dateOfBirth}</Text>
                <Text style={styles.heading}>Country: {this.state.countryCode}</Text>
                <Text style={styles.heading}>Player Type: {this.state.playerType}</Text>
                <Text style={styles.heading}>Height: {this.state.height}</Text>
                <Text style={styles.heading}>Weight: {this.state.weight}</Text>

                <Button title="Edit" onPress={this.editProfile} />
                <Button title="Match history" onPress={this._showMatchHistory} />
                <Button title="Random match" onPress={this._randomMatch} />

            </View>
        );
    }
}

const styles = StyleSheet.create({
    container: {
        flex: 1,
        alignItems: 'center',
        paddingBottom: '35%',
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
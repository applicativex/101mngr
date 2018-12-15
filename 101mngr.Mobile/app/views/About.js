import React from 'react';
import { StyleSheet, Text, ScrollView, Image } from 'react-native';

const aboutGlobo = 'dddddddddddddddddddddddddddjbjgbeibrvuberv';

const whatGlobo = 'djfbwbegowepjcpwejcbnwebvwib';

export class About extends React.Component {
    static navigationOptions = {
        header: null
    };

    render() {
        return(
            <ScrollView style={styles.container}>
            <Text style={styles.aboutTitle}>Who we are</Text>
            <Text style={styles.aboutText}>{aboutGlobo}</Text>
            
            <Text style={styles.aboutTitle}>What we do</Text>
            <Text style={styles.aboutText}>{whatGlobo}</Text>

            <Text onPress={()=>this.props.navigation.goBack()} style={styles.backButton}>GO BACK</Text>
        </ScrollView>
        );
    }
}

const styles = StyleSheet.create({
    container: {
        paddingTop: 20,
        paddingBottom: 30,
        backgroundColor: '#ffffff'
    },
    aboutTitle: {
        paddingTop: 10,
        textAlign: 'center'
    },
    aboutText: {
        paddingBottom: 20
    },
    backButton: {
        paddingBottom: 50,
        textAlign: 'center'
    }
});
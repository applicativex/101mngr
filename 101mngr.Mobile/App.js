import React from 'react';
import { createStackNavigator, createDrawerNavigator, createAppContainer } from 'react-navigation';
import { Home } from './app/views/Home.js';
import { Register } from './app/views/Register.js';
import { Login } from './app/views/Login.js';
import { About } from './app/views/About.js';
import { MatchList } from './app/views/MatchList.js';
import { NewMatch } from './app/views/NewMatch.js';
import { MatchInfo } from './app/views/MatchInfo.js';
import { MatchHistory } from './app/views/MatchHistory.js';

const HomeStack = createStackNavigator(
  {
    HomeRT: { screen: Home }
  },
  {
    navigationOptions: ({ navigation }) => ({
      drawerLabel: 'Home',
      drawerLockMode: (
        navigation.state.routes[navigation.state.index].params || {}
      ).drawerLockMode
    }),
  }
);

const LoginStack = createStackNavigator(
  {
    LoginRT: { screen: Login }
  },
  {
    navigationOptions: ({ navigation }) => ({
      drawerLabel: 'Login',
      drawerLockMode: (
        navigation.state.routes[navigation.state.index].params || {}
      ).drawerLockMode
    }),
  }
);

const RegisterStack = createStackNavigator(
  {
    RegisterRT: { screen: Register }
  },
  {
    navigationOptions: ({ navigation }) => ({
      drawerLabel: 'Register',
      drawerLockMode: (
        navigation.state.routes[navigation.state.index].params || {}
      ).drawerLockMode
    }),
  }
);

const MatchListStack = createStackNavigator(
  {
    MatchListRT: { screen: MatchList },
    NewMatchRT: { screen: NewMatch },
    MatchInfoRT: { screen: MatchInfo },
  },
  {
    navigationOptions: ({ navigation }) => ({
      drawerLabel: 'Current Matches',
      drawerLockMode: (
        navigation.state.routes[navigation.state.index].params || {}
      ).drawerLockMode
    }),
  }
);

const MatchHistoryStack = createStackNavigator(
  {
    MatchHistoryRT: { screen: MatchHistory },
    MatchInfoRT: { screen: MatchInfo },
  },
  {
    navigationOptions: ({ navigation }) => ({
      drawerLabel: 'Match History',
      drawerLockMode: (
        navigation.state.routes[navigation.state.index].params || {}
      ).drawerLockMode
    }),
  }
);

const MyRoutes = createDrawerNavigator({
  HomeRT: {
    screen: HomeStack
  },
  RegisterRT: {
    screen: RegisterStack
  },
  LoginRT: {
    screen: LoginStack
  },
  MatchListRT: {
    screen: MatchListStack
  },
  MatchHistoryRT: {
    screen: MatchHistoryStack
  }
}, {
  initialRouteName: 'HomeRT'
});

const AppContainer = createAppContainer(MyRoutes);

// Now AppContainer is the main component for React to render

export default AppContainer;

// export default class App extends React.Component {
//   render() { 
//     return (
//       <MyRoutes />
//     );
//   }
// }
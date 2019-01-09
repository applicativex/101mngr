import React from 'react';
import { createDrawerNavigator, createAppContainer } from 'react-navigation';
import { Home } from './app/views/Home.js';
import { Register } from './app/views/Register.js';
import { Login } from './app/views/Login.js';
import { About } from './app/views/About.js';
import { MatchList } from './app/views/MatchList.js';
import { NewMatch } from './app/views/NewMatch.js';
import { MatchInfo } from './app/views/MatchInfo.js';
import { MatchHistory } from './app/views/MatchHistory.js';

const MyRoutes = createDrawerNavigator({
  HomeRT: {
    screen: Home
  },
  RegisterRT: {
    screen: Register
  },
  LoginRT: {
    screen: Login
  },
  AboutRT: {
    screen: About
  },
  MatchListRT: {
    screen: MatchList
  },
  NewMatchRT: {
    screen: NewMatch
  },
  MatchInfoRT: {
    screen: MatchInfo
  },
  MatchHistoryRT: {
    screen: MatchHistory
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
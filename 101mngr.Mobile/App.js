import React from 'react';
import { StackNavigator  } from 'react-navigation';
import { Home } from './app/views/Home.js';
import { Register } from './app/views/Register.js';
import { Login } from './app/views/Login.js';
import { About } from './app/views/About.js';
import { Match } from './app/views/Match.js';
import { PlayerPick } from './app/views/PlayerPick.js';
import { StartMatch } from './app/views/StartMatch.js';
import { EndMatch } from './app/views/EndMatch.js';
import { NewMatch } from './app/views/NewMatch.js';
import { CreateMatch } from './app/views/CreateMatch.js';
import { MatchInfo } from './app/views/MatchInfo.js';
import { MatchHistory } from './app/views/MatchHistory.js';

const MyRoutes = StackNavigator({
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
  MatchRT: {
    screen: Match
  },
  PlayerPickRT: {
    screen: PlayerPick
  },
  StartMatchRT: {
    screen: StartMatch
  },
  EndMatchRT: {
    screen: EndMatch
  },
  NewMatchRT: {
    screen: NewMatch
  },
  CreateMatchRT: {
    screen: CreateMatch
  },
  MatchInfoRT: {
    screen: MatchInfo
  },
  MatchHistoryRT: {
    screen: MatchHistory
  }
},
{
  initialRouteName: 'HomeRT'
});

export default class App extends React.Component {
  render() { 
    return (
      <MyRoutes />
    );
  }
}
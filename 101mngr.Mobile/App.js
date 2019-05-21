import React from 'react';
import { createStackNavigator, createDrawerNavigator, createAppContainer, createSwitchNavigator } from 'react-navigation';
import { Home } from './app/views/Home.js';
import { Register } from './app/views/Register.js';
import { Login } from './app/views/Login.js';
import { About } from './app/views/About.js';
import { MatchList } from './app/views/MatchList.js';
import { NewMatch } from './app/views/NewMatch.js';
import { MatchInfo } from './app/views/MatchInfo.js';
import { MatchHistory } from './app/views/MatchHistory.js';
import { Profile } from './app/views/Profile.js';
import { AuthLoading } from './app/views/AuthLoading.js';
import { Other } from './app/views/Other.js';
import { SignIn } from './app/views/SignIn.js';
import { Countries } from './app/views/Countries';
import { Leagues } from './app/views/Leagues';
import { Seasons } from './app/views/Seasons';
import { Teams } from './app/views/Teams';
import { Players } from './app/views/Players';
import { Leaderboard } from './app/views/Leaderboard';
import { Training } from './app/views/Training';
import { MatchStats } from './app/views/MatchStats';
import { ThemeProvider } from 'react-native-elements';

const AppStack = createStackNavigator({ 
  Home: Home, 
  Other: Other, 
  MatchList: MatchList, 
  MatchInfo: MatchInfo, 
  NewMatch: NewMatch,
  MatchStats: MatchStats,

  Profile: Profile,
  MatchHistory: MatchHistory,

  Countries: Countries,
  Leagues: Leagues,
  Seasons: Seasons,
  Teams: Teams,
  Players: Players,
  Leaderboard: Leaderboard,

  Training: Training
});
const AuthStack = createStackNavigator({ SignIn: SignIn, Register: Register });

const AppContainer = createAppContainer(createSwitchNavigator(
  {
    AuthLoading: AuthLoading,
    App: AppStack,
    Auth: AuthStack
  },  
  {
    initialRouteName: 'AuthLoading',
  }
));

const App = () => {
  return (
    <ThemeProvider>
      <AppContainer />
    </ThemeProvider>
  );
};

export default App;
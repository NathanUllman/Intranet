import React from 'react';
import {Route} from 'react-router-dom';
import HomePage from './home/HomePage';
import LatestPage from './LatestPage';
import EssentialsPage from "./EssentialsPage";
import PeoplePage from "./PeoplePage";
import PlacesPage from "./PlacesPage";
import DirectoriesPage from "./DirectoriesPage";
import SocialPage from "./SocialPage";
import AdminPage from "./AdminPage";
import CarouselEditor from "./main/CarouselEditor/Main"
import { hot } from 'react-hot-loader';

const Body = () => (
    <div id={"body-content"}>
        <HomePage/>
    </div>
)

export default hot(module)(Body);
/*<Route exact path={"/"} component={HomePage}/>
        <Route exact path={"/latest"} component={LatestPage}/>
        <Route exact path={"/essentials"} component={EssentialsPage}/>
        <Route exact path={"/people"} component={PeoplePage}/>
        <Route exact path={"/places"} component={PlacesPage}/>
        <Route exact path={"/directories"} component={DirectoriesPage}/>
        <Route exact path={"/social"} component={SocialPage}/>
        <Route exact path={"/admin"} component={AdminPage} />
        <Route exact path={"/edit"} component={CarouselEditor} />*/
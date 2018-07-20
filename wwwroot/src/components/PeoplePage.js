import React from 'react';
import { hot } from 'react-hot-loader';

class PeoplePage extends React.Component {
    render(){
        return(
            <div id={"peoplepage"}>
                <h1>People</h1>
                <p>This is the People Page section.</p>
            </div>
        );
    }
}

export default hot(module)(PeoplePage);
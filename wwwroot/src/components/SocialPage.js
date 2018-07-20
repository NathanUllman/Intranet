import React from 'react';
import { hot } from 'react-hot-loader';

class SocialPage extends React.Component {
    render(){
        return(
            <div id={"socialpage"}>
                <h1>Social</h1>
                <p>This is the Social Page section.</p>
            </div>
        );
    }
}

export default hot(module)(SocialPage);
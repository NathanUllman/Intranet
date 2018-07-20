import React from 'react';
import { hot } from 'react-hot-loader';

class EssentialsPage extends React.Component {
    render(){
        return(
            <div id={"essentialspage"}>
                <h1>Essentials</h1>
                <p>This is the Essentials Page section.</p>
            </div>
        );
    }
}

export default hot(module)(EssentialsPage);
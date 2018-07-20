import React from 'react';
import { hot } from 'react-hot-loader';

class AdminPage extends React.Component {
    render(){
        return(
            <div id={"adminpage"}>
                <h1>Admin</h1>
                <p>This is the Admin Page section.</p>
            </div>
        );
    }
}

export default hot(module)(AdminPage);
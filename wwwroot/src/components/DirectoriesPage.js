import React from 'react';
import { hot } from 'react-hot-loader';

class DirectoriesPage extends React.Component {
    render(){
        return(
            <div id={"directoriespage"}>
                <h1>Directories</h1>
                <p>This is the Directories Page section.</p>
            </div>
        );
    }
}

export default hot(module)(DirectoriesPage);
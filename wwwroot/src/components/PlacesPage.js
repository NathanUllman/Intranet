import React from 'react';
import { hot } from 'react-hot-loader';

class PlacesPage extends React.Component {
    render(){
        return(
            <div id={"placespage"}>
                <h1>Places</h1>
                <p>This is the Places Page section.</p>
            </div>
        );
    }
}

export default hot(module)(PlacesPage);
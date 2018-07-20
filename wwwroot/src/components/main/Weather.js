import React from 'react';
import { hot } from 'react-hot-loader';

const Weather = () => (
    <div className="tile is-parent">
        <div id="weather" className="tile is-child box">
            <h3>Weather Widget</h3>
        </div>
    </div>
)

export default hot(module)(Weather);
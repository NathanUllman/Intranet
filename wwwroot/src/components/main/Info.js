import React from 'react';
import { hot } from 'react-hot-loader';

const Info = () => (
    <div className="tile is-parent">
        <div id="info" className="tile is is-child box">
            <div className={"card-header"}>
                <div className={"card-header-title"}>
                    <h4>Info</h4>
                </div>
            </div>
            <div className={"card-content"}>
                <p>Some dummy info...</p>
            </div>
        </div>
    </div>
)

export default hot(module)(Info);
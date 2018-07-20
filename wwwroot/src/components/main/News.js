import React from 'react';
import { hot } from 'react-hot-loader';

const News = () => (
    <div className="tile is-parent">
        <div id="news" className="tile is-child box">
            <div className={"card-header"}>
                <div className={"card-header-title"}>
                    <h4>News</h4>
                </div>
            </div>
            <div className={"card-content"}>
                <p>Everything is in the news!</p>
            </div>
        </div>
    </div>
)

export default hot(module)(News);
import React from 'react';
import { hot } from 'react-hot-loader';

const RecentSales = () => (
    <div className="tile is-parent">
        <div id="recentsales" className="tile is is-child box">
            <div className={"card-header"}>
                <div className={"card-header-title"}>
                    <h4>Recent Sales</h4>
                </div>
            </div>
            <div className={"card-content"}>
                <p>We sold everything and now are amazing!</p>
            </div>
        </div>
    </div>
)

export default hot(module)(RecentSales);
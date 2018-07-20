import React from 'react';
import { hot } from 'react-hot-loader';

const RecentActivity = () => (
    <div className="tile is-parent">
        <div id="recentactivity" className="tile is-child box">
            <div className={"card-header"}>
                <div className={"card-header-title"}>
                    <h4>Recent Activity</h4>
                </div>
            </div>
            <div className={"card-content"}>
                <p>PenLink was featured in this article: </p>
            </div>
        </div>
    </div>
)

export default hot(module)(RecentActivity);
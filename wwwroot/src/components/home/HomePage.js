import React, { Component } from 'react';
import { hot } from 'react-hot-loader';
import CarouselWithTabs from '../main/carousel/Carousel';
import RecentActivity from '../main/RecentActivity';
import Weather from '../main/Weather';
import Info from '../main/Info';
import NewHires from '../main/NewHires';
import News from '../main/News';
import RecentSales from '../main/RecentSales';
import '../../../../node_modules/bootstrap/dist/css/bootstrap.min.css';

class imageInfo {
    constructor(displayName, imageLocation) {
        this.displayName = displayName;
        this.imageLocation = imageLocation;
    }
}

class HomePage extends Component {

    render() {
        return (
            <div id={"homepage"}>
                <div id="tile-ancestor" className="tile is-ancestor">
                    <div id={"tile-first-col"} className="tile is-vertical is-2">
                        <RecentActivity/>
                        <Weather/>
                        <Info/>
                    </div>

                    <div id={"carousel-parent"} className="tile is-parent is-8">
                        <div id="carousel" className="tile is-child box">
                            <CarouselWithTabs
                            />
                        </div>
                    </div>
                    <div id={"tile-last-col"} className="tile is-vertical is-2">
                        <NewHires/>
                        <News/>
                        <RecentSales/>
                    </div>
                </div>
            </div>
            );
    }
}

HomePage.defaultProps = {};

export default hot(module)(HomePage);
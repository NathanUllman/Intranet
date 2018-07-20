import React, { Component } from 'react';
import { hot } from 'react-hot-loader';


class NewHires extends Component {

    constructor(props) {
        super(props);
        this.state = {
            newHireInfo: []
        };
        this.callback = this.callback.bind(this);
    }


    callback(response) {
        this.setState({ newHireInfo: JSON.parse(response) });
    }
    componentDidMount() {  
        // gets new hire info
        var callback = this.callback; // gotta do this cause of scope I think :D
        var xmlHttp = new XMLHttpRequest();
        xmlHttp.onreadystatechange = function () {
            if (xmlHttp.readyState === 4 && xmlHttp.status === 200)
                callback(xmlHttp.responseText);
        }
        xmlHttp.open("GET", "React/GetNewHireInfo", true); // true for asynchronous 
        xmlHttp.send(null);
    }


    render() {
        const data = this.state.newHireInfo;

        return (
            <div className = "tile is-parent" >
                 <div id="info" className="tile is-child box">
                    <div className={"card-header"}>
                        <div className={"card-header-title"}>
                            <h4>New Hires</h4>
                        </div>
                    </div>
                    <div className={"card-content"}>
                        {data.map((item) =>
                            <div key={item.ID}>
                                <p>{item.FirstName}</p>
                                <p>{item.Description}</p>
                                <b>this message has been brought to you by Database</b>
                                <p> "Data in your face? Get a Database!"</p>
                            </div>                          
                            )}
                    </div>
                </div>
            </div>
        );
    }
}

export default NewHires;
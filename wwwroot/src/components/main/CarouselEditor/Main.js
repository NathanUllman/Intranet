import React, { Component } from 'react';
import PropTypes from 'prop-types';
import DashSortableComponent from './DashSortableList'
import DashItemsSortableComponent from './DashItemsSortableList'
import '../../../styles/styles.css';
import Header from '../../common/Header'

import {
    SortableContainer,
    SortableElement,
    SortableHandle,
    arrayMove,
} from 'react-sortable-hoc';


class CarouselEditor extends Component {

    constructor(props) {
        super(props);
        this.state = {
            selectedId: -1,
            dashboards: [],
            dashboardItems: [],
            filteredDashboardItems: []
        };

        this.GET = this.GET.bind(this);
        this.handleDashSelected = this.handleDashSelected.bind(this);
        this.onSortEndDash = this.onSortEndDash.bind(this);
        this.onSortEndDashItems = this.onSortEndDashItems.bind(this);
        this.POST = this.POST.bind(this);
        this.UpdateItems = this.UpdateItems.bind(this);
    }

    componentDidMount() {

        // Nested so that GET requests are done in order and not async todo fix dis and make it more fancy

        this.GET("/React/GetItemsForDashboard", (response) => {
            this.setState({ dashboardItems: JSON.parse(response) });

            this.GET("/React/GetDashboards", (response) => {
                this.setState({ dashboards: JSON.parse(response) });

                var dashboards = JSON.parse(response);
                if (dashboards.length > 0) {
                    this.state.selectedId = dashboards[0].DashboardID;
                    this.handleDashSelected(dashboards[0].DashboardID);
                }

            });
        });    
    }



    GET(route, callback) {
        var xmlHttp = new XMLHttpRequest();
        xmlHttp.onreadystatechange = function () {
            if (xmlHttp.readyState === 4 && xmlHttp.status === 200)
                callback(xmlHttp.responseText);
        }
        xmlHttp.open("GET", route, true);
        xmlHttp.send(null);
    }

    POST(route, items, callback) {

        var xhttp = new XMLHttpRequest();
        xhttp.onreadystatechange = function () {
            if (this.readyState === 4 && this.status === 200) {
                callback();
            }
        };

        xhttp.open("POST", route, true);
        xhttp.setRequestHeader("Content-type", "application/json");
        xhttp.send(JSON.stringify(items));
    }

    UpdateItems() {
        //   this.GET("/React/GetItemsForDashboard", (response) => {
        //      this.setState({ dashboardItems: JSON.parse(response) });
        // });
    }



    handleDashSelected(id) {
        this.setState({ selectedId: id });

        // change what dash items are showed based on active ID
        var filterList = this.state.dashboardItems;
        console.log(filterList);
        filterList = filterList.filter(item => item.DashboardID == id); // filter so we only show Dash items for selected Dashboard
        this.setState({ filteredDashboardItems: filterList });
    }

    onSortEndDash(Index, e) {
        var dashboards = this.state.dashboards;
        this.setState({
            dashboards: arrayMove(dashboards, Index.oldIndex, Index.newIndex),
        });

        dashboards = this.state.dashboards;
        // sort order for each dashboard is updated after drag/drop
        var updatedOrder = [];
        dashboards.map((element) => {
            element.SortOrder = dashboards.indexOf(element);
            updatedOrder.push(element);
        });

        this.setState({
            dashboards: updatedOrder
        });
    }

    onSortEndDashItems(Index, e) {
        var filteredList = this.state.filteredDashboardItems;
        this.setState({
            filteredDashboardItems: arrayMove(filteredList, Index.oldIndex, Index.newIndex),
        });

        filteredList = this.state.filteredDashboardItems;

        var updatedOrder = [];
        filteredList.map((element) => {
            element.SortOrder = filteredList.indexOf(element);
            updatedOrder.push(element);
        });

        this.setState({
            filteredDashboardItems: updatedOrder
        });
    }

    render() {

        return (
            <div className="container-fluid body-content">
                <Header />
                <div className="row">
                    <div className="col-sm-4">

                        <div className="panel panel-default">
                            <div className="panel-heading">
                                <h2>Dashboards</h2>
                            </div>
                            <div className="panel-body">

                                <DashSortableComponent
                                    dashboards={this.state.dashboards}
                                    ItemSelectedHandler={this.handleDashSelected}
                                    activeId={this.state.selectedId}
                                    onSortEnd={this.onSortEndDash} />

                                <a href="DashboardForms/AddDashboard/" className="btn btn-primary">Add Dashboard</a>
                                <a className="btn btn-default" onClick={() => this.POST("/React/UpdateDashboards", this.state.dashboards, function(){} )}>Save Order</a>
                                <a href={"/DashboardForms/EditDashboard?DashboardId=" + this.state.selectedId} className="btn btn-success">Edit Selected</a>
                                <a href={"/React/DeleteDashboard?DashboardId=" + this.state.selectedId} className="btn btn-danger">Delete Selected</a>

                            </div>
                        </div>
                    </div>
                    <div className="col-sm-8">

                        <div className="panel panel-default">
                            <div className="panel-heading">
                                <h2>Corresponding Dash Items</h2>
                            </div>
                            <div className="panel-body">
                                <DashItemsSortableComponent
                                    dashboardItems={this.state.filteredDashboardItems}
                                    activeId={this.state.selectedId}
                                    onSortEnd={this.onSortEndDashItems}
                                />
                                <span>
                                    <div className="dropdown">
                                        <button className="btn btn-primary dropdown-toggle" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                            Add dashboard Item
                                    </button>

                                        <div className="dropdown-menu" aria-labelledby="dropdownMenuButton">
                                            <a className="dropdown-item" href={"/DashboardForms/ImageUpload?DashboardId=" +
                                                this.state.selectedId}>Uploaded Image</a>
                                            <a className="dropdown-item" href={"/DashboardForms/ImageHtmlScrapping?DashboardId=" +
                                                this.state.selectedId}>html Scrapped Image</a>
                                            <a className="dropdown-item" href={"/DashboardForms/TextOnly?DashboardId=" +
                                                this.state.selectedId}>Text</a>
                                        </div>
                                    </div>
                                    <a className="btn btn-default" onClick={() => this.POST("/React/UpdateDashboardItems", this.state.filteredDashboardItems, this.UpdateItems)}>Save Order</a>
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}
export default CarouselEditor
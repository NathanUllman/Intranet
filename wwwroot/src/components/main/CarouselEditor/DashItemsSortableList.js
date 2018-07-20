import React, { Component } from 'react';
import PropTypes from 'prop-types';
//import { DashSelector } from './DashSelector';
//import {ItemsDisplay } from './ItemsDisplay'
import '../../../styles/styles.css';
import {
    SortableContainer,
    SortableElement,
    SortableHandle,
    arrayMove,
} from 'react-sortable-hoc';

const DragHandle = SortableHandle(() => <td className="hoverCursor">:::</td>); // This can be any component you want

const DashItemsSortableItem = SortableElement(({ dashboardItem, activeId }) => {
    return (
        <tr >
            <DragHandle  />
            <td style={{ paddingRight: 20 }}>{dashboardItem.DashboardItemID}</td>
            <td style={{ paddingRight: 20 }}>{dashboardItem.Title}</td>
            <td style={{ paddingRight: 20 }}>{dashboardItem.SortOrder}</td>
            <td style={{ paddingRight: 20 }}>{dashboardItem.StartDateTime}</td>
            <td style={{ paddingRight: 20 }}>{dashboardItem.EndDateTime}</td>
            <td style={{ paddingRight: 20 }}>{dashboardItem.DashboardTypeID}</td>
            <td style={{ paddingRight: 20 }}>{dashboardItem.DashboardItemStatusID}</td>
            <td style={{ paddingRight: 20 }}><a href={"/DashboardForms/EditDashboardItem?DashboardItemId=" +
                dashboardItem.DashboardItemID} className="btn btn-success">Edit</a></td>
            <td style={{ paddingRight: 20 }}><a href={"/React/DeleteDashboardItem?DashboardItemId=" +
                dashboardItem.DashboardItemID} className="btn btn-danger">Delete</a></td>
        </tr>
    );
});

const DashItemsSortableList = SortableContainer(({ items, activeId }) => {
    return (
        <table className="table ">
            <tbody>
                <tr><td></td><th>Id</th><th>Title</th><th>Sort Order</th><th>Start Date Time</th><th>End Date Time</th><th>Type ID</th><th>Status ID</th><th></th><th></th></tr>
                {items.map((value, index) => (
                    <DashItemsSortableItem
                        key={`item-${index}`}
                        index={index}
                        dashboardItem={value}
                        activeId={activeId}
                    />
                ))}

            </tbody>
        </table>
    );
});

class DashItemsSortableComponent extends Component {

    render() {

        return <DashItemsSortableList
            items={this.props.dashboardItems}
            activeId={this.props.activeId}
            onSortEnd={this.props.onSortEnd}
            useDragHandle={true}
            lockToContainerEdges={true}
        />;
    }
}




export default DashItemsSortableComponent;

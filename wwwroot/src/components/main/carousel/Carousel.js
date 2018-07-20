import React, { Component } from 'react';

import { CarouselModal } from './CarouselModal';
import { Carousel } from 'react-bootstrap';
import { Nav } from 'react-bootstrap';
import { NavItem } from 'react-bootstrap';
import { Button } from 'react-bootstrap';

import PropTypes from 'prop-types';

class CarouselWithTabs extends Component {

    constructor(props) {
        super(props);
        this.state = {
            isOpen: false, // states if modal is shown or not
            selectedImage: "", // img tag of clicked image, gotten by e.target of click event
            currentKey: 0, // current key for both tabs and carousel body,
            carouselInterval: 5000, //the interval for carousel body
            carouselIntervalIsRunning: false, // states if the carousel is rotating or not
            displayName: "CarouselWithTabs", // for some reason debugging required me to add a name for each component
            currentUserRoles: [], // api call in beginning fills this with all the current user's roles
            itemsToShow : [] //info for what images should be displayed in the carousel, does not need to be set a default value
        };

        this.GET = this.GET.bind(this);

        this.startCarouselInterval = this.startCarouselInterval.bind(this);
        this.stopCarouselInterval = this.stopCarouselInterval.bind(this);
        this.incrementCarousel = this.incrementCarousel.bind(this);
        this.toggleCarouselInterval = this.toggleCarouselInterval.bind(this);

        this.toggleModal = this.toggleModal.bind(this);
        this.handleImageClick = this.handleImageClick.bind(this);
        this.handleCurrentKeyChangeForTabs = this.handleCurrentKeyChangeForTabs.bind(this);
    }

    /* ------------------------------------------------------------------------------------------------*/
    /*--------------------------- API Functions -------------------------------------------------------*/
    /*-------------------------------------------------------------------------------------------------*/

    GET(route, callback) {
        var xmlHttp = new XMLHttpRequest();
        xmlHttp.onreadystatechange = function () {
            if (xmlHttp.readyState === 4 && xmlHttp.status === 200)
                callback(xmlHttp.responseText);
        }
        xmlHttp.open("GET", route, true); // true for asynchronous 
        xmlHttp.send(null);
    }

    /* ------------------------------------------------------------------------------------------------*/
    /*------------------------- Beginning/End component Functions -------------------------------------*/
    /*-------------------------------------------------------------------------------------------------*/

     componentDidMount() { // this function is called right when component is built
         this.startCarouselInterval(); // start carousel rotation

         this.GET("React/GetCurrentUserRoles", (response) => { // grab needed info from api, stores response using passed function
             this.setState({ currentUserRoles: JSON.parse(response)});
         });
         this.GET("React/GetCarouselImages", (response) => {
            this.setState({ itemsToShow: JSON.parse(response)});
         });   
    }

    componentWillUnmount() {// this function is called right before component removed
        this.stopCarouselInterval();
    }

    /* ------------------------------------------------------------------------------------------------*/
    /*------------------------- Carousel Interval Functions -------------------------------------------*/
    /*-------------------------------------------------------------------------------------------------*/

    startCarouselInterval() {
        this.stopCarouselInterval(); // make sure we dont have more than one running
        this.interval = setInterval(() => this.incrementCarousel(), this.state.carouselInterval); //set incrementCarousel() to be called on every interval of time specified by this.state.carouselInterval
        this.setState({ carouselIntervalIsRunning: true });
    }

    stopCarouselInterval() {
        clearInterval(this.interval); // self explanitory
        this.setState({ carouselIntervalIsRunning: false });
    }

    incrementCarousel() {
        if (this.state.currentKey === this.state.itemsToShow.length - 1) { // if we are at end of carousel, go to front again
            this.setState({ currentKey: 0 });
        } else {
            this.setState({ currentKey: this.state.currentKey + 1 }); // move to next
        }
    }

    toggleCarouselInterval() {
        if (this.state.carouselIntervalIsRunning) {           
            this.stopCarouselInterval();
        } else {
            this.startCarouselInterval();
        }
    }

    /* ------------------------------------------------------------------------------------------------*/
    /*------------------------- Event Handlers --------------------------------------------------------*/
    /*-------------------------------------------------------------------------------------------------*/

    toggleModal() {
        this.setState({
            isOpen: !this.state.isOpen
        });
    }

    handleImageClick(e) {
        this.toggleModal(); // modal opened

        this.setState({
            selectedImage: e.target
        });

        this.stopCarouselInterval();
    }
    // when tab is selectd, we want to stop the rotation of carousel
    handleCurrentKeyChangeForTabs(key) {
        this.setState({
            currentKey: key
        });

        this.stopCarouselInterval();
    }

    /* ------------------------------------------------------------------------------------------------*/
    /*------------------------- Render Function -------------------------------------------------------*/
    /*-------------------------------------------------------------------------------------------------*/

    render() {
        return (
            <div>
                <Button bsStyle="primary" onClick={this.toggleCarouselInterval}>Turn Carousel Rotation {(this.state.carouselIntervalIsRunning) ? 'off' : 'on'}</Button>
                <CarouselTabs
                    data={this.state.itemsToShow}
                    activeKey={this.state.currentKey}
                    handleTabChange={this.handleCurrentKeyChangeForTabs} />

                <CarouselBody
                    data={this.state.itemsToShow}
                    activeKey={this.state.currentKey}
                    onImageClick={this.handleImageClick}
                    currentUserRoles={this.state.currentUserRoles} />
            
                <CarouselModal
                    isOpen={this.state.isOpen}
                    onClose={this.toggleModal}
                    imageToShowSrc={this.state.selectedImage.src}
                />
            </div>
        );
    }
}

CarouselWithTabs.defaultProps = {};

export default CarouselWithTabs;


/* ================================================================================================================= */
/* ================================================================================================================= */

class CarouselTabs extends Component {
    constructor() {
        super();
        this.state = {
            displayName: "CarouselTabs"
        };
    }

    render() {
        const data = this.props.data;

        // todo toshowTimes cannot be duplicated/ list cannot change
        return (
            <Nav bsStyle="tabs" activeKey={this.props.activeKey.toString()} onSelect={key => this.props.handleTabChange(parseInt(key, 10))}>
                {data.map((item) =>
                    <NavItem eventKey={data.indexOf(item).toString()} key={item.ID}>
                        {item.DisplayName}
                    </NavItem>
                )}
            </Nav>
        );
    }
}

CarouselTabs.defaultProps = {
    data: [
        {
            ID: 0,
            DisplayName: "Default DisplayName for CarouselTabs, Error Has Occured.",
            ImageLocation: "./images/Error.png",
            IsEditable : false           
        }],
    activeKey: 0
};
CarouselTabs.propTypes = {
    data: PropTypes.arrayOf(PropTypes.shape({
        ID: PropTypes.number.isRequired,
        DisplayName: PropTypes.string.isRequired,
        ImageLocation: PropTypes.string,
        IsEditable: PropTypes.bool
    })),
    activeKey: PropTypes.number.isRequired
};

/* ================================================================================================================= */
/* ================================================================================================================= */

class CarouselBody extends Component {
    constructor() {
        super();
        this.state = {
            displayName: CarouselBody
        };
    }
    render() {
        const data = this.props.data;

        // todo toshowTimes cannot be duplicated/ list cannot change
        return (
            <Carousel indicators={false} controls={false} activeIndex={this.props.activeKey}>
                {data.map((item) =>
                    <Carousel.Item key={item.ImageLocation}>
                        <img className={"resize-image"} src={item.ImageLocation} onClick={(item.Editable === true & this.props.currentUserRoles.includes("Administrator")) ? this.props.onImageClick : function(){}} />
                    </Carousel.Item>
                )}
            </Carousel>
        );
    }
}
CarouselBody.defaultProps = {
    data: [
        {
            ID: 0,
            DisplayName: "Default DisplayName for CarouselTabs, Error Has Occured.",
            ImageLocation: "./images/Error.png",
            IsEditable: false
        }],
    activeKey: 0
};
CarouselBody.propTypes = {
    data: PropTypes.arrayOf(PropTypes.shape({
        ID: PropTypes.number.isRequired,
        DisplayName: PropTypes.string,
        ImageLocation: PropTypes.string.isRequired,
        IsEditable: PropTypes.bool.isRequired
    })),
    activeKey: PropTypes.number.isRequired
};
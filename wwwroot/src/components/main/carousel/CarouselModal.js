import React, { Component } from 'react';
import '../../../styles/styles.css';
import PropTypes from 'prop-types';

//https://daveceddia.com/open-modal-in-react/ 
export class CarouselModal extends Component {
    constructor() {
        super();
        this.state = {
            currentUploadedImagePage: "",// you could put a default image place holder here
            displayName: "CarouselModal"
        };

        this.componentDidMount = this.componentDidMount.bind(this);
        this.checkWindow = this.checkWindow.bind(this);
        this.handleUploadImageChange = this.handleUploadImageChange.bind(this);
    }


    componentDidMount() {
        window.addEventListener('click', this.checkWindow);
    }

    checkWindow(e) { // modal will close if user clicks outside of modal window
        var modal = document.getElementById('myModal');

        if (e.target === modal) {
            this.props.onClose(e);
        }
    }

    //https://stackoverflow.com/questions/4459379/preview-an-image-before-it-is-uploaded
    handleUploadImageChange(e) {

        // takes the file that was uploaded and displays it in the modal to the user
        var file = e.target.files[0];
        var reader = new FileReader();

        reader.onload = function () {
            document.getElementById('PreviewImg').setAttribute('src', reader.result);
        };
        reader.readAsDataURL(file);
    }


    render() {
        
        if (!this.props.isOpen) { // will not show unless isOpen state for Carousel is true
            return null;
        }
        
        return (       
            <div id="myModal" className="myModal"> {/*uses my own css located in style.css*/}
                <div className="myModal-content">
                    <span onClick={this.props.onClose} className="myClose">&times;</span>
                    <p>Current Image</p>
                    <img src={this.props.imageToShowSrc} width="25%" height="25%" />
                    <img src={this.state.currentUploadedImagePage} width="25%" height="25%" id="PreviewImg" />
                    <ImageInputForm
                        uploadedImageChange={this.handleUploadImageChange}
                        imageLocation={this.props.imageToShowSrc} // done so that hidden input can have necessary value
                    />
                </div>
           </div>               
        );
    }
}

CarouselModal.defaultProps = {
    isOpen: false,
    imageToShowSrc: "./images/Error.png"

};
CarouselModal.propTypes = {
    isOpen: PropTypes.bool.isRequired,
    imageToShowSrc: PropTypes.string.isRequired
};

class ImageInputForm extends Component {

    constructor() {
        super();
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    // to clear up any confusion about submission process (this function is not actually needed)
    handleSubmit() {
        e.preventDefault(); // dont want the page to refresh and go 
        document.getElementById("ImageUploadForm").submit(); // these two steps done so that file is submited before this function is continued   
    }

    render() {
        return (
            <form id="ImageUploadForm" method="post" encType="multipart/form-data" action="/React/UploadImage" onSubmit={this.handleSubmit}>
                <div className="form-group">
                    <h2>Upload one or more files using this form:</h2>
                    <input type="file" name="files" multiple accept="image/*" onChange={this.props.uploadedImageChange} />
                    <input type="hidden" value={this.props.imageLocation} name="location" />
                </div>
                <div className="form-group">
                    <input type="submit" value="Upload" className="btn btn-default" />
                </div>
            </form>
        );
    }
}

ImageInputForm.defaultProps = {
    imageLocation: "./images/Error.png"
};

ImageInputForm.propTypes = {
    imageLocation : PropTypes.string.isRequired
};
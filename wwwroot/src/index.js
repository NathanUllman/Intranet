import React from 'react';
import { render } from 'react-dom';
import App from './App';
import CarouselEditor from './components/main/CarouselEditor/main'
import 'bulma/css/bulma.min.css';
import 'bootstrap/dist/css/bootstrap.min.css';

//const root = document.createElement('div');
//document.body.appendChild(root);
//root.id = 'content';
//const root = document.getElementById('root');
//const test = document.getElementById('ya');

if (document.getElementById('root')) {
    render(<App />, document.getElementById('root'));
}
if (document.getElementById('ya')) {
    render(<CarouselEditor />, document.getElementById('ya'));
}

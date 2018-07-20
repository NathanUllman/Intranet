import React from 'react';
import {BrowserRouter, Route} from 'react-router-dom';
import PrimaryLayout from './PrimaryLayout';

const App = () => (
    <BrowserRouter>
        <PrimaryLayout />
    </BrowserRouter>
)

export default App;
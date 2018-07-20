import React from 'react';
import Header from './components/common/Header';
import Body from './components/Body';
import Footer from './components/common/Footer';
import { hot } from 'react-hot-loader';

const PrimaryLayout = () => (
    <section id={"primarylayout"}>
        <Header/>
        <Body/>
        <Footer/>
    </section>
)

export default hot(module)(PrimaryLayout);
import React from 'react';
import { hot } from 'react-hot-loader';
import '../../marquee.css';

const Marquee = () => (
    <p className="marquee">
        This text is scrolling. If it is not, it is the other intern's fault. This is where news items will appear.

    </p>
)

export default hot(module)(Marquee);
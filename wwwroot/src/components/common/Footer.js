﻿import React from 'react';
import { hot } from 'react-hot-loader';

const Footer = () => (
    <footer id={"footer"} className={"level"}>
        <div className="container">
            <div className="has-text-centered level-item">
                <p>
                    <strong>Bulma</strong> by <a href="https://jgthms.com">Jeremy Thomas</a>. The source code is licensed
                    <a href="http://opensource.org/licenses/mit-license.php">MIT</a>. The website content
                    is licensed <a href="http://creativecommons.org/licenses/by-nc-sa/4.0/">CC BY NC SA 4.0</a>.
                </p>
            </div>
        </div>
    </footer>
)

export default hot(module)(Footer);

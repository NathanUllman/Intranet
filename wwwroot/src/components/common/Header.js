import React from 'react';
import PropTypes from 'prop-types';
import {NavLink} from 'react-router-dom';
import { hot } from 'react-hot-loader';


const Header = () => (
    <div id={"header"}>
        <nav id="navmenu" className="navbar is-marginless" role="navigation" aria-label="main navigation">
            <div className="navbar-brand">
                <a className="navbar-item">
                    <img src="https://www.penlink.com//wp-content/uploads/2017/02/Logo.png" />
                </a>
            </div>
            <div className="navbar-menu">
                <div className="navbar-end">
                    <a href="/Account/Login" className={"navbar-item"}>Login</a>
                    <a href="/AdminTools/DashManager" className={"navbar-item"}>Dash Manager</a>
                    <a href="/AdminTools/UserList" className={"navbar-item"}>User Manager</a>
                </div>
            </div>
        </nav>
    </div>
)

export default hot(module)(Header);

/* <NavLink to={"/latest"} activeClassName={"active"} className={"navbar-item"}>Latest</NavLink>
                    <NavLink to={"/essentials"} activeClassName={"active"} >Essentials</NavLink>
                    <NavLink to={"/people"} activeClassName={"active"} className={"navbar-item"}>People</NavLink>
                    <NavLink to={"/places"} activeClassName={"active"} className={"navbar-item"}>Places</NavLink>
                    <NavLink to={"/directories"} activeClassName={"active"} className={"navbar-item"}>Directories</NavLink>
                    <NavLink to={"/social"} activeClassName={"active"} className={"navbar-item"}>Social</NavLink>
                    <NavLink to={"/admin"} activeClassName={"active"} className={"navbar-item"}>Admin</NavLink>
                    <NavLink to={"/edit"} activeClassName={"active"} className={"navbar-item"}>Edit</NavLink>*/
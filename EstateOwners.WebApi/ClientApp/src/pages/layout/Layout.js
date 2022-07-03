import React, {useEffect, useState} from 'react';
import {Container} from 'reactstrap';
import NavMenu from './NavMenu';
import {AuthContext} from "../../context/AuthContext";
import AuthService from "../../core/auth/AuthService";
import Loader from "../../components/loader/loader";

const Layout = (props) => {
    return (
        <div>
            <NavMenu/>
            <Container>
                {props.children}
            </Container>
        </div>
    );
}

export default Layout;
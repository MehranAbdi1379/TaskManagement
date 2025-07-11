import React from "react";
import {Outlet} from "react-router-dom";
import {Box} from "@chakra-ui/react";
import Navbar from "./Navbar";

const Layout: React.FC = () => {
    return (
        <Box>
            {/* Navbar */}
            <Navbar></Navbar>
            {/* Page Content */}
            <Box p={4}>
                <Outlet/> {/* This is where child routes will be rendered */}
            </Box>
        </Box>
    );
};

export default Layout;

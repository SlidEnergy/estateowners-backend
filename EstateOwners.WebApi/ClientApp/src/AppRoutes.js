import {Counter} from "./pages/Counter";
import {FetchData} from "./pages/FetchData";
import {Home} from "./pages/Home";
import AdminLogin from "./pages/admin/AdminLogin";
import Login from "./pages/Login";
import Users from "./pages/admin/Users";
import User from "./pages/admin/User";

import React, {useContext} from 'react';
import {Route, Routes} from "react-router-dom";
import {ProtectedRoute} from "./routing/ProtectedRoute";
import {AuthContext} from "./context/AuthContext";

const AppRoutes = () => {
    const {isAuth, setIsAuth} = useContext(AuthContext);

    return (
        <Routes>
            <Route index element={<Home />}></Route>
            <Route path='/login' element={<Login />}></Route>
            <Route path='/counter' element={<Counter />}></Route>
            <Route path='/fetch-data' element={<FetchData />}></Route>
            <Route path='/admin' >
                <Route element={<ProtectedRoute isAllowed={!isAuth} redirectPath='/admin/users'></ProtectedRoute>}>
                    <Route index element={<AdminLogin />}></Route>
                    <Route path='/admin/login' element={<AdminLogin />}></Route>
                </Route>
                <Route element={<ProtectedRoute isAllowed={isAuth} redirectPath='/admin/login' />}>
                    <Route path='/admin/users' element={<Users />}>
                        <Route path=':id' element={<User />}></Route>
                        <Route index element={<Users />}></Route>
                    </Route>
                </Route>
            </Route>
        </Routes>
    );
};

export default AppRoutes;
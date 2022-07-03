import {useEffect, useState} from 'react';
import AuthService from "./AuthService";
import {AuthContext} from "../../context/AuthContext";
import Loader from "../../components/loader/loader";

const AuthProvider = ({children}) => {
    const [isAuth, setIsAuth] = useState(undefined);

    useEffect(() => {
        const result = AuthService.isAuth();
        setIsAuth(result);
    }, []);

    return (
        <AuthContext.Provider value={{isAuth, setIsAuth}}>
            {isAuth === undefined
                ? <Loader/>
                : <div>{children}</div>
            }
        </AuthContext.Provider>
    );
};

export default AuthProvider;
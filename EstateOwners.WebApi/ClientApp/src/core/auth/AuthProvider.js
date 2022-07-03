import {useEffect, useState} from 'react';
import AuthService from "./AuthService";
import {AuthContext} from "../../context/AuthContext";
import Loader from "../../components/loader/loader";

const AuthProvider = ({children}) => {
    const [isAuth, setIsAuth] = useState(undefined);
    const [auth, setAuth] = useState(undefined);

    useEffect(() => {
        const auth = AuthService.getAuth();
        setIsAuthAndAuth(auth);
    }, []);

    function setIsAuthAndAuth(auth) {
        const value = !!(auth && auth.token);
        setIsAuth(value);

        if(value)
            setAuth(auth);
    }

    return (
        <AuthContext.Provider value={{isAuth, auth, setAuth: setIsAuthAndAuth}}>
            {isAuth === undefined
                ? <Loader/>
                : <div>{children}</div>
            }
        </AuthContext.Provider>
    );
};

export default AuthProvider;
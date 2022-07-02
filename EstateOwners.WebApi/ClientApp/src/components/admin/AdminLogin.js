import React, {useState} from 'react';
import AuthService from "../../core/authService";
import Loader from "../shared/loader/loader";
import {useFetching} from "../../hooks/useFetching";

const AdminLogin = () => {
    const [login, setLogin] = useState('');
        const [password, setPassword] = useState('');
        const [logInFetching, isLoading, error] = useFetching(async () => {
            return await AuthService.login(login, password);
        });

    async function logIn(e) {
        e.preventDefault();

        const result = await logInFetching();

        if(result) {
            //redirect
            return;
        } else {
            // show error
            console.log("Не удалось войти в систему");
        }
    }

    return (
        <div>
            <h1>Login</h1>
            <form>
                <input type="text" placeholder="Имя пользователя" name="login" value={login} onChange={e => setLogin(e.target.value)}/>
                <input type="password" placeholder="Пароль" name="password" value={password} onChange={e => setPassword(e.target.value)} />
                <button disabled={isLoading} onClick={logIn}>Login</button>
                {isLoading  &&
                    <div style={{display: 'flex', justifyContent: 'center', marginTop: '50px'}}><Loader></Loader></div>
                }
                {error &&
                    <div style={{marginTop: '20px'}}>
                    <h5>Произошла ошибка</h5>
                    <div>{error}</div>
                    </div>
                }
            </form>
        </div>
    );
};

export default AdminLogin;
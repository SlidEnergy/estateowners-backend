import TokenService from "../../api/TokenService";

export default class AuthService {
    static async login(email, password) {
        const auth = await TokenService.get(email, password);

        console.log(auth);

        if(!auth)
            return false;

        localStorage.setItem('auth', JSON.stringify(auth));

        return true;
    }

    static logout() {
        localStorage.removeItem('auth');
    }

    static isAuth() {
        const auth = localStorage.getItem('auth')

        return !!auth;
    }
}
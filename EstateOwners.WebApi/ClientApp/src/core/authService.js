import TokenService from "../api/tokenService";

export default class AuthService {
    static async login(email, password) {
        const auth = await TokenService.get(email, password);

        if(!auth)
            return false;

        localStorage.setItem('auth', JSON.stringify(auth));

        return true;
    }
}
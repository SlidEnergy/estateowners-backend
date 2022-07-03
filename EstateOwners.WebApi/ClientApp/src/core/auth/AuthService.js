import TokenService from "../api/TokenService";

export default class AuthService {
    static async login(email, password) {
        const auth = await TokenService.get(email, password);

        if (!auth)
            return false;

        localStorage.setItem('auth', JSON.stringify(auth));

        return auth;
    }

    static logout() {
        localStorage.removeItem('auth');
    }

    static getAuth() {
        const item = localStorage.getItem('auth')

        try {
            const auth = item && JSON.parse(item);
            if (auth && auth.token)
                return auth;
            else
                return undefined;
        } catch {
            return undefined;
        }
    }
}
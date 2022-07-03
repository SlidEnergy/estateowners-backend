import axios from "axios";

export default class TokenService {
    static async get(email, password) {
        const auth = await axios.post('/api/v1/token', {email, password});

        return auth;
    }
}
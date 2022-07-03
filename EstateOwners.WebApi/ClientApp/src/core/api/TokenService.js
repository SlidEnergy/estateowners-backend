import {http} from "../http-common";

export default class TokenService {
    static async get(email, password) {
        const response = await http.post('/api/v1/token', {email, password});

        return response.data;
    }
}
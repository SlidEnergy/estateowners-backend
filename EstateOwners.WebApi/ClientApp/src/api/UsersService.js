import axios from "axios";

export class UsersService {
    static async getList() {
        const response = await axios.get('/api/v1/users');
        return response.data;
    }

    static async getById(id) {
        const response = await axios.get('/api/v1/users' + id);
        return response.data;
    }
}
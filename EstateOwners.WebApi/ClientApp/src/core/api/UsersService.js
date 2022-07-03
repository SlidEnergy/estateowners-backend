import axios from "axios";
import AuthService from "../auth/AuthService";
import {http} from "../http-common";

export class UsersService {
    static async getList() {
        const auth = AuthService.getAuth();

        const response = await http.get('/users', {headers: { 'Authorization': 'Bearer ' + auth.token}});
        return response.data;
    }

    static async getById(id) {
        const auth = AuthService.getAuth();

        const response = await http.get('/users/' + id, {headers: { 'Authorization': 'Bearer ' + auth.token}});
        return response.data;
    }
}
import React, {useEffect, useState} from 'react';
import {useFetching} from "../../hooks/useFetching";
import {UsersService} from "../../api/UsersService";
import Loader from "../../components/loader/loader";

const Users = () => {
    const [users, setUsers] = useState();

    const [fetchUsers, isLoading, error] = useFetching(async () => {
        const list = await UsersService.getList();
        setUsers(list);
    })

    useEffect( () => {
        fetchUsers();
    }, []);

    return (
        <div>
            {isLoading &&
                <div style={{display: 'flex', justifyContent: 'center', marginTop: '50px'}}><Loader></Loader></div>
            }
            {error &&
                <div style={{marginTop: '20px'}}>
                    <h5>Произошла ошибка</h5>
                    <div>{error}</div>
                </div>
            }

            {users && users.map((user, index) =>
                <div key={user.id}>{user.email}</div>
            )}
        </div>
    );
};

export default Users;
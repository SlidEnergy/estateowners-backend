import {useState} from "react";

export const useFetching = (callback) => {
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState('');

    async function fetching(...args) {
        try{
            setIsLoading(true);
            setError('')
            return await callback(...args);
        }
        catch (error) {
            console.log(error)
            setError(error.message);
        }
        finally {
            setIsLoading(false);
        }
    }

    return [fetching, isLoading, error];
}
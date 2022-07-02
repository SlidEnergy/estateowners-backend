import {useState} from "react";

export const useFetching = (callback) => {
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState('');

    async function fetching() {
        try{
            setIsLoading(true);
            setError('')
            return await callback();
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
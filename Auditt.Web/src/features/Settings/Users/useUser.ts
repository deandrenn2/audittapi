import { useQuery } from "@tanstack/react-query";
import { getUser } from "./UserServices";

const KEY = "user";
export const useUser = () =>{
    const queryUser = useQuery({
        queryKey: [`${KEY}`],
        queryFn: getUser,
    });
return{
    Users: queryUser?.data?.data,
    queryUser,
}
    
}
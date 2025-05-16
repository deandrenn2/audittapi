import { useMutation, useQuery } from "@tanstack/react-query";
import { createUsertServices, getUser, updateUserServices } from "./UserServices";
import { toast } from "react-toastify";

const KEY = "user";
export const useUser = () =>{
    const queryUser = useQuery({
        queryKey: [`${KEY}`],
        queryFn: getUser,
    });

    const createUser = useMutation({
		mutationFn: createUsertServices,
		onSuccess: (data) => {
			if (!data.isSuccess) {
				toast.info(data.message);
			} else {
				if (data.isSuccess) {
					toast.success(data.message);
					queryUser.refetch();
				}
			}
		},
	});

    const updateUser = useMutation({
		mutationFn: updateUserServices,
		onSuccess: (data) => {
			if (!data.isSuccess) {
				toast.info(data.message);
			} else {
				if (data.isSuccess) {
					toast.success(data.message);
					queryUser.refetch();
				}
			}
		},
	});

return{
    users: queryUser?.data?.data,
    queryUser,
    createUser,
    updateUser,
}
    
}
import { useMutation, useQuery } from "@tanstack/react-query";
import { createRoleServices, deleteRoleServices, getRoles, updateRoleServices } from "./RolesServices";
import { toast } from "react-toastify";
const KEY = "roles";
export const useRoles = () => {
    const queryRoles = useQuery({
        queryKey: [`${KEY}`],
        queryFn: getRoles,
    });

    const createRole = useMutation({
        mutationFn: createRoleServices,
        onSuccess: (data) => {
            if (!data.isSuccess) {
                toast.info(data.message);
            } else {
                if (data.isSuccess) {
                    toast.success(data.message);
                    queryRoles.refetch();
                }
            }
        },
    });

    const updateRole = useMutation({
        mutationFn: updateRoleServices,
        onSuccess: (data) => {
            if (!data.isSuccess) {
                toast.info(data.message);
            } else {
                if (data.isSuccess) {
                    toast.success(data.message);
                    queryRoles.refetch();
                }
            }
        },
    });

    const deleteRole = useMutation({
        mutationFn: deleteRoleServices,
        onSuccess: (data) => {
            if (!data.isSuccess) {
                toast.info(data.message);
            } else {
                if (data.isSuccess) {
                    toast.success(data.message);
                    queryRoles.refetch();
                }
            }
        },
    });

    return {
        roles: queryRoles?.data?.data,
        queryRoles,
        createRole,
        deleteRole,
        updateRole,
    };
}


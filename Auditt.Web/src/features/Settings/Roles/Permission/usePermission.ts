import { useMutation, useQuery } from "@tanstack/react-query";
import { toast } from "react-toastify";
import { createPermissionServices, deletePermissionServices, getPermission, updatePermissionServices } from "./PermissionServices";

const KEY = "Permission";
export const usePermission = () => {
	const queryScale = useQuery({
		queryKey: [`${KEY}`],
		queryFn: getPermission,
	});

	const createPermission = useMutation({
		mutationFn: createPermissionServices,
		onSuccess: (data) => {
			if (!data.isSuccess) {
				toast.info(data.message);
			} else {
				if (data.isSuccess) {
					toast.success(data.message);
					queryScale.refetch();
				}
			}
		},
	});

	const updatePermission = useMutation({
		mutationFn: updatePermissionServices,
		onSuccess: (data) => {
			if (!data.isSuccess) {
				toast.info(data.message);
			} else {
				if (data.isSuccess) {
					toast.success(data.message);
					queryScale.refetch();
				}
			}
		},
	});

	const deletePermission = useMutation({
		mutationFn: deletePermissionServices,
		onSuccess: (data) => {
			if (!data.isSuccess) {
				toast.info(data.message);
			} else {
				if (data.isSuccess) {
					toast.success(data.message);
					queryScale.refetch();
				}
			}
		},
	});

	return {
		permissions: queryScale?.data?.data,
		queryScale,
		createPermission,
		deletePermission,
		updatePermission,
	};
};


import { useMutation, useQuery } from "@tanstack/react-query";
import { toast } from "react-toastify";
import {
	createScaleServices,
	deleteScaleServives,
	getScale,
	getScaleById,
	updateScaleServices,
} from "./ScalesServices";

const KEY = "Scale";
export const useScales = () => {
	const queryScale = useQuery({
		queryKey: [`${KEY}`],
		queryFn: getScale,
	});

	const createScale = useMutation({
		mutationFn: createScaleServices,
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

	const updateScale = useMutation({
		mutationFn: updateScaleServices,
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

	const deleteScale = useMutation({
		mutationFn: deleteScaleServives,
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
		scales: queryScale?.data?.data,
		queryScale,
		createScale,
		deleteScale,
		updateScale,
	};
};

export const useScaleById = (id: number) => {
	const queryScale = useQuery({
		queryKey: [`${KEY}`, id],
		queryFn: () => getScaleById(id),
		refetchOnWindowFocus: false,
		refetchOnMount: false,
	});

	return {
		scale: queryScale?.data?.data,
		queryScale,
	};
};

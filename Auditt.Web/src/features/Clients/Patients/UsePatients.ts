import { useMutation, useQuery } from "@tanstack/react-query";
import {
	createPatientsServices,
	deletePatientsServices,
	getPatientByDocument,
	getPatientById,
	getPatients,
	updatePatientsServices,
} from "./PantientsServices";
import { toast } from "react-toastify";

const KEY = "patients";
export const usePatients = () => {
	const queryPatients = useQuery({
		queryKey: [`${KEY}`],
		queryFn: getPatients,
	});

	const createPatients = useMutation({
		mutationFn: createPatientsServices,
		onSuccess: (data) => {
			if (!data.isSuccess) {
				if (data?.message) {
					toast.info(data.message);
				}
				if (data?.error) {
					toast.info(data.error.message);
				}
			} else {
				if (data.isSuccess) {
					toast.success(data.message);
					queryPatients.refetch();
				}
			}
		},
	});

	const updatePatients = useMutation({
		mutationFn: updatePatientsServices,
		onSuccess: (data) => {
			if (!data.isSuccess) {
				if (data?.message) {
					toast.info(data.message);
				}
				if (data?.error) {
					toast.info(data.error.message);
				}
			} else {
				if (data.isSuccess) {
					toast.success(data.message);
					queryPatients.refetch();
				}
			}
		},
	});

	const deletePatients = useMutation({
		mutationFn: deletePatientsServices,
		onSuccess: async (data) => {
			if (!data.isSuccess) {
				if (data?.message) {
					toast.info(data.message);
				}
				if (data?.error) {
					toast.info(data.error.message);
				}
			} else {
				if (data.isSuccess) {
					toast.success(data.message);
					await queryPatients.refetch();
				}
			}
		},
	});

	return {
		patients: queryPatients?.data?.data,
		queryPatients,
		createPatients,
		deletePatients,
		updatePatients,
	};
};

export const usePatientById = (id: number) => {
	const queryPatient = useQuery({
		queryKey: [`${KEY}`, id],
		queryFn: () => getPatientById(id),
	});
	return { queryPatient, patient: queryPatient.data?.data };
};

export const usePatientByDocumentMutation = () => {
	const getPatientByDocumentMutation = useMutation({
		mutationFn: getPatientByDocument,
		onSuccess: (data) => {
			if (!data.isSuccess) {
				if (data?.message) {
					toast.info(data.message);
				}
				if (data?.error) {
					toast.info(data.error.message);
				}
			} else {
				if (data.isSuccess) {
					toast.success(data.message);
				}
			}
		},
	});

	return { getPatientByDocumentMutation };
};

export const usePatientByDocument = (identity: string) => {
	const queryPatient = useQuery({
		queryKey: [`${KEY}`, identity],
		queryFn: () => getPatientByDocument(identity),
		enabled: false,
	});
	return { queryPatient, patient: queryPatient.data?.data };
};

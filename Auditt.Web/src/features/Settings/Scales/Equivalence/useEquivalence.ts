
import { useMutation, useQuery } from "@tanstack/react-query";
import { toast } from "react-toastify";
import { getEquivalence, createEquivalenceServices, deleteEqvalenceServives,  updateEquivalenceServices } from "./EquivalenceServices";


const KEY = "Equivalence";
export const useEquivalence = () => {
    const queryEquivalence = useQuery({
        queryKey: [`${KEY}`],
        queryFn: getEquivalence,
    });

    const createEquivalence = useMutation({
        mutationFn: createEquivalenceServices,
        onSuccess: (data) => {
            if (!data.isSuccess) {
                toast.info(data.message);
            } else {
                if (data.isSuccess) {
                    toast.success(data.message);
                    queryEquivalence.refetch();
                }
            }
        },
    });

    const updateEquivalence = useMutation({
        mutationFn: updateEquivalenceServices,
        onSuccess: (data) => {
            if (!data.isSuccess) {
                toast.info(data.message);
            } else {
                if (data.isSuccess) {
                    toast.success(data.message);
                    queryEquivalence.refetch();
                }
            }
        },
    });

    const deleteEqvalence = useMutation({
        mutationFn: deleteEqvalenceServives,
        onSuccess: (data) => {
            if (!data.isSuccess) {
                toast.info(data.message);
            } else {
                if (data.isSuccess) {
                    toast.success(data.message);
                    queryEquivalence.refetch();
                }
            }
        },
    });

    return {
        equivalences: queryEquivalence?.data?.data,
        queryEquivalence,
        createEquivalence,
        updateEquivalence,
        deleteEqvalence,
    };
}


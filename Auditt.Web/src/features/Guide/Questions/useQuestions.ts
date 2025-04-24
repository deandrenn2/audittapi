import { useMutation, useQuery } from "@tanstack/react-query";

import { toast } from "react-toastify";
import {
	createQuestionServices,
	deleteQuestionServices,
	getQuestions,
	updateQuestionServices,
} from "./QuestionsServices";

export const useQuestions = (idGuide: number) => {
	const KEY = "questions";
	const queryQuestions = useQuery({
		queryKey: [`${KEY}`, idGuide],
		queryFn: () => getQuestions(idGuide),
		enabled: !!idGuide,
	});

	const createQuestion = useMutation({
		mutationFn: createQuestionServices,
		onSuccess: (data) => {
			if (!data.isSuccess) {
				if (data?.message) toast.info(data.message);

				if (data?.error) toast.error(data.error.message);
			} else {
				if (data.isSuccess) {
					toast.success(data.message);
					queryQuestions.refetch();
				}
			}
		},
	});

	const updateQuestion = useMutation({
		mutationFn: updateQuestionServices,
		onSuccess: (data) => {
			if (!data.isSuccess) {
				if (data?.message) toast.info(data.message);

				if (data?.error) toast.error(data.error.message);
			} else {
				if (data.isSuccess) {
					toast.success(data.message);
					queryQuestions.refetch();
				}
			}
		},
	});

	const deleteQuestion = useMutation({
		mutationFn: deleteQuestionServices,
		onSuccess: (data) => {
			if (!data.isSuccess) {
				if (data?.message) toast.info(data.message);

				if (data?.error) toast.error(data.error.message);
			} else {
				if (data.isSuccess) {
					toast.success(data.message);
					queryQuestions.refetch();
				}
			}
		},
	});

	return {
		questions: queryQuestions?.data?.data,
		queryQuestions,
		createQuestion,
		deleteQuestion,
		updateQuestion,
	};
};

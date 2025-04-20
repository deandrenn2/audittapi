import { ApiClient } from "../../../shared/helpers/ApiClient";
import { MsgResponse } from "../../../shared/model";
import { QuestionsModel } from "./QuestionsModel";


export const getQuestions = async () => {
    const url = "api/questions";
    const response = await ApiClient.get<MsgResponse<QuestionsModel[]>>(url);

    if (response.status !== 200) {
        return {
            isSuccess: false,
            message: "Error al obtener las preguntas",
            isFailure: true,
            error: {
                code: response.status.toString(),
                message: response.statusText,
            },
        };
    }

    return response.data;
}

export const createQuestionServices = async (
    model: QuestionsModel
): Promise<MsgResponse<QuestionsModel>> => {
    const url = `api/questions/{idGuide}`;
    const response = await ApiClient.post<MsgResponse<QuestionsModel>>(url, model);

    if (response.status !== 200 && response.status !== 201) {
        return {
            isSuccess: false,
            message: "Error al crear la pregunta",
            isFailure: true,
            error: {
                code: response.status.toString(),
                message: response.statusText,
            },
        };
    }

    return response.data;
};


export const updateQuestionServices = async (
    model: QuestionsModel
): Promise<MsgResponse<QuestionsModel>> => {
    const url = "api/questions";
    const response = await ApiClient.put<MsgResponse<QuestionsModel>>(url, model);

    if (response.status !== 200 && response.status !== 201) {
        return {
            isSuccess: false,
            message: "Error al actualizar la pregunta",
            isFailure: true,
            error: {
                code: response.status.toString(),
                message: response.statusText,
            },
        };
    }

    return response.data;
};

export const deleteQuestionServices = async (
    id: number
): Promise<MsgResponse<QuestionsModel>> => {
    const url = `api/questions/${id}`;
    const response = await ApiClient.delete<MsgResponse<QuestionsModel>>(url);

    if (response.status !== 200 && response.status !== 201) {
        return {
            isSuccess: false,
            message: "Error al eliminar la pregunta",
            isFailure: true,
            error: {
                code: response.status.toString(),
                message: response.statusText,
            },
        };
    }

    return response.data;
};
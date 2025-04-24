import { useRef } from "react";
import { useQuestions } from "./useQuestions";
import { QuestionsModel } from "./QuestionsModel";

export const QuestionsCreate = ({ idGuide }: { idGuide: number }) => { 
    const { createQuestion } = useQuestions();
    const refForm = useRef<HTMLFormElement>(null);

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        const form = e.target as HTMLFormElement;

        const formData = new FormData(form);
        interface FormDataType {
            text: string;
            order: string;
        }

        const question = Object.fromEntries(formData.entries()) as unknown as FormDataType;

        const newQuestion: QuestionsModel = {
            text: question.text ?? "",
            order: Number(question.order),
            idGuide: idGuide ?? 0,
        };

        const response = await createQuestion.mutateAsync(newQuestion);

        if (response.isSuccess) {
            refForm.current?.reset();
        } 
    };

    return (
        <div className="w-full">
            <form ref={refForm} onSubmit={handleSubmit}>
                <label className="block font-medium mb-2" htmlFor="order">Orden</label>
                <input
                    type="number"
                    id="order"
                    name="order"
                    className="w-full mb-4 p-3 border rounded"
                    required
                />

                <label className="block font-medium mb-2" htmlFor="pregunta">Pregunta</label>
                <section className="w-full">
                    <textarea
                        id="text"
                        name="text"
                        className="w-full h-60 p-3 border rounded resize-none mb-4"
                        required
                    />
                </section>
                <button className="bg-indigo-900 hover:bg-indigo-800 text-white font-semibold px-6 py-2 rounded">
                    Crear
                </button>
            </form>
        </div>
    );
};

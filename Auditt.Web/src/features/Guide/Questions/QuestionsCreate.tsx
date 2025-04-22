import { useRef } from "react";
import { useQuestions } from "./useQuestions";
import { QuestionsModel } from "./QuestionsModel";

type formData = {
    text?: string;
    order: number;
    idGuide: number;
    idUser: number;
};

export const QuestionsCreate = ({ idGuide }: { idGuide: number }) => {
    const { createQuestion } = useQuestions();
    const refForm = useRef<HTMLFormElement>(null);

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        const form = e.target as HTMLFormElement;

        const formData = new FormData(form);
        const question = Object.fromEntries(formData.entries()) as unknown as formData;

        const newQuestion: QuestionsModel = {
            text: question.text ?? "",
            order: question.order ?? 0,
            idGuide: idGuide,
            idUser: question.idUser ?? 0,
        };

        const response = await createQuestion.mutateAsync(newQuestion);

        if (response.isSuccess) {
            refForm.current?.reset();
        }
    }

    return (
        <div className="w-full">
            <form ref={refForm} onSubmit={handleSubmit}>
                <label className="block text-2xl font-medium mb-2" htmlFor="pregunta">Pregunta</label>
                <section className="w-full">
                    <textarea
                        id="pregunta"
                        className="w-full h-60 p-3 border rounded resize-none mb-4"
                        name="text" // Asegúrate de poner el atributo `name` para poder acceder al campo
                    />
                </section>
                <button className="bg-indigo-900 hover:bg-indigo-800 text-white font-semibold px-6 py-2 rounded">
                    Crear
                </button>
            </form>
        </div>
    );
};

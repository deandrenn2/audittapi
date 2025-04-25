import { useRef, useState } from "react";
import { QuestionsModel } from "./QuestionsModel";
import { useQuestions } from "./useQuestions";

export const QuestionsUpdate = ({ data }: { data: QuestionsModel }) => {
    const idGuide = data.idGuide;
    const { updateQuestion } = useQuestions(idGuide);
    const [question, setQuestion] = useState<QuestionsModel>(data);
    const refForm = useRef<HTMLFormElement>(null);

    const handleChange = (e: React.ChangeEvent<HTMLTextAreaElement>) => {
        setQuestion({ ...question, text: e.target.value });
    };

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        const response = await updateQuestion.mutateAsync(question);

        if (response.isSuccess) {
            refForm.current?.reset();
        }
    };

    return (
        <div className="w-full">
            <form ref={refForm} onSubmit={handleSubmit}>
                <label className="block font-medium mb-2" htmlFor="text">Pregunta</label>
                <section className="w-full">
                    <textarea
                        id="text"
                        name="text"
                        value={question.text}
                        onChange={handleChange}
                        className="w-full h-60 border border-gray-300 rounded px-3 py-2 transition duration-200 hover:border-indigo-500
                         hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-400"
                        required
                    />
                </section>
                <button
                    type="submit"
                    className="mt-4 bg-indigo-900 hover:bg-indigo-800 text-white font-semibold px-6 py-2 rounded"
                >
                    Actualizar
                </button>
            </form>
        </div>
    );
};

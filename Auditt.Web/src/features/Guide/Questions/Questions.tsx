import { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import { QuestionsCreate } from "./QuestionsCreate";
import OffCanvas from "../../../shared/components/OffCanvas/Index";
import { Direction } from "../../../shared/components/OffCanvas/Models";
import { ButtonPlus } from "../../../shared/components/Buttons/ButtonMas";
import { useQuestions } from "./useQuestions";
import { useGuide } from "../useGuide";
import { GuideModel } from "../GuideModel";
export const Questions = () => {
    const { id } = useParams<{ id: string }>();
    const { questions } = useQuestions();
    const { guides, } = useGuide();
    const [selectedGuide, setSelectedGuide] = useState<GuideModel | undefined>(undefined);
    const [visible, setVisible] = useState(false);

    const handleClose = () => setVisible(false);
    const handleClick = () => setVisible(true);

    useEffect(() => {
        if (id && guides) {
            const guide = guides.find((g) => g.id === Number(id));
            if (guide) {
                setSelectedGuide(guide);
            }
        }
    }, [id, guides]);

    return (
        <div className="w-full">
            <div className="flex space-x-7 mb-4">
                <section className="w-full p-2 bg-white">
                    <div className="mb-4">
                        <label className="block text-sm font-semibold mb-2">Instrumento de Adherencia a GPC</label>
                        <div className="flex items-center gap-2">
                            <select
                                name="idGuide"
                                className="border rounded px-3 py-2"
                                required
                                value={selectedGuide?.id || ""}
                                onChange={(e) => setSelectedGuide(guides?.find(guide => guide.id === Number(e.target.value)))}
                            >
                                <option value="">Selecciona una guía</option>
                                {guides?.map((guide) => (
                                    <option key={guide.id} value={guide.id}>
                                        {guide.name}
                                    </option>
                                ))}
                            </select>
                            <button onClick={handleClick}>
                                <ButtonPlus />
                            </button>
                        </div>
                    </div>

                    {selectedGuide && (
                        <div className="mb-4">
                            <h3 className="text-lg font-semibold">Preguntas de la guía: {selectedGuide.name}</h3>
                        </div>
                    )}

                    {selectedGuide ? (
                        <div className="space-y-4">
                            <div className="bg-white px-2 py-2 border border-gray-200">
                                <div className="bg-green-100 text-sm text-gray-800 p-4 rounded">
                                    {questions
                                        ?.filter((question) => question.idGuide === selectedGuide.id)
                                        .map((question) => (
                                            <div key={question.id} className="bg-green-100 text-sm text-gray-800 p-4 rounded flex">
                                                {question.text}
                                            </div>
                                        ))}
                                </div>
                            </div>
                        </div>
                    ) : (
                        <p className="text-sm text-gray-500">Selecciona una guía para ver las preguntas.</p>
                    )}
                </section>
            </div>

            <OffCanvas titlePrincipal="Crear Pregunta" visible={visible} xClose={handleClose} position={Direction.Right}>
                    {selectedGuide && selectedGuide.id !== undefined && <QuestionsCreate idGuide={selectedGuide.id} />}
            </OffCanvas>
        </div>
    );
};

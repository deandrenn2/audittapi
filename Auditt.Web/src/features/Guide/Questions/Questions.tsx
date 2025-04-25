import { useEffect, useState } from "react";
import { QuestionsCreate } from "./QuestionsCreate";
import OffCanvas from "../../../shared/components/OffCanvas/Index";
import { Direction } from "../../../shared/components/OffCanvas/Models";
import { ButtonPlus } from "../../../shared/components/Buttons/ButtonMas";
import { useQuestions } from "./useQuestions";
import { useGuide } from "../useGuide";
import Swal from "sweetalert2";
import { ButtonDetail } from "../../../shared/components/Buttons/ButtonDetail";
import { QuestionsModel } from "./QuestionsModel";
import { QuestionsUpdate } from "./QuestiosUpdate";
import ButtonDelete from "../../../shared/components/Buttons/ButtonDelete";
export const Questions = () => {
    const [visible, setVisible] = useState(false);
    const [visibleUpdate, setVisibleUpdate] = useState(false);
    const [questionSelected, setQuestionSelected] = useState<QuestionsModel | null>(null);

    const { guides } = useGuide();
    const [selectedIdguide, setSelectedIdguide] = useState<number>(0);
    const { questions, deleteQuestion } = useQuestions(selectedIdguide);

    useEffect(() => {
        if (guides && guides.length > 0 && selectedIdguide === 0) {
            setSelectedIdguide(guides[0]?.id ?? 0);
        }
    }, [guides, selectedIdguide]);

    const selectedGuide = guides?.find((guide) => guide.id === selectedIdguide);

    const handleClose = () => {
        setVisible(false);
    };

    const handleClick = () => {
        setVisible(true);
    };

    const handleClickDetail = (question: QuestionsModel) => {
        setQuestionSelected(question);
        setVisibleUpdate(true);
    };

    const handleDelete = (e: React.MouseEvent<HTMLButtonElement>, id: number): void => {
        e.preventDefault();
        Swal.fire({
            title: '¿Estás seguro de eliminar esta pregunta?',
            text: 'Esta acción no se puede deshacer',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'Confirmar',
            cancelButtonText: 'Cancelar',
            preConfirm: async () => {
                await deleteQuestion.mutateAsync(id);
            }
        });
    };

    return (
        <div className="w-full">
            <div className="flex space-x-2">
                <section className=" p-2 bg-white">
                    <div className="mb-4">
                        <label className="block text-sm font-semibold mb-2">Instrumento de Adherencia a GPC</label>
                        <div className="flex items-center gap-1">
                            <select
                                name="idGuide"
                                className="border rounded px-3 py-2"
                                required
                                value={selectedIdguide}
                                onChange={(e) => setSelectedIdguide(Number(e.target.value))}>
                                {guides?.map((guide) => (
                                    <option key={guide.id} value={guide.id}>
                                        {guide.name}
                                    </option>
                                ))}
                            </select>
                            <div onClick={handleClick}>
                                <ButtonPlus />
                            </div>
                        </div>
                    </div>

                    {selectedGuide && (
                        <div className="mb-4">
                            <h3 className="text-lg font-semibold">Preguntas de la guía: {selectedGuide.name}</h3>
                        </div>
                    )}

                    {questions
                        ?.filter((question) => question.idGuide === selectedIdguide)
                        .map((question) => (
                            <div key={question.id} className="flex items-start space-x-1 mb-4">
                                <div className="flex-1 bg-green-100 text-gray-900 p-2 rounded break-words whitespace-pre-wrap overflow-hidden max-h-60">
                                    {question.text}
                                </div>
                                <div className="flex items-center space-x-1">
                                    <ButtonDelete id={question.id ?? 0} onDelete={handleDelete} />
                                    <div onClick={() => handleClickDetail(question)}>
                                        <ButtonDetail />
                                    </div>
                                </div>
                            </div>
                        ))}
                </section>
            </div>
            <OffCanvas titlePrincipal="Crear Pregunta" visible={visible} xClose={handleClose} position={Direction.Right}>
                <QuestionsCreate idGuide={selectedIdguide} />
            </OffCanvas>
            {questionSelected && (
                <OffCanvas
                    titlePrincipal="Actualizar Pregunta"
                    visible={visibleUpdate}
                    xClose={() => setVisibleUpdate(false)}
                    position={Direction.Right}>
                    <QuestionsUpdate data={questionSelected} />
                </OffCanvas>
            )}
        </div>
    );
};

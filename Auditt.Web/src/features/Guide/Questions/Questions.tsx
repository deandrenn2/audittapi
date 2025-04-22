import { useState } from "react";
import { QuestionsCreate } from "./QuestionsCreate";
import OffCanvas from "../../../shared/components/OffCanvas/Index";
import { Direction } from "../../../shared/components/OffCanvas/Models";
import { ButtonPlus } from "../../../shared/components/Buttons/ButtonMas";
import { useQuestions } from "./useQuestions";


export const Questions = () => {
    const { questions,  } = useQuestions();
    const [visible, setVisible] = useState(false);

    const handleClose = () => {
        setVisible(false);
    }

    const handleClick = () => {
        setVisible(true);
    }

    return (
        <div className="w-full">
            <div className="flex space-x-7 mb-4">
                <section className="w-full p-2 bg-white">
                    <div className="mb-4">
                        <label className="block text-sm font-semibold mb-2">Instrumento de Adherencia a GPC</label>
                        <div className="flex items-center gap-2">
                            <select className="border rounded px-3 py-2 ">
                                <option>Instrumento HTA</option>
                            </select>
                            <button onClick={handleClick}>
                                <ButtonPlus />
                            </button>

                        </div>
                    </div>

                    <div className="space-y-4">
                        <div className="bg-white px-2 py-2 border border-gray-200">
                            <div className="bg-green-100 text-sm text-gray-800 p-4 rounded">
                                {questions?.map((question) => (
                                    <div>
                                        <div className="bg-green-100 text-sm text-gray-800 p-4 rounded flex">
                                            {question.text}
                                        </div>
                                        
                                    </div>
                                ))
                                }
                            </div>
                        </div>

                        <div className="bg-green-100 text-sm text-gray-800 p-4 rounded flex">
                            En los primeros tres meses después del diagnóstico de HTA, debe realizarse una muestra de orina casual, evaluando la relación proteinuria/creatinina con tiras reactivas. (FUERTE A FAVOR = efectos deseables)
                        </div>
                    </div>
                </section>
            </div>
            <OffCanvas titlePrincipal='Crear de Pregunta' visible={visible} xClose={handleClose} position={Direction.Right}>
                <QuestionsCreate idGuide={0} />
            </OffCanvas>
        </div>
    )
}
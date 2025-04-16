import { useState } from "react";
import OffCanvas from "../../shared/components/OffCanvas/Index"
import { Direction } from "../../shared/components/OffCanvas/Models";
import { QuestionsForm } from "./QuestionsForm";

export const InstrumentsForm = () => {
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
                            <button onClick={handleClick} className="bg-green-600 hover:bg-green-700 text-white rounded-full w-8 h-8 flex items-center justify-center text-xl">+</button>
                        </div>
                    </div>

                    <div className="space-y-4">
                        <div className="bg-green-100 text-sm text-gray-800 p-4 rounded">
                            En cuidado primario y rutinario de pacientes con HTA estadio 1, realizar fundoscopia para valoración de daño micro vascular. (FUERTE A FAVOR = efectos deseables)
                        </div>
                        <div className="bg-green-100 text-sm text-gray-800 p-4 rounded flex">
                            En los primeros tres meses después del diagnóstico de HTA, debe realizarse una muestra de orina casual, evaluando la relación proteinuria/creatinina con tiras reactivas. (FUERTE A FAVOR = efectos deseables)
                        </div>
                    </div>
                </section>
            </div>
            <OffCanvas titlePrincipal='Creación de Pregunta' visible={visible} xClose={handleClose} position={Direction.Right}>
                <QuestionsForm />
            </OffCanvas>
        </div>
    )
}
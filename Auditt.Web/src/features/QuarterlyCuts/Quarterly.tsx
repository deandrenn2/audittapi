import { useState } from "react";
import { Direction } from "../../shared/components/OffCanvas/Models";
import OffCanvas from "../../shared/components/OffCanvas/Index";
import { QuarterlyCreateForm, } from "./QuarterlCreateForm";
import { ButtonDetail } from "../../shared/components/Buttons/ButtonDetail";
export const Quarterly = () => {
    const [visible, setVisible] = useState(false);

    const hadbleClick = () => {
        setVisible(true);
    }


    const handleClose = () => {
        setVisible(false);
    }

    return (
        <div className="flex-1 p-8 ">
            <div className="">
                <div className="flex items-center space-x-4 mb-4">
                    <span className="font-medium">IPS</span>
                    <select className="border rounded px-3 py-1">
                        <option>HOSPITAL SAN SEBASTIAN</option>
                    </select>
                </div>

            </div>
            <h1 className="text-2xl font-semibold mb-4">Cortes trimestrales de auditoria</h1>
            <button onClick={hadbleClick} className="bg-indigo-500 hover:bg-indigo-900 text-white px-6 py-2 rounded-lg font-semibold mb-2">
                Crear
            </button>

            <div>
                <div className="grid grid-cols-[2fr_1fr_1fr_1fr_2fr] w-full min-w-[700px]">
                    <div className="font-semibold bg-gray-300  text-gray-800 px-2 py-1">NOMBRE</div>
                    <div className="font-semibold bg-gray-300  text-gray-800 px-2 py-1">MAX HISTORIAS</div>
                    <div className="font-semibold bg-gray-300  text-gray-800 px-2 py-1">FECHA INICIAL</div>
                    <div className="font-semibold bg-gray-300  text-gray-800 px-2 py-1">FECHA FINAL</div>
                    <div className="font-semibold bg-gray-300  text-gray-800 px-2 py-1 text-center">OPCIONES</div>
                </div>
                <div className="bg-white px-2 py-2 border border-gray-200">
                    <div className="grid grid-cols-[2fr_1fr_1fr_1fr_2fr] w-full min-w-[700px]">
                        <div className="text-sm bg-white px-2 py-2 border border-gray-300">1er Trimestre 2025</div>
                        <div className="text-sm bg-white px-2 py-2 border border-gray-300">220</div>
                        <div className="text-sm bg-white px-2 py-2 border border-gray-300">01-01-2025</div>
                        <div className="text-sm bg-white px-2 py-2 border border-gray-300">31-03-2025</div>
                        <div className=" flex justify-center">
                            <div>
                                <ButtonDetail />
                            </div>

                        </div>
                    </div>
                </div>
                <OffCanvas titlePrincipal='Crear Cortes Trimestrales' visible={visible} xClose={handleClose} position={Direction.Right}  >
                    <QuarterlyCreateForm />
                </OffCanvas>
            </div>
        </div>

    );
}
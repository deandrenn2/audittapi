import { useState } from "react";
import OffCanvas from "../../../shared/components/OffCanvas/Index";
import { Direction } from "../../../shared/components/OffCanvas/Models";
import { ProfessionalsCreate } from "./ProfessionalsCreate";

import { LinkClients } from "../../Dashboard/LinkClients";

export const Professionals = () => {
    const [visible, setVisible] = useState(false);


    const handleClose = () => {
        setVisible(false);
    }
    const handleClick = () => {
        setVisible(true);
    }

    return (

        <div className="flex">
            <div className="flex space-x-4 mb-4">
                <div className="flex-1 p-8">
                    <div className="flex space-x-8 text-lg font-medium mb-6 mr-2">
                        <LinkClients/>
                    </div>
                    <h2 className="text-2xl font-semibold mb-4">Profesionales </h2>

                    <button onClick={handleClick} className="bg-indigo-500 hover:bg-indigo-900 text-white px-6 py-2 rounded-lg font-semibold mb-2">
                        Crear Profesionales
                    </button>

                    <div>
                        <div className="grid grid-cols-4">
                            <div className="gap-3 font-semibold bg-gray-300  text-gray-800 px-2 py-1 rounded mr-2 mb-2">IdPaciente</div>
                            <div className="gap-3 font-semibold bg-gray-300  text-gray-800 px-2 py-1 rounded mr-2 mb-2 ">Fecha Nacimiento</div>
                            <div className="gap-3 font-semibold bg-gray-300  text-gray-800 px-2 py-1 rounded mr-2 mb-2 ">Eps</div>
                            <div className="gap-3 font-semibold bg-gray-300  text-gray-800 px-2 py-1 rounded mr-2 mb-2 ">Opciones</div>
                        </div>

                        <div className="grid grid-cols-4">
                            <div className="grid grid-cols-3 gap-3 text-sm bg-white px-2 py-2 border border-gray-300 mr-2 mb-2">1039094744</div>
                            <div className="grid grid-cols-3 gap-3 text-sm bg-white px-2 py-2 border border-gray-300 mr-2 mb-2">02-02-1990</div>
                            <div className="grid grid-cols-3 gap-3 text-sm bg-white px-2 py-2 border border-gray-300 mr-2 mb-2">Nueva EPS</div>
                            <div className="grid grid-cols-3 gap-3 text-sm bg-white px-2 py-2 border border-gray-300 mr-2 mb-2"></div>
                        </div>
                    </div>
                </div>
            </div>
            <OffCanvas titlePrincipal='Registro de Profesionales' visible={visible} xClose={handleClose} position={Direction.Right}  >
                <ProfessionalsCreate />
            </OffCanvas>
        </div>

    );
}
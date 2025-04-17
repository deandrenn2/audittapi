import { useState } from "react";
import OffCanvas from "../../../../shared/components/OffCanvas/Index";
import { Direction } from "../../../../shared/components/OffCanvas/Models";
import { ProfessionalsCreate } from "./ProfessionalsCreate";

import { LinkClients } from "../../../Dashboard/LinkClients";
import ButtonDetail from "../../../../shared/components/Buttons/ButtonDetail";
import ButtonDelete from "../../../../shared/components/Buttons/ButtonDelete";

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
                <div className=" p-8">
                    <div className="flex space-x-8 text-lg font-medium mb-6 mr-2">
                        <LinkClients/>
                    </div>
                    <h2 className="text-2xl font-semibold mb-4">Profesionales </h2>
                    <button onClick={handleClick} className="bg-indigo-500 hover:bg-indigo-900 text-white px-6 py-2 rounded-lg font-semibold mb-2">
                        Crear Profesionales
                    </button>

                    <div>
                        <div className="grid grid-cols-4">
                            <div className="gap-3 font-semibold bg-gray-300  text-gray-800 px-2 py-1 ">Nombre</div>
                            <div className="gap-3 font-semibold bg-gray-300  text-gray-800 px-2 py-1 ">Apellido</div>
                            <div className="gap-3 font-semibold bg-gray-300  text-gray-800 px-2 py-1 ">Identificación</div>
                            <div className="gap-3 font-semibold bg-gray-300  text-gray-800 px-2 py-1 text-center ">Opciones</div>
                        </div>

                        <div className="bg-white px-2 py-2 border border-gray-200">
                        <div className="grid grid-cols-4">
                            <div className="grid grid-cols-3 gap-3 text-sm bg-white px-2 py-2 border border-gray-300 mr-2 mb-2">Virtor</div>
                            <div className="grid grid-cols-3 gap-3 text-sm bg-white px-2 py-2 border border-gray-300 mr-2 mb-2">Riva</div>
                            <div className="grid grid-cols-3 gap-3 text-sm bg-white px-2 py-2 border border-gray-300 mr-2 mb-2">1000711758</div>
                            <div className="flex justify-center">
                                <ButtonDelete id={0} onDelete={undefined}/>
                                <ButtonDetail url={""}/>
                            </div>
                        </div>
                        </div>
                        
                    </div>
                </div>    
            <OffCanvas titlePrincipal='Crear Profesionales' visible={visible} xClose={handleClose} position={Direction.Right}  >
                <ProfessionalsCreate />
            </OffCanvas>
        </div>

    );
}
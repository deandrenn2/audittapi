import { useState } from "react";
import OffCanvas from "../../shared/components/OffCanvas/Index";
import { Direction } from "../../shared/components/OffCanvas/Models";
import { ClientsCreate } from "./ClientsCreate";
import ButtonDetail from "../../shared/components/Buttons/ButtonDetail";
import { LinkClients } from "../Dashboard/LinkClients";
import ButtonDelete from "../../shared/components/Buttons/ButtonDelete";
export const Clients = () => {
    const [visible, setVisible] = useState(false);

    const handleClose = () => {
        setVisible(false);

    }

    const handleClick = () => {
        setVisible(true);
    }

    return (
        <div className="flex p-8">
            <div>
                <div className="flex space-x-8 text-lg font-medium mb-6 mr-2">
                    <LinkClients />
                </div>

                <h2 className="text-2xl font-semibold mb-4">Clientes o Instituciones</h2>

                <button onClick={handleClick} className="bg-indigo-500 hover:bg-indigo-900 text-white px-6 py-2 rounded-lg font-semibold mb-2">
                    Crear Clientes
                </button>
                <div>
                    <div className="grid grid-cols-5">
                        <div className=" font-semibold bg-gray-300  text-gray-800 px-2 py-1">Razón Social</div>
                        <div className=" font-semibold bg-gray-300  text-gray-800 px-2 py-1 ">Abreviatura</div>
                        <div className=" font-semibold bg-gray-300  text-gray-800 px-2 py-1">Nit</div>
                        <div className=" font-semibold bg-gray-300  text-gray-800 px-2 py-1">Ciudad</div>
                        <div className=" font-semibold bg-gray-300  text-gray-800 px-2 py-1 text-center">Opciones</div>
                    </div>

                    <div className=" bg-white px-2 py-2 border border-gray-200">
                        <div className="grid grid-cols-5">
                            <div className=" gap-3 text-sm bg-white px-2 py-2 border border-gray-300 mr-2">Hospital San Sebastián de Urabá</div>
                            <div className=" gap-3 text-sm bg-white px-2 py-2 border border-gray-300 mr-2">ESE HOSPITAL</div>
                            <div className=" gap-3 text-sm bg-white px-2 py-2 border border-gray-300 mr-2">232343434</div>
                            <div className=" gap-3 text-sm bg-white px-2 py-2 border border-gray-300 mr-2">Necoclí</div>
                            <div className=" flex justify-center">
                            <ButtonDelete id={0} onDelete={undefined}/>    
                            <ButtonDetail url={""}/>
                            </div>
                        </div>
                    </div>

                </div>
                <OffCanvas titlePrincipal='Crear Cliente' visible={visible} xClose={handleClose} position={Direction.Right}  >
                    <ClientsCreate />
                </OffCanvas>
            </div>
        </div>
    );
}
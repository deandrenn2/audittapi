import { useState } from "react";
import OffCanvas from "../../../shared/components/OffCanvas/Index";
import ButtonDetail from "../../../shared/components/Buttons/ButtonDetail";

import { LinkClients } from "../../Dashboard/LinkClients";
import { Direction } from "../../../shared/components/OffCanvas/Models";
import { FunctionaryCreate } from "./FunctionaryCreate";
import { FunctionaryModel } from "./FuntionaryModel";
import { useFunctionary } from "./UseFuntionary";
import { Bar } from "../../../shared/components/Progress/Bar";
import Swal from "sweetalert2";
import ButtonDelete from "../../../shared/components/Buttons/ButtonDelete";
import { FunctionaryUpdate } from "./FunctionaryUpdate";
export const Functionary = () => {
    const [visible, setVisible] = useState(false);
    const [visibleUpdate, setVisibleUpdate] = useState(false);
    const {Functionary , queryFunctionary, deleteFunctionary} = useFunctionary();
    const [functionary, setFunctionary] = useState<FunctionaryModel>();

    const handleClickDetail = (functionarySelected: FunctionaryModel) => {
        if (functionarySelected) {
            setFunctionary(functionarySelected);
            setVisibleUpdate(true);
        }
    }
    const handleDelete = (e: React.MouseEvent<HTMLButtonElement>, id: number): void => {
        e.preventDefault();
        Swal.fire({
            title: '¿Estás seguro de eliminar este cliente?',
            text: 'Esta acción no se puede deshacer',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'Confirmar',
            cancelButtonText: 'Cancelar',
            preConfirm: async () => {
                await deleteFunctionary.mutateAsync(id);
            }
    })
}   

if (queryFunctionary.isLoading)
    return <Bar />

    const handleClose = () => {
        setVisible(false);
    }
    
    return (
        <div className="flex">
                <div className=" p-8">
                    <div className="flex space-x-8 text-lg font-medium mb-6 mr-2">
                        <LinkClients/>
                    </div>
                    <h2 className="text-2xl font-semibold mb-4">Profesionales </h2>
                    <button  onClick={() => setVisible(true)} className="bg-indigo-500 hover:bg-indigo-900 text-white px-6 py-2 rounded-lg font-semibold mb-2">
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
                            {Functionary?.map((functionary) => (
                                <div className="grid grid-cols-4">
                                    <div className="gap-3 text-sm bg-white px-2 py-2 border border-gray-300 mr-2 mb-2">{functionary?.firstName}</div>
                                    <div className="gap-3 text-sm bg-white px-2 py-2 border border-gray-300 mr-2 mb-2">{functionary?.lastName}</div>
                                    <div className="gap-3 text-sm bg-white px-2 py-2 border border-gray-300 mr-2 mb-2">{functionary?.identification}</div>
                                    <div className="flex justify-center">
                                        <ButtonDelete id={functionary.id ?? 0} onDelete={handleDelete} />
                                        <ButtonDetail url={""} xClick={() => handleClickDetail(functionary)}/>
                                    </div>
                                </div>
                            ))}
                        </div>
                        
                    </div>
                </div>    
            <OffCanvas titlePrincipal='Crear Profesionales' visible={visible} xClose={handleClose} position={Direction.Right}  >
                <FunctionaryCreate />
            </OffCanvas>
            {
                functionary &&
                <OffCanvas titlePrincipal='Detalle Profesionales' visible={visibleUpdate} xClose={() => setVisibleUpdate(false)} position={Direction.Right}  >
                    <FunctionaryUpdate data={functionary} />
                </OffCanvas>
            }
        </div>

    );
}
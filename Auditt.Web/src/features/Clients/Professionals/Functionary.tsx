import { useState } from "react";
import OffCanvas from "../../../shared/components/OffCanvas/Index";
import { LinkClients } from "../../Dashboard/LinkClients";
import { Direction } from "../../../shared/components/OffCanvas/Models";
import { FunctionaryCreate } from "./FunctionaryCreate";
import { FunctionaryModel } from "./FuntionaryModel";
import { useFunctionary } from "./UseFuntionary";
import { Bar } from "../../../shared/components/Progress/Bar";
import Swal from "sweetalert2";
import ButtonDelete from "../../../shared/components/Buttons/ButtonDelete";
import { FunctionaryUpdate } from "./FunctionaryUpdate";
import { ButtonUpdate } from "../../../shared/components/Buttons/ButtonDetail";
import { faMagnifyingGlass, } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
export const Functionary = () => {
    const [visible, setVisible] = useState(false);
    const [visibleUpdate, setVisibleUpdate] = useState(false);
    const { functionarys, queryFunctionary, deleteFunctionary } = useFunctionary();
    const [functionary, setFunctionary] = useState<FunctionaryModel>();
    const [searFunctionarys, setSearFunctionarys] = useState('');

    const handleClickDetail = (functionarySelected: FunctionaryModel) => {
        if (functionarySelected) {
            setFunctionary(functionarySelected);
            setVisibleUpdate(true);
        }
    }
    const handleDelete = (e: React.MouseEvent<HTMLButtonElement>, id: number): void => {
        e.preventDefault();
        Swal.fire({
            title: '¿Estás seguro de eliminar este Profesional?',
            text: 'Esta acción no se puede deshacer',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'Confirmar',
            cancelButtonText: 'Cancelar',
            preConfirm: async () => {
                await deleteFunctionary.mutateAsync(id);
            }
        });
    };

    const handleClose = () => {
        setVisible(false);
    }
    
    if (queryFunctionary.isLoading)
        return <Bar />

    const normalizeText = (text: string) =>
        text.normalize("NFD").replace(/[\u0300-\u036f]/g, "").toLowerCase();

    const search = normalizeText(searFunctionarys.trim());

    const filteredFunctionnarys = (functionarys ?? []).filter(functionary =>{
      const fields =  `${functionary.lastName} ${functionary.firstName}
        ${functionary.identification}`;
        const words = normalizeText(fields).split(/\s+/);
        return words.some(word => word.startsWith(search));
});

    return (
        <div className="w-full p-6">
            <div>
                <div className="flex space-x-8 text-lg font-medium mb-6 mr-2">
                    <LinkClients />
                </div>

                <div className="flex">
                    <h2 className="text-2xl font-semibold mb-3 mr-2">Profesionales </h2>
                    <div className="relative mb-2 mr-2">
                        <div className=" inline-flex">
                            <input type="text"
                                value={searFunctionarys}
                                onChange={(e) => setSearFunctionarys(e.target.value)}
                                placeholder="Buscar Profecional"
                                className="border rounded px-3 py-1 transition duration-200 border-gray-300 hover:border-indigo-500 
                                 hover:bg-gray-50 focus:outline-none focus:ring-2 text-center focus:ring-indigo-400"/>
                            <FontAwesomeIcon icon={faMagnifyingGlass} className="fas fa-search absolute left-3 top-3 text-gray-400"/>
                        </div>
                    </div>
                    <button onClick={() => setVisible(true)} className="cursor-pointer bg-[#392F5A] hover:bg-indigo-900 text-white px-5 rounded-lg font-semibold mb-3 mr-2">
                        Crear Profesional
                    </button>
                </div>

                <div>
                    <div className="grid grid-cols-4 w-full">
                        <div className="font-semibold bg-gray-300 text-gray-800 px-2 py-1 text-center">NOMBRE</div>
                        <div className="font-semibold bg-gray-300 text-gray-800 px-2 py-1 text-center">APELLIDO</div>
                        <div className="font-semibold bg-gray-300 text-gray-800 px-2 py-1 text-center">IDENTIFICACION</div>
                        <div className="font-semibold bg-gray-300 text-gray-800 px-2 py-1 text-center">OPCIONES</div>
                    </div>

                    <div className="bg-white px-2 py-2 border border-gray-200">
                        {filteredFunctionnarys?.map((functionary) => (
                            <div className="grid grid-cols-4 w-full hover:bg-[#F4EDEE] transition-colors">
                                <div className="text-sm px-2 py-2 border border-gray-300 text-center">{functionary.firstName}</div>
                                <div className="text-sm px-2 py-2 border border-gray-300 text-center">{functionary.lastName}</div>
                                <div className="text-sm px-2 py-2 border border-gray-300 text-center">{functionary.identification}</div>
                                <div className="flex justify-center text-sm px-2 b text-center border border-gray-300 py-1">
                                    <div onClick={() => handleClickDetail(functionary)}>
                                        <ButtonUpdate />
                                    </div>
                                    <ButtonDelete id={functionary.id ?? 0} onDelete={handleDelete} />
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
                <OffCanvas titlePrincipal='Actualizar Profesional' visible={visibleUpdate} xClose={() => setVisibleUpdate(false)} position={Direction.Right}  >
                    <FunctionaryUpdate data={functionary} />
                </OffCanvas>
            }
        </div>
    );
}
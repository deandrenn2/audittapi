import { useState } from "react";
import ButtonDelete from "../../../shared/components/Buttons/ButtonDelete";
import { LinkClients } from "../../Dashboard/LinkClients";
import OffCanvas from "../../../shared/components/OffCanvas/Index";
import { Direction } from "../../../shared/components/OffCanvas/Models";
import { PatientsCreate } from "./PatientsCreate";
import Swal from "sweetalert2";
import { usePatients } from "./UsePatients";
import { PatientsModel } from "./PantientsModel";
import { PatientsUpdate } from "./PatientsUpdate";
import { Bar } from "../../../shared/components/Progress/Bar";
import { ButtonUpdate } from "../../../shared/components/Buttons/ButtonDetail";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faMagnifyingGlass } from "@fortawesome/free-solid-svg-icons";
export const Patients = () => {
    const [visible, setVisible] = useState(false);
    const [visibleUpdate, setVisibleUpdate] = useState(false);
    const { patients, queryPatients, deletePatients, } = usePatients();
    const [patient, setPatient] = useState<PatientsModel>();
    const [searPantients, setSearPantients] = useState('');

    const handleClickDetail = (patientSelected: PatientsModel) => {
        if (patientSelected) {
            setPatient(patientSelected);
            setVisibleUpdate(true);
        }
    }

    const handleDelete = (e: React.MouseEvent<HTMLButtonElement>, id: number): void => {
        e.preventDefault();
        Swal.fire({
            title: '¿Estás seguro de eliminar este Paciente?',
            text: 'Esta acción no se puede deshacer',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'Confirmar',
            cancelButtonText: 'Cancelar',
            preConfirm: async () => {

                await deletePatients.mutateAsync(id);
                queryPatients.refetch();
            }
        });
    };

    const handleClose = () => {
        setVisible(false);
    }

    if (queryPatients.isLoading)
        return <Bar/>

    const filteredPatients = patients?.filter(patient =>
        `${patient.identification}  ${patient.birthDate} ${patient.eps}`.toLowerCase().includes(searPantients.toLowerCase())
    )

    return (
        <div className="w-full p-6">
            <div>
                <div className="flex-1">
                    <div className="flex space-x-8 text-lg font-medium mb-4 mr-2">
                        <LinkClients/>
                    </div>
                    <div className="flex justify-between">
                            <h2 className="text-2xl font-semibold mb-3 mr-2">Pacientes o historias</h2>
                        <div className="flex">
                            <div className="relative mr-4">
                                <div className=" inline-flex">
                                    <input type="text"
                                        value={searPantients}
                                        onChange={(e) => setSearPantients(e.target.value)}
                                        placeholder="Buscar Paciente"
                                        className="border rounded bg-white px-3 py-1 transition duration-200 border-gray-300 hover:border-indigo-500 
                                 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-400"/>
                                    <FontAwesomeIcon icon={faMagnifyingGlass} className="fas fa-search absolute right-2 top-2 text-gray-400"/>
                                </div>
                            </div>
                            <button onClick={() => setVisible(true)} className="cursor-pointer mr-2 bg-[#392F5A] hover:bg-indigo-900 text-white px-5 rounded-lg  font-semibold mb-3">
                                Crear Paciente
                            </button>
                        </div>
                    </div>

                    <div>
                        <div className="grid grid-cols-4">
                            <div className=" font-semibold bg-gray-300  text-gray-800 px-2 py-1 text-center">ID Paciente</div>
                            <div className=" font-semibold bg-gray-300  text-gray-800 px-2 py-1 text-center">Fecha de Nacimiento</div>
                            <div className=" font-semibold bg-gray-300  text-gray-800 px-2 py-1 text-center ">Eps</div>
                            <div className=" font-semibold bg-gray-300  text-gray-800 px-2 py-1 text-center">Opciones
                            </div>
                        </div>
                        <div className="bg-white px-2 py-2 border border-gray-200">
                            {filteredPatients?.map((patient) => (
                                <div className="grid grid-cols-4 hover:bg-[#F4EDEE] transition-colors">
                                    <div className="text-sm px-2 py-2 border border-gray-300 text-center">{patient.identification}</div>
                                    <div className="text-sm px-2 py-2 border border-gray-300 text-center">{patient.birthDate}</div>
                                    <div className="text-sm px-2 py-2 border border-gray-300 text-center">{patient.eps}</div>
                                    <div className="flex justify-center text-sm px-2 border border-gray-300 py-1">
                                        <div onClick={() => handleClickDetail(patient)}  >
                                            <ButtonUpdate />
                                        </div>
                                        <ButtonDelete id={patient.id ?? 0} onDelete={handleDelete} />
                                    </div>
                                </div>
                            ))}
                        </div>

                    </div>
                </div>
            </div>
            <OffCanvas titlePrincipal='Crear Paciente' visible={visible} xClose={handleClose} position={Direction.Right}  >
                <PatientsCreate />
            </OffCanvas>{
                patient &&
                <OffCanvas titlePrincipal='Actualizar Paciente' visible={visibleUpdate} xClose={() => setVisibleUpdate(false)} position={Direction.Right}  >
                    <PatientsUpdate data={patient} />
                </OffCanvas>
            }
        </div>

    );
}

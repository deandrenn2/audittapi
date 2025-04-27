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
export const Patients = () => {
    const [visible, setVisible] = useState(false);
    const [visibleUpdate, setVisibleUpdate] = useState(false);
    const { patients, queryPatients, deletePatients, } = usePatients();
    const [patient, setPatient] = useState<PatientsModel>();

    const handleClickDetail = (patientSelected: PatientsModel) => {
        if (patientSelected) {
            setPatient(patientSelected);
            setVisibleUpdate(true);
        }
    }

    const handleDelete = (e: React.MouseEvent<HTMLButtonElement>, id: number): void => {
        e.preventDefault();
        Swal.fire({
            title: '¿Estás seguro de eliminar este paciente?',
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
        return <Bar />

    return (
        <div className="flex w-full">
            <div>
                <div className="flex-1 p-8">
                    <div className="flex space-x-8 text-lg font-medium mb-6 mr-2">
                        <LinkClients />
                    </div>
                    <h2 className="text-2xl font-semibold mb-4">Pacientes o historias </h2>

                    <button onClick={() => setVisible(true)} className="bg-[#392F5A] hover:bg-indigo-900 text-white px-6 py-2 rounded-lg  font-semibold mb-2">
                        Crear Paciente
                    </button>
                    <div>

                        <div className="grid grid-cols-[4fr_4fr_3fr_1fr] w-full">
                            <div className=" font-semibold bg-gray-300  text-gray-800 px-2 py-1 text-center">ID PACIENTE </div>
                            <div className=" font-semibold bg-gray-300  text-gray-800 px-2 py-1 text-center">FECHA DE NACIMIENTO</div>
                            <div className=" font-semibold bg-gray-300  text-gray-800 px-2 py-1 text-center ">EPS</div>
                            <div className=" font-semibold bg-gray-300  text-gray-800 px-2 py-1 text-center">OPCIONES</div>
                        </div>

                        <div className="bg-white px-2 py-2 border border-gray-200">
                            {patients?.map((patient) => (
                                <div className="grid grid-cols-[4fr_4fr_3fr_1fr] w-full hover:bg-[#F4EDEE] transition-colors">
                                    <div className=" gap-3 text-sm px-2 py-2 border border-gray-300">{patient.identification}</div>
                                    <div className=" gap-3 text-sm px-2 py-2 border border-gray-300">{patient.birthDate}</div>
                                    <div className=" gap-3 text-sm px-2 py-2 border border-gray-300">{patient.eps}</div>

                                    <div className="flex text-sm px-2 border border-gray-300">
                                        <ButtonDelete id={patient.id ?? 0} onDelete={handleDelete} />
                                       <div onClick={() => handleClickDetail(patient)}  >
                                       <ButtonUpdate />
                                       </div>
                                        
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
                <OffCanvas titlePrincipal='Detalle Paciente' visible={visibleUpdate} xClose={() => setVisibleUpdate(false)} position={Direction.Right}  >
                    <PatientsUpdate data={patient} />
                </OffCanvas>
            }
        </div>

    );
}

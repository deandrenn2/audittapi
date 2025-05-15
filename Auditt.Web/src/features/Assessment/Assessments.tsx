import React, { useState } from "react";
import { ClientSelect } from "../Clients/ClientSelect";
import { Option } from "../../shared/model";
import { SingleValue } from "react-select";
import { Link } from "react-router-dom";
import { useAssessments } from "./useAssessment";
import { Bar } from "../../shared/components/Progress/Bar";
import useUserContext from "../../shared/context/useUserContext";
import Swal from "sweetalert2";
import { DataCutSelect } from "../DataCuts/DataCutsSelect";
import { GuideSelect } from "../Guide/GuideSelect";
import useAssessmentContext from "../../shared/context/useAssessmentContext";

export const Assessments = () => {
    const { queryAssessments, assessments, deleteAssessment } = useAssessments();
    const { setSelectedDataCut: setDataCut, setSelectedGuide: setGuide } = useAssessmentContext();

    const { client } = useUserContext();
    const [selectedClient, setSelectedClient] = useState<Option | undefined>(() => ({
        value: client?.id?.toString(),
        label: client?.name,
    }));


    const handleChangeClient = (newValue: SingleValue<Option>) => {
        setSelectedClient({
            value: newValue?.value,
            label: newValue?.label,
        });
    }

    const [selectedDataCut, setSelectedDataCut] = useState<Option | undefined>(() => ({
        value: "0",
        label: "Seleccione un corte",
    }));



    const [selectedGuide, setSelectedGuide] = useState<Option | undefined>(() => ({
        value: "0",
        label: "Seleccione una guía",
    }));

    const handleChangeDataCut = (newValue: SingleValue<Option>) => {
        setSelectedDataCut({
            value: newValue?.value,
            label: newValue?.label,
        });
        setDataCut(Number(newValue?.value));
    }

    const handleChangeGuide = (newValue: SingleValue<Option>) => {
        setSelectedGuide({
            value: newValue?.value,
            label: newValue?.label,
        });
        setGuide(Number(newValue?.value));
    }

    function handleDelete(e: React.MouseEvent<HTMLButtonElement, MouseEvent>, id: number): void {
        e.preventDefault();
        Swal.fire({
            title: '¿Estás seguro que deseas eliminar esta evaluación?',
            text: 'Esta acción no se puede deshacer',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'Confirmar',
            cancelButtonText: 'Cancelar',
            preConfirm: async () => {
                await deleteAssessment.mutate(id);
            }
        })
    }

    if (queryAssessments.isLoading)
        return <Bar />;



    return (
        <>
            <div className="flex gap-4 p-4 justify-between">
                <div className="flex items-center gap-4">
                    <span className="font-medium">IPS</span>
                    <ClientSelect className="w-lg" selectedValue={selectedClient} xChange={handleChangeClient} isSearchable={true} />
                </div>
                <div className="flex items-center">
                    <Link to={'/Reports'} className="bg-[#392F5A] hover:bg-indigo-900 text-white px-6 py-2 rounded-lg font-semibold mb-2" >
                        Ir a Indicadores e informes</Link>
                </div>
            </div>
            <div className="px-4 flex gap-4">
                <h1 className="text-2xl font-semibold">Evaluaciones o auditorias</h1>
                <Link to={'/Assessments/Create'} title='Crear' className="bg-[#392F5A] hover:bg-indigo-900 text-white px-6 py-2 rounded-lg font-semibold ">
                    Realizar valoraciones
                </Link>
            </div>
            <div className="flex-1 p-4">
                <div className=" flex justify-center gap-4 p-2">
                    <div className="flex flex-col ">
                        <span className="font-medium">Corte de Auditoria</span>
                        <DataCutSelect className="w-full min-w-60" selectedValue={selectedDataCut} xChange={handleChangeDataCut} isSearchable={true} />
                    </div>

                    <div className="flex flex-col ">
                        <span className="font-medium">Instrumento de adherencia a GPC</span>
                        <GuideSelect className="w-full" selectedValue={selectedGuide} xChange={handleChangeGuide} isSearchable={true} />
                    </div>

                </div>
                <div className="grid grid-cols-4">
                    <div className="font-semibold bg-gray-300  text-gray-800 px-2 py-1">Historia</div>
                    <div className="font-semibold bg-gray-300  text-gray-800 px-2 py-1">Profesional</div>
                    <div className="font-semibold bg-gray-300  text-gray-800 px-2 py-1">Fecha de Atención</div>
                    <div className="font-semibold bg-gray-300  text-gray-800 px-2 py-1 text-center"></div>
                </div>
                <div className="bg-white px-2 py-2 border border-gray-200">
                    {assessments?.map((assessment) => (
                        <div className="grid grid-cols-4 pb-2">
                            <div className="text-sm bg-white px-2 py-2 border border-gray-300">{assessment.identificationPatient}</div>
                            <div className="text-sm bg-white px-2 py-2 border border-gray-300">{assessment.functionaryName}</div>
                            <div className="text-sm bg-white px-2 py-2 border border-gray-300">{assessment.date.toString()}</div>
                            <div className=" flex ml-4">
                                <button
                                    className="border-[#FF677D] border-2 hover:bg-[#ff677e88] transition-all text-[#921729c4]  px-6 py-2 rounded-lg font-semibold cursor-pointer"
                                    onClick={(e) => handleDelete(e, assessment.id)}
                                >
                                    Eliminar
                                </button>
                            </div>
                        </div>
                    ))}
                </div>
            </div>
        </>
    );
}
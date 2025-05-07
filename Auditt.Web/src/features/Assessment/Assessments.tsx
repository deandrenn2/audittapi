import { useState } from "react";
import { ClientSelect } from "../Clients/ClientSelect";
import { Option } from "../../shared/model";
import { SingleValue } from "react-select";
import { Link } from "react-router-dom";
import { useAssessments } from "./useAssessment";
import { Bar } from "../../shared/components/Progress/Bar";
import useUserContext from "../../shared/context/useUserContext";

export const Assessments = () => {
    const { queryAssessments, assessments, deleteAssessment } = useAssessments();
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

    if (queryAssessments.isLoading)
        return <Bar />;



    return (
        <div className="flex-1 p-8">
            <div className="">
                <div className="flex items-center space-x-4 mb-4">
                    <span className="font-medium">IPS</span>
                    <ClientSelect className="w-lg" selectedValue={selectedClient} xChange={handleChangeClient} isSearchable={true} />
                </div>

            </div>
            <h1 className="text-2xl font-semibold mb-4">Evaluaciones o auditorias</h1>
            <Link to={'/Assessments/Create'} title='Crear' className="bg-[#392F5A] hover:bg-indigo-900 text-white px-6 py-2 rounded-lg font-semibold mb-2">
                Realizar valoraciones
            </Link>

            <div>
                <div className="grid grid-cols-5">
                    <div className="gap-3 font-semibold bg-gray-300  text-gray-800 px-2 py-1">Historia</div>
                    <div className="gap-3 font-semibold bg-gray-300  text-gray-800 px-2 py-1">Profesional</div>
                    <div className="gap-3 font-semibold bg-gray-300  text-gray-800 px-2 py-1">Fecha de Atención</div>
                    <div className="gap-3 font-semibold bg-gray-300  text-gray-800 px-2 py-1 text-center"></div>
                </div>
                <div className="bg-white px-2 py-2 border border-gray-200">
                    {assessments?.map((assessment) => (
                        <div className="grid grid-cols-5">
                            <div className=" gap-3 text-sm bg-white px-2 py-2 border border-gray-300 mr-2">{assessment.identificationPatient}</div>
                            <div className=" gap-3 text-sm bg-white px-2 py-2 border border-gray-300 mr-2">{assessment.functionaryName}</div>
                            <div className=" gap-3 text-sm bg-white px-2 py-2 border border-gray-300 mr-2">{assessment.date.toString()}</div>
                            <div className=" flex justify-center">
                                <button
                                    className="bg-indigo-500 hover:bg-indigo-900 text-white px-6 py-2 rounded-lg font-semibold mb-2"
                                    onClick={() => deleteAssessment.mutate(assessment.id)}
                                >
                                    Eliminar
                                </button>
                            </div>
                        </div>
                    ))}
                </div>
            </div>
        </div>

    );
}
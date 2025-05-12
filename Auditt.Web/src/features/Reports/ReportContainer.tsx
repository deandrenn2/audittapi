import { useState } from "react";
import { Option } from "../../shared/model";
import { FunctionarySelect } from "../Clients/Professionals/FunctionarySelect"
import { DataCutSelect } from "../DataCuts/DataCutsSelect"
import { GuideSelect } from "../Guide/GuideSelect"
import useUserContext from "../../shared/context/useUserContext";
import { SingleValue } from "react-select";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faArrowCircleLeft } from "@fortawesome/free-solid-svg-icons";
import { Link } from "react-router-dom";
import { ClientSelect } from "../Clients/ClientSelect";

export const ReportContainer = () => {
    const { client } = useUserContext();
    const selectedClient: Option | undefined = {
        value: client?.id?.toString(),
        label: client?.name,
    };

    const [selectedDataCut, setSelectedDataCut] = useState<Option | undefined>(() => ({
        value: "0",
        label: "Seleccione un corte",
    }));

    const [selectedFunctionary, setSelectedFunctionary] = useState<Option | undefined>(() => ({
        value: "0",
        label: "Seleccione un profesional",
    }));

    const [selectedGuide, setSelectedGuide] = useState<Option | undefined>(() => ({
        value: "0",
        label: "Seleccione una guía",
    }));

    console.log(selectedGuide, "guide", selectedFunctionary, "funcionario", selectedDataCut, "corte");

    const handleChangeDataCut = (newValue: SingleValue<Option>) => {
        setSelectedDataCut({
            value: newValue?.value,
            label: newValue?.label,
        });
    }

    const handleChangeFunctionary = (newValue: SingleValue<Option>) => {
        setSelectedFunctionary({
            value: newValue?.value,
            label: newValue?.label,
        });
    }

    const handleChangeGuide = (newValue: SingleValue<Option>) => {
        setSelectedGuide({
            value: newValue?.value,
            label: newValue?.label,
        });
    }

    return (
        <>
            <div className="w-full">
                <div className="flex gap-4 p-4 justify-between">
                    <Link to={'/Assessments'}
                        title='Volver' className="bg-gray-300 hover:bg-gray-300 text-gray-700 hover:text-gray-800 border border-gray-400 hover:border-gray-600 px-4 py-2 rounded font-bold flex items-center transition-all">
                        <FontAwesomeIcon
                            icon={faArrowCircleLeft}
                            className="fa-search top-3 pr-2 font-bold"
                        />Volver
                    </Link>

                    <div className="flex items-center space-x-4 mb-4">
                        <span className="font-medium">IPS</span>
                        <ClientSelect className="w-lg" selectedValue={selectedClient} isSearchable={true} />
                    </div>
                    <div className="flex items-center">
                        <Link to={'/Assessments/Create'} className="bg-[#392F5A] hover:bg-indigo-900 text-white px-6 py-2 rounded-lg font-semibold mb-2" >
                            Ir a Indicadores e informes</Link>
                    </div>
                </div>
            </div>
            <div className="w-full p-4 ">

                <div className="flex py-2">
                    <h1 className="text-2xl font-semibold mb-2">Medición de Adherencia</h1>

                </div>
                <div className="grid grid-cols-2 gap-4">
                    <div className="p-1">
                        <div className="grid grid-cols-2 gap-4">
                            <div className="flex flex-col space-x-4 mb-4">
                                <span className="font-medium">Corte de Auditoria</span>
                                <DataCutSelect className="w-full" selectedValue={selectedDataCut} xChange={handleChangeDataCut} isSearchable={true} />
                            </div>
                            <div className="flex flex-col space-x-4 mb-4">
                                <span className="font-medium">Profesional evaluado</span>
                                <FunctionarySelect className="w-full" selectedValue={selectedFunctionary} xChange={handleChangeFunctionary} isSearchable={true} />
                            </div>
                        </div>

                        <div className="flex flex-col space-x-4 mb-4">
                            <span className="font-medium">Instrumento de adherencia a GPC</span>
                            <GuideSelect className="w-full" selectedValue={selectedGuide} xChange={handleChangeGuide} isSearchable={true} />
                        </div>

                    </div>
                </div>
            </div>
        </>
    )
}   
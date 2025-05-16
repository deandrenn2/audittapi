import { useState } from "react";
import { Option } from "../../shared/model";
import { DataCutSelect } from "../DataCuts/DataCutsSelect"
import { GuideSelect } from "../Guide/GuideSelect"
import useUserContext from "../../shared/context/useUserContext";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faArrowCircleLeft } from "@fortawesome/free-solid-svg-icons";
import { Link } from "react-router-dom";
import { ClientSelect } from "../Clients/ClientSelect";
import { ReportDashboard } from "./ReportDashboard";
import { ReportList } from "./Common/ReportList";
import { ReportGeneral } from "./ReportGeneral";
import { ReportFunctionaries } from "./ReportFunctionaries";

export const ReportContainer = () => {
    const { client } = useUserContext();
    const REPORT_LIST = [
        {
            idReport: 1,
            name: "Informe por preguntas",
            description: "Informe de adherencia por preguntas, para la información del corte seleccionado",
        },
        {
            idReport: 2,
            name: "Informe por Profesional",
            description: `Informe por Profesional evaluado
Adherencia Estricta(Adherencia global o no estricta) y Por Criterio`,
        },
    ];
    const [selectedReport, setSelectedReport] = useState<number>(1);
    const selectedClient: Option | undefined = {
        value: client?.id?.toString(),
        label: client?.name,
    };



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
            <div className="w-full">

                <div className="flex py-2 bg-white px-4">
                    <h1 className="text-2xl font-semibold mb-2">Medición de Adherencia</h1>
                </div>
                <div className=" bg-white px-4">
                    <div className=" flex justify-center gap-4 p-2">
                        <div className="flex flex-col ">
                            <span className="font-medium">Corte de Auditoria</span>
                            <DataCutSelect className="w-full min-w-60" isSearchable={true} />
                        </div>

                        <div className="flex flex-col ">
                            <span className="font-medium">Instrumento de adherencia a GPC</span>
                            <GuideSelect className="w-full" isSearchable={true} />
                        </div>

                    </div>
                    <div>
                        <ReportDashboard />
                    </div>
                </div>
                <div className="px-4">
                    <div className="flex flex-col space-y-4">
                        <ReportList listReports={REPORT_LIST} setSelected={setSelectedReport} idSelected={selectedReport} />
                    </div>
                </div>
                <div className="px-4">
                    {
                        selectedReport == 1 && <ReportGeneral />
                    }
                    {
                        selectedReport == 2 && <ReportFunctionaries />
                    }
                </div>
            </div>
        </>
    )
}   
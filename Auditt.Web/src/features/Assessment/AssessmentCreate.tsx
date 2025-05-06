import { useState } from "react";
import { Option } from "../../shared/model";
import { ClientSelect } from "../Clients/ClientSelect"
import { SingleValue } from "react-select";
import { DataCutSelect } from "../DataCuts/DataCutsSelect";
import { FunctionarySelect } from "../Clients/Professionals/FunctionarySelect";
import { GuideSelect } from "../Guide/GuideSelect";
import { Link } from "react-router-dom";
import { useAssessmentByDocumentMutation, useAssessments } from "./useAssessment";
import { usePatientByDocumentMutation } from "../Clients/Patients/UsePatients";
import { parseISO, differenceInYears } from "date-fns";
import { AssessmentDetailModel } from "./AssessmentModel";
import { AssessmentValuations } from "./AssessmentValuations";

export const AssessmentCreate = () => {
    const { createAssessment } = useAssessments();
    const { getPatientByDocumentMutation } = usePatientByDocumentMutation();
    const { getAssessmentByDocumentMutation } = useAssessmentByDocumentMutation();
    const [assessment, setAssessment] = useState<AssessmentDetailModel | undefined>(undefined);
    const [selectedClient, setSelectedClient] = useState<Option | undefined>(() => ({
        value: "0",
        label: "Seleccione un cliente",
    }));

    const [selectedDataCut, setSelectedDataCut] = useState<Option | undefined>(() => ({
        value: "0",
        label: "Seleccione un corte",
    }));

    const [selectedFunctionary, setSelectedFunctionary] = useState<Option | undefined>(() => ({
        value: "0",
        label: "Seleccione un corte",
    }));

    const [selectedGuide, setSelectedGuide] = useState<Option | undefined>(() => ({
        value: "0",
        label: "Seleccione un corte",
    }));


    const handleChangeClient = (newValue: SingleValue<Option>) => {
        setSelectedClient({
            value: newValue?.value,
            label: newValue?.label,
        });
    }

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

    const handlePatientFind = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        const form = e.target as HTMLFormElement;

        const formData = new FormData(form);
        const client = Object.fromEntries(formData.entries());
        const documentSearch = client["document"].toString();
        const patientRes = await getPatientByDocumentMutation.mutateAsync(documentSearch);



        if (patientRes.isSuccess) {
            const patient = patientRes?.data;

            const assessment = await getAssessmentByDocumentMutation.mutateAsync(documentSearch);

            if (assessment.isSuccess) {
                setAssessment(assessment?.data);
            } else {

                const birthDate = parseISO(patient?.birthDate ?? new Date().toString()); // Año, mes (0-indexado), día
                const today = new Date();

                const age = differenceInYears(today, birthDate);

                const res = await createAssessment.mutateAsync({
                    idInstitucion: Number(selectedClient?.value),
                    idDataCut: Number(selectedDataCut?.value),
                    idFunctionary: Number(selectedFunctionary?.value),
                    idPatient: patient?.id ?? 0,
                    date: new Date(),
                    eps: patient?.eps ?? "",
                    idUser: 1,
                    idGuide: Number(selectedGuide?.value),
                    yearOld: age.toString(),
                });

                if (res.isSuccess) {
                    const assessment = await getAssessmentByDocumentMutation.mutateAsync(documentSearch);
                    if (assessment.isSuccess)
                        setAssessment(assessment?.data);

                }
            }
        }
    }


    return (
        <div className="w-full">
            <div className="flex space-x-4 mb-4 p-4">
                <h1 className="text-2xl font-semibold mb-4">Medición de Adherencia</h1>
                <Link to={'/Assessments/Create'} className="bg-[#392F5A] hover:bg-indigo-900 text-white px-6 py-2 rounded-lg font-semibold mb-2" >
                    Ir a Indicadores e informes</Link>
            </div>
            <div className="grid grid-cols-2">
                <div>


                    <div className="flex items-center space-x-4 mb-4">
                        <span className="font-medium">IPS</span>
                        <ClientSelect className="w-lg" selectedValue={selectedClient} xChange={handleChangeClient} isSearchable={true} />
                    </div>
                    <div className="flex items-center space-x-4 mb-4">
                        <span className="font-medium">Corte de Auditoria</span>
                        <DataCutSelect className="w-lg" selectedValue={selectedDataCut} xChange={handleChangeDataCut} isSearchable={true} />
                    </div>
                    <div className="flex items-center space-x-4 mb-4">
                        <span className="font-medium">Profesiona evaluado</span>
                        <FunctionarySelect className="w-lg" selectedValue={selectedFunctionary} xChange={handleChangeFunctionary} isSearchable={true} />
                    </div>
                    <div className="flex items-center space-x-4 mb-4">
                        <span className="font-medium">Instrumento de adherencia a GPC</span>
                        <GuideSelect className="w-lg" selectedValue={selectedGuide} xChange={handleChangeGuide} isSearchable={true} />
                    </div>

                </div>
                <div>

                    <div className="flex flex-col space-y-4 p-4">
                        <form onSubmit={handlePatientFind} className="flex flex-col space-y-4">
                            <div className="flex flex-col">
                                <label htmlFor="licenseInput" className="font-medium mb-2">Id del Paciente</label>
                                <input
                                    id="licenseInput"
                                    type="text"
                                    name="document"
                                    placeholder="Ingrese el número de identificación del paciente"
                                    className="border border-gray-300 rounded-lg px-4 py-2 focus:outline-none focus:ring-2 focus:ring-indigo-500"
                                />
                            </div>
                            <button
                                className="bg-[#392F5A] hover:bg-purple-950 text-white px-6 py-2 rounded-lg font-semibold cursor-pointer"
                                disabled={createAssessment.isPending}
                            >
                                {createAssessment.isPending ? "Generando..." : "Diligenciar"}
                            </button>
                        </form>
                        <div className="flex">
                            <div>
                                <label htmlFor="licenseInput" className="font-medium mb-2">Edad</label>
                                <input
                                    disabled
                                    type="text"
                                    value={assessment?.yearOld}
                                    placeholder="Edad"
                                    className="border disabled:bg-gray-200 border-gray-300 rounded-lg px-4 py-2 focus:outline-none focus:ring-2 focus:ring-indigo-500"
                                />
                            </div>
                            <div>
                                <label htmlFor="licenseInput" className="font-medium mb-2">Fecha de Atención</label>
                                <input
                                    type="text"
                                    value={assessment?.date}
                                    placeholder="Fecha de Atención"
                                    className="border disabled:bg-gray-200 border-gray-300 rounded-lg px-4 py-2 focus:outline-none focus:ring-2 focus:ring-indigo-500"
                                />
                            </div>
                            <div>
                                <label htmlFor="licenseInput" className="font-medium mb-2">Eps</label>
                                <input
                                    disabled
                                    type="text"
                                    value={assessment?.eps}
                                    placeholder="Eps"
                                    className="border disabled:bg-gray-200 border-gray-300 rounded-lg px-4 py-2 focus:outline-none focus:ring-2 focus:ring-indigo-500"
                                />
                            </div>
                        </div>
                    </div>
                </div>

            </div>
            <div className="bg-white text-2xl font-semibold mb-4">
                <h1>Evaluación de adherencia</h1>
            </div>
            <AssessmentValuations idScale={assessment?.idScale} valuations={assessment?.valuations} />
        </div>
    )
}


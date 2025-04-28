import { useState } from "react";
import { Option } from "../../shared/model";
import { ClientSelect } from "../Clients/ClientSelect"
import { SingleValue } from "react-select";
import { DataCutSelect } from "../DataCuts/DataCutsSelect";
import { FunctionarySelect } from "../Clients/Professionals/FunctionarySelect";
import { GuideSelect } from "../Guide/GuideSelect";
import { Link, useNavigate } from "react-router-dom";
import { useAssessments } from "./useAssessment";
import { usePatientByDocument } from "../Clients/Patients/UsePatients";

export const AssessmentCreate = () => {
    const { createAssessment } = useAssessments();
    const [document, setDocument] = useState<string>("");
    const { patient } = usePatientByDocument(document);
    const [selectedClient, setSelectedClient] = useState<Option | undefined>(() => ({
        value: "0",
        label: "Seleccione un cliente",
    }));

    const navigate = useNavigate();

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

    const handleCreateAssessment = async () => {
        const res = await createAssessment.mutateAsync({
            idInstitucion: Number(selectedClient?.value),
            idDataCut: Number(selectedDataCut?.value),
            idFunctionary: Number(selectedFunctionary?.value),
            idPatient: patient?.id ?? 0,
            date: new Date(),
            eps: patient?.eps ?? "",
            idUser: 1,
            idGuide: 0,
            yearOld: patient?.birthDate?.toString() ?? "",
        });

        if (res.isSuccess) {
            navigate("/Assessments/Create/" + res?.data?.id);
        }
    }

    const handleChangeDocument = (e: React.ChangeEvent<HTMLInputElement>) => {
        setDocument(e.target.value);
    }


    return (
        <div className="w-full">
            <div className="grid grid-cols-2">
                <div>

                    <div className="flex space-x-4 mb-4 p-4">
                        <h1 className="text-2xl font-semibold mb-4">Medición de Adherencia</h1>
                    </div>
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
                    </div>'
                    <div className="flex items-center space-x-4 mb-4">
                        <span className="font-medium">Instrumento de adherencia a GPC</span>
                        <GuideSelect className="w-lg" selectedValue={selectedGuide} xChange={handleChangeGuide} isSearchable={true} />
                    </div>
                    <div className="w-2/3 bg-white p-4 rounded-lg">
                        <h1>Formulario de medición de adherencia</h1>
                        {/* Aquí puedes agregar el formulario */}
                    </div>
                </div>
                <div>
                    <Link to={'/Assessments/Create'} className="bg-indigo-500 hover:bg-indigo-900 text-white px-6 py-2 rounded-lg font-semibold mb-2" >
                        Ir a Indicadores e informes</Link>
                    <div className="flex flex-col space-y-4 p-4">
                        <div className="flex flex-col">
                            <label htmlFor="licenseInput" className="font-medium mb-2">Id del Paciente</label>
                            <input
                                id="licenseInput"
                                type="text"
                                onChange={(e) => handleChangeDocument(e)}
                                placeholder="Ingrese el número de identificación del paciente"
                                className="border border-gray-300 rounded-lg px-4 py-2 focus:outline-none focus:ring-2 focus:ring-indigo-500"
                            />
                        </div>
                        <button
                            className="bg-[#392F5A] hover:bg-purple-950 text-white px-6 py-2 rounded-lg font-semibold cursor-pointer"
                            onClick={handleCreateAssessment}
                        >
                            Diligenciar
                        </button>
                        <div className="flex">
                            <div>
                                <label htmlFor="licenseInput" className="font-medium mb-2">Edad</label>
                                <input
                                    disabled
                                    type="text"
                                    placeholder="Edad"
                                    className="border disabled:bg-gray-200 border-gray-300 rounded-lg px-4 py-2 focus:outline-none focus:ring-2 focus:ring-indigo-500"
                                />
                            </div>
                            <div>
                                <label htmlFor="licenseInput" className="font-medium mb-2">Fecha de Atención</label>
                                <input
                                    disabled
                                    type="text"
                                    placeholder="Fecha de Atención"
                                    className="border disabled:bg-gray-200 border-gray-300 rounded-lg px-4 py-2 focus:outline-none focus:ring-2 focus:ring-indigo-500"
                                />
                            </div>
                            <div>
                                <label htmlFor="licenseInput" className="font-medium mb-2">Eps</label>
                                <input
                                    disabled
                                    type="text"
                                    placeholder="Eps"
                                    className="border disabled:bg-gray-200 border-gray-300 rounded-lg px-4 py-2 focus:outline-none focus:ring-2 focus:ring-indigo-500"
                                />
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    )
}